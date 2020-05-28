#define ASSERTIVE_ENTITY_ACCESS
#define ASSERTIVE_COMPONENT_ACCESS
#define ASSERTIVE_FUNCTION_CALLS

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
    public const uint FIREPOWER = 0x00000020;
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
    
    public const float FLOAT_EPSILON = 1.19e-07f;
    public const double DOUBLE_EPSILON = .22e-16;

    public const float FLOAT_PI = 3.1415927f;
    public const double DOUBLE_PI = 3.1415926535897931;

    public const float FLOAT_2PI = 2*3.1415927f;
    public const double DOUBLE_2PI = 2*3.1415926535897931;

    public static uint _rngMax;
    public static uint _rngCount;
    public static uint _rngStateLength;
    public static uint[] _rngState = new uint[0];
    
    public static float _dT = 0.0f;
    public static uint _firearmDescCount = 0;
    public static FirearmDesc[] _firearmDescBuffer = new FirearmDesc[0];
    public static uint _personnelDescCount = 0;
    public static PersonnelDesc[] _personnelDescBuffer = new PersonnelDesc[0];
    public static uint _entityCount = 0;
    public static uint[] _descBuffer = new uint[0];
    public static Transform[] _transformBuffer = new Transform[0];
    public static Hierarchy[] _hierarchyBuffer = new Hierarchy[0];
    public static Personnel[] _personnelBuffer = new Personnel[0];
    public static Firearm[] FirearmBuffer = new Firearm[0];
    public static Movement[] _movementBuffer = new Movement[0];
    public static Firepower[] _firepowerBuffer = new Firepower[0];
    
    // COMPONENT MASKS
    
    public const uint MOVABLE_MASK = TRANSFORM | MOVEMENT;
    public const uint MOVABLE_PERSONNEL_MASK = TRANSFORM | MOVEMENT | PERSONNEL;
    public const uint HIERARCHY_PERSONNEL_MASK = HIERARCHY | PERSONNEL; 
    
    public static bool HasComponents(uint entityId, uint mask)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        return ((_descBuffer[entityId] & mask) == mask);
    }
    
    // MOVEMENT HELPERS

    public static bool IsMoving(uint entityId)
    {
        #if ASSERTIVE_ENTITY_ACCESS
            Debug.Assert(entityId > 0 && entityId < _entityCount);
        #endif
        #if ASSERTIVE_COMPONENT_ACCESS
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
            Debug.Assert((_descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK);
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
}
