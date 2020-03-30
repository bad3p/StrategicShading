using Types;
using UnityEngine;

public partial class Simulation
{
    public static float abs(float v)
    {
        return Mathf.Abs(v); 
    }
    
    public static float sqrt(float v)
    {
        return Mathf.Sqrt(v); 
    }
    
    public static float log(float v)
    {
        return Mathf.Log(v); 
    }
    
    public static float cos(float v)
    {
        return Mathf.Cos(v); 
    }
    
    public static float sin(float v)
    {
        return Mathf.Sin(v); 
    }
    
    public static float lerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, t);
    }
    
    public static float length(float2 v)
    {
        return sqrt(v.x * v.x + v.y * v.y);
    }
    
    public static float length(float3 v)
    {
        return sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
    }
    
    public static float length(float4 v)
    {
        return sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
    }
    
    public static float distance(float2 v1, float2 v2)
    {
        return length(v1 - v2);
    }
    
    public static float distance(float3 v1, float3 v2)
    {
        return length(v1 - v2);
    }
    
    public static float distance(float4 v1, float4 v2)
    {
        return length(v1 - v2);
    }
}