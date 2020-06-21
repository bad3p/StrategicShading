//#define PAUSE_ON_PERSONNEL_KILLED
//#define PAUSE_ON_PERSONNEL_WOUNDED

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
            _eventAggregatorBuffer[entityId].firstEventId = 0;
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
        
        float morale = _personnelBuffer[entityId].morale;
        
        // TEST MORALE LOSS
        /*         

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
        */
        float moraleLoss = 0.0f;
        
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
            
            float3 dirToTarget = new float3(0,0,0);
            float distToTarget = 0;
            float firepower = 0.0f;

            if (targetEntityId > 0)
            {
                dirToTarget = _transformBuffer[targetEntityId].position - _transformBuffer[entityId].position;
                distToTarget = length(dirToTarget);
                if (distToTarget > 0.0f)
                {
                    dirToTarget *= 1.0f / distToTarget;
                }
                firepower = GetFirearmFirepower(entityId, distToTarget);
            }

            // TODO: provide throshold in Targeting component?
            const float FirepowerThreshold = 75.0f;
                        
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
                        // reverse unmounting
                        if (firearmState == FIREARM_STATE_UNMOUNTING)
                        {
                            float mountingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].mountingTime - _firearmBuffer[entityId].timeout;
                            SetFirearmState(entityId, FIREARM_STATE_MOUNTING, mountingTime);
                        }
                        // complete mounting
                        else if (firearmState == FIREARM_STATE_MOUNTING)
                        {
                            _firearmBuffer[entityId].timeout -= _dT;
                            if (_firearmBuffer[entityId].timeout <= 0.0f)
                            {
                                SetFirearmState(entityId, FIREARM_STATE_IDLE);
                            }
                        }
                        // aim firearm
                        else if (firearmState == FIREARM_STATE_IDLE)
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
                                
                                // generate firearm event
                                
                                float4 eventParam = new float4(dirToTarget.x, dirToTarget.y, dirToTarget.z, firepower);
                                AddEvent(targetEntityId, entityId, EVENT_FIREARM_DAMAGE, eventParam);
                                
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
                                uint maxClipAmmo = _firearmDescBuffer[_firearmBuffer[entityId].descId].maxClipAmmo;
                                _firearmBuffer[entityId].clipAmmo = min( _firearmBuffer[entityId].ammo, maxClipAmmo );
                                _firearmBuffer[entityId].ammo -= _firearmBuffer[entityId].clipAmmo;
                                SetFirearmState(entityId, FIREARM_STATE_IDLE);
                            }
                        }
                    }
                }
            }
            else if (IsFirearmReady(entityId))
            {
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
                    // abort aiming immediately
                    if (firearmState == FIREARM_STATE_AIMING)
                    {
                        SetFirearmState(entityId, FIREARM_STATE_IDLE);
                    }
                    // complete reloading
                    else if (firearmState == FIREARM_STATE_RELOADING)
                    {
                        _firearmBuffer[entityId].timeout -= _dT;
                        if (_firearmBuffer[entityId].timeout <= 0.0f)
                        {
                            uint maxClipAmmo = _firearmDescBuffer[_firearmBuffer[entityId].descId].maxClipAmmo;
                            _firearmBuffer[entityId].clipAmmo = min( _firearmBuffer[entityId].ammo, maxClipAmmo );
                            _firearmBuffer[entityId].ammo -= _firearmBuffer[entityId].clipAmmo;
                            SetFirearmState(entityId, FIREARM_STATE_IDLE);
                        }
                    }
                    // unmount
                    else 
                    {
                        if (firearmState != FIREARM_STATE_UNMOUNTING)
                        {
                            float mountingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].mountingTime;
                            SetFirearmState(entityId, FIREARM_STATE_UNMOUNTING, mountingTime);
                        }
                        else
                        {
                            _firearmBuffer[entityId].timeout -= _dT;
                            if (_firearmBuffer[entityId].timeout <= 0.0f)
                            {
                                SetFirearmReady(entityId, false);
                                SetFirearmState(entityId, FIREARM_STATE_IDLE);
                            }
                        }    
                    }
                }
            }
            else
            {
                if (firearmState == FIREARM_STATE_MOUNTING)
                {
                    float mountingTime = _firearmDescBuffer[_firearmBuffer[entityId].descId].mountingTime - _firearmBuffer[entityId].timeout;
                    SetFirearmState(entityId, FIREARM_STATE_UNMOUNTING, mountingTime);
                }
                else if(firearmState == FIREARM_STATE_UNMOUNTING)
                {
                    _firearmBuffer[entityId].timeout -= _dT;
                    if (_firearmBuffer[entityId].timeout <= 0.0f)
                    {
                        SetFirearmReady(entityId, false);
                        SetFirearmState(entityId, FIREARM_STATE_IDLE);
                    }
                }
            }
            
            /*for (uint otherEntityId = 1; otherEntityId < _entityCount; otherEntityId++)
            {
                if (HasComponents(otherEntityId, EVENT_AGGREGATOR))
                {
                    if(rngRange(0.0f,1.0f,rngIndex(id.x)) < 0.5 )
                    {
                        float3 dir = (_transformBuffer[otherEntityId].position - _transformBuffer[entityId].position);
                        float dist = length(dir);
                        if (dist > 0.0f)
                        {
                            dir *= 1.0f / dist;
                        }
                        float power = GetFirearmFirepower(entityId, dist);
                        float4 eventParam = new float4(dir.x, dir.y, dir.z, power);
                        AddEvent(otherEntityId, entityId, EVENT_FIREARM_DAMAGE, eventParam);
                    }
                }
            }*/
        }
    }
    
    [NumThreads(256, 1, 1)]
    static public void ProcessEvents(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }

        if ( HasComponents(entityId, PERSONNEL_EVENT_AGGREGATOR) )
        {
            int eventId = _eventAggregatorBuffer[entityId].firstEventId;
            while (eventId > 0)
            {
                if (_eventBuffer[eventId].id == EVENT_FIREARM_DAMAGE)
                {                    
                    float firepower = _eventBuffer[eventId].param.w;
                    
                    // TODO: configure probabilities (?)
                    float4 firepowerColumn = new float4( 59.0f, 88.0f, 133.0f, 199.0f );
                    float4 killProbabilityColumn = new float4( 12.0f, 24.0f, 49.0f, 99.0f );
                    float4 woundProbabilityColumn = new float4( 29.0f, 44.0f, 66.0f, 99.0f );
                    float4 moraleDamageColumn = new float4( 54.0f, 73.0f, 99.0f, 133.0f );
                    const float KillMoraleDamage = 233.0f;
                    const float WoundMoraleDamage = 166.0f;

                    float killProbability = 0.0f;
                    float woundProbability = 0.0f;
                    float moraleDamage = 0.0f;

                    if (firepower < firepowerColumn.x)
                    {
                        killProbability = killProbabilityColumn.x;
                        woundProbability = woundProbabilityColumn.x;
                        moraleDamage = moraleDamageColumn.x;
                    }
                    else if (firepower < firepowerColumn.y)
                    {
                        float t = (firepower - firepowerColumn.x) / (firepowerColumn.y - firepowerColumn.x);
                        killProbability = lerp(killProbabilityColumn.x, killProbabilityColumn.y, t);
                        woundProbability = lerp(woundProbabilityColumn.x, woundProbabilityColumn.y, t);
                        moraleDamage = lerp(moraleDamageColumn.x, moraleDamageColumn.y, t);
                    }
                    else if (firepower < firepowerColumn.z)
                    {
                        float t = (firepower - firepowerColumn.y) / (firepowerColumn.z - firepowerColumn.y);
                        killProbability = lerp(killProbabilityColumn.y, killProbabilityColumn.z, t);
                        woundProbability = lerp(woundProbabilityColumn.y, woundProbabilityColumn.z, t);
                        moraleDamage = lerp(moraleDamageColumn.y, moraleDamageColumn.z, t);
                    }
                    else if (firepower < firepowerColumn.w)
                    {
                        float t = (firepower - firepowerColumn.z) / (firepowerColumn.w - firepowerColumn.z);
                        killProbability = lerp(killProbabilityColumn.z, killProbabilityColumn.w, t);
                        woundProbability = lerp(woundProbabilityColumn.z, woundProbabilityColumn.w, t);
                        moraleDamage = lerp(moraleDamageColumn.z, moraleDamageColumn.w, t);
                    }
                    else
                    {
                        killProbability = killProbabilityColumn.w;
                        woundProbability = woundProbabilityColumn.w;
                        moraleDamage = moraleDamageColumn.w;
                    }

                    float killDice = rngRange( 0.0f, 100.0f, rngIndex(id.x) );
                    if (killDice <= killProbability)
                    {
                        uint status0 = _personnelBuffer[entityId].status; 
                        bool killWounded = SwitchPersonnelStatus(entityId, PERSONNEL_STATUS_WOUNDED, PERSONNEL_STATUS_KILLED);
                        if( killWounded )
                        {
                            #if PAUSE_ON_PERSONNEL_KILLED
                            #if UNITY_EDITOR
                                uint status1 = _personnelBuffer[entityId].status;
                                Debug.Log( "Wounded personnel killed in " + entityId + " s0 = " + status0.ToString("X8") + " s1 = " + status1.ToString("X8") );
                                UnityEditor.EditorApplication.isPaused = true;
                            #endif
                            #endif
                            moraleDamage = KillMoraleDamage;
                        }
                        else
                        {
                            bool kiilHealthy = SwitchPersonnelStatus(entityId, PERSONNEL_STATUS_HEALTHY, PERSONNEL_STATUS_KILLED);
                            if (kiilHealthy)
                            {
                                #if PAUSE_ON_PERSONNEL_KILLED
                                #if UNITY_EDITOR
                                    uint status1 = _personnelBuffer[entityId].status;
                                    Debug.Log( "Healthy personnel killed in " + entityId + " s0 = " + status0.ToString("X8") + " s1 = " + status1.ToString("X8") );
                                    UnityEditor.EditorApplication.isPaused = true;
                                #endif
                                #endif
                                moraleDamage = KillMoraleDamage;
                            }                            
                        }
                    }
                    else
                    {
                        float woundDice = rngRange( 0.0f, 100.0f, rngIndex(id.x) );
                        if (woundDice <= woundProbability)
                        {
                            uint status0 = _personnelBuffer[entityId].status;
                            bool woundHealthy = SwitchPersonnelStatus(entityId, PERSONNEL_STATUS_HEALTHY, PERSONNEL_STATUS_WOUNDED);
                            if (woundHealthy)
                            {
                                #if PAUSE_ON_PERSONNEL_WOUNDED
                                #if UNITY_EDITOR
                                    uint status1 = _personnelBuffer[entityId].status;
                                    Debug.Log("Healthy personnel wounded in " + entityId + " s0 = " + status0.ToString("X8") + " s1 = " + status1.ToString("X8"));
                                    UnityEditor.EditorApplication.isPaused = true;
                                #endif
                                #endif
                                moraleDamage = WoundMoraleDamage;
                            }
                            else
                            {
                                bool killWounded = SwitchPersonnelStatus(entityId, PERSONNEL_STATUS_WOUNDED, PERSONNEL_STATUS_KILLED);
                                if( killWounded )
                                {
                                    #if PAUSE_ON_PERSONNEL_KILLED
                                    #if UNITY_EDITOR
                                        uint status1 = _personnelBuffer[entityId].status;
                                        Debug.Log( "Wounded personnel wounded again and killed in " + entityId + " s0 = " + status0.ToString("X8") + " s1 = " + status1.ToString("X8") );
                                        UnityEditor.EditorApplication.isPaused = true;
                                    #endif
                                    #endif
                                    moraleDamage = KillMoraleDamage;
                                }
                            }
                        }
                    }
                    
                    float morale = _personnelBuffer[entityId].morale;
                    morale = clamp( morale - moraleDamage, PERSONNEL_MORALE_MIN, PERSONNEL_MORALE_MAX );
                    _personnelBuffer[entityId].morale = morale;
                }                
                eventId = _eventBuffer[eventId].nextEventId;
            }
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
