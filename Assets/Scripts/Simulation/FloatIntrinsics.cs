using Types;
using UnityEngine;

public partial class Simulation
{
    public float2 float2(float x, float y)
    {
        return new float2(x,y);
    }
    
    public float3 float3(float x, float y, float z)
    {
        return new float3(x,y,z);
    }
    
    public float4 float4(float x, float y, float z, float w)
    {
        return new float4(x,y,z,w);
    }
    
    public float sqrt(float v)
    {
        return Mathf.Sqrt(v); 
    }
    
    public float log(float v)
    {
        return Mathf.Log(v); 
    }
    
    public float cos(float v)
    {
        return Mathf.Cos(v); 
    }
    
    public float sin(float v)
    {
        return Mathf.Sin(v); 
    }
    
    public float length(float2 v)
    {
        return sqrt(v.x * v.x + v.y * v.y);
    }
    
    public float length(float3 v)
    {
        return sqrt(v.x * v.x + v.y * v.y + v.z * v.z);
    }
    
    public float length(float4 v)
    {
        return sqrt(v.x * v.x + v.y * v.y + v.z * v.z + v.w * v.w);
    }
    
    public float distance(float2 v1, float2 v2)
    {
        return length(v1 - v2);
    }
    
    public float distance(float3 v1, float3 v2)
    {
        return length(v1 - v2);
    }
    
    public float distance(float4 v1, float4 v2)
    {
        return length(v1 - v2);
    }
}