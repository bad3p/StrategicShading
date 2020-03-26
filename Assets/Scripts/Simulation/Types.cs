using System.Collections.Generic;

namespace Types
{
    public struct float2
    {
        public float x;
        public float y;
        
        public float2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        public static float2 operator +(float2 a) => a;
        public static float2 operator -(float2 a) => new float2(-a.x, -a.y);

        public static float2 operator +(float2 a, float2 b) => new float2( a.x+b.x, a.y+b.y );

        public static float2 operator -(float2 a, float2 b) => a + (-b);

        public static float2 operator *(float2 a, float b) => new float2( a.x*b, a.y*b );
        public static float2 operator *(float2 a, float2 b) => new float2( a.x*b.x, a.y*b.y );

        public static float2 operator /(float2 a, float b) => new float2( a.x/b, a.y/b );
        public static float2 operator /(float2 a, float2 b) => new float2( a.x/b.x, a.y/b.y );
        
        public static implicit operator float2(double2 a) => new float2( (float)a.x, (float)a.y );
        public static implicit operator float2(int2 a) => new float2( a.x, a.y );
    };
    
    public struct float3
    {
        public float x;
        public float y;
        public float z;
        
        public float3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static float3 operator +(float3 a) => a;
        public static float3 operator -(float3 a) => new float3(-a.x, -a.y, -a.z);

        public static float3 operator +(float3 a, float3 b) => new float3( a.x+b.x, a.y+b.y, a.z+b.z );

        public static float3 operator -(float3 a, float3 b) => a + (-b);

        public static float3 operator *(float3 a, float b) => new float3( a.x*b, a.y*b, a.z*b );
        public static float3 operator *(float3 a, float3 b) => new float3( a.x*b.x, a.y*b.y, a.z*b.z );

        public static float3 operator /(float3 a, float b) => new float3( a.x/b, a.y/b, a.z/b );
        public static float3 operator /(float3 a, float3 b) => new float3( a.x/b.x, a.y/b.y, a.z/b.z );
        
        public static implicit operator float3(double3 a) => new float3( (float)a.x, (float)a.y, (float)a.z );
        public static implicit operator float3(int3 a) => new float3( a.x, a.y, a.z );
    };
    
    public struct float4
    {
        public float x;
        public float y;
        public float z;
        public float w;
        
        public float4(float _x, float _y, float _z, float _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public static float4 operator +(float4 a) => a;
        public static float4 operator -(float4 a) => new float4(-a.x, -a.y, -a.z, -a.w);

        public static float4 operator +(float4 a, float4 b) => new float4( a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w );

        public static float4 operator -(float4 a, float4 b) => a + (-b);

        public static float4 operator *(float4 a, float b) => new float4( a.x*b, a.y*b, a.z*b, a.w*b );
        public static float4 operator *(float4 a, float4 b) => new float4( a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w );

        public static float4 operator /(float4 a, float b) => new float4( a.x/b, a.y/b, a.z/b, a.w/b );
        public static float4 operator /(float4 a, float4 b) => new float4( a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w );
        
        public static implicit operator float4(double4 a) => new float4( (float)a.x, (float)a.y, (float)a.z, (float)a.w );
        public static implicit operator float4(int4 a) => new float4( a.x, a.y, a.z, a.w );
    };
    
    public struct double2
    {
        public double x;
        public double y;
        
        public double2(double _x, double _y)
        {
            x = _x;
            y = _y;
        }

        public static double2 operator +(double2 a) => a;
        public static double2 operator -(double2 a) => new double2(-a.x, -a.y);

        public static double2 operator +(double2 a, double2 b) => new double2( a.x+b.x, a.y+b.y );

        public static double2 operator -(double2 a, double2 b) => a + (-b);

        public static double2 operator *(double2 a, double b) => new double2( a.x*b, a.y*b );
        public static double2 operator *(double2 a, double2 b) => new double2( a.x*b.x, a.y*b.y );

        public static double2 operator /(double2 a, double b) => new double2( a.x/b, a.y/b );
        public static double2 operator /(double2 a, double2 b) => new double2( a.x/b.x, a.y/b.y );
        
        public static implicit operator double2(float2 a) => new double2( a.x, a.y );
        public static implicit operator double2(int2 a) => new double2( a.x, a.y );
    };
    
    public struct double3
    {
        public double x;
        public double y;
        public double z;
        
        public double3(double _x, double _y, double _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static double3 operator +(double3 a) => a;
        public static double3 operator -(double3 a) => new double3(-a.x, -a.y, -a.z);

        public static double3 operator +(double3 a, double3 b) => new double3( a.x+b.x, a.y+b.y, a.z+b.z );

        public static double3 operator -(double3 a, double3 b) => a + (-b);

        public static double3 operator *(double3 a, double b) => new double3( a.x*b, a.y*b, a.z*b );
        public static double3 operator *(double3 a, double3 b) => new double3( a.x*b.x, a.y*b.y, a.z*b.z );

        public static double3 operator /(double3 a, double b) => new double3( a.x/b, a.y/b, a.z/b );
        public static double3 operator /(double3 a, double3 b) => new double3( a.x/b.x, a.y/b.y, a.z/b.z );
        
        public static implicit operator double3(float3 a) => new double3( a.x, a.y, a.z );
        public static implicit operator double3(int3 a) => new double3( a.x, a.y, a.z );
    };
    
