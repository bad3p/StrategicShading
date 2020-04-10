using Types;
using Structs;

public partial class ComputeShaderEmulator
{
    public static int _outBufferSizeX;
    public static int _outBufferSizeY;
    public static int _outBufferSizeZ;
    public static RWStructuredBuffer<int3> _outBuffer;

    [NumThreads(8,8,8)] 
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
}
