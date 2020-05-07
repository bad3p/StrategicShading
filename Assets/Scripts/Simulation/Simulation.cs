using Types;
using Structs;
using UnityEngine;
using Transform = Structs.Transform;

public partial class ComputeShaderEmulator
{
    public static int _outBufferSizeX;
    public static int _outBufferSizeY;
    public static int _outBufferSizeZ;
    public static RWStructuredBuffer<int3> _outBuffer;

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
    
    public static int _outRenderTextureWidth;
    public static int _outRenderTextureHeight;
    public static RWTexture2D<float> _outRenderTexture;
    
    [NumThreads(256,1,1)]
    static public void GenerateRandomNumbers(uint3 id)
    {
        int index = (int)(id.x);
        if( index < _outRenderTextureWidth * _outRenderTextureHeight )    
        {
            int y = index / _outRenderTextureWidth;
            int x = index - y * _outRenderTextureWidth;
            int2 xy = new int2( x, y );
            _outRenderTexture[xy] = rngNormal(rngIndex(index));
        }
    }

    public static float _dT = 0.0f;
    
    public static int _entityCount = 0;
    public static int _transformCount = 0;
    public static int _hierarchyCount = 0;
    public static int _personnelCount = 0;
    public static int _firearmsCount = 0;
    public static int _movementCount = 0;
    public static int _firepowerCount = 0;

    public static RWStructuredBuffer<Entity> _entityBuffer = new RWStructuredBuffer<Entity>();
    public static RWStructuredBuffer<Transform> _transformBuffer = new RWStructuredBuffer<Transform>();
    public static RWStructuredBuffer<Hierarchy> _hierarchyBuffer = new RWStructuredBuffer<Hierarchy>();
    public static RWStructuredBuffer<Personnel> _personnelBuffer = new RWStructuredBuffer<Personnel>();
    public static RWStructuredBuffer<Firearms> _firearmsBuffer = new RWStructuredBuffer<Firearms>();
    public static RWStructuredBuffer<Movement> _movementBuffer = new RWStructuredBuffer<Movement>();
    public static RWStructuredBuffer<Firepower> _firepowerBuffer = new RWStructuredBuffer<Firepower>();
    
    [NumThreads(256,1,1)]
    static public void UpdateMovement(uint3 id)
    {
        int index = (int)id.x;
        if( index < _movementCount )
        {
            if (_movementBuffer[index].targetVelocity > 0)
            {
                double2 targetPosition;
                targetPosition.x = _movementBuffer[index].targetPosition.x;
                targetPosition.y = _movementBuffer[index].targetPosition.z;
                
                double2 currentPosition;
                currentPosition.x = _transformBuffer[index].position.x;
                currentPosition.y = _transformBuffer[index].position.z;
                
                float2 targetDir = targetPosition - currentPosition;
                float targetDist = length( targetDir );
                if (targetDist > FLOAT_EPSILON)
                {
                    targetDir *= 1.0f / targetDist;
                }

                float3 transformForward = new float3(0, 0, 1);
                transformForward = rotate(transformForward, _transformBuffer[index].rotation);
                Debug.DrawLine( _transformBuffer[index].position.ToVector3(), _transformBuffer[index].position.ToVector3() + new float3(targetDir.x,0,targetDir.y).ToVector3().normalized * 10, Color.green );
                Debug.DrawLine( _transformBuffer[index].position.ToVector3(), _transformBuffer[index].position.ToVector3() + transformForward.ToVector3() * 10, Color.red );

                float2 currentDir;
                currentDir.x = transformForward.x;
                currentDir.y = transformForward.z;
                float currentDirMagnitude = length(currentDir);
                if (currentDirMagnitude > FLOAT_EPSILON)
                {
                    currentDir *= 1.0f / currentDirMagnitude;
                }

                float currentAngle = sigangle(currentDir, targetDir);
                float targetAngularVelocity = radians(5.0f); // TODO: configure
                float deltaAngle = -sign(currentAngle) * targetAngularVelocity * _dT;
                if (abs(deltaAngle) > abs(currentAngle))
                {
                    deltaAngle = -currentAngle;
                }

                float4 args = new float4
                (
                    radians(0.0f),
                    _movementBuffer[index].targetVelocity,
                    radians(30.0f),
                    0.0f
                );

                float currentVelocity = lerpargs(args, abs(currentAngle));

                float deltaDist = currentVelocity * _dT;
                if (abs(deltaDist) > targetDist)
                {
                    deltaDist = targetDist;
                }
                
                Transform temp = _transformBuffer[index];
                temp.position += new double3( targetDir.x, 0, targetDir.y ) * deltaDist;

                float4 deltaRotation = quaternionFromAsixAngle(deltaAngle, new float3(0, 1, 0));
                temp.rotation = transformQuaternion(deltaRotation, temp.rotation);

                _transformBuffer[index] = temp;
            }
        }
    }
}