    public struct double4
    {
        public double x;
        public double y;
        public double z;
        public double w;
        
        public double4(double _x, double _y, double _z, double _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public static double4 operator +(double4 a) => a;
        public static double4 operator -(double4 a) => new double4(-a.x, -a.y, -a.z, -a.w);

        public static double4 operator +(double4 a, double4 b) => new double4( a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w );

        public static double4 operator -(double4 a, double4 b) => a + (-b);

        public static double4 operator *(double4 a, double b) => new double4( a.x*b, a.y*b, a.z*b, a.w*b );
        public static double4 operator *(double4 a, double4 b) => new double4( a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w );

        public static double4 operator /(double4 a, double b) => new double4( a.x/b, a.y/b, a.z/b, a.w/b );
        public static double4 operator /(double4 a, double4 b) => new double4( a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w );
        
        public static implicit operator double4(float4 a) => new double4( a.x, a.y, a.z, a.w );
        public static implicit operator double4(int4 a) => new double4( a.x, a.y, a.z, a.w );        
    };
    
    public struct int2
    {
        public int x;
        public int y;
        
        public int2(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static int2 operator +(int2 a) => a;
        public static int2 operator -(int2 a) => new int2(-a.x, -a.y);

        public static int2 operator +(int2 a, int2 b) => new int2( a.x+b.x, a.y+b.y );

        public static int2 operator -(int2 a, int2 b) => a + (-b);

        public static int2 operator *(int2 a, int b) => new int2( a.x*b, a.y*b );
        public static int2 operator *(int2 a, int2 b) => new int2( a.x*b.x, a.y*b.y );

        public static int2 operator /(int2 a, int b) => new int2( a.x/b, a.y/b );
        public static int2 operator /(int2 a, int2 b) => new int2( a.x/b.x, a.y/b.y );
        
        public static implicit operator int2(float2 a) => new int2( (int)a.x, (int)a.y );
        public static implicit operator int2(double2 a) => new int2( (int)a.x, (int)a.y );
    };
    
    public struct int3
    {
        public int x;
        public int y;
        public int z;
        
        public int3(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static int3 operator +(int3 a) => a;
        public static int3 operator -(int3 a) => new int3(-a.x, -a.y, -a.z);

        public static int3 operator +(int3 a, int3 b) => new int3( a.x+b.x, a.y+b.y, a.z+b.z );

        public static int3 operator -(int3 a, int3 b) => a + (-b);

        public static int3 operator *(int3 a, int b) => new int3( a.x*b, a.y*b, a.z*b );
        public static int3 operator *(int3 a, int3 b) => new int3( a.x*b.x, a.y*b.y, a.z*b.z );

        public static int3 operator /(int3 a, int b) => new int3( a.x/b, a.y/b, a.z/b );
        public static int3 operator /(int3 a, int3 b) => new int3( a.x/b.x, a.y/b.y, a.z/b.z );
        
        public static implicit operator int3(float3 a) => new int3( (int)a.x, (int)a.y, (int)a.z );
        public static implicit operator int3(double3 a) => new int3( (int)a.x, (int)a.y, (int)a.z );
    };
    
    public struct int4
    {
        public int x;
        public int y;
        public int z;
        public int w;
        
        public int4(int _x, int _y, int _z, int _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public static int4 operator +(int4 a) => a;
        public static int4 operator -(int4 a) => new int4(-a.x, -a.y, -a.z, -a.w);

        public static int4 operator +(int4 a, int4 b) => new int4( a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w );

        public static int4 operator -(int4 a, int4 b) => a + (-b);

        public static int4 operator *(int4 a, int b) => new int4( a.x*b, a.y*b, a.z*b, a.w*b );
        public static int4 operator *(int4 a, int4 b) => new int4( a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w );

        public static int4 operator /(int4 a, int b) => new int4( a.x/b, a.y/b, a.z/b, a.w/b );
        public static int4 operator /(int4 a, int4 b) => new int4( a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w );
        
        public static implicit operator int4(float4 a) => new int4( (int)a.x, (int)a.y, (int)a.z, (int)a.w );
        public static implicit operator int4(double4 a) => new int4( (int)a.x, (int)a.y, (int)a.z, (int)a.w );        
    };

    public class RWStructuredBuffer<T> : List<T>
    {
    };

    public class RWTexture2D<T> : Dictionary<int2, T>
    {
    };
}
