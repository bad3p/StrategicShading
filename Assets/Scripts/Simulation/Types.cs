using System;
using System.Collections.Generic;
using UnityEngine;

namespace Types
{
    [Serializable]    
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
        public static implicit operator float2(Vector2 a) => new float2( a.x, a.y );
        public static implicit operator float2(Vector2Int a) => new float2( a.x, a.y );
        
        public Vector2 ToVector2() => new Vector2(x, y);
        public Vector2Int ToVector2Int() => new Vector2Int((int)x, (int)y);
    };
    
    [Serializable]
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
        public static implicit operator float3(Vector3 a) => new float3( a.x, a.y, a.z );
        public static implicit operator float3(Vector3Int a) => new float3( a.x, a.y, a.z );
        
        public Vector3 ToVector3() => new Vector3(x, y, z);
        public Vector3Int ToVector3Int() => new Vector3Int((int)x, (int)y, (int)z);
    };
    
    [Serializable]
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
        public static implicit operator float4(Vector4 a) => new float4( a.x, a.y, a.z, a.w );
        public static implicit operator float4(Quaternion a) => new float4( a.x, a.y, a.z, a.w );
        
        public Vector4 ToVector4() => new Vector4( x, y, z, w );
        public Quaternion ToQuaternion() => new Quaternion( x, y, z, w );
    };
    
    [Serializable]
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
        public static implicit operator double2(Vector2 a) => new double2( a.x, a.y );
        public static implicit operator double2(Vector2Int a) => new double2( a.x, a.y );
        
        public Vector2 ToVector2() => new Vector2((float)x, (float)y);
        public Vector2Int ToVector2Int() => new Vector2Int((int)x, (int)y);
    };
    
    [Serializable]
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
        public static implicit operator double3(Vector3 a) => new double3( a.x, a.y, a.z );
        public static implicit operator double3(Vector3Int a) => new double3( a.x, a.y, a.z );
        
        public Vector3 ToVector3() => new Vector3((float)x, (float)y, (float)z);
        public Vector3Int ToVector3Int() => new Vector3Int((int)x, (int)y, (int)z);
    };
    
    [Serializable]
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
        public static implicit operator double4(Vector4 a) => new double4( a.x, a.y, a.z, a.w );
        public static implicit operator double4(Quaternion a) => new double4( a.x, a.y, a.z, a.w );
        
        public Vector4 ToVector4() => new Vector4( (float)x, (float)y, (float)z, (float)w );
        public Quaternion ToQuaternion() => new Quaternion( (float)x, (float)y, (float)z, (float)w );
    };
    
    [Serializable]
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
        public static implicit operator int2(Vector2Int a) => new int2(a.x, a.y);
        public static implicit operator int2(Vector2 a) => new int2( (int)a.x, (int)a.y );

        public Vector2 ToVector2() => new Vector2(x, y);
        public Vector2Int ToVector2Int() => new Vector2Int(x, y);
    };
    
    [Serializable]
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
        public static implicit operator int3(Vector3Int a) => new int3( a.x, a.y, a.z );
        public static implicit operator int3(Vector3 a) => new int3( (int)a.x, (int)a.y, (int)a.z );
        
        public Vector3 ToVector3() => new Vector3(x, y, z);
        public Vector3Int ToVector3Int() => new Vector3Int(x, y, z);
    };
    
    [Serializable]
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
        public static implicit operator int4(Vector4 a) => new int4( (int)a.x, (int)a.y, (int)a.z, (int)a.w );
        public static implicit operator int4(Quaternion a) => new int4( (int)a.x, (int)a.y, (int)a.z, (int)a.w );
        
        public Vector4 ToVector4() => new Vector4(x, y, z, w);
        public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
    };
    
    [Serializable]
    public struct uint2
    {
        public uint x;
        public uint y;
        
        public uint2(uint _x, uint _y)
        {
            x = _x;
            y = _y;
        }

        public static uint2 operator +(uint2 a) => a;
        public static uint2 operator -(uint2 a) => new uint2(a.x, a.y);

        public static uint2 operator +(uint2 a, uint2 b) => new uint2( a.x+b.x, a.y+b.y );

        public static uint2 operator -(uint2 a, uint2 b) => a + (-b);

        public static uint2 operator *(uint2 a, uint b) => new uint2( a.x*b, a.y*b );
        public static uint2 operator *(uint2 a, uint2 b) => new uint2( a.x*b.x, a.y*b.y );

        public static uint2 operator /(uint2 a, uint b) => new uint2( a.x/b, a.y/b );
        public static uint2 operator /(uint2 a, uint2 b) => new uint2( a.x/b.x, a.y/b.y );
        
        public static implicit operator uint2(float2 a) => new uint2( (uint)a.x, (uint)a.y );
        public static implicit operator uint2(double2 a) => new uint2( (uint)a.x, (uint)a.y );
        public static implicit operator uint2(Vector2 a) => new uint2( (uint)a.x, (uint)a.y );
        public static implicit operator uint2(Vector2Int a) => new uint2( (uint)a.x, (uint)a.y );
        
        public Vector2 ToVector2() => new Vector2(x, y);
        public Vector2Int ToVector2Int() => new Vector2Int( (int)x, (int)y );
    };
    
    [Serializable]
    public struct uint3
    {
        public uint x;
        public uint y;
        public uint z;
        
        public uint3(uint _x, uint _y, uint _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public static uint3 operator +(uint3 a) => a;
        public static uint3 operator -(uint3 a) => new uint3(a.x, a.y, a.z);

        public static uint3 operator +(uint3 a, uint3 b) => new uint3( a.x+b.x, a.y+b.y, a.z+b.z );

        public static uint3 operator -(uint3 a, uint3 b) => a + (-b);

        public static uint3 operator *(uint3 a, uint b) => new uint3( a.x*b, a.y*b, a.z*b );
        public static uint3 operator *(uint3 a, uint3 b) => new uint3( a.x*b.x, a.y*b.y, a.z*b.z );

        public static uint3 operator /(uint3 a, uint b) => new uint3( a.x/b, a.y/b, a.z/b );
        public static uint3 operator /(uint3 a, uint3 b) => new uint3( a.x/b.x, a.y/b.y, a.z/b.z );
        
        public static implicit operator uint3(float3 a) => new uint3( (uint)a.x, (uint)a.y, (uint)a.z );
        public static implicit operator uint3(double3 a) => new uint3( (uint)a.x, (uint)a.y, (uint)a.z );
        public static implicit operator uint3(Vector3 a) => new uint3( (uint)a.x, (uint)a.y, (uint)a.z );
        public static implicit operator uint3(Vector3Int a) => new uint3( (uint)a.x, (uint)a.y, (uint)a.z );
        
        public Vector3 ToVector3() => new Vector3(x, y, z);
        public Vector3Int ToVector3Int() => new Vector3Int( (int)x, (int)y, (int)z );
    };
    
    [Serializable]
    public struct uint4
    {
        public uint x;
        public uint y;
        public uint z;
        public uint w;
        
        public uint4(uint _x, uint _y, uint _z, uint _w)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public static uint4 operator +(uint4 a) => a;
        public static uint4 operator -(uint4 a) => new uint4(a.x, a.y, a.z, a.w);

        public static uint4 operator +(uint4 a, uint4 b) => new uint4( a.x+b.x, a.y+b.y, a.z+b.z, a.w+b.w );

        public static uint4 operator -(uint4 a, uint4 b) => a + (-b);

        public static uint4 operator *(uint4 a, uint b) => new uint4( a.x*b, a.y*b, a.z*b, a.w*b );
        public static uint4 operator *(uint4 a, uint4 b) => new uint4( a.x*b.x, a.y*b.y, a.z*b.z, a.w*b.w );

        public static uint4 operator /(uint4 a, uint b) => new uint4( a.x/b, a.y/b, a.z/b, a.w/b );
        public static uint4 operator /(uint4 a, uint4 b) => new uint4( a.x/b.x, a.y/b.y, a.z/b.z, a.w/b.w );
        
        public static implicit operator uint4(float4 a) => new uint4( (uint)a.x, (uint)a.y, (uint)a.z, (uint)a.w );
        public static implicit operator uint4(double4 a) => new uint4( (uint)a.x, (uint)a.y, (uint)a.z, (uint)a.w );
        public static implicit operator uint4(Vector4 a) => new uint4( (uint)a.x, (uint)a.y, (uint)a.z, (uint)a.w );
        public static implicit operator uint4(Quaternion a) => new uint4( (uint)a.x, (uint)a.y, (uint)a.z, (uint)a.w );
        
        public Vector4 ToVector4() => new Vector4(x, y, z, w);
        public Quaternion ToQuaternion() => new Quaternion(x, y, z, w);
    };
    
    //--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------
    
    public struct float4x4
    {
        public float _m00;
        public float _m01;
        public float _m02;
        public float _m03;
        
        public float _m10;
        public float _m11;
        public float _m12;
        public float _m13;
        
        public float _m20;
        public float _m21;
        public float _m22;
        public float _m23;
        
        public float _m30;
        public float _m31;
        public float _m32;
        public float _m33;
        
        public float4x4(float4 x, float4 y, float4 z, float4 w)
        {
            _m00 = x.x; _m01 = y.x; _m02 = z.x; _m03 = w.x;
            _m10 = x.y; _m11 = y.y; _m12 = z.y; _m13 = w.y;
            _m20 = x.z; _m21 = y.z; _m22 = z.z; _m23 = w.z;
            _m30 = x.w; _m31 = y.w; _m32 = z.w; _m33 = w.w;
        }

        public static float4x4 operator *(float4x4 a, float4x4 b)
        {
            float4x4 result = new float4x4();
            
            result._m00 = a._m00*b._m00 + a._m01*b._m10 + a._m02*b._m20 + a._m03*b._m30;
            result._m01 = a._m00*b._m01 + a._m01*b._m11 + a._m02*b._m21 + a._m03*b._m31;
            result._m02 = a._m00*b._m02 + a._m01*b._m12 + a._m02*b._m22 + a._m03*b._m32;
            result._m03 = a._m00*b._m03 + a._m01*b._m13 + a._m02*b._m23 + a._m03*b._m33;
            
            result._m10 = a._m10*b._m00 + a._m11*b._m10 + a._m12*b._m20 + a._m13*b._m30;
            result._m11 = a._m10*b._m01 + a._m11*b._m11 + a._m12*b._m21 + a._m13*b._m31;
            result._m12 = a._m10*b._m02 + a._m11*b._m12 + a._m12*b._m22 + a._m13*b._m32;
            result._m13 = a._m10*b._m03 + a._m11*b._m13 + a._m12*b._m23 + a._m13*b._m33;
            
            result._m20 = a._m20*b._m00 + a._m21*b._m10 + a._m22*b._m20 + a._m23*b._m30;
            result._m21 = a._m20*b._m01 + a._m21*b._m11 + a._m22*b._m21 + a._m23*b._m31;
            result._m22 = a._m20*b._m02 + a._m21*b._m12 + a._m22*b._m22 + a._m23*b._m32;
            result._m23 = a._m20*b._m03 + a._m21*b._m13 + a._m22*b._m23 + a._m23*b._m33;
            
            result._m30 = a._m30*b._m00 + a._m31*b._m10 + a._m32*b._m20 + a._m33*b._m30;
            result._m31 = a._m30*b._m01 + a._m31*b._m11 + a._m32*b._m21 + a._m33*b._m31;
            result._m32 = a._m30*b._m02 + a._m31*b._m12 + a._m32*b._m22 + a._m33*b._m32;
            result._m33 = a._m30*b._m03 + a._m31*b._m13 + a._m32*b._m23 + a._m33*b._m33;

            return result;
        }

        public static implicit operator float4x4(Matrix4x4 m)
        {
            float4x4 result = new float4x4();
            result._m00 = m.m00; result._m01 = m.m01; result._m02 = m.m02; result._m03 = m.m03;
            result._m10 = m.m10; result._m11 = m.m11; result._m12 = m.m12; result._m13 = m.m13;
            result._m20 = m.m20; result._m21 = m.m21; result._m22 = m.m22; result._m23 = m.m23;
            result._m30 = m.m30; result._m31 = m.m31; result._m32 = m.m32; result._m33 = m.m33;
            return result;
        }

        public Matrix4x4 ToMatrix4x4()
        {
            Matrix4x4 result = new Matrix4x4();
            result.m00 = _m00; result.m01 = _m01; result.m02 = _m02; result.m03 = _m03;
            result.m10 = _m10; result.m11 = _m11; result.m12 = _m12; result.m13 = _m13;
            result.m20 = _m20; result.m21 = _m21; result.m22 = _m22; result.m23 = _m23;
            result.m30 = _m30; result.m31 = _m31; result.m32 = _m32; result.m33 = _m33;
            return result;
        }
    };
    
    //--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------
    //--------------------------------------------------------------------------------

    public delegate void ComputeShaderKernel(uint3 id);

    public class RWStructuredBuffer<T> : List<T>
    {
        public RWStructuredBuffer() : base()
        {
            
        }
        public RWStructuredBuffer(int length, T defaultValue) : base()
        {
            for (int i = 0; i < length; i++)
            {
                Add( defaultValue );       
            }
        }
    };

    public class RWTexture2D<T>
    {
        private int _width;
        private int _height;
        private T[] _pixels;
        
        public RWTexture2D(int width, int height)
        {
            _width = width;
            _height = height;
            _pixels = new T[width*height];
        }
        
        public int width
        {
            get => _width;
        }
        
        public int height
        {
            get => _height;
        }
        
        public T this[int2 key]
        {
            get { return _pixels[key.y * _width + key.x]; }
            set { _pixels[key.y * _width + key.x] = value; }
        }
    };
    
    [System.AttributeUsage(System.AttributeTargets.Method)]  
    public class NumThreads : System.Attribute  
    {  
        private int _x;
        private int _y;
        private int _z;
        
        public int x
        {
            get => _x;
        }
        
        public int y
        {
            get => _y;
        }
        
        public int z
        {
            get => _z;
        }
  
        public NumThreads()
        {
            _x = 1;
            _y = 1;
            _z = 1;
        }
        
        public NumThreads(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }  
    }
}
