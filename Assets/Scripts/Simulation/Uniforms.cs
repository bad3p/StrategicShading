#define ASSERTIVE_ENTITY_ACCESS
#define ASSERTIVE_COMPONENT_ACCESS
#define ASSERTIVE_FUNCTION_CALLS
#define DRAW_LINE_OF_SIGHT
#define DRAW_INDOOR_ENTITIES

using Types;
using Structs;
using UnityEngine;
using UnityEngine.Assertions;
using Transform = Structs.Transform;

public partial class ComputeShaderEmulator
{
    public const uint TRANSFORM = 0x00000001;
    public const uint HIERARCHY = 0x00000002;
    public const uint PERSONNEL = 0x00000004;
    public const uint FIREARMS = 0x00000008;
    public const uint MOVEMENT = 0x00000010;
    public const uint TARGETING = 0x00000020;
    public const uint BUILDING = 0x00000040;
    public const uint EVENT_AGGREGATOR = 0x00000080;
    public const uint TEAM_BITMASK = 0xC0000000;
    public const uint TEAM_SHIFT = 30;
    
    public const uint PERSONNEL_POSE_BITMASK = 0xC0000000;
    public const uint PERSONNEL_POSE_SHIFT = 30;
    public const uint PERSONNEL_POSE_STANDING = 3;
    public const uint PERSONNEL_POSE_CROUCHING = 2;
    public const uint PERSONNEL_POSE_LAYING = 1;
    public const uint PERSONNEL_POSE_HIDING = 0;
    public const uint MAX_PERSONNEL = 10;
    public const uint PERSONNEL_STATUS_BITMASK = 0x3;
    public const uint PERSONNEL_STATUS_HEALTHY = 3;
    public const uint PERSONNEL_STATUS_WOUNDED = 2;
    public const uint PERSONNEL_STATUS_KILLED = 1;
    public const uint PERSONNEL_STATUS_ABSENT = 0;
    public const uint SUPPRESSION_OKAY = 0;
    public const uint SUPPRESSION_SHAKEN = 1;
    public const uint SUPPRESSION_PINNED = 2;
    public const uint SUPPRESSION_PANIC = 3;
    public const uint SUPPRESSION_BROKEN = 4;

    public const float PERSONNEL_MORALE_MAX = 600.0f;
    public const float PERSONNEL_MORALE_MIN = 1.0f;
    
    public const float PERSONNEL_FITNESS_MAX = 14400.0f;
    public const float PERSONNEL_FITNESS_MIN = 1.0f;
    
    public const uint EVENT_FIREARM = 1;
    public const uint EVENT_LAST = 1;
    
    public const float FLOAT_EPSILON = 1.19e-07f;
    public const double DOUBLE_EPSILON = .22e-16;

    public const float FLOAT_PI = 3.1415927f;
    public const double DOUBLE_PI = 3.1415926535897931;

    public const float FLOAT_2PI = 2*3.1415927f;
    public const double DOUBLE_2PI = 2*3.1415926535897931;

    public static int _rngMax;
    public static int _rngCount;
    public static int _rngStateLength;
    public static int[] _rngState = new int[0];
    
    public static float _dT = 0.0f;
    public static uint _firearmDescCount = 0;
    public static FirearmDesc[] _firearmDescBuffer = new FirearmDesc[0];
    public static uint _personnelDescCount = 0;
    public static PersonnelDesc[] _personnelDescBuffer = new PersonnelDesc[0];
    public static uint _entityCount = 0;
    public static uint _selectedEntityId = 0;
    public static uint[] _descBuffer = new uint[0];
    public static Transform[] _transformBuffer = new Transform[0];
    public static Hierarchy[] _hierarchyBuffer = new Hierarchy[0];
    public static Personnel[] _personnelBuffer = new Personnel[0];
    public static Firearm[] _firearmBuffer = new Firearm[0];
    public static Movement[] _movementBuffer = new Movement[0];
    public static Targeting[] _targetingBuffer = new Targeting[0];
    public static EventAggregator[] _eventAggregatorBuffer = new EventAggregator[0];
    public static ArrayHeader[] _arrayHeaderBuffer = new ArrayHeader[0];
    public static FirearmEvent[] _firearmEventBuffer = new FirearmEvent[0];
    
