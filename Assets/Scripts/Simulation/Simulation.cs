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
    public static int _entityCount = 0;
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
        int entityId = (int)id.x;
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

            float3 transformForward = new float3(0, 0, 1);
            transformForward = rotate(transformForward, _transformBuffer[entityId].rotation);
            Debug.DrawLine( _transformBuffer[entityId].position.ToVector3(), _transformBuffer[entityId].position.ToVector3() + new float3(targetDir.x,0,targetDir.y).ToVector3().normalized * 10, Color.green );
            Debug.DrawLine( _transformBuffer[entityId].position.ToVector3(), _transformBuffer[entityId].position.ToVector3() + transformForward.ToVector3() * 10, Color.red );

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
            
            Transform tempTransform = _transformBuffer[entityId];
            tempTransform.position += new double3( targetDir.x, 0, targetDir.y ) * deltaDist;

            float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, new float3(0, 1, 0));
            tempTransform.rotation = transformQuaternion(deltaRotation, tempTransform.rotation);

            _transformBuffer[entityId] = tempTransform;
            
            if( stop )
            {
                Movement tempMovement = _movementBuffer[entityId];
                tempMovement.targetVelocity = 0;
                _movementBuffer[entityId] = tempMovement;
            }
            
            // TODO: configure
            float4 fitnessByVelocity = new float4
            (
                1.4f,
                1.0f,
                4.17f,
                8.0f
            );
            
            float dFitnessByDt = lerpargs(fitnessByVelocity, abs(currentVelocity));
            float dFitness = dFitnessByDt * _dT;

            Personnel tempPersonnel = _personnelBuffer[entityId];
            if (dFitness > tempPersonnel.fitness)
            {
                dFitness = tempPersonnel.fitness;
            }
            tempPersonnel.fitness -= dFitness;
            _personnelBuffer[entityId] = tempPersonnel;
        }
        else if (_movementBuffer[entityId].targetAngularVelocity > 0)
        {
            // rotate to target
            
            float3 targetForward = new float3(0, 0, 1);
            targetForward = rotate(targetForward, _movementBuffer[entityId].targetRotation);
            
            float3 currentForward = new float3(0, 0, 1);
            currentForward = rotate(currentForward, _transformBuffer[entityId].rotation);
            
            Debug.DrawLine( _transformBuffer[entityId].position.ToVector3(), _transformBuffer[entityId].position.ToVector3() + targetForward.ToVector3() * 10, Color.yellow );
            Debug.DrawLine( _transformBuffer[entityId].position.ToVector3(), _transformBuffer[entityId].position.ToVector3() + currentForward.ToVector3() * 10, Color.red );

            float3 axis = normalize(cross(currentForward, targetForward));

            float currentAngle = sigangle(currentForward, targetForward, axis);

            bool stop = false;
            float deltaAngle = sign(currentAngle) * _movementBuffer[entityId].targetAngularVelocity * _dT;
            if (abs(deltaAngle) > abs(currentAngle))
            {
                deltaAngle = currentAngle;
                stop = true;
            }
            
            Transform tempTransform = _transformBuffer[entityId];

            float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, axis);
            tempTransform.rotation = transformQuaternion(deltaRotation, tempTransform.rotation);

            _transformBuffer[entityId] = tempTransform;
            
            if( stop )
            {
                Movement tempMovement = _movementBuffer[entityId];
                tempMovement.targetAngularVelocity = 0;
                _movementBuffer[entityId] = tempMovement;
            }
        }
    }
}
