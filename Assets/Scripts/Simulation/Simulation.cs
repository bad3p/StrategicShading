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
    static public void UpdateMovement(uint3 id)
    {
        uint entityId = id.x;
        if (entityId == 0 || entityId >= _entityCount)
        {
            return;
        }
        
        if ( !HasComponents( entityId, MOVABLE_PERSONNEL_MASK ) )
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
                StopMovement(entityId);
            }
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

        if ( !HasComponents( entityId, MOVABLE_PERSONNEL_MASK ) )
        {
            return;
        }
        
        // TEST MORALE LOSS

        float morale = _personnelBuffer[entityId].morale; 

        float4 moraleLossProbabilityByMorale = new float4
        (
            400.0f,
            1.0f / 3300.0f,
            600.0f,
            1.0f / 330.0f
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
        
        // FITNESS

        float fitnessConsumptionRate = GetPersonnelFitnessConsumptionRate(entityId);

        if (fitnessConsumptionRate > FLOAT_EPSILON)
        {
            float fitness = _personnelBuffer[entityId].fitness;
            _personnelBuffer[entityId].fitness = clamp(fitness + fitnessConsumptionRate * _dT, PERSONNEL_FITNESS_MIN, PERSONNEL_FITNESS_MAX);
        }
    }
}
