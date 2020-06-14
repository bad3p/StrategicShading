using Types;
using Structs;
using UnityEngine;
using Transform = Structs.Transform;

public partial class ComputeShaderEmulator
{
    public static int _outBufferSizeX;
    public static int _outBufferSizeY;
    public static int _outBufferSizeZ;
    public static int3[] _outBuffer = new int3[0];

    [NumThreads(16,8,4)]
    static public void GenerateThreadIDs(uint3 id)
    {
        int index = (int)(id.z) * _outBufferSizeX *_outBufferSizeY + (int)(id.y) * _outBufferSizeX + (int)(id.x);
        if( index < _outBufferSizeX * _outBufferSizeY * _outBufferSizeZ )
        {
            var temp = _outBuffer[index];
            temp.x += (int)(id.x);
            temp.y += (int)(id.y);
            temp.z += (int)(id.z);
            _outBuffer[index] = temp;
        }
    }
    
    [NumThreads(256,1,1)]
    static public void Cleanup(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }
        
        if ( HasComponents( entityId, EVENT_AGGREGATOR ) )
        {
            _eventAggregatorBuffer[entityId].eventCount = 0;
            _eventAggregatorBuffer[entityId].firearmEventIndex = 0;
        }
    }
    
    [NumThreads(256,1,1)]
    static public void UpdateMovement(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }
        
        if ( !HasComponents( entityId, HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT ) )
        {
            return;
        }

        if (IsMoving(entityId))
        {
            double2 targetPosition = _movementBuffer[entityId].targetPosition.xz;
            double2 currentPosition = _transformBuffer[entityId].position.xz;
            
            float2 targetDir = targetPosition - currentPosition;
            float targetDist = length( targetDir );
            if (targetDist > FLOAT_EPSILON)
            {
                targetDir *= 1.0f / targetDist;
            }

            // TODO : improve broken & pinned behaviour
            uint suppression = GetPersonnelSuppression(entityId);
            if (suppression >= SUPPRESSION_BROKEN)
            {
                targetDir *= -1;
            }
            else if (suppression >= SUPPRESSION_PINNED)
            {
                targetDir = new float2(0,0);
            }

            float3 transformForward = new float3(0, 0, 1);
            transformForward = rotate(transformForward, _transformBuffer[entityId].rotation);

            float2 currentDir = transformForward.xz;
            float currentDirMagnitude = length(currentDir);
            if (currentDirMagnitude > FLOAT_EPSILON)
            {
                currentDir *= 1.0f / currentDirMagnitude;
            }

            uint personnelDescId = _personnelBuffer[entityId].descId; 

            float currentAngle = sigangle(currentDir, targetDir);
            float targetAngularVelocity = _personnelDescBuffer[personnelDescId].angularVelocity;
            float deltaAngle = -sign(currentAngle) * targetAngularVelocity * _dT;
            if (abs(deltaAngle) > abs(currentAngle))
            {
                deltaAngle = -currentAngle;
            }

            float targetVelocity = GetPersonnelTargetVelocity(entityId);
            float4 targetVelocityByAngle = new float4
            (
                radians(0.0f),
                targetVelocity,
                radians(45.0f),
                0.0f
            );

            float currentVelocity = lerpargs(targetVelocityByAngle, abs(currentAngle));

            bool stop = false;
            float deltaDist = currentVelocity * _dT;
            if (abs(deltaDist) >= targetDist)
            {
                deltaDist = targetDist;
                stop = true;
            }

            _movementBuffer[entityId].deltaPosition = new float3( targetDir.x, 0, targetDir.y ) * deltaDist;
            _transformBuffer[entityId].position += new double3( targetDir.x, 0, targetDir.y ) * deltaDist;
            float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, new float3(0, 1, 0));
            _transformBuffer[entityId].rotation = transformQuaternion(deltaRotation, _transformBuffer[entityId].rotation);

            if( stop )
            {
                _movementBuffer[entityId].targetPosition = _transformBuffer[entityId].position;
                _movementBuffer[entityId].targetVelocityByDistance = new float4(0,0,0,0);
            }
            
            // adjust altitude
            
            float mapAltitude = GetMapAltitude(_transformBuffer[entityId].position.xz);
            float bottomAltitude = (float) _transformBuffer[entityId].position.y - _transformBuffer[entityId].scale.y / 2.0f;
            float deltaAltitude = mapAltitude - bottomAltitude;
            if (abs(deltaAltitude) > FLOAT_EPSILON)
            {
                _transformBuffer[entityId].position.y += deltaAltitude;
            }
            
            // enter indoor

            _hierarchyBuffer[entityId].indoorEntityId = GetIndoorEntityId(entityId);
        }
        else
        {
            // rotate to target
            
            float3 targetForward = new float3(0, 0, 1);
            targetForward = rotate(targetForward, _movementBuffer[entityId].targetRotation);
            
            float3 currentForward = new float3(0, 0, 1);
            currentForward = rotate(currentForward, _transformBuffer[entityId].rotation);

            float3 axis = normalize(cross(currentForward, targetForward));

            float currentAngle = sigangle(currentForward, targetForward, axis);

            if (abs(currentAngle) > FLOAT_EPSILON)
            {
                uint personnelDescId = _personnelBuffer[entityId].descId;
                float targetAngularVelocity = _personnelDescBuffer[personnelDescId].angularVelocity;

                float deltaAngle = sign(currentAngle) * targetAngularVelocity * _dT;
                if (abs(deltaAngle) > abs(currentAngle))
                {
                    deltaAngle = currentAngle;
                }

                float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, axis);
                _transformBuffer[entityId].rotation = transformQuaternion(deltaRotation, _transformBuffer[entityId].rotation);
            }

            _movementBuffer[entityId].deltaPosition = new float3(0, 0, 0);
        }
    }

    [NumThreads(256, 1, 1)]
    static public void UpdatePersonnel(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }

        if ( !HasComponents( entityId, HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT ) )
        {
            return;
        }
        
        // TEST MORALE LOSS

        float morale = _personnelBuffer[entityId].morale; 

        float4 moraleLossProbabilityByMorale = new float4
        (
            400.0f,
            1.0f / 3333.0f,
            600.0f,
            1.0f / 333.0f
        );

        float diceThreshold = lerpargs(moraleLossProbabilityByMorale, morale);
        float dice = rngRange(0.0f, 1.0f, rngIndex(entityId));
        float moraleLoss = 0.0f;
        if (dice < diceThreshold)
        {
            moraleLoss = rngRange(25.0f, 50.0f, rngIndex(entityId));     
        }
        
        // MORALE RECOVERY

        float moraleRecoveryRate = GetPersonnelMoraleRecoveryRate(entityId);
        float moraleGain = moraleRecoveryRate * _dT;

        // MORALE DYNAMICS
        
        _personnelBuffer[entityId].morale = clamp(morale - moraleLoss + moraleGain, PERSONNEL_MORALE_MIN, PERSONNEL_MORALE_MAX);
        
        // POSE DYNAMICS

        uint suppression = GetPersonnelSuppression(entityId);
        if (suppression >= SUPPRESSION_PANIC)
        {
            SetPersonnelPose(entityId, PERSONNEL_POSE_HIDING);
        }
        else if (suppression >= SUPPRESSION_PINNED)
        {
            SetPersonnelPose(entityId, PERSONNEL_POSE_LAYING);
        }
        else if (suppression >= SUPPRESSION_SHAKEN)
        {
            SetPersonnelPose(entityId, PERSONNEL_POSE_CROUCHING);
        }
        else
        {
            SetPersonnelPose(entityId, PERSONNEL_POSE_STANDING);
        }
        
        // JOIN & LEAVE HIERARCHY REQUESTS
        
        if (suppression >= SUPPRESSION_PINNED )
        {
            uint parentEntityId = _hierarchyBuffer[entityId].parentEntityId;
            if (parentEntityId > 0)
            {
                _hierarchyBuffer[parentEntityId].joinEntityId = entityId;
            }
        }
        else if (suppression < SUPPRESSION_PINNED )
        {
            uint parentEntityId = _hierarchyBuffer[entityId].parentEntityId;
            uint joinEntityId = _hierarchyBuffer[entityId].joinEntityId;
            if (parentEntityId == 0 && joinEntityId > 0)
            {
                _hierarchyBuffer[joinEntityId].joinEntityId = entityId;
            }
        }
        
        // FITNESS

        float fitnessConsumptionRate = GetPersonnelFitnessConsumptionRate(entityId);

        if (fitnessConsumptionRate > FLOAT_EPSILON)
        {
            float fitness = _personnelBuffer[entityId].fitness;
            _personnelBuffer[entityId].fitness = clamp(fitness - fitnessConsumptionRate * _dT, PERSONNEL_FITNESS_MIN, PERSONNEL_FITNESS_MAX);
        }
    }

    [NumThreads(256, 1, 1)]
    static public void UpdateTargeting(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }

        if ( !HasComponents( entityId, TRANSFORM_TARGETING ) )
        {
            return;
        }

        uint team = GetTeam(entityId);
        double3 position = _transformBuffer[entityId].position;
        
        uint numEnemies = 0;
        uint numAllies = 0;
        float3 front = new float3(0,0,0);
        uint4 firearmTargetIds = new uint4(0,0,0,0);
        float4 firearmTargetWeights = new float4(0.0f,0.0f,0.0f,0.0f); 
        
        for (uint otherEntityId = 1; otherEntityId < _entityCount; otherEntityId++)
        {
            if (otherEntityId != entityId)
            {
                if (HasComponents(otherEntityId, TRANSFORM))
                {
                    float3 dirToOtherEntity = _transformBuffer[otherEntityId].position - position;
                    float distToOtherEntity = length(dirToOtherEntity);
                    if (distToOtherEntity > FLOAT_EPSILON)
                    {
                        dirToOtherEntity *= 1.0f / distToOtherEntity;
                    }

                    uint otherTeam = GetTeam(otherEntityId);
                    
                    // structure
                    if (otherTeam == 0)
                    {
                    }
                    // enemy
                    else if (otherTeam != team)
                    {
                        if (GetLineOfSight(entityId, otherEntityId))
                        {
                            // TODO: consider enemy firepower
                            float exposure = GetExposure(otherEntityId);
                            float weight = distToOtherEntity / exposure;
                            InsertTarget(ref firearmTargetIds, ref firearmTargetWeights, otherEntityId, weight);
                            numEnemies++;
                            front += dirToOtherEntity;
                        }
                    }
                    // ally
                    else
                    {
                        numAllies++;
                    }
                }
            }
        }

        front = normalize(front);

        _targetingBuffer[entityId].numEnemies = numEnemies;
        _targetingBuffer[entityId].numAllies = numAllies + 1;
        _targetingBuffer[entityId].front = front;
        _targetingBuffer[entityId].firearmTargetIds = firearmTargetIds;
        _targetingBuffer[entityId].firearmTargetWeights = firearmTargetWeights;
    }
    
    [NumThreads(256, 1, 1)]
    static public void UpdateFirearms(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }

        if ( HasComponents( entityId, TRANSFORM_PERSONNEL_FIREARMS_TARGETING ) )
        {
            uint targetEntityId = _targetingBuffer[entityId].firearmTargetIds.x;
            float firepower = 0.0f;

            if (targetEntityId > 0)
            {
                float3 dirToTarget = _transformBuffer[targetEntityId].position - _transformBuffer[entityId].position;
                float distToTarget = length(dirToTarget);
                firepower = GetFirearmFirepower(entityId, distToTarget);
                if (_firearmDescBuffer[_firearmBuffer[entityId].descId].crew <= 1)
                {
                    firepower *= GetPersonnelCount(entityId);
                }
                if (_firearmDescBuffer[_firearmBuffer[entityId].descId].maxBurstAmmo > 1)
                {
                    firepower *= _firearmDescBuffer[_firearmBuffer[entityId].descId].maxBurstAmmo;
                }
            }

            // TODO: provide in Targeting component
            const float FirepowerThreshold = 50.0f;
                        
            uint firearmState = GetFirearmState(entityId);
            if (targetEntityId > 0 && firepower >= FirepowerThreshold)
            {
                // ready firearm
                if (!IsFirearmReady(entityId))
                {
                    if (firearmState != FIREARM_STATE_MOUNTING)
                    {
                        float mountingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].mountingTime;
                        SetFirearmState(entityId, FIREARM_STATE_MOUNTING, mountingTime);
                    }
                    else
                    {
                        _firearmBuffer[entityId].timeout -= _dT;
                        if (_firearmBuffer[entityId].timeout <= 0.0f)
                        {
                            SetFirearmReady(entityId, true);
                            SetFirearmState(entityId, FIREARM_STATE_IDLE);
                        }
                    }
                }
                else
                {
                    // unjam firearm
                    if (IsFirearmJammed(entityId))
                    {
                        if (firearmState != FIREARM_STATE_UNJAMMING)
                        {
                            float unjammingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].unjammingTime;
                            SetFirearmState(entityId, FIREARM_STATE_UNJAMMING, unjammingTime);
                        }
                        else
                        {
                            _firearmBuffer[entityId].timeout -= _dT;
                            if (_firearmBuffer[entityId].timeout <= 0.0f)
                            {
                                SetFirearmJammed(entityId, false);
                                SetFirearmState(entityId, FIREARM_STATE_IDLE);
                            }    
                        }
                    }
                    else
                    {
                        // aim firearm
                        if (firearmState == FIREARM_STATE_IDLE)
                        {
                            float aimingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].aimingTime;
                            SetFirearmState(entityId, FIREARM_STATE_AIMING, aimingTime);
                        }
                        else if (firearmState == FIREARM_STATE_AIMING)
                        {
                            _firearmBuffer[entityId].timeout -= _dT;
                            if (_firearmBuffer[entityId].timeout <= 0.0f)
                            {
                                // spent ammo
                                
                                uint burstAmmo = 1;
                                uint maxBurstAmmo = _firearmDescBuffer[_firearmBuffer[entityId].descId].maxBurstAmmo;
                                if (maxBurstAmmo > 1)
                                {
                                    burstAmmo = (uint)rngRange(1, (int) maxBurstAmmo, rngIndex(id.x));
                                }
                                burstAmmo = min( _firearmBuffer[entityId].clipAmmo, burstAmmo );
                                _firearmBuffer[entityId].clipAmmo -= burstAmmo; 
                                
                                // TODO: generate firearm event
                                // TODO: randomly jam firearm

                                if (_firearmBuffer[entityId].clipAmmo == 0)
                                {
                                    float reloadingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].reloadingTime;
                                    SetFirearmState(entityId, FIREARM_STATE_RELOADING, reloadingTime);
                                }
                                else
                                {
                                    SetFirearmState(entityId, FIREARM_STATE_IDLE);
                                }
                            }
                        }
                        else if (firearmState == FIREARM_STATE_RELOADING)
                        {
                            _firearmBuffer[entityId].timeout -= _dT;
                            if (_firearmBuffer[entityId].timeout <= 0.0f)
                            {
                                uint maxClipAmmo = _firearmDescBuffer[_firearmBuffer[entityId].descId].maxAmmo;
                                _firearmBuffer[entityId].clipAmmo = min( _firearmBuffer[entityId].ammo, maxClipAmmo );
                                _firearmBuffer[entityId].ammo -= _firearmBuffer[entityId].clipAmmo;
                                SetFirearmState(entityId, FIREARM_STATE_IDLE);
                            }
                        }
                    }
                }
            }
            else
            {
                
            }


            /*
            for (uint targetEntityId = 1; targetEntityId < _entityCount; targetEntityId++)
            {
                if (HasComponents(targetEntityId, EVENT_AGGREGATOR))
                {
                    if(rngRange(0.0f,1.0f,rngIndex(id.x)) < 0.9 )
                    {
                        AddFirearmEvent(entityId, targetEntityId, rngRange(10.0f, 100.0f, rngIndex(id.x)));
                    }
                }
            }*/
        }
    }    

    [NumThreads(256, 1, 1)]
    static public void UpdateJoinRequests(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }
        
        // process only entities representing groups of sub-units

        if ( (_descBuffer[entityId] & ~TEAM_BITMASK ) == HIERARCHY )
        {
            uint childEntityId = _hierarchyBuffer[entityId].joinEntityId; 
            if ( childEntityId > 0 )
            {
                // request resolution : entity want to leave hierarchy
                if (_hierarchyBuffer[childEntityId].parentEntityId == entityId)
                {
                    DisconnectChildHierarchy(entityId, childEntityId);
                    _hierarchyBuffer[childEntityId].joinEntityId = entityId;
                }
                // request resolution : entity want to join hierarchy
                else if (_hierarchyBuffer[childEntityId].parentEntityId == 0)
                {
                    ConnectChildHierarchy(entityId,childEntityId);
                    _hierarchyBuffer[childEntityId].joinEntityId = 0;
                }
                _hierarchyBuffer[entityId].joinEntityId = 0;
            }
        }
    }
}
