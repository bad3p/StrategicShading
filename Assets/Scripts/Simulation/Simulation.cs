using Types;
using Structs;

public partial class Simulation
{
    public static int _outRenderTextureWidth;
    public static int _outRenderTextureHeight;
    public static RWTexture2D<float> _outRenderTexture;

    static public void GenerateRandomNumbers(uint3 id)
    {
        int index = (int)(id.x);
        if( index < _outRenderTextureWidth * _outRenderTextureHeight )    
        {
            int y = index / _outRenderTextureWidth;
            int x = index - y * _outRenderTextureWidth;
            int2 xy = new int2( x, y );
            _outRenderTexture[xy] = rngRange(0.0f, 1.0f, rngIndex(index)) * rngRange( -1.0f, 1.0f, rngIndex(index) );
        }
    }
}