    // MAP HELPERS
    
    public static float GetMapAltitude(double2 coordinate)
    {
        return 0.0f;
    }
    
    // COMPONENT MASKS
    
    public const uint TRANSFORM_MOVEMENT = TRANSFORM | MOVEMENT;
    public const uint HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT = HIERARCHY | TRANSFORM | PERSONNEL | MOVEMENT;
    public const uint HIERARCHY_TRANSFORM = HIERARCHY | TRANSFORM;
    public const uint TRANSFORM_TARGETING = TRANSFORM | TARGETING;
    public const uint TRANSFORM_BUILDING = TRANSFORM | BUILDING;
    
    public static bool HasComponents(uint entityId, uint mask)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        return ((_descBuffer[entityId] & mask) == mask);
    }

    public static uint GetTeam(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        return _descBuffer[entityId] >> (int)TEAM_SHIFT;
    }
    
    // HIERARCHY HELPERS

    public static bool IsChildHierarchy(uint entityId, uint childEntityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
            Debug.Assert(childEntityId > 0 && childEntityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY) == HIERARCHY);
            Debug.Assert((_descBuffer[childEntityId] & HIERARCHY) == HIERARCHY);
        #endif

        if (_hierarchyBuffer[entityId].firstChildEntityId == childEntityId)
        {
            return true;
        }
        else
        {
            uint nextSiblingEntityId = _hierarchyBuffer[entityId].firstChildEntityId;
            while (_hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0 && _hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId != childEntityId)
            {
                nextSiblingEntityId = _hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
            }
            return _hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId == childEntityId;
        }
    }
    
    public static void ConnectChildHierarchy(uint entityId, uint childEntityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
            Debug.Assert(childEntityId > 0 && childEntityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY) == HIERARCHY);
            Debug.Assert((_descBuffer[childEntityId] & HIERARCHY) == HIERARCHY);
        #endif
        #if ASSERTIVE_FUNCTION_CALLS
            Debug.Assert(_hierarchyBuffer[childEntityId].parentEntityId == 0);
            Debug.Assert(!IsChildHierarchy(entityId,childEntityId));
        #endif
        
        if (_hierarchyBuffer[entityId].firstChildEntityId == 0)
        {
            _hierarchyBuffer[entityId].firstChildEntityId = childEntityId;
            _hierarchyBuffer[childEntityId].parentEntityId = entityId;
        }
        else
        {
            if (_hierarchyBuffer[entityId].firstChildEntityId > childEntityId)
            {
                _hierarchyBuffer[childEntityId].parentEntityId = entityId;
                _hierarchyBuffer[childEntityId].nextSiblingEntityId = _hierarchyBuffer[entityId].firstChildEntityId;
                _hierarchyBuffer[entityId].firstChildEntityId = childEntityId;
            }
            else
            {
                uint lastSiblingEntityId = _hierarchyBuffer[entityId].firstChildEntityId;
                while (_hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId > 0 && 
                       _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId < childEntityId)
                {
                    lastSiblingEntityId = _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId;
                }
                
                _hierarchyBuffer[childEntityId].parentEntityId = entityId;
                _hierarchyBuffer[childEntityId].nextSiblingEntityId = _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId;
                _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId = childEntityId; 

                /*
                uint lastSiblingEntityId = _hierarchyBuffer[entityId].firstChildEntityId;
                while (_hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId > 0 )
                {
                    lastSiblingEntityId = _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId;
                }
                _hierarchyBuffer[lastSiblingEntityId].nextSiblingEntityId = childEntityId;
                _hierarchyBuffer[childEntityId].parentEntityId = entityId;
                */
            }
        }
    }

    public static void DisconnectChildHierarchy(uint entityId, uint childEntityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
            Debug.Assert(childEntityId > 0 && childEntityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY) == HIERARCHY);
            Debug.Assert((_descBuffer[childEntityId] & HIERARCHY) == HIERARCHY);
        #endif
        #if ASSERTIVE_FUNCTION_CALLS
            Debug.Assert(_hierarchyBuffer[childEntityId].parentEntityId == entityId);
            Debug.Assert(IsChildHierarchy(entityId,childEntityId));
        #endif
        
        if (_hierarchyBuffer[entityId].firstChildEntityId == childEntityId)
        {
            _hierarchyBuffer[entityId].firstChildEntityId = _hierarchyBuffer[childEntityId].nextSiblingEntityId;
            _hierarchyBuffer[childEntityId].parentEntityId = 0;
            _hierarchyBuffer[childEntityId].nextSiblingEntityId = 0;
        }
        else
        {
            uint prevSiblingEntityId = _hierarchyBuffer[entityId].firstChildEntityId;
            while (_hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId > 0 && _hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId != childEntityId)
            {
                prevSiblingEntityId = _hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId;
            }
            #if ASSERTIVE_FUNCTION_CALLS
                Debug.Assert( _hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId == childEntityId );
            #endif           
            if (_hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId == childEntityId)
            {
                _hierarchyBuffer[prevSiblingEntityId].nextSiblingEntityId = _hierarchyBuffer[childEntityId].nextSiblingEntityId;
                _hierarchyBuffer[childEntityId].parentEntityId = 0;
                _hierarchyBuffer[childEntityId].nextSiblingEntityId = 0;
            }
        }
    }
    
    // MOVEMENT HELPERS

    public static bool IsMoving(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        return abs(_movementBuffer[entityId].targetVelocityByDistance.y) > FLOAT_EPSILON ||
               abs(_movementBuffer[entityId].targetVelocityByDistance.w) > FLOAT_EPSILON;
    }

    public static bool IsMovementCompleted(uint entityId, float distanceThreshold)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        float3 delta = _transformBuffer[entityId].position - _movementBuffer[entityId].targetPosition;
        return dot(delta, delta) < distanceThreshold * distanceThreshold;
    }

    public static bool IsMovementCompleted(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        float3 delta = _transformBuffer[entityId].position - _movementBuffer[entityId].targetPosition;
        return dot(delta, delta) < dot(_transformBuffer[entityId].scale, _transformBuffer[entityId].scale);
    }
    
    public static void StartMovement(uint entityId, double3 targetPosition, float4 targetRotation, float4 targetVelocityByDistance)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        _movementBuffer[entityId].targetPosition = targetPosition;
        _movementBuffer[entityId].targetRotation = targetRotation;
        _movementBuffer[entityId].targetVelocityByDistance = targetVelocityByDistance;
    }
    
    public static void AdjustMovement(uint entityId, float distanceThreshold, double3 targetPosition, float4 targetRotation, float4 targetVelocityByDistance)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif

        double3 targetPositionError = _movementBuffer[entityId].targetPosition - targetPosition;
        if (length(targetPositionError) > distanceThreshold)
        {
            StartMovement( entityId, targetPosition, targetRotation, targetVelocityByDistance );
        }
    }
    
    public static void StopMovement(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        _movementBuffer[entityId].targetPosition = _transformBuffer[entityId].position;
        _movementBuffer[entityId].targetRotation = _transformBuffer[entityId].rotation;
        _movementBuffer[entityId].targetVelocityByDistance = new float4(0,0,0,0);
    }
    
    // PERSONNEL HELPERS

    public static uint GetPersonnelPose(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & PERSONNEL) == PERSONNEL);
        #endif
        return (_personnelBuffer[entityId].status & PERSONNEL_POSE_BITMASK) >> (int)PERSONNEL_POSE_SHIFT;
    }
    
    public static void SetPersonnelPose(uint entityId, uint pose)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT) == HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT);
        #endif

        pose = pose < PERSONNEL_POSE_HIDING ? PERSONNEL_POSE_HIDING : pose;
        pose = pose > PERSONNEL_POSE_STANDING ? PERSONNEL_POSE_STANDING : pose;

        uint prevPose = GetPersonnelPose(entityId);
        if (prevPose != pose)
        {
            uint status = _personnelBuffer[entityId].status;
            status &= ~PERSONNEL_POSE_BITMASK;
            status |= pose << (int)PERSONNEL_POSE_SHIFT;

            _personnelBuffer[entityId].status = status;

            if (pose == PERSONNEL_POSE_STANDING)
            {
                _transformBuffer[entityId].scale.y = 1.8f;
            }
            else if (pose == PERSONNEL_POSE_CROUCHING)
            {
                _transformBuffer[entityId].scale.y = 1.2f;
            }
            else
            {
                _transformBuffer[entityId].scale.y = 0.4f;
            }
        }
    }

    public static uint GetPersonnelSuppression(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & PERSONNEL) == PERSONNEL);
        #endif
        uint descId = _personnelBuffer[entityId].descId;
        float4 moraleThreshold = _personnelDescBuffer[descId].moraleThreshold;
        float morale = _personnelBuffer[entityId].morale;
        if (morale > moraleThreshold.x)
        {
            return SUPPRESSION_OKAY;
        }
        else if (morale > moraleThreshold.y)
        {
            return SUPPRESSION_SHAKEN;
        }
        else if (morale > moraleThreshold.z)
        {
            return SUPPRESSION_PINNED;
        }
        else if (morale > moraleThreshold.w)
        {
            return SUPPRESSION_PANIC;
        }
        else
        {
            return SUPPRESSION_BROKEN;
        }
    }

    public static float GetPersonnelTargetVelocity(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        #if ASSERTIVE_FUNCTION_CALLS
            Debug.Assert(IsMoving(entityId));
        #endif
            
        uint personnelPose = GetPersonnelPose(entityId);
        if (personnelPose == PERSONNEL_POSE_HIDING)
        {
            return 0.0f;
        }
        else
        {
            float3 directionToTarget = _movementBuffer[entityId].targetPosition - _transformBuffer[entityId].position;
            float distanceToTarget = length(directionToTarget);
        
            uint descId = _personnelBuffer[entityId].descId;
            float3 linearVelocitySlow = _personnelDescBuffer[descId].linearVelocitySlow;
            float3 linearVelocityFast = _personnelDescBuffer[descId].linearVelocityFast;

            float normalizedTargetVelocity = lerpargs(_movementBuffer[entityId].targetVelocityByDistance, distanceToTarget);
            normalizedTargetVelocity = saturate(normalizedTargetVelocity);

            float2 targetVelocity = new float2(0.0f, 0.0f); 
            if (personnelPose == PERSONNEL_POSE_LAYING)
            {
                targetVelocity.x = linearVelocitySlow.x;
                targetVelocity.y = linearVelocityFast.x;
            }
            else if (personnelPose == PERSONNEL_POSE_CROUCHING)
            {
                targetVelocity.x = linearVelocitySlow.y;
                targetVelocity.y = linearVelocityFast.y;                
            }
            else
            {
                targetVelocity.x = linearVelocitySlow.z;
                targetVelocity.y = linearVelocityFast.z;                
            }

            if (normalizedTargetVelocity < 0.5f)
            {
                return targetVelocity.x * normalizedTargetVelocity * 2.0f;
            }
            else
            {
                return lerp(targetVelocity.x, targetVelocity.y, (normalizedTargetVelocity - 0.5f) * 2.0f);
            }
        }
    }

    public static float GetPersonnelFitnessConsumptionRate(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        
        uint personnelPose = GetPersonnelPose(entityId);
        if (personnelPose == PERSONNEL_POSE_HIDING)
        {
            return 0.0f;
        }
        else
        {
            uint descId = _personnelBuffer[entityId].descId;
            float3 fitnessConsumptionRateSlow = _personnelDescBuffer[descId].fitnessConsumptionRateSlow;
            float3 fitnessConsumptionRateFast = _personnelDescBuffer[descId].fitnessConsumptionRateFast;
            float3 linearVelocitySlow = _personnelDescBuffer[descId].linearVelocitySlow;
            float3 linearVelocityFast = _personnelDescBuffer[descId].linearVelocityFast;
            
            float3 deltaPosition = _movementBuffer[entityId].deltaPosition;
            float movementDistance = length( deltaPosition );
            float movementVelocity = movementDistance / _dT;

            float normalizedMovementVelocity = 0.0f;
            float2 fitnessConsumptionRate = new float2(0.0f, 0.0f); 
            if (personnelPose == PERSONNEL_POSE_LAYING)
            {
                if (movementVelocity < linearVelocitySlow.x)
                {
                    normalizedMovementVelocity = movementVelocity / linearVelocitySlow.x * 0.5f;
                }
                else
                {
                    normalizedMovementVelocity = (movementVelocity - linearVelocitySlow.x)/ (linearVelocityFast.x - linearVelocitySlow.x) * 0.5f + 0.5f;
                }
                fitnessConsumptionRate.x = fitnessConsumptionRateSlow.x;
                fitnessConsumptionRate.y = fitnessConsumptionRateFast.x;
            }
            else if (personnelPose == PERSONNEL_POSE_CROUCHING)
            {
                if (movementVelocity < linearVelocitySlow.y)
                {
                    normalizedMovementVelocity = movementVelocity / linearVelocitySlow.y * 0.5f;
                }
                else
                {
                    normalizedMovementVelocity = (movementVelocity - linearVelocitySlow.y)/ (linearVelocityFast.y - linearVelocitySlow.y) * 0.5f + 0.5f;
                }
                fitnessConsumptionRate.x = fitnessConsumptionRateSlow.y;
                fitnessConsumptionRate.y = fitnessConsumptionRateFast.y;                
            }
            else
            {
                if (movementVelocity < linearVelocitySlow.z)
                {
                    normalizedMovementVelocity = movementVelocity / linearVelocitySlow.z * 0.5f;
                }
                else
                {
                    normalizedMovementVelocity = (movementVelocity - linearVelocitySlow.z)/ (linearVelocityFast.z - linearVelocitySlow.z) * 0.5f + 0.5f;
                }
                fitnessConsumptionRate.x = fitnessConsumptionRateSlow.z;
                fitnessConsumptionRate.y = fitnessConsumptionRateFast.z;                
            }

            if (normalizedMovementVelocity < 0.5f)
            {
                return fitnessConsumptionRate.x * normalizedMovementVelocity * 2.0f;
            }
            else
            {
                return lerp(fitnessConsumptionRate.x, fitnessConsumptionRate.y, (normalizedMovementVelocity - 0.5f) * 2.0f);
            }
        }
    }

    public static float GetPersonnelMoraleRecoveryRate(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM_MOVEMENT) == TRANSFORM_MOVEMENT);
        #endif
        
        uint personnelDescId = _personnelBuffer[entityId].descId;
        float4 moraleRecoveryRate = _personnelDescBuffer[personnelDescId].moraleRecoveryRate;
        float4 moraleThreshold = _personnelDescBuffer[personnelDescId].moraleThreshold;
        float morale = _personnelBuffer[entityId].morale;

        if (morale > moraleThreshold.x)
        {
            return moraleRecoveryRate.x;
        }
        else if (morale > moraleThreshold.y)
        {
            float t = (morale - moraleThreshold.y) / (moraleThreshold.x - moraleThreshold.y);
            return lerp(moraleRecoveryRate.x, moraleRecoveryRate.y, t);
        }
        else if (morale > moraleThreshold.z)
        {
            float t = (morale - moraleThreshold.z) / (moraleThreshold.y - moraleThreshold.z);
            return lerp(moraleRecoveryRate.y, moraleRecoveryRate.z, t);
        }
        else if (morale > moraleThreshold.w)
        {
            float t = (morale - moraleThreshold.w) / (moraleThreshold.z - moraleThreshold.w);
            return lerp(moraleRecoveryRate.z, moraleRecoveryRate.w, t);
        }
        else
        {
            return moraleRecoveryRate.w;
        }
    }
    
    // TARGETING HELPERS

    public static void InsertTarget(ref uint4 ids, ref float4 weights, uint id, float weight)
    {
        if (ids.x == 0 || ids.x > 0 && weights.x > weight)
        {
            ids.yzw = ids.xyz;
            weights.yzw = weights.xyz;
            ids.x = id;
            weights.x = weight;
        }
        else if( ids.y == 0 || ids.y > 0 && weights.y > weight )
        {
            ids.zw = ids.yz;
            weights.zw = weights.yz;
            ids.y = id;
            weights.y = weight;
        }
        else if( ids.z == 0 || ids.z > 0 && weights.z > weight )
        {
            ids.w = ids.z;
            weights.w = weights.z;
            ids.z = id;
            weights.z = weight;
        }
        else if (ids.w == 0 || ids.w > 0 && weights.w > weight)
        {
            ids.w = id;
            weights.w = weight;
        }
    }
    
    public static uint GetIndoorEntityId(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & TRANSFORM) == TRANSFORM);
        #endif
        
        double3 point = _transformBuffer[entityId].position;

        for (uint structureEntityId = 1; structureEntityId < _entityCount; structureEntityId++)
        {
            uint team = GetTeam(structureEntityId);
            if (team == 0)
            {
                if (HasComponents(structureEntityId, TRANSFORM_BUILDING))
                {
                    double3 structureBoxPosition = _transformBuffer[structureEntityId].position;
                    float4 structureBoxRotation = _transformBuffer[structureEntityId].rotation;
                    float3 structureBoxScale = _transformBuffer[structureEntityId].scale;
                    
                    float4 invStructureBoxRotation = quaternionInverse(structureBoxRotation);
                    
                    float3 localPoint = point - structureBoxPosition;
                    localPoint = rotate(localPoint, invStructureBoxRotation);

                    float3 structureBoxSup = structureBoxScale * 0.5f;
                    float3 structureBoxInf = -structureBoxSup;

                    if (PointInsideAABB(localPoint, structureBoxInf, structureBoxSup))
                    {
                        #if DRAW_INDOOR_ENTITIES
                        if (entityId == _selectedEntityId && NumCPUThreads == 1)
                        {
                            uint entityTeam = GetTeam(entityId);
                            Color entityColor = (entityTeam == 1) ? (Color.red) : (entityTeam == 2 ? Color.blue : Color.green);
                            Vector3 c = structureBoxPosition.ToVector3();
                            structureBoxSup += new float3(0.1f, 0.1f, 0.1f);
                            structureBoxInf -= new float3(0.1f, 0.1f, 0.1f);
                            Vector3 v0 = new Vector3(structureBoxInf.x, structureBoxInf.y, structureBoxInf.z);
                            Vector3 v1 = new Vector3(structureBoxInf.x, structureBoxInf.y, structureBoxSup.z);
                            Vector3 v2 = new Vector3(structureBoxSup.x, structureBoxInf.y, structureBoxSup.z);
                            Vector3 v3 = new Vector3(structureBoxSup.x, structureBoxInf.y, structureBoxInf.z);
                            Vector3 v4 = new Vector3(structureBoxInf.x, structureBoxSup.y, structureBoxInf.z);
                            Vector3 v5 = new Vector3(structureBoxInf.x, structureBoxSup.y, structureBoxSup.z);
                            Vector3 v6 = new Vector3(structureBoxSup.x, structureBoxSup.y, structureBoxSup.z);
                            Vector3 v7 = new Vector3(structureBoxSup.x, structureBoxSup.y, structureBoxInf.z);
                            v0 = c + rotate(v0, structureBoxRotation).ToVector3();
                            v1 = c + rotate(v1, structureBoxRotation).ToVector3();
                            v2 = c + rotate(v2, structureBoxRotation).ToVector3();
                            v3 = c + rotate(v3, structureBoxRotation).ToVector3();
                            v4 = c + rotate(v4, structureBoxRotation).ToVector3();
                            v5 = c + rotate(v5, structureBoxRotation).ToVector3();
                            v6 = c + rotate(v6, structureBoxRotation).ToVector3();
                            v7 = c + rotate(v7, structureBoxRotation).ToVector3();
                            Debug.DrawLine( v0, v1, entityColor );
                            Debug.DrawLine( v1, v2, entityColor );
                            Debug.DrawLine( v2, v3, entityColor );
                            Debug.DrawLine( v3, v0, entityColor );
                            Debug.DrawLine( v4, v5, entityColor );
                            Debug.DrawLine( v5, v6, entityColor );
                            Debug.DrawLine( v6, v7, entityColor );
                            Debug.DrawLine( v7, v4, entityColor );
                            Debug.DrawLine( v0, v4, entityColor );
                            Debug.DrawLine( v1, v5, entityColor );
                            Debug.DrawLine( v2, v6, entityColor );
                            Debug.DrawLine( v3, v7, entityColor );
                        }
                        #endif
                        return structureEntityId;
                    }
                }
            }
        }

        return 0;
    }

    public static bool GetLineOfSight(uint entityId, uint otherEntityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY_TRANSFORM) == HIERARCHY_TRANSFORM);
            Debug.Assert((_descBuffer[otherEntityId] & HIERARCHY_TRANSFORM) == HIERARCHY_TRANSFORM);
        #endif

        uint indoorEntityId = _hierarchyBuffer[entityId].indoorEntityId;
        uint otherIndoorEntityId = _hierarchyBuffer[otherEntityId].indoorEntityId;
        
        double3 rayStart = _transformBuffer[entityId].position;
        double3 rayEnd = _transformBuffer[otherEntityId].position;
        float3 rayDir = rayEnd - rayStart;
        float rayLength = length(rayDir);
        if (rayLength > FLOAT_EPSILON)
        {
            rayDir = rayDir * 1.0f / rayLength;
        }
        float rayLengthSquared = rayLength * rayLength; 

        float3 localHit = new float3(0,0,0);
        double3 hit = new double3(0,0,0);

        for (uint obstacleEntityId = 1; obstacleEntityId < _entityCount; obstacleEntityId++)
        {
            if (obstacleEntityId != indoorEntityId && obstacleEntityId != otherIndoorEntityId)
            {
                uint team = GetTeam(obstacleEntityId);
                if (team == 0)
                {
                    if (HasComponents(obstacleEntityId, TRANSFORM_BUILDING))
                    {
                        double3 obstacleBoxPosition = _transformBuffer[obstacleEntityId].position;
                        float4 obstacleBoxRotation = _transformBuffer[obstacleEntityId].rotation;
                        float3 obstacleBoxScale = _transformBuffer[obstacleEntityId].scale;

                        float4 invObstacleBoxRotation = quaternionInverse(obstacleBoxRotation);

                        float3 localRayStart = rayStart - obstacleBoxPosition;
                        localRayStart = rotate(localRayStart, invObstacleBoxRotation);
                        float3 localRayDir = rotate(rayDir, invObstacleBoxRotation);

                        float3 obstacleBoxSup = obstacleBoxScale * 0.5f;
                        float3 obstacleBoxInf = -obstacleBoxSup;

                        if (CollideRayAABB(localRayStart, localRayDir, obstacleBoxInf, obstacleBoxSup, out localHit))
                        {
                            float3 dirToHit = localHit - localRayStart;
                            float distToHitSquared = dot(dirToHit, dirToHit);
                            if (distToHitSquared < rayLengthSquared)
                            {
                                localHit = rotate(localHit, obstacleBoxRotation);
                                hit = localHit;
                                hit += obstacleBoxPosition;
                                #if DRAW_LINE_OF_SIGHT
                                    if (entityId == _selectedEntityId && NumCPUThreads == 1)
                                    {
                                        Color blockedSightColor = Color.red;
                                        blockedSightColor.a = 0.5f;
                                        Debug.DrawLine(rayStart.ToVector3(), hit.ToVector3(), blockedSightColor);
                                    }
                                #endif
                                return false;
                            }
                        }
                    }
                }
            }
        }
        
        #if DRAW_LINE_OF_SIGHT
            if (entityId == _selectedEntityId && NumCPUThreads == 1)
            {
                Color clearSightColor = Color.green;
                clearSightColor.a = 0.5f;
                Debug.DrawLine(rayStart.ToVector3(), rayEnd.ToVector3(), clearSightColor);
            }
        #endif
        return true;
    }

    public static float GetExposure(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT) == HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT);
        #endif

        // normalized exposure [0.0...1.0]
        float exposure = 0.0f;
        
        // exposure by pose
        uint pose = GetPersonnelPose(entityId);
        if (pose == PERSONNEL_POSE_HIDING )
        {
            exposure = 0.0f;
        }
        else if (pose == PERSONNEL_POSE_LAYING)
        {
            exposure = 0.1f;
        }
        else if (pose == PERSONNEL_POSE_CROUCHING)
        {
            exposure = 0.5f;
        }
        else
        {
            exposure = 1.0f;
        }
        
        // TODO: temporary expose when moving (?)
        
        // TODO: temporary expose when open fire
        
        // TODO: reduce exposure when using terrain features like bushes or forests  

        return exposure;
    }
    
    // EVENT AGGREGATOR HELPERS
    
    public static void AddFirearmEvent(uint entityId, uint targetEntityId, float firepower)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
            Debug.Assert(targetEntityId > 0 && targetEntityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[targetEntityId] & EVENT_AGGREGATOR) == EVENT_AGGREGATOR);
        #endif

        if (_arrayHeaderBuffer[EVENT_FIREARM].count < _arrayHeaderBuffer[EVENT_FIREARM].capacity)
        {
            int eventIndex = -1;
            InterlockedAdd(ref _arrayHeaderBuffer[EVENT_FIREARM].count, 1, out eventIndex);
            _firearmEventBuffer[eventIndex].entityId = entityId;
            _firearmEventBuffer[eventIndex].firepower = firepower;

            // try to put event to the head of the list
            int lastEventIndex = _eventAggregatorBuffer[targetEntityId].firearmEventIndex;
            int originalLastEventIndex = lastEventIndex;
            if (lastEventIndex == 0)
            {
                InterlockedCompareExchange(ref _eventAggregatorBuffer[targetEntityId].firearmEventIndex, lastEventIndex, eventIndex, out originalLastEventIndex);
            }

            if (lastEventIndex != originalLastEventIndex)
            {
                Debug.Log("AddFirearmEvent(" + entityId + ", " + targetEntityId + ") failed to add even to the head of the list.");
            }
            
            // put event to the tail of the list
            while (lastEventIndex != originalLastEventIndex)
            {
                // search for the tail
                lastEventIndex = originalLastEventIndex;
                while (_firearmEventBuffer[lastEventIndex].nextIndex != 0)
                {
                    lastEventIndex = _firearmEventBuffer[lastEventIndex].nextIndex;
                }
                    
                // try to put event to the tail of the list
                originalLastEventIndex = lastEventIndex;
                InterlockedCompareExchange(ref _firearmEventBuffer[lastEventIndex].nextIndex, lastEventIndex, eventIndex, out originalLastEventIndex);
                
                if (lastEventIndex != originalLastEventIndex)
                {
                    Debug.Log("AddFirearmEvent(" + entityId + ", " + targetEntityId + ") failed to add even to the tail of the list.");
                }
            }

            int currentEventCount = 0;
            InterlockedAdd(ref _eventAggregatorBuffer[targetEntityId].eventCount, 1, out currentEventCount);
        }
    }
}
