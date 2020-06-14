using Types;
using UnityEngine;
using UnityEngine.Networking;

public partial class ComputeShaderEmulator
{
    public static int clamp(int value, int min, int max)
    {
        return value < min ? min : ( value > max ? max : value );
    }
    
    public static uint clamp(uint value, uint min, uint max)
    {
        return value < min ? min : ( value > max ? max : value );
    }
    
    public static int abs(int v)
    {
        int s = v < 0 ? -1 : ( v > 0 ? 1 : 0 );
        return v * s;
    }
    
    public static int sign(int v)
    {
        return v < 0 ? -1 : ( v > 0 ? 1 : 0 );
    }
    
    public static int max(int a, int b)
    {
        return a > b ? a : b;
    }
    
    public static uint max(uint a, uint b)
    {
        return a > b ? a : b;
    }
    
    public static int min(int a, int b)
    {
        return a < b ? a : b;
    }
    
    public static uint min(uint a, uint b)
    {
        return a < b ? a : b;
    }
}