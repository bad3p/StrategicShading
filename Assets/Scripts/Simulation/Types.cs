using System.Collections.Generic;

namespace Types
{
    public struct float2
    {
        public float x;
        public float y;
    };
    
    public struct float3
    {
        public float x;
        public float y;
        public float z;
    };
    
    public struct float4
    {
        public float x;
        public float y;
        public float z;
        public float w;
    };
    
    public struct double2
    {
        public double x;
        public double y;
    };
    
    public struct double3
    {
        public double x;
        public double y;
        public double z;
    };
    
    public struct double4
    {
        public double x;
        public double y;
        public double z;
        public double w;
    };
    
    public struct int2
    {
        public int x;
        public int y;
    };
    
    public struct int3
    {
        public int x;
        public int y;
        public int z;
    };
    
    public struct int4
    {
        public int x;
        public int y;
        public int z;
        public int w;
    };

    public class RWStructuredBuffer<T> : List<T>
    {
    };

    public class RWTexture2D<T> : Dictionary<int2, T>
    {
    };
}
