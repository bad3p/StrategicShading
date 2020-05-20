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

    public static float _dT = 0.0f;
    public static uint _entityCount = 0;
    public static uint[] _descBuffer = new uint[0];
    public static Transform[] _transformBuffer = new Transform[0];
    public static Hierarchy[] _hierarchyBuffer = new Hierarchy[0];
    public static Personnel[] _personnelBuffer = new Personnel[0];
    public static Firearms[] _firearmsBuffer = new Firearms[0];
    public static Movement[] _movementBuffer = new Movement[0];
    public static Firepower[] _firepowerBuffer = new Firepower[0];
    
    [NumThreads(256,1,1)]
    static public void UpdateMovement(uint3 id)
    {
        uint entityId = id.x;
        if (entityId >= _entityCount)
        {
            return;
        }

        const uint COMPONENT_MASK = TRANSFORM | MOVEMENT | PERSONNEL; 

        if ( (_descBuffer[entityId] & COMPONENT_MASK) != COMPONENT_MASK )
        {
            return;
        }

        if (_movementBuffer[entityId].targetVelocity > 0)
        {
            double2 targetPosition = _movementBuffer[entityId].targetPosition.xz;
            double2 currentPosition = _transformBuffer[entityId].position.xz;
            
            float2 targetDir = targetPosition - currentPosition;
            float targetDist = length( targetDir );
            if (targetDist > FLOAT_EPSILON)
            {
                targetDir *= 1.0f / targetDist;
            }

            // TODO : move away from this system
            const float PinnedMoraleThreshold = 400.0f;
            const float RoutedMoraleThreshold = 300.0f;
            if (_personnelBuffer[entityId].morale < RoutedMoraleThreshold)
            {
                targetDir *= -1;
            }
            else if (_personnelBuffer[entityId].morale < PinnedMoraleThreshold)
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

            float currentAngle = sigangle(currentDir, targetDir);
            float targetAngularVelocity = radians(45.0f); // TODO: configure
            float deltaAngle = -sign(currentAngle) * targetAngularVelocity * _dT;
            if (abs(deltaAngle) > abs(currentAngle))
            {
                deltaAngle = -currentAngle;
            }

            // TODO: configure
            float4 velocityByAngle = new float4
            (
                radians(0.0f),
                _movementBuffer[entityId].targetVelocity,
                radians(45.0f),
                0.0f
            );

            float currentVelocity = lerpargs(velocityByAngle, abs(currentAngle));

            bool stop = false;
            float deltaDist = currentVelocity * _dT;
            if (abs(deltaDist) >= targetDist)
            {
                deltaDist = targetDist;
                stop = true;
            }
            
            _transformBuffer[entityId].position += new double3( targetDir.x, 0, targetDir.y ) * deltaDist;
            float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, new float3(0, 1, 0));
            _transformBuffer[entityId].rotation = transformQuaternion(deltaRotation, _transformBuffer[entityId].rotation);
            
            if( stop )
            {
                _movementBuffer[entityId].targetVelocity = 0;
            }
        }
        else if (_movementBuffer[entityId].targetAngularVelocity > 0)
        {
            // rotate to target
            
            float3 targetForward = new float3(0, 0, 1);
            targetForward = rotate(targetForward, _movementBuffer[entityId].targetRotation);
            
            float3 currentForward = new float3(0, 0, 1);
            currentForward = rotate(currentForward, _transformBuffer[entityId].rotation);

            float3 axis = normalize(cross(currentForward, targetForward));

            float currentAngle = sigangle(currentForward, targetForward, axis);

            bool stop = false;
            float deltaAngle = sign(currentAngle) * _movementBuffer[entityId].targetAngularVelocity * _dT;
            if (abs(deltaAngle) > abs(currentAngle))
            {
                deltaAngle = currentAngle;
                stop = true;
            }

            float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, axis);
            _transformBuffer[entityId].rotation = transformQuaternion(deltaRotation, _transformBuffer[entityId].rotation);
            
            if( stop )
            {
                _movementBuffer[entityId].targetAngularVelocity = 0;
            }
        }
    }

    [NumThreads(256, 1, 1)]
    static public void UpdatePersonnel(uint3 id)
    {
        uint entityId = id.x;
        if (entityId >= _entityCount)
        {
            return;
        }

        const uint COMPONENT_MASK = TRANSFORM | MOVEMENT | PERSONNEL;

        if ((_descBuffer[entityId] & COMPONENT_MASK) != COMPONENT_MASK)
        {
            return;
        }
        
        // MORALE

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
        
        // TODO: configure
        float4 moraleRecoveryRateByMorale = new float4
        (
            200.0f,
            5.0f,
            400.0f,
            1.0f
        );
        float moraleRecoveryRate = lerpargs(moraleRecoveryRateByMorale, morale);
        float moraleGain = moraleRecoveryRate * _dT;

        // TODO: configure
        const float MinMorale = 0.0f;
        const float MaxMorale = 600.0f;
        
        _personnelBuffer[entityId].morale = clamp(morale - moraleLoss + moraleGain, MinMorale, MaxMorale);
        
        // FITNESS

        if (abs(_movementBuffer[entityId].targetVelocity) > FLOAT_EPSILON)
        {
            // TODO: configure
            float4 fitnessByVelocity = new float4
            (
                1.4f,
                1.0f,
                4.17f,
                8.0f
            );

            float dFitnessByDt = lerpargs(fitnessByVelocity, abs(_movementBuffer[entityId].targetVelocity));
            float dFitness = dFitnessByDt * _dT;

            if (dFitness > _personnelBuffer[entityId].fitness)
            {
                dFitness = _personnelBuffer[entityId].fitness;
            }

            _personnelBuffer[entityId].fitness -= dFitness;
        }
    }
}
