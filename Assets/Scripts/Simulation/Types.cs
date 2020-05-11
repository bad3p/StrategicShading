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
        
        public float2 xx { get => new float2(x,x); }
        public float2 yy { get => new float2(y,y); }
        public float2 xy { get => new float2(x,y); set { x = value.x; y = value.y; } }
        public float2 yx { get => new float2(y,x); set { y = value.x; x = value.y; } }

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
        
        public float2 xx { get => new float2(x,x); }
        public float2 yy { get => new float2(y,y); }
        public float2 zz { get => new float2(z,z); }
        
        public float2 xy { get => new float2(x,y); set { x = value.x; y = value.y; } }
        public float2 xz { get => new float2(x,z); set { x = value.x; z = value.y; } }
        public float2 yx { get => new float2(y,x); set { y = value.x; x = value.y; } }
        public float2 yz { get => new float2(y,z); set { y = value.x; z = value.y; } }
        public float2 zy { get => new float2(z,y); set { z = value.x; y = value.y; } }
        public float2 zx { get => new float2(z,x); set { z = value.x; x = value.y; } }
        
        public float3 xxx { get => new float3(x,x,x); }
        public float3 xxy { get => new float3(x,x,y); }
        public float3 xxz { get => new float3(x,x,z); }
        public float3 xyx { get => new float3(x,y,x); }
        public float3 xyy { get => new float3(x,y,y); }
        public float3 xzx { get => new float3(x,z,x); }
        public float3 xzz { get => new float3(x,z,z); }
        public float3 yxx { get => new float3(y,x,x); }
        public float3 yxy { get => new float3(y,x,y); }
        public float3 yyx { get => new float3(y,y,x); }
        public float3 yyy { get => new float3(y,y,y); }
        public float3 yyz { get => new float3(y,y,z); }
        public float3 yzy { get => new float3(y,z,y); }
        public float3 yzz { get => new float3(y,z,z); }
        public float3 zxx { get => new float3(z,x,x); }
        public float3 zxz { get => new float3(z,x,z); }
        public float3 zyy { get => new float3(z,y,y); }
        public float3 zyz { get => new float3(z,y,z); }
        public float3 zzx { get => new float3(z,z,x); }
        public float3 zzy { get => new float3(z,z,y); }
        public float3 zzz { get => new float3(z,z,z); }

        public float3 xyz { get => new float3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public float3 xzy { get => new float3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public float3 yxz { get => new float3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public float3 yzx { get => new float3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public float3 zxy { get => new float3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public float3 zyx { get => new float3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }

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
        
        public float2 xx { get => new float2(x,x); }
        public float2 xy { get => new float2(x,y); set { x = value.x; y = value.y; } }
        public float2 xz { get => new float2(x,z); set { x = value.x; z = value.y; } }
        public float2 xw { get => new float2(x,w); set { x = value.x; w = value.y; } }
        public float2 yx { get => new float2(y,x); set { y = value.x; x = value.y; } }
        public float2 yy { get => new float2(y,y); }
        public float2 yz { get => new float2(y,z); set { y = value.x; z = value.y; } }
        public float2 yw { get => new float2(y,w); set { y = value.x; w = value.y; } }
        public float2 zx { get => new float2(z,x); set { z = value.x; x = value.y; } }
        public float2 zy { get => new float2(z,y); set { z = value.x; y = value.y; } }
        public float2 zz { get => new float2(z,z); }
        public float2 zw { get => new float2(z,w); set { z = value.x; w = value.y; } }
        public float2 wx { get => new float2(w,x); set { w = value.x; x = value.y; } }
        public float2 wy { get => new float2(w,y); set { w = value.x; y = value.y; } }
        public float2 wz { get => new float2(w,z); set { w = value.x; z = value.y; } }
        public float2 ww { get => new float2(w,w); }
        
        public float3 xxx { get => new float3(x,x,x); }
        public float3 xxy { get => new float3(x,x,y); }
        public float3 xxz { get => new float3(x,x,z); }
        public float3 xxw { get => new float3(x,x,w); }
        public float3 xyx { get => new float3(x,y,x); }
        public float3 xyy { get => new float3(x,y,y); }
        public float3 xyz { get => new float3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public float3 xyw { get => new float3(x,y,w); set { x = value.x; y = value.y; w = value.z; } }
        public float3 xzx { get => new float3(x,z,x); }
        public float3 xzy { get => new float3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public float3 xzz { get => new float3(x,z,z); }
        public float3 xzw { get => new float3(x,z,w); set { x = value.x; z = value.y; w = value.z; } }
        public float3 xwx { get => new float3(x,w,x); }
        public float3 xwy { get => new float3(x,w,y); set { x = value.x; w = value.y; y = value.z; } }
        public float3 xwz { get => new float3(x,w,z); set { x = value.x; w = value.y; z = value.z; } }
        public float3 xww { get => new float3(x,w,w); }
        public float3 yxx { get => new float3(y,x,x); }
        public float3 yxy { get => new float3(y,x,y); }
        public float3 yxz { get => new float3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public float3 yxw { get => new float3(y,x,w); set { y = value.x; x = value.y; w = value.z; } }
        public float3 yyx { get => new float3(y,y,x); }
        public float3 yyy { get => new float3(y,y,y); }
        public float3 yyz { get => new float3(y,y,z); }
        public float3 yyw { get => new float3(y,y,w); }
        public float3 yzx { get => new float3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public float3 yzy { get => new float3(y,z,y); }
        public float3 yzz { get => new float3(y,z,z); }
        public float3 yzw { get => new float3(y,z,w); set { y = value.x; z = value.y; w = value.z; } }
        public float3 ywx { get => new float3(y,w,x); set { y = value.x; w = value.y; x = value.z; } }
        public float3 ywy { get => new float3(y,w,y); }
        public float3 ywz { get => new float3(y,w,z); set { y = value.x; w = value.y; z = value.z; } }
        public float3 yww { get => new float3(y,w,w); }
        public float3 zxx { get => new float3(z,x,x); }
        public float3 zxy { get => new float3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public float3 zxz { get => new float3(z,x,z); }
        public float3 zxw { get => new float3(z,x,w); set { z = value.x; x = value.y; w = value.z; } }
        public float3 zyx { get => new float3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }
        public float3 zyy { get => new float3(z,y,y); }
        public float3 zyz { get => new float3(z,y,z); }
        public float3 zyw { get => new float3(z,y,w); set { z = value.x; y = value.y; w = value.z; } }
        public float3 zzx { get => new float3(z,z,x); }
        public float3 zzy { get => new float3(z,z,y); }
        public float3 zzz { get => new float3(z,z,z); }
        public float3 zzw { get => new float3(z,z,w); }
        public float3 zwx { get => new float3(z,w,x); set { z = value.x; w = value.y; x = value.z; } }
        public float3 zwy { get => new float3(z,w,y); set { z = value.x; w = value.y; y = value.z; } }
        public float3 zwz { get => new float3(z,w,z); }
        public float3 zww { get => new float3(z,w,w); }
        public float3 wxx { get => new float3(w,x,x); }
        public float3 wxy { get => new float3(w,x,y); set { w = value.x; x = value.y; y = value.z; } }
        public float3 wxz { get => new float3(w,x,z); set { w = value.x; x = value.y; z = value.z; } }
        public float3 wxw { get => new float3(w,x,w); }
        public float3 wyx { get => new float3(w,y,x); set { w = value.x; y = value.y; x = value.z; } }
        public float3 wyy { get => new float3(w,y,y); }
        public float3 wyz { get => new float3(w,y,z); set { w = value.x; y = value.y; z = value.z; } }
        public float3 wyw { get => new float3(w,y,w); }
        public float3 wzx { get => new float3(w,z,x); set { w = value.x; z = value.y; x = value.z; } }
        public float3 wzy { get => new float3(w,z,y); set { w = value.x; z = value.y; y = value.z; } }
        public float3 wzz { get => new float3(w,z,z); }
        public float3 wzw { get => new float3(w,z,w); }
        public float3 wwx { get => new float3(w,w,x); }
        public float3 wwy { get => new float3(w,w,y); }
        public float3 wwz { get => new float3(w,w,z); }
        public float3 www { get => new float3(w,w,w); }
        
        public float4 xxxx { get => new float4(x,x,x,x); }
        public float4 xxxy { get => new float4(x,x,x,y); }
        public float4 xxxz { get => new float4(x,x,x,z); }
        public float4 xxxw { get => new float4(x,x,x,w); }
        public float4 xxyx { get => new float4(x,x,y,x); }
        public float4 xxyy { get => new float4(x,x,y,y); }
        public float4 xxyz { get => new float4(x,x,y,z); }
        public float4 xxyw { get => new float4(x,x,y,w); }
        public float4 xxzx { get => new float4(x,x,z,x); }
        public float4 xxzy { get => new float4(x,x,z,y); }
        public float4 xxzz { get => new float4(x,x,z,z); }
        public float4 xxzw { get => new float4(x,x,z,w); }
        public float4 xxwx { get => new float4(x,x,w,x); }
        public float4 xxwy { get => new float4(x,x,w,y); }
        public float4 xxwz { get => new float4(x,x,w,z); }
        public float4 xxww { get => new float4(x,x,w,w); }
        public float4 xyxx { get => new float4(x,y,x,x); }
        public float4 xyxy { get => new float4(x,y,x,y); }
        public float4 xyxz { get => new float4(x,y,x,z); }
        public float4 xyxw { get => new float4(x,y,x,w); }
        public float4 xyyx { get => new float4(x,y,y,x); }
        public float4 xyyy { get => new float4(x,y,y,y); }
        public float4 xyyz { get => new float4(x,y,y,z); }
        public float4 xyyw { get => new float4(x,y,y,w); }
        public float4 xyzx { get => new float4(x,y,z,x); }
        public float4 xyzy { get => new float4(x,y,z,y); }
        public float4 xyzz { get => new float4(x,y,z,z); }
        public float4 xyzw { get => new float4(x,y,z,w); set { x = value.x; y = value.y; z = value.z; w = value.w; } }
        public float4 xywx { get => new float4(x,y,w,x); }
        public float4 xywy { get => new float4(x,y,w,y); }
        public float4 xywz { get => new float4(x,y,w,z); set { x = value.x; y = value.y; w = value.z; z = value.w; } }
        public float4 xyww { get => new float4(x,y,w,w); }
        public float4 xzxx { get => new float4(x,z,x,x); }
        public float4 xzxy { get => new float4(x,z,x,y); }
        public float4 xzxz { get => new float4(x,z,x,z); }
        public float4 xzxw { get => new float4(x,z,x,w); }
        public float4 xzyx { get => new float4(x,z,y,x); }
        public float4 xzyy { get => new float4(x,z,y,y); }
        public float4 xzyz { get => new float4(x,z,y,z); }
        public float4 xzyw { get => new float4(x,z,y,w); set { x = value.x; z = value.y; y = value.z; w = value.w; } }
        public float4 xzzx { get => new float4(x,z,z,x); }
        public float4 xzzy { get => new float4(x,z,z,y); }
        public float4 xzzz { get => new float4(x,z,z,z); }
        public float4 xzzw { get => new float4(x,z,z,w); }
        public float4 xzwx { get => new float4(x,z,w,x); }
        public float4 xzwy { get => new float4(x,z,w,y); set { x = value.x; z = value.y; w = value.z; y = value.w; } }
        public float4 xzwz { get => new float4(x,z,w,z); }
        public float4 xzww { get => new float4(x,z,w,w); }
        public float4 xwxx { get => new float4(x,w,x,x); }
        public float4 xwxy { get => new float4(x,w,x,y); }
        public float4 xwxz { get => new float4(x,w,x,z); }
        public float4 xwxw { get => new float4(x,w,x,w); }
        public float4 xwyx { get => new float4(x,w,y,x); }
        public float4 xwyy { get => new float4(x,w,y,y); }
        public float4 xwyz { get => new float4(x,w,y,z); set { x = value.x; w = value.y; y = value.z; z = value.w; } }
        public float4 xwyw { get => new float4(x,w,y,w); }
        public float4 xwzx { get => new float4(x,w,z,x); }
        public float4 xwzy { get => new float4(x,w,z,y); set { x = value.x; w = value.y; z = value.z; y = value.w; } }
        public float4 xwzz { get => new float4(x,w,z,z); }
        public float4 xwzw { get => new float4(x,w,z,w); }
        public float4 xwwx { get => new float4(x,w,w,x); }
        public float4 xwwy { get => new float4(x,w,w,y); }
        public float4 xwwz { get => new float4(x,w,w,z); }
        public float4 xwww { get => new float4(x,w,w,w); }
        public float4 yxxx { get => new float4(y,x,x,x); }
        public float4 yxxy { get => new float4(y,x,x,y); }
        public float4 yxxz { get => new float4(y,x,x,z); }
        public float4 yxxw { get => new float4(y,x,x,w); }
        public float4 yxyx { get => new float4(y,x,y,x); }
        public float4 yxyy { get => new float4(y,x,y,y); }
        public float4 yxyz { get => new float4(y,x,y,z); }
        public float4 yxyw { get => new float4(y,x,y,w); }
        public float4 yxzx { get => new float4(y,x,z,x); }
        public float4 yxzy { get => new float4(y,x,z,y); }
        public float4 yxzz { get => new float4(y,x,z,z); }
        public float4 yxzw { get => new float4(y,x,z,w); set { y = value.x; x = value.y; z = value.z; w = value.w; } }
        public float4 yxwx { get => new float4(y,x,w,x); }
        public float4 yxwy { get => new float4(y,x,w,y); }
        public float4 yxwz { get => new float4(y,x,w,z); set { y = value.x; x = value.y; w = value.z; z = value.w; } }
        public float4 yxww { get => new float4(y,x,w,w); }
        public float4 yyxx { get => new float4(y,y,x,x); }
        public float4 yyxy { get => new float4(y,y,x,y); }
        public float4 yyxz { get => new float4(y,y,x,z); }
        public float4 yyxw { get => new float4(y,y,x,w); }
        public float4 yyyx { get => new float4(y,y,y,x); }
        public float4 yyyy { get => new float4(y,y,y,y); }
        public float4 yyyz { get => new float4(y,y,y,z); }
        public float4 yyyw { get => new float4(y,y,y,w); }
        public float4 yyzx { get => new float4(y,y,z,x); }
        public float4 yyzy { get => new float4(y,y,z,y); }
        public float4 yyzz { get => new float4(y,y,z,z); }
        public float4 yyzw { get => new float4(y,y,z,w); }
        public float4 yywx { get => new float4(y,y,w,x); }
        public float4 yywy { get => new float4(y,y,w,y); }
        public float4 yywz { get => new float4(y,y,w,z); }
        public float4 yyww { get => new float4(y,y,w,w); }
        public float4 yzxx { get => new float4(y,z,x,x); }
        public float4 yzxy { get => new float4(y,z,x,y); }
        public float4 yzxz { get => new float4(y,z,x,z); }
        public float4 yzxw { get => new float4(y,z,x,w); set { y = value.x; z = value.y; x = value.z; w = value.w; } }
        public float4 yzyx { get => new float4(y,z,y,x); }
        public float4 yzyy { get => new float4(y,z,y,y); }
        public float4 yzyz { get => new float4(y,z,y,z); }
        public float4 yzyw { get => new float4(y,z,y,w); }
        public float4 yzzx { get => new float4(y,z,z,x); }
        public float4 yzzy { get => new float4(y,z,z,y); }
        public float4 yzzz { get => new float4(y,z,z,z); }
        public float4 yzzw { get => new float4(y,z,z,w); }
        public float4 yzwx { get => new float4(y,z,w,x); set { y = value.x; z = value.y; w = value.z; x = value.w; } }
        public float4 yzwy { get => new float4(y,z,w,y); }
        public float4 yzwz { get => new float4(y,z,w,z); }
        public float4 yzww { get => new float4(y,z,w,w); }
        public float4 ywxx { get => new float4(y,w,x,x); }
        public float4 ywxy { get => new float4(y,w,x,y); }
        public float4 ywxz { get => new float4(y,w,x,z); set { y = value.x; w = value.y; x = value.z; z = value.w; } }
        public float4 ywxw { get => new float4(y,w,x,w); }
        public float4 ywyx { get => new float4(y,w,y,x); }
        public float4 ywyy { get => new float4(y,w,y,y); }
        public float4 ywyz { get => new float4(y,w,y,z); }
        public float4 ywyw { get => new float4(y,w,y,w); }
        public float4 ywzx { get => new float4(y,w,z,x); set { y = value.x; w = value.y; z = value.z; x = value.w; } }
        public float4 ywzy { get => new float4(y,w,z,y); }
        public float4 ywzz { get => new float4(y,w,z,z); }
        public float4 ywzw { get => new float4(y,w,z,w); }
        public float4 ywwx { get => new float4(y,w,w,x); }
        public float4 ywwy { get => new float4(y,w,w,y); }
        public float4 ywwz { get => new float4(y,w,w,z); }
        public float4 ywww { get => new float4(y,w,w,w); }
        public float4 zxxx { get => new float4(z,x,x,x); }
        public float4 zxxy { get => new float4(z,x,x,y); }
        public float4 zxxz { get => new float4(z,x,x,z); }
        public float4 zxxw { get => new float4(z,x,x,w); }
        public float4 zxyx { get => new float4(z,x,y,x); }
        public float4 zxyy { get => new float4(z,x,y,y); }
        public float4 zxyz { get => new float4(z,x,y,z); }
        public float4 zxyw { get => new float4(z,x,y,w); set { z = value.x; x = value.y; y = value.z; w = value.w; } }
        public float4 zxzx { get => new float4(z,x,z,x); }
        public float4 zxzy { get => new float4(z,x,z,y); }
        public float4 zxzz { get => new float4(z,x,z,z); }
        public float4 zxzw { get => new float4(z,x,z,w); }
        public float4 zxwx { get => new float4(z,x,w,x); }
        public float4 zxwy { get => new float4(z,x,w,y); set { z = value.x; x = value.y; w = value.z; y = value.w; } }
        public float4 zxwz { get => new float4(z,x,w,z); }
        public float4 zxww { get => new float4(z,x,w,w); }
        public float4 zyxx { get => new float4(z,y,x,x); }
        public float4 zyxy { get => new float4(z,y,x,y); }
        public float4 zyxz { get => new float4(z,y,x,z); }
        public float4 zyxw { get => new float4(z,y,x,w); set { z = value.x; y = value.y; x = value.z; w = value.w; } }
        public float4 zyyx { get => new float4(z,y,y,x); }
        public float4 zyyy { get => new float4(z,y,y,y); }
        public float4 zyyz { get => new float4(z,y,y,z); }
        public float4 zyyw { get => new float4(z,y,y,w); }
        public float4 zyzx { get => new float4(z,y,z,x); }
        public float4 zyzy { get => new float4(z,y,z,y); }
        public float4 zyzz { get => new float4(z,y,z,z); }
        public float4 zyzw { get => new float4(z,y,z,w); }
        public float4 zywx { get => new float4(z,y,w,x); set { z = value.x; y = value.y; w = value.z; x = value.w; } }
        public float4 zywy { get => new float4(z,y,w,y); }
        public float4 zywz { get => new float4(z,y,w,z); }
        public float4 zyww { get => new float4(z,y,w,w); }
        public float4 zzxx { get => new float4(z,z,x,x); }
        public float4 zzxy { get => new float4(z,z,x,y); }
        public float4 zzxz { get => new float4(z,z,x,z); }
        public float4 zzxw { get => new float4(z,z,x,w); }
        public float4 zzyx { get => new float4(z,z,y,x); }
        public float4 zzyy { get => new float4(z,z,y,y); }
        public float4 zzyz { get => new float4(z,z,y,z); }
        public float4 zzyw { get => new float4(z,z,y,w); }
        public float4 zzzx { get => new float4(z,z,z,x); }
        public float4 zzzy { get => new float4(z,z,z,y); }
        public float4 zzzz { get => new float4(z,z,z,z); }
        public float4 zzzw { get => new float4(z,z,z,w); }
        public float4 zzwx { get => new float4(z,z,w,x); }
        public float4 zzwy { get => new float4(z,z,w,y); }
        public float4 zzwz { get => new float4(z,z,w,z); }
        public float4 zzww { get => new float4(z,z,w,w); }
        public float4 zwxx { get => new float4(z,w,x,x); }
        public float4 zwxy { get => new float4(z,w,x,y); set { z = value.x; w = value.y; x = value.z; y = value.w; } }
        public float4 zwxz { get => new float4(z,w,x,z); }
        public float4 zwxw { get => new float4(z,w,x,w); }
        public float4 zwyx { get => new float4(z,w,y,x); set { z = value.x; w = value.y; y = value.z; x = value.w; } }
        public float4 zwyy { get => new float4(z,w,y,y); }
        public float4 zwyz { get => new float4(z,w,y,z); }
        public float4 zwyw { get => new float4(z,w,y,w); }
        public float4 zwzx { get => new float4(z,w,z,x); }
        public float4 zwzy { get => new float4(z,w,z,y); }
        public float4 zwzz { get => new float4(z,w,z,z); }
        public float4 zwzw { get => new float4(z,w,z,w); }
        public float4 zwwx { get => new float4(z,w,w,x); }
        public float4 zwwy { get => new float4(z,w,w,y); }
        public float4 zwwz { get => new float4(z,w,w,z); }
        public float4 zwww { get => new float4(z,w,w,w); }
        public float4 wxxx { get => new float4(w,x,x,x); }
        public float4 wxxy { get => new float4(w,x,x,y); }
        public float4 wxxz { get => new float4(w,x,x,z); }
        public float4 wxxw { get => new float4(w,x,x,w); }
        public float4 wxyx { get => new float4(w,x,y,x); }
        public float4 wxyy { get => new float4(w,x,y,y); }
        public float4 wxyz { get => new float4(w,x,y,z); set { w = value.x; x = value.y; y = value.z; z = value.w; } }
        public float4 wxyw { get => new float4(w,x,y,w); }
        public float4 wxzx { get => new float4(w,x,z,x); }
        public float4 wxzy { get => new float4(w,x,z,y); set { w = value.x; x = value.y; z = value.z; y = value.w; } }
        public float4 wxzz { get => new float4(w,x,z,z); }
        public float4 wxzw { get => new float4(w,x,z,w); }
        public float4 wxwx { get => new float4(w,x,w,x); }
        public float4 wxwy { get => new float4(w,x,w,y); }
        public float4 wxwz { get => new float4(w,x,w,z); }
        public float4 wxww { get => new float4(w,x,w,w); }
        public float4 wyxx { get => new float4(w,y,x,x); }
        public float4 wyxy { get => new float4(w,y,x,y); }
        public float4 wyxz { get => new float4(w,y,x,z); set { w = value.x; y = value.y; x = value.z; z = value.w; } }
        public float4 wyxw { get => new float4(w,y,x,w); }
        public float4 wyyx { get => new float4(w,y,y,x); }
        public float4 wyyy { get => new float4(w,y,y,y); }
        public float4 wyyz { get => new float4(w,y,y,z); }
        public float4 wyyw { get => new float4(w,y,y,w); }
        public float4 wyzx { get => new float4(w,y,z,x); set { w = value.x; y = value.y; z = value.z; x = value.w; } }
        public float4 wyzy { get => new float4(w,y,z,y); }
        public float4 wyzz { get => new float4(w,y,z,z); }
        public float4 wyzw { get => new float4(w,y,z,w); }
        public float4 wywx { get => new float4(w,y,w,x); }
        public float4 wywy { get => new float4(w,y,w,y); }
        public float4 wywz { get => new float4(w,y,w,z); }
        public float4 wyww { get => new float4(w,y,w,w); }
        public float4 wzxx { get => new float4(w,z,x,x); }
        public float4 wzxy { get => new float4(w,z,x,y); set { w = value.x; z = value.y; x = value.z; y = value.w; } }
        public float4 wzxz { get => new float4(w,z,x,z); }
        public float4 wzxw { get => new float4(w,z,x,w); }
        public float4 wzyx { get => new float4(w,z,y,x); set { w = value.x; z = value.y; y = value.z; x = value.w; } }
        public float4 wzyy { get => new float4(w,z,y,y); }
        public float4 wzyz { get => new float4(w,z,y,z); }
        public float4 wzyw { get => new float4(w,z,y,w); }
        public float4 wzzx { get => new float4(w,z,z,x); }
        public float4 wzzy { get => new float4(w,z,z,y); }
        public float4 wzzz { get => new float4(w,z,z,z); }
        public float4 wzzw { get => new float4(w,z,z,w); }
        public float4 wzwx { get => new float4(w,z,w,x); }
        public float4 wzwy { get => new float4(w,z,w,y); }
        public float4 wzwz { get => new float4(w,z,w,z); }
        public float4 wzww { get => new float4(w,z,w,w); }
        public float4 wwxx { get => new float4(w,w,x,x); }
        public float4 wwxy { get => new float4(w,w,x,y); }
        public float4 wwxz { get => new float4(w,w,x,z); }
        public float4 wwxw { get => new float4(w,w,x,w); }
        public float4 wwyx { get => new float4(w,w,y,x); }
        public float4 wwyy { get => new float4(w,w,y,y); }
        public float4 wwyz { get => new float4(w,w,y,z); }
        public float4 wwyw { get => new float4(w,w,y,w); }
        public float4 wwzx { get => new float4(w,w,z,x); }
        public float4 wwzy { get => new float4(w,w,z,y); }
        public float4 wwzz { get => new float4(w,w,z,z); }
        public float4 wwzw { get => new float4(w,w,z,w); }
        public float4 wwwx { get => new float4(w,w,w,x); }
        public float4 wwwy { get => new float4(w,w,w,y); }
        public float4 wwwz { get => new float4(w,w,w,z); }
        public float4 wwww { get => new float4(w,w,w,w); }

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
        
        public double2 xx { get => new double2(x,x); }
        public double2 yy { get => new double2(y,y); }
        public double2 xy { get => new double2(x,y); set { x = value.x; y = value.y; } }
        public double2 yx { get => new double2(y,x); set { y = value.x; x = value.y; } }

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
        
        public double2 xx { get => new double2(x,x); }
        public double2 yy { get => new double2(y,y); }
        public double2 zz { get => new double2(z,z); }
        
        public double2 xy { get => new double2(x,y); set { x = value.x; y = value.y; } }
        public double2 xz { get => new double2(x,z); set { x = value.x; z = value.y; } }
        public double2 yx { get => new double2(y,x); set { y = value.x; x = value.y; } }
        public double2 yz { get => new double2(y,z); set { y = value.x; z = value.y; } }
        public double2 zy { get => new double2(z,y); set { z = value.x; y = value.y; } }
        public double2 zx { get => new double2(z,x); set { z = value.x; x = value.y; } }
        
        public double3 xxx { get => new double3(x,x,x); }
        public double3 xxy { get => new double3(x,x,y); }
        public double3 xxz { get => new double3(x,x,z); }
        public double3 xyx { get => new double3(x,y,x); }
        public double3 xyy { get => new double3(x,y,y); }
        public double3 xzx { get => new double3(x,z,x); }
        public double3 xzz { get => new double3(x,z,z); }
        public double3 yxx { get => new double3(y,x,x); }
        public double3 yxy { get => new double3(y,x,y); }
        public double3 yyx { get => new double3(y,y,x); }
        public double3 yyy { get => new double3(y,y,y); }
        public double3 yyz { get => new double3(y,y,z); }
        public double3 yzy { get => new double3(y,z,y); }
        public double3 yzz { get => new double3(y,z,z); }
        public double3 zxx { get => new double3(z,x,x); }
        public double3 zxz { get => new double3(z,x,z); }
        public double3 zyy { get => new double3(z,y,y); }
        public double3 zyz { get => new double3(z,y,z); }
        public double3 zzx { get => new double3(z,z,x); }
        public double3 zzy { get => new double3(z,z,y); }
        public double3 zzz { get => new double3(z,z,z); }

        public double3 xyz { get => new double3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public double3 xzy { get => new double3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public double3 yxz { get => new double3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public double3 yzx { get => new double3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public double3 zxy { get => new double3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public double3 zyx { get => new double3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }

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
        
        public double2 xx { get => new double2(x,x); }
        public double2 xy { get => new double2(x,y); set { x = value.x; y = value.y; } }
        public double2 xz { get => new double2(x,z); set { x = value.x; z = value.y; } }
        public double2 xw { get => new double2(x,w); set { x = value.x; w = value.y; } }
        public double2 yx { get => new double2(y,x); set { y = value.x; x = value.y; } }
        public double2 yy { get => new double2(y,y); }
        public double2 yz { get => new double2(y,z); set { y = value.x; z = value.y; } }
        public double2 yw { get => new double2(y,w); set { y = value.x; w = value.y; } }
        public double2 zx { get => new double2(z,x); set { z = value.x; x = value.y; } }
        public double2 zy { get => new double2(z,y); set { z = value.x; y = value.y; } }
        public double2 zz { get => new double2(z,z); }
        public double2 zw { get => new double2(z,w); set { z = value.x; w = value.y; } }
        public double2 wx { get => new double2(w,x); set { w = value.x; x = value.y; } }
        public double2 wy { get => new double2(w,y); set { w = value.x; y = value.y; } }
        public double2 wz { get => new double2(w,z); set { w = value.x; z = value.y; } }
        public double2 ww { get => new double2(w,w); }
        
        public double3 xxx { get => new double3(x,x,x); }
        public double3 xxy { get => new double3(x,x,y); }
        public double3 xxz { get => new double3(x,x,z); }
        public double3 xxw { get => new double3(x,x,w); }
        public double3 xyx { get => new double3(x,y,x); }
        public double3 xyy { get => new double3(x,y,y); }
        public double3 xyz { get => new double3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public double3 xyw { get => new double3(x,y,w); set { x = value.x; y = value.y; w = value.z; } }
        public double3 xzx { get => new double3(x,z,x); }
        public double3 xzy { get => new double3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public double3 xzz { get => new double3(x,z,z); }
        public double3 xzw { get => new double3(x,z,w); set { x = value.x; z = value.y; w = value.z; } }
        public double3 xwx { get => new double3(x,w,x); }
        public double3 xwy { get => new double3(x,w,y); set { x = value.x; w = value.y; y = value.z; } }
        public double3 xwz { get => new double3(x,w,z); set { x = value.x; w = value.y; z = value.z; } }
        public double3 xww { get => new double3(x,w,w); }
        public double3 yxx { get => new double3(y,x,x); }
        public double3 yxy { get => new double3(y,x,y); }
        public double3 yxz { get => new double3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public double3 yxw { get => new double3(y,x,w); set { y = value.x; x = value.y; w = value.z; } }
        public double3 yyx { get => new double3(y,y,x); }
        public double3 yyy { get => new double3(y,y,y); }
        public double3 yyz { get => new double3(y,y,z); }
        public double3 yyw { get => new double3(y,y,w); }
        public double3 yzx { get => new double3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public double3 yzy { get => new double3(y,z,y); }
        public double3 yzz { get => new double3(y,z,z); }
        public double3 yzw { get => new double3(y,z,w); set { y = value.x; z = value.y; w = value.z; } }
        public double3 ywx { get => new double3(y,w,x); set { y = value.x; w = value.y; x = value.z; } }
        public double3 ywy { get => new double3(y,w,y); }
        public double3 ywz { get => new double3(y,w,z); set { y = value.x; w = value.y; z = value.z; } }
        public double3 yww { get => new double3(y,w,w); }
        public double3 zxx { get => new double3(z,x,x); }
        public double3 zxy { get => new double3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public double3 zxz { get => new double3(z,x,z); }
        public double3 zxw { get => new double3(z,x,w); set { z = value.x; x = value.y; w = value.z; } }
        public double3 zyx { get => new double3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }
        public double3 zyy { get => new double3(z,y,y); }
        public double3 zyz { get => new double3(z,y,z); }
        public double3 zyw { get => new double3(z,y,w); set { z = value.x; y = value.y; w = value.z; } }
        public double3 zzx { get => new double3(z,z,x); }
        public double3 zzy { get => new double3(z,z,y); }
        public double3 zzz { get => new double3(z,z,z); }
        public double3 zzw { get => new double3(z,z,w); }
        public double3 zwx { get => new double3(z,w,x); set { z = value.x; w = value.y; x = value.z; } }
        public double3 zwy { get => new double3(z,w,y); set { z = value.x; w = value.y; y = value.z; } }
        public double3 zwz { get => new double3(z,w,z); }
        public double3 zww { get => new double3(z,w,w); }
        public double3 wxx { get => new double3(w,x,x); }
        public double3 wxy { get => new double3(w,x,y); set { w = value.x; x = value.y; y = value.z; } }
        public double3 wxz { get => new double3(w,x,z); set { w = value.x; x = value.y; z = value.z; } }
        public double3 wxw { get => new double3(w,x,w); }
        public double3 wyx { get => new double3(w,y,x); set { w = value.x; y = value.y; x = value.z; } }
        public double3 wyy { get => new double3(w,y,y); }
        public double3 wyz { get => new double3(w,y,z); set { w = value.x; y = value.y; z = value.z; } }
        public double3 wyw { get => new double3(w,y,w); }
        public double3 wzx { get => new double3(w,z,x); set { w = value.x; z = value.y; x = value.z; } }
        public double3 wzy { get => new double3(w,z,y); set { w = value.x; z = value.y; y = value.z; } }
        public double3 wzz { get => new double3(w,z,z); }
        public double3 wzw { get => new double3(w,z,w); }
        public double3 wwx { get => new double3(w,w,x); }
        public double3 wwy { get => new double3(w,w,y); }
        public double3 wwz { get => new double3(w,w,z); }
        public double3 www { get => new double3(w,w,w); }
        
        public double4 xxxx { get => new double4(x,x,x,x); }
        public double4 xxxy { get => new double4(x,x,x,y); }
        public double4 xxxz { get => new double4(x,x,x,z); }
        public double4 xxxw { get => new double4(x,x,x,w); }
        public double4 xxyx { get => new double4(x,x,y,x); }
        public double4 xxyy { get => new double4(x,x,y,y); }
        public double4 xxyz { get => new double4(x,x,y,z); }
        public double4 xxyw { get => new double4(x,x,y,w); }
        public double4 xxzx { get => new double4(x,x,z,x); }
        public double4 xxzy { get => new double4(x,x,z,y); }
        public double4 xxzz { get => new double4(x,x,z,z); }
        public double4 xxzw { get => new double4(x,x,z,w); }
        public double4 xxwx { get => new double4(x,x,w,x); }
        public double4 xxwy { get => new double4(x,x,w,y); }
        public double4 xxwz { get => new double4(x,x,w,z); }
        public double4 xxww { get => new double4(x,x,w,w); }
        public double4 xyxx { get => new double4(x,y,x,x); }
        public double4 xyxy { get => new double4(x,y,x,y); }
        public double4 xyxz { get => new double4(x,y,x,z); }
        public double4 xyxw { get => new double4(x,y,x,w); }
        public double4 xyyx { get => new double4(x,y,y,x); }
        public double4 xyyy { get => new double4(x,y,y,y); }
        public double4 xyyz { get => new double4(x,y,y,z); }
        public double4 xyyw { get => new double4(x,y,y,w); }
        public double4 xyzx { get => new double4(x,y,z,x); }
        public double4 xyzy { get => new double4(x,y,z,y); }
        public double4 xyzz { get => new double4(x,y,z,z); }
        public double4 xyzw { get => new double4(x,y,z,w); set { x = value.x; y = value.y; z = value.z; w = value.w; } }
        public double4 xywx { get => new double4(x,y,w,x); }
        public double4 xywy { get => new double4(x,y,w,y); }
        public double4 xywz { get => new double4(x,y,w,z); set { x = value.x; y = value.y; w = value.z; z = value.w; } }
        public double4 xyww { get => new double4(x,y,w,w); }
        public double4 xzxx { get => new double4(x,z,x,x); }
        public double4 xzxy { get => new double4(x,z,x,y); }
        public double4 xzxz { get => new double4(x,z,x,z); }
        public double4 xzxw { get => new double4(x,z,x,w); }
        public double4 xzyx { get => new double4(x,z,y,x); }
        public double4 xzyy { get => new double4(x,z,y,y); }
        public double4 xzyz { get => new double4(x,z,y,z); }
        public double4 xzyw { get => new double4(x,z,y,w); set { x = value.x; z = value.y; y = value.z; w = value.w; } }
        public double4 xzzx { get => new double4(x,z,z,x); }
        public double4 xzzy { get => new double4(x,z,z,y); }
        public double4 xzzz { get => new double4(x,z,z,z); }
        public double4 xzzw { get => new double4(x,z,z,w); }
        public double4 xzwx { get => new double4(x,z,w,x); }
        public double4 xzwy { get => new double4(x,z,w,y); set { x = value.x; z = value.y; w = value.z; y = value.w; } }
        public double4 xzwz { get => new double4(x,z,w,z); }
        public double4 xzww { get => new double4(x,z,w,w); }
        public double4 xwxx { get => new double4(x,w,x,x); }
        public double4 xwxy { get => new double4(x,w,x,y); }
        public double4 xwxz { get => new double4(x,w,x,z); }
        public double4 xwxw { get => new double4(x,w,x,w); }
        public double4 xwyx { get => new double4(x,w,y,x); }
        public double4 xwyy { get => new double4(x,w,y,y); }
        public double4 xwyz { get => new double4(x,w,y,z); set { x = value.x; w = value.y; y = value.z; z = value.w; } }
        public double4 xwyw { get => new double4(x,w,y,w); }
        public double4 xwzx { get => new double4(x,w,z,x); }
        public double4 xwzy { get => new double4(x,w,z,y); set { x = value.x; w = value.y; z = value.z; y = value.w; } }
        public double4 xwzz { get => new double4(x,w,z,z); }
        public double4 xwzw { get => new double4(x,w,z,w); }
        public double4 xwwx { get => new double4(x,w,w,x); }
        public double4 xwwy { get => new double4(x,w,w,y); }
        public double4 xwwz { get => new double4(x,w,w,z); }
        public double4 xwww { get => new double4(x,w,w,w); }
        public double4 yxxx { get => new double4(y,x,x,x); }
        public double4 yxxy { get => new double4(y,x,x,y); }
        public double4 yxxz { get => new double4(y,x,x,z); }
        public double4 yxxw { get => new double4(y,x,x,w); }
        public double4 yxyx { get => new double4(y,x,y,x); }
        public double4 yxyy { get => new double4(y,x,y,y); }
        public double4 yxyz { get => new double4(y,x,y,z); }
        public double4 yxyw { get => new double4(y,x,y,w); }
        public double4 yxzx { get => new double4(y,x,z,x); }
        public double4 yxzy { get => new double4(y,x,z,y); }
        public double4 yxzz { get => new double4(y,x,z,z); }
        public double4 yxzw { get => new double4(y,x,z,w); set { y = value.x; x = value.y; z = value.z; w = value.w; } }
        public double4 yxwx { get => new double4(y,x,w,x); }
        public double4 yxwy { get => new double4(y,x,w,y); }
        public double4 yxwz { get => new double4(y,x,w,z); set { y = value.x; x = value.y; w = value.z; z = value.w; } }
        public double4 yxww { get => new double4(y,x,w,w); }
        public double4 yyxx { get => new double4(y,y,x,x); }
        public double4 yyxy { get => new double4(y,y,x,y); }
        public double4 yyxz { get => new double4(y,y,x,z); }
        public double4 yyxw { get => new double4(y,y,x,w); }
        public double4 yyyx { get => new double4(y,y,y,x); }
        public double4 yyyy { get => new double4(y,y,y,y); }
        public double4 yyyz { get => new double4(y,y,y,z); }
        public double4 yyyw { get => new double4(y,y,y,w); }
        public double4 yyzx { get => new double4(y,y,z,x); }
        public double4 yyzy { get => new double4(y,y,z,y); }
        public double4 yyzz { get => new double4(y,y,z,z); }
        public double4 yyzw { get => new double4(y,y,z,w); }
        public double4 yywx { get => new double4(y,y,w,x); }
        public double4 yywy { get => new double4(y,y,w,y); }
        public double4 yywz { get => new double4(y,y,w,z); }
        public double4 yyww { get => new double4(y,y,w,w); }
        public double4 yzxx { get => new double4(y,z,x,x); }
        public double4 yzxy { get => new double4(y,z,x,y); }
        public double4 yzxz { get => new double4(y,z,x,z); }
        public double4 yzxw { get => new double4(y,z,x,w); set { y = value.x; z = value.y; x = value.z; w = value.w; } }
        public double4 yzyx { get => new double4(y,z,y,x); }
        public double4 yzyy { get => new double4(y,z,y,y); }
        public double4 yzyz { get => new double4(y,z,y,z); }
        public double4 yzyw { get => new double4(y,z,y,w); }
        public double4 yzzx { get => new double4(y,z,z,x); }
        public double4 yzzy { get => new double4(y,z,z,y); }
        public double4 yzzz { get => new double4(y,z,z,z); }
        public double4 yzzw { get => new double4(y,z,z,w); }
        public double4 yzwx { get => new double4(y,z,w,x); set { y = value.x; z = value.y; w = value.z; x = value.w; } }
        public double4 yzwy { get => new double4(y,z,w,y); }
        public double4 yzwz { get => new double4(y,z,w,z); }
        public double4 yzww { get => new double4(y,z,w,w); }
        public double4 ywxx { get => new double4(y,w,x,x); }
        public double4 ywxy { get => new double4(y,w,x,y); }
        public double4 ywxz { get => new double4(y,w,x,z); set { y = value.x; w = value.y; x = value.z; z = value.w; } }
        public double4 ywxw { get => new double4(y,w,x,w); }
        public double4 ywyx { get => new double4(y,w,y,x); }
        public double4 ywyy { get => new double4(y,w,y,y); }
        public double4 ywyz { get => new double4(y,w,y,z); }
        public double4 ywyw { get => new double4(y,w,y,w); }
        public double4 ywzx { get => new double4(y,w,z,x); set { y = value.x; w = value.y; z = value.z; x = value.w; } }
        public double4 ywzy { get => new double4(y,w,z,y); }
        public double4 ywzz { get => new double4(y,w,z,z); }
        public double4 ywzw { get => new double4(y,w,z,w); }
        public double4 ywwx { get => new double4(y,w,w,x); }
        public double4 ywwy { get => new double4(y,w,w,y); }
        public double4 ywwz { get => new double4(y,w,w,z); }
        public double4 ywww { get => new double4(y,w,w,w); }
        public double4 zxxx { get => new double4(z,x,x,x); }
        public double4 zxxy { get => new double4(z,x,x,y); }
        public double4 zxxz { get => new double4(z,x,x,z); }
        public double4 zxxw { get => new double4(z,x,x,w); }
        public double4 zxyx { get => new double4(z,x,y,x); }
        public double4 zxyy { get => new double4(z,x,y,y); }
        public double4 zxyz { get => new double4(z,x,y,z); }
        public double4 zxyw { get => new double4(z,x,y,w); set { z = value.x; x = value.y; y = value.z; w = value.w; } }
        public double4 zxzx { get => new double4(z,x,z,x); }
        public double4 zxzy { get => new double4(z,x,z,y); }
        public double4 zxzz { get => new double4(z,x,z,z); }
        public double4 zxzw { get => new double4(z,x,z,w); }
        public double4 zxwx { get => new double4(z,x,w,x); }
        public double4 zxwy { get => new double4(z,x,w,y); set { z = value.x; x = value.y; w = value.z; y = value.w; } }
        public double4 zxwz { get => new double4(z,x,w,z); }
        public double4 zxww { get => new double4(z,x,w,w); }
        public double4 zyxx { get => new double4(z,y,x,x); }
        public double4 zyxy { get => new double4(z,y,x,y); }
        public double4 zyxz { get => new double4(z,y,x,z); }
        public double4 zyxw { get => new double4(z,y,x,w); set { z = value.x; y = value.y; x = value.z; w = value.w; } }
        public double4 zyyx { get => new double4(z,y,y,x); }
        public double4 zyyy { get => new double4(z,y,y,y); }
        public double4 zyyz { get => new double4(z,y,y,z); }
        public double4 zyyw { get => new double4(z,y,y,w); }
        public double4 zyzx { get => new double4(z,y,z,x); }
        public double4 zyzy { get => new double4(z,y,z,y); }
        public double4 zyzz { get => new double4(z,y,z,z); }
        public double4 zyzw { get => new double4(z,y,z,w); }
        public double4 zywx { get => new double4(z,y,w,x); set { z = value.x; y = value.y; w = value.z; x = value.w; } }
        public double4 zywy { get => new double4(z,y,w,y); }
        public double4 zywz { get => new double4(z,y,w,z); }
        public double4 zyww { get => new double4(z,y,w,w); }
        public double4 zzxx { get => new double4(z,z,x,x); }
        public double4 zzxy { get => new double4(z,z,x,y); }
        public double4 zzxz { get => new double4(z,z,x,z); }
        public double4 zzxw { get => new double4(z,z,x,w); }
        public double4 zzyx { get => new double4(z,z,y,x); }
        public double4 zzyy { get => new double4(z,z,y,y); }
        public double4 zzyz { get => new double4(z,z,y,z); }
        public double4 zzyw { get => new double4(z,z,y,w); }
        public double4 zzzx { get => new double4(z,z,z,x); }
        public double4 zzzy { get => new double4(z,z,z,y); }
        public double4 zzzz { get => new double4(z,z,z,z); }
        public double4 zzzw { get => new double4(z,z,z,w); }
        public double4 zzwx { get => new double4(z,z,w,x); }
        public double4 zzwy { get => new double4(z,z,w,y); }
        public double4 zzwz { get => new double4(z,z,w,z); }
        public double4 zzww { get => new double4(z,z,w,w); }
        public double4 zwxx { get => new double4(z,w,x,x); }
        public double4 zwxy { get => new double4(z,w,x,y); set { z = value.x; w = value.y; x = value.z; y = value.w; } }
        public double4 zwxz { get => new double4(z,w,x,z); }
        public double4 zwxw { get => new double4(z,w,x,w); }
        public double4 zwyx { get => new double4(z,w,y,x); set { z = value.x; w = value.y; y = value.z; x = value.w; } }
        public double4 zwyy { get => new double4(z,w,y,y); }
        public double4 zwyz { get => new double4(z,w,y,z); }
        public double4 zwyw { get => new double4(z,w,y,w); }
        public double4 zwzx { get => new double4(z,w,z,x); }
        public double4 zwzy { get => new double4(z,w,z,y); }
        public double4 zwzz { get => new double4(z,w,z,z); }
        public double4 zwzw { get => new double4(z,w,z,w); }
        public double4 zwwx { get => new double4(z,w,w,x); }
        public double4 zwwy { get => new double4(z,w,w,y); }
        public double4 zwwz { get => new double4(z,w,w,z); }
        public double4 zwww { get => new double4(z,w,w,w); }
        public double4 wxxx { get => new double4(w,x,x,x); }
        public double4 wxxy { get => new double4(w,x,x,y); }
        public double4 wxxz { get => new double4(w,x,x,z); }
        public double4 wxxw { get => new double4(w,x,x,w); }
        public double4 wxyx { get => new double4(w,x,y,x); }
        public double4 wxyy { get => new double4(w,x,y,y); }
        public double4 wxyz { get => new double4(w,x,y,z); set { w = value.x; x = value.y; y = value.z; z = value.w; } }
        public double4 wxyw { get => new double4(w,x,y,w); }
        public double4 wxzx { get => new double4(w,x,z,x); }
        public double4 wxzy { get => new double4(w,x,z,y); set { w = value.x; x = value.y; z = value.z; y = value.w; } }
        public double4 wxzz { get => new double4(w,x,z,z); }
        public double4 wxzw { get => new double4(w,x,z,w); }
        public double4 wxwx { get => new double4(w,x,w,x); }
        public double4 wxwy { get => new double4(w,x,w,y); }
        public double4 wxwz { get => new double4(w,x,w,z); }
        public double4 wxww { get => new double4(w,x,w,w); }
        public double4 wyxx { get => new double4(w,y,x,x); }
        public double4 wyxy { get => new double4(w,y,x,y); }
        public double4 wyxz { get => new double4(w,y,x,z); set { w = value.x; y = value.y; x = value.z; z = value.w; } }
        public double4 wyxw { get => new double4(w,y,x,w); }
        public double4 wyyx { get => new double4(w,y,y,x); }
        public double4 wyyy { get => new double4(w,y,y,y); }
        public double4 wyyz { get => new double4(w,y,y,z); }
        public double4 wyyw { get => new double4(w,y,y,w); }
        public double4 wyzx { get => new double4(w,y,z,x); set { w = value.x; y = value.y; z = value.z; x = value.w; } }
        public double4 wyzy { get => new double4(w,y,z,y); }
        public double4 wyzz { get => new double4(w,y,z,z); }
        public double4 wyzw { get => new double4(w,y,z,w); }
        public double4 wywx { get => new double4(w,y,w,x); }
        public double4 wywy { get => new double4(w,y,w,y); }
        public double4 wywz { get => new double4(w,y,w,z); }
        public double4 wyww { get => new double4(w,y,w,w); }
        public double4 wzxx { get => new double4(w,z,x,x); }
        public double4 wzxy { get => new double4(w,z,x,y); set { w = value.x; z = value.y; x = value.z; y = value.w; } }
        public double4 wzxz { get => new double4(w,z,x,z); }
        public double4 wzxw { get => new double4(w,z,x,w); }
        public double4 wzyx { get => new double4(w,z,y,x); set { w = value.x; z = value.y; y = value.z; x = value.w; } }
        public double4 wzyy { get => new double4(w,z,y,y); }
        public double4 wzyz { get => new double4(w,z,y,z); }
        public double4 wzyw { get => new double4(w,z,y,w); }
        public double4 wzzx { get => new double4(w,z,z,x); }
        public double4 wzzy { get => new double4(w,z,z,y); }
        public double4 wzzz { get => new double4(w,z,z,z); }
        public double4 wzzw { get => new double4(w,z,z,w); }
        public double4 wzwx { get => new double4(w,z,w,x); }
        public double4 wzwy { get => new double4(w,z,w,y); }
        public double4 wzwz { get => new double4(w,z,w,z); }
        public double4 wzww { get => new double4(w,z,w,w); }
        public double4 wwxx { get => new double4(w,w,x,x); }
        public double4 wwxy { get => new double4(w,w,x,y); }
        public double4 wwxz { get => new double4(w,w,x,z); }
        public double4 wwxw { get => new double4(w,w,x,w); }
        public double4 wwyx { get => new double4(w,w,y,x); }
        public double4 wwyy { get => new double4(w,w,y,y); }
        public double4 wwyz { get => new double4(w,w,y,z); }
        public double4 wwyw { get => new double4(w,w,y,w); }
        public double4 wwzx { get => new double4(w,w,z,x); }
        public double4 wwzy { get => new double4(w,w,z,y); }
        public double4 wwzz { get => new double4(w,w,z,z); }
        public double4 wwzw { get => new double4(w,w,z,w); }
        public double4 wwwx { get => new double4(w,w,w,x); }
        public double4 wwwy { get => new double4(w,w,w,y); }
        public double4 wwwz { get => new double4(w,w,w,z); }
        public double4 wwww { get => new double4(w,w,w,w); }

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
        
        public int2 xx { get => new int2(x,x); }
        public int2 yy { get => new int2(y,y); }
        public int2 xy { get => new int2(x,y); set { x = value.x; y = value.y; } }
        public int2 yx { get => new int2(y,x); set { y = value.x; x = value.y; } }

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
        
        public int2 xx { get => new int2(x,x); }
        public int2 yy { get => new int2(y,y); }
        public int2 zz { get => new int2(z,z); }
        
        public int2 xy { get => new int2(x,y); set { x = value.x; y = value.y; } }
        public int2 xz { get => new int2(x,z); set { x = value.x; z = value.y; } }
        public int2 yx { get => new int2(y,x); set { y = value.x; x = value.y; } }
        public int2 yz { get => new int2(y,z); set { y = value.x; z = value.y; } }
        public int2 zy { get => new int2(z,y); set { z = value.x; y = value.y; } }
        public int2 zx { get => new int2(z,x); set { z = value.x; x = value.y; } }
        
        public int3 xxx { get => new int3(x,x,x); }
        public int3 xxy { get => new int3(x,x,y); }
        public int3 xxz { get => new int3(x,x,z); }
        public int3 xyx { get => new int3(x,y,x); }
        public int3 xyy { get => new int3(x,y,y); }
        public int3 xzx { get => new int3(x,z,x); }
        public int3 xzz { get => new int3(x,z,z); }
        public int3 yxx { get => new int3(y,x,x); }
        public int3 yxy { get => new int3(y,x,y); }
        public int3 yyx { get => new int3(y,y,x); }
        public int3 yyy { get => new int3(y,y,y); }
        public int3 yyz { get => new int3(y,y,z); }
        public int3 yzy { get => new int3(y,z,y); }
        public int3 yzz { get => new int3(y,z,z); }
        public int3 zxx { get => new int3(z,x,x); }
        public int3 zxz { get => new int3(z,x,z); }
        public int3 zyy { get => new int3(z,y,y); }
        public int3 zyz { get => new int3(z,y,z); }
        public int3 zzx { get => new int3(z,z,x); }
        public int3 zzy { get => new int3(z,z,y); }
        public int3 zzz { get => new int3(z,z,z); }

        public int3 xyz { get => new int3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public int3 xzy { get => new int3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public int3 yxz { get => new int3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public int3 yzx { get => new int3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public int3 zxy { get => new int3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public int3 zyx { get => new int3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }

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
        
        public int2 xx { get => new int2(x,x); }
        public int2 xy { get => new int2(x,y); set { x = value.x; y = value.y; } }
        public int2 xz { get => new int2(x,z); set { x = value.x; z = value.y; } }
        public int2 xw { get => new int2(x,w); set { x = value.x; w = value.y; } }
        public int2 yx { get => new int2(y,x); set { y = value.x; x = value.y; } }
        public int2 yy { get => new int2(y,y); }
        public int2 yz { get => new int2(y,z); set { y = value.x; z = value.y; } }
        public int2 yw { get => new int2(y,w); set { y = value.x; w = value.y; } }
        public int2 zx { get => new int2(z,x); set { z = value.x; x = value.y; } }
        public int2 zy { get => new int2(z,y); set { z = value.x; y = value.y; } }
        public int2 zz { get => new int2(z,z); }
        public int2 zw { get => new int2(z,w); set { z = value.x; w = value.y; } }
        public int2 wx { get => new int2(w,x); set { w = value.x; x = value.y; } }
        public int2 wy { get => new int2(w,y); set { w = value.x; y = value.y; } }
        public int2 wz { get => new int2(w,z); set { w = value.x; z = value.y; } }
        public int2 ww { get => new int2(w,w); }
        
        public int3 xxx { get => new int3(x,x,x); }
        public int3 xxy { get => new int3(x,x,y); }
        public int3 xxz { get => new int3(x,x,z); }
        public int3 xxw { get => new int3(x,x,w); }
        public int3 xyx { get => new int3(x,y,x); }
        public int3 xyy { get => new int3(x,y,y); }
        public int3 xyz { get => new int3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public int3 xyw { get => new int3(x,y,w); set { x = value.x; y = value.y; w = value.z; } }
        public int3 xzx { get => new int3(x,z,x); }
        public int3 xzy { get => new int3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public int3 xzz { get => new int3(x,z,z); }
        public int3 xzw { get => new int3(x,z,w); set { x = value.x; z = value.y; w = value.z; } }
        public int3 xwx { get => new int3(x,w,x); }
        public int3 xwy { get => new int3(x,w,y); set { x = value.x; w = value.y; y = value.z; } }
        public int3 xwz { get => new int3(x,w,z); set { x = value.x; w = value.y; z = value.z; } }
        public int3 xww { get => new int3(x,w,w); }
        public int3 yxx { get => new int3(y,x,x); }
        public int3 yxy { get => new int3(y,x,y); }
        public int3 yxz { get => new int3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public int3 yxw { get => new int3(y,x,w); set { y = value.x; x = value.y; w = value.z; } }
        public int3 yyx { get => new int3(y,y,x); }
        public int3 yyy { get => new int3(y,y,y); }
        public int3 yyz { get => new int3(y,y,z); }
        public int3 yyw { get => new int3(y,y,w); }
        public int3 yzx { get => new int3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public int3 yzy { get => new int3(y,z,y); }
        public int3 yzz { get => new int3(y,z,z); }
        public int3 yzw { get => new int3(y,z,w); set { y = value.x; z = value.y; w = value.z; } }
        public int3 ywx { get => new int3(y,w,x); set { y = value.x; w = value.y; x = value.z; } }
        public int3 ywy { get => new int3(y,w,y); }
        public int3 ywz { get => new int3(y,w,z); set { y = value.x; w = value.y; z = value.z; } }
        public int3 yww { get => new int3(y,w,w); }
        public int3 zxx { get => new int3(z,x,x); }
        public int3 zxy { get => new int3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public int3 zxz { get => new int3(z,x,z); }
        public int3 zxw { get => new int3(z,x,w); set { z = value.x; x = value.y; w = value.z; } }
        public int3 zyx { get => new int3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }
        public int3 zyy { get => new int3(z,y,y); }
        public int3 zyz { get => new int3(z,y,z); }
        public int3 zyw { get => new int3(z,y,w); set { z = value.x; y = value.y; w = value.z; } }
        public int3 zzx { get => new int3(z,z,x); }
        public int3 zzy { get => new int3(z,z,y); }
        public int3 zzz { get => new int3(z,z,z); }
        public int3 zzw { get => new int3(z,z,w); }
        public int3 zwx { get => new int3(z,w,x); set { z = value.x; w = value.y; x = value.z; } }
        public int3 zwy { get => new int3(z,w,y); set { z = value.x; w = value.y; y = value.z; } }
        public int3 zwz { get => new int3(z,w,z); }
        public int3 zww { get => new int3(z,w,w); }
        public int3 wxx { get => new int3(w,x,x); }
        public int3 wxy { get => new int3(w,x,y); set { w = value.x; x = value.y; y = value.z; } }
        public int3 wxz { get => new int3(w,x,z); set { w = value.x; x = value.y; z = value.z; } }
        public int3 wxw { get => new int3(w,x,w); }
        public int3 wyx { get => new int3(w,y,x); set { w = value.x; y = value.y; x = value.z; } }
        public int3 wyy { get => new int3(w,y,y); }
        public int3 wyz { get => new int3(w,y,z); set { w = value.x; y = value.y; z = value.z; } }
        public int3 wyw { get => new int3(w,y,w); }
        public int3 wzx { get => new int3(w,z,x); set { w = value.x; z = value.y; x = value.z; } }
        public int3 wzy { get => new int3(w,z,y); set { w = value.x; z = value.y; y = value.z; } }
        public int3 wzz { get => new int3(w,z,z); }
        public int3 wzw { get => new int3(w,z,w); }
        public int3 wwx { get => new int3(w,w,x); }
        public int3 wwy { get => new int3(w,w,y); }
        public int3 wwz { get => new int3(w,w,z); }
        public int3 www { get => new int3(w,w,w); }
        
        public int4 xxxx { get => new int4(x,x,x,x); }
        public int4 xxxy { get => new int4(x,x,x,y); }
        public int4 xxxz { get => new int4(x,x,x,z); }
        public int4 xxxw { get => new int4(x,x,x,w); }
        public int4 xxyx { get => new int4(x,x,y,x); }
        public int4 xxyy { get => new int4(x,x,y,y); }
        public int4 xxyz { get => new int4(x,x,y,z); }
        public int4 xxyw { get => new int4(x,x,y,w); }
        public int4 xxzx { get => new int4(x,x,z,x); }
        public int4 xxzy { get => new int4(x,x,z,y); }
        public int4 xxzz { get => new int4(x,x,z,z); }
        public int4 xxzw { get => new int4(x,x,z,w); }
        public int4 xxwx { get => new int4(x,x,w,x); }
        public int4 xxwy { get => new int4(x,x,w,y); }
        public int4 xxwz { get => new int4(x,x,w,z); }
        public int4 xxww { get => new int4(x,x,w,w); }
        public int4 xyxx { get => new int4(x,y,x,x); }
        public int4 xyxy { get => new int4(x,y,x,y); }
        public int4 xyxz { get => new int4(x,y,x,z); }
        public int4 xyxw { get => new int4(x,y,x,w); }
        public int4 xyyx { get => new int4(x,y,y,x); }
        public int4 xyyy { get => new int4(x,y,y,y); }
        public int4 xyyz { get => new int4(x,y,y,z); }
        public int4 xyyw { get => new int4(x,y,y,w); }
        public int4 xyzx { get => new int4(x,y,z,x); }
        public int4 xyzy { get => new int4(x,y,z,y); }
        public int4 xyzz { get => new int4(x,y,z,z); }
        public int4 xyzw { get => new int4(x,y,z,w); set { x = value.x; y = value.y; z = value.z; w = value.w; } }
        public int4 xywx { get => new int4(x,y,w,x); }
        public int4 xywy { get => new int4(x,y,w,y); }
        public int4 xywz { get => new int4(x,y,w,z); set { x = value.x; y = value.y; w = value.z; z = value.w; } }
        public int4 xyww { get => new int4(x,y,w,w); }
        public int4 xzxx { get => new int4(x,z,x,x); }
        public int4 xzxy { get => new int4(x,z,x,y); }
        public int4 xzxz { get => new int4(x,z,x,z); }
        public int4 xzxw { get => new int4(x,z,x,w); }
        public int4 xzyx { get => new int4(x,z,y,x); }
        public int4 xzyy { get => new int4(x,z,y,y); }
        public int4 xzyz { get => new int4(x,z,y,z); }
        public int4 xzyw { get => new int4(x,z,y,w); set { x = value.x; z = value.y; y = value.z; w = value.w; } }
        public int4 xzzx { get => new int4(x,z,z,x); }
        public int4 xzzy { get => new int4(x,z,z,y); }
        public int4 xzzz { get => new int4(x,z,z,z); }
        public int4 xzzw { get => new int4(x,z,z,w); }
        public int4 xzwx { get => new int4(x,z,w,x); }
        public int4 xzwy { get => new int4(x,z,w,y); set { x = value.x; z = value.y; w = value.z; y = value.w; } }
        public int4 xzwz { get => new int4(x,z,w,z); }
        public int4 xzww { get => new int4(x,z,w,w); }
        public int4 xwxx { get => new int4(x,w,x,x); }
        public int4 xwxy { get => new int4(x,w,x,y); }
        public int4 xwxz { get => new int4(x,w,x,z); }
        public int4 xwxw { get => new int4(x,w,x,w); }
        public int4 xwyx { get => new int4(x,w,y,x); }
        public int4 xwyy { get => new int4(x,w,y,y); }
        public int4 xwyz { get => new int4(x,w,y,z); set { x = value.x; w = value.y; y = value.z; z = value.w; } }
        public int4 xwyw { get => new int4(x,w,y,w); }
        public int4 xwzx { get => new int4(x,w,z,x); }
        public int4 xwzy { get => new int4(x,w,z,y); set { x = value.x; w = value.y; z = value.z; y = value.w; } }
        public int4 xwzz { get => new int4(x,w,z,z); }
        public int4 xwzw { get => new int4(x,w,z,w); }
        public int4 xwwx { get => new int4(x,w,w,x); }
        public int4 xwwy { get => new int4(x,w,w,y); }
        public int4 xwwz { get => new int4(x,w,w,z); }
        public int4 xwww { get => new int4(x,w,w,w); }
        public int4 yxxx { get => new int4(y,x,x,x); }
        public int4 yxxy { get => new int4(y,x,x,y); }
        public int4 yxxz { get => new int4(y,x,x,z); }
        public int4 yxxw { get => new int4(y,x,x,w); }
        public int4 yxyx { get => new int4(y,x,y,x); }
        public int4 yxyy { get => new int4(y,x,y,y); }
        public int4 yxyz { get => new int4(y,x,y,z); }
        public int4 yxyw { get => new int4(y,x,y,w); }
        public int4 yxzx { get => new int4(y,x,z,x); }
        public int4 yxzy { get => new int4(y,x,z,y); }
        public int4 yxzz { get => new int4(y,x,z,z); }
        public int4 yxzw { get => new int4(y,x,z,w); set { y = value.x; x = value.y; z = value.z; w = value.w; } }
        public int4 yxwx { get => new int4(y,x,w,x); }
        public int4 yxwy { get => new int4(y,x,w,y); }
        public int4 yxwz { get => new int4(y,x,w,z); set { y = value.x; x = value.y; w = value.z; z = value.w; } }
        public int4 yxww { get => new int4(y,x,w,w); }
        public int4 yyxx { get => new int4(y,y,x,x); }
        public int4 yyxy { get => new int4(y,y,x,y); }
        public int4 yyxz { get => new int4(y,y,x,z); }
        public int4 yyxw { get => new int4(y,y,x,w); }
        public int4 yyyx { get => new int4(y,y,y,x); }
        public int4 yyyy { get => new int4(y,y,y,y); }
        public int4 yyyz { get => new int4(y,y,y,z); }
        public int4 yyyw { get => new int4(y,y,y,w); }
        public int4 yyzx { get => new int4(y,y,z,x); }
        public int4 yyzy { get => new int4(y,y,z,y); }
        public int4 yyzz { get => new int4(y,y,z,z); }
        public int4 yyzw { get => new int4(y,y,z,w); }
        public int4 yywx { get => new int4(y,y,w,x); }
        public int4 yywy { get => new int4(y,y,w,y); }
        public int4 yywz { get => new int4(y,y,w,z); }
        public int4 yyww { get => new int4(y,y,w,w); }
        public int4 yzxx { get => new int4(y,z,x,x); }
        public int4 yzxy { get => new int4(y,z,x,y); }
        public int4 yzxz { get => new int4(y,z,x,z); }
        public int4 yzxw { get => new int4(y,z,x,w); set { y = value.x; z = value.y; x = value.z; w = value.w; } }
        public int4 yzyx { get => new int4(y,z,y,x); }
        public int4 yzyy { get => new int4(y,z,y,y); }
        public int4 yzyz { get => new int4(y,z,y,z); }
        public int4 yzyw { get => new int4(y,z,y,w); }
        public int4 yzzx { get => new int4(y,z,z,x); }
        public int4 yzzy { get => new int4(y,z,z,y); }
        public int4 yzzz { get => new int4(y,z,z,z); }
        public int4 yzzw { get => new int4(y,z,z,w); }
        public int4 yzwx { get => new int4(y,z,w,x); set { y = value.x; z = value.y; w = value.z; x = value.w; } }
        public int4 yzwy { get => new int4(y,z,w,y); }
        public int4 yzwz { get => new int4(y,z,w,z); }
        public int4 yzww { get => new int4(y,z,w,w); }
        public int4 ywxx { get => new int4(y,w,x,x); }
        public int4 ywxy { get => new int4(y,w,x,y); }
        public int4 ywxz { get => new int4(y,w,x,z); set { y = value.x; w = value.y; x = value.z; z = value.w; } }
        public int4 ywxw { get => new int4(y,w,x,w); }
        public int4 ywyx { get => new int4(y,w,y,x); }
        public int4 ywyy { get => new int4(y,w,y,y); }
        public int4 ywyz { get => new int4(y,w,y,z); }
        public int4 ywyw { get => new int4(y,w,y,w); }
        public int4 ywzx { get => new int4(y,w,z,x); set { y = value.x; w = value.y; z = value.z; x = value.w; } }
        public int4 ywzy { get => new int4(y,w,z,y); }
        public int4 ywzz { get => new int4(y,w,z,z); }
        public int4 ywzw { get => new int4(y,w,z,w); }
        public int4 ywwx { get => new int4(y,w,w,x); }
        public int4 ywwy { get => new int4(y,w,w,y); }
        public int4 ywwz { get => new int4(y,w,w,z); }
        public int4 ywww { get => new int4(y,w,w,w); }
        public int4 zxxx { get => new int4(z,x,x,x); }
        public int4 zxxy { get => new int4(z,x,x,y); }
        public int4 zxxz { get => new int4(z,x,x,z); }
        public int4 zxxw { get => new int4(z,x,x,w); }
        public int4 zxyx { get => new int4(z,x,y,x); }
        public int4 zxyy { get => new int4(z,x,y,y); }
        public int4 zxyz { get => new int4(z,x,y,z); }
        public int4 zxyw { get => new int4(z,x,y,w); set { z = value.x; x = value.y; y = value.z; w = value.w; } }
        public int4 zxzx { get => new int4(z,x,z,x); }
        public int4 zxzy { get => new int4(z,x,z,y); }
        public int4 zxzz { get => new int4(z,x,z,z); }
        public int4 zxzw { get => new int4(z,x,z,w); }
        public int4 zxwx { get => new int4(z,x,w,x); }
        public int4 zxwy { get => new int4(z,x,w,y); set { z = value.x; x = value.y; w = value.z; y = value.w; } }
        public int4 zxwz { get => new int4(z,x,w,z); }
        public int4 zxww { get => new int4(z,x,w,w); }
        public int4 zyxx { get => new int4(z,y,x,x); }
        public int4 zyxy { get => new int4(z,y,x,y); }
        public int4 zyxz { get => new int4(z,y,x,z); }
        public int4 zyxw { get => new int4(z,y,x,w); set { z = value.x; y = value.y; x = value.z; w = value.w; } }
        public int4 zyyx { get => new int4(z,y,y,x); }
        public int4 zyyy { get => new int4(z,y,y,y); }
        public int4 zyyz { get => new int4(z,y,y,z); }
        public int4 zyyw { get => new int4(z,y,y,w); }
        public int4 zyzx { get => new int4(z,y,z,x); }
        public int4 zyzy { get => new int4(z,y,z,y); }
        public int4 zyzz { get => new int4(z,y,z,z); }
        public int4 zyzw { get => new int4(z,y,z,w); }
        public int4 zywx { get => new int4(z,y,w,x); set { z = value.x; y = value.y; w = value.z; x = value.w; } }
        public int4 zywy { get => new int4(z,y,w,y); }
        public int4 zywz { get => new int4(z,y,w,z); }
        public int4 zyww { get => new int4(z,y,w,w); }
        public int4 zzxx { get => new int4(z,z,x,x); }
        public int4 zzxy { get => new int4(z,z,x,y); }
        public int4 zzxz { get => new int4(z,z,x,z); }
        public int4 zzxw { get => new int4(z,z,x,w); }
        public int4 zzyx { get => new int4(z,z,y,x); }
        public int4 zzyy { get => new int4(z,z,y,y); }
        public int4 zzyz { get => new int4(z,z,y,z); }
        public int4 zzyw { get => new int4(z,z,y,w); }
        public int4 zzzx { get => new int4(z,z,z,x); }
        public int4 zzzy { get => new int4(z,z,z,y); }
        public int4 zzzz { get => new int4(z,z,z,z); }
        public int4 zzzw { get => new int4(z,z,z,w); }
        public int4 zzwx { get => new int4(z,z,w,x); }
        public int4 zzwy { get => new int4(z,z,w,y); }
        public int4 zzwz { get => new int4(z,z,w,z); }
        public int4 zzww { get => new int4(z,z,w,w); }
        public int4 zwxx { get => new int4(z,w,x,x); }
        public int4 zwxy { get => new int4(z,w,x,y); set { z = value.x; w = value.y; x = value.z; y = value.w; } }
        public int4 zwxz { get => new int4(z,w,x,z); }
        public int4 zwxw { get => new int4(z,w,x,w); }
        public int4 zwyx { get => new int4(z,w,y,x); set { z = value.x; w = value.y; y = value.z; x = value.w; } }
        public int4 zwyy { get => new int4(z,w,y,y); }
        public int4 zwyz { get => new int4(z,w,y,z); }
        public int4 zwyw { get => new int4(z,w,y,w); }
        public int4 zwzx { get => new int4(z,w,z,x); }
        public int4 zwzy { get => new int4(z,w,z,y); }
        public int4 zwzz { get => new int4(z,w,z,z); }
        public int4 zwzw { get => new int4(z,w,z,w); }
        public int4 zwwx { get => new int4(z,w,w,x); }
        public int4 zwwy { get => new int4(z,w,w,y); }
        public int4 zwwz { get => new int4(z,w,w,z); }
        public int4 zwww { get => new int4(z,w,w,w); }
        public int4 wxxx { get => new int4(w,x,x,x); }
        public int4 wxxy { get => new int4(w,x,x,y); }
        public int4 wxxz { get => new int4(w,x,x,z); }
        public int4 wxxw { get => new int4(w,x,x,w); }
        public int4 wxyx { get => new int4(w,x,y,x); }
        public int4 wxyy { get => new int4(w,x,y,y); }
        public int4 wxyz { get => new int4(w,x,y,z); set { w = value.x; x = value.y; y = value.z; z = value.w; } }
        public int4 wxyw { get => new int4(w,x,y,w); }
        public int4 wxzx { get => new int4(w,x,z,x); }
        public int4 wxzy { get => new int4(w,x,z,y); set { w = value.x; x = value.y; z = value.z; y = value.w; } }
        public int4 wxzz { get => new int4(w,x,z,z); }
        public int4 wxzw { get => new int4(w,x,z,w); }
        public int4 wxwx { get => new int4(w,x,w,x); }
        public int4 wxwy { get => new int4(w,x,w,y); }
        public int4 wxwz { get => new int4(w,x,w,z); }
        public int4 wxww { get => new int4(w,x,w,w); }
        public int4 wyxx { get => new int4(w,y,x,x); }
        public int4 wyxy { get => new int4(w,y,x,y); }
        public int4 wyxz { get => new int4(w,y,x,z); set { w = value.x; y = value.y; x = value.z; z = value.w; } }
        public int4 wyxw { get => new int4(w,y,x,w); }
        public int4 wyyx { get => new int4(w,y,y,x); }
        public int4 wyyy { get => new int4(w,y,y,y); }
        public int4 wyyz { get => new int4(w,y,y,z); }
        public int4 wyyw { get => new int4(w,y,y,w); }
        public int4 wyzx { get => new int4(w,y,z,x); set { w = value.x; y = value.y; z = value.z; x = value.w; } }
        public int4 wyzy { get => new int4(w,y,z,y); }
        public int4 wyzz { get => new int4(w,y,z,z); }
        public int4 wyzw { get => new int4(w,y,z,w); }
        public int4 wywx { get => new int4(w,y,w,x); }
        public int4 wywy { get => new int4(w,y,w,y); }
        public int4 wywz { get => new int4(w,y,w,z); }
        public int4 wyww { get => new int4(w,y,w,w); }
        public int4 wzxx { get => new int4(w,z,x,x); }
        public int4 wzxy { get => new int4(w,z,x,y); set { w = value.x; z = value.y; x = value.z; y = value.w; } }
        public int4 wzxz { get => new int4(w,z,x,z); }
        public int4 wzxw { get => new int4(w,z,x,w); }
        public int4 wzyx { get => new int4(w,z,y,x); set { w = value.x; z = value.y; y = value.z; x = value.w; } }
        public int4 wzyy { get => new int4(w,z,y,y); }
        public int4 wzyz { get => new int4(w,z,y,z); }
        public int4 wzyw { get => new int4(w,z,y,w); }
        public int4 wzzx { get => new int4(w,z,z,x); }
        public int4 wzzy { get => new int4(w,z,z,y); }
        public int4 wzzz { get => new int4(w,z,z,z); }
        public int4 wzzw { get => new int4(w,z,z,w); }
        public int4 wzwx { get => new int4(w,z,w,x); }
        public int4 wzwy { get => new int4(w,z,w,y); }
        public int4 wzwz { get => new int4(w,z,w,z); }
        public int4 wzww { get => new int4(w,z,w,w); }
        public int4 wwxx { get => new int4(w,w,x,x); }
        public int4 wwxy { get => new int4(w,w,x,y); }
        public int4 wwxz { get => new int4(w,w,x,z); }
        public int4 wwxw { get => new int4(w,w,x,w); }
        public int4 wwyx { get => new int4(w,w,y,x); }
        public int4 wwyy { get => new int4(w,w,y,y); }
        public int4 wwyz { get => new int4(w,w,y,z); }
        public int4 wwyw { get => new int4(w,w,y,w); }
        public int4 wwzx { get => new int4(w,w,z,x); }
        public int4 wwzy { get => new int4(w,w,z,y); }
        public int4 wwzz { get => new int4(w,w,z,z); }
        public int4 wwzw { get => new int4(w,w,z,w); }
        public int4 wwwx { get => new int4(w,w,w,x); }
        public int4 wwwy { get => new int4(w,w,w,y); }
        public int4 wwwz { get => new int4(w,w,w,z); }
        public int4 wwww { get => new int4(w,w,w,w); }

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
        
        public uint2 xx { get => new uint2(x,x); }
        public uint2 yy { get => new uint2(y,y); }
        public uint2 xy { get => new uint2(x,y); set { x = value.x; y = value.y; } }
        public uint2 yx { get => new uint2(y,x); set { y = value.x; x = value.y; } }

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
        
        public uint2 xx { get => new uint2(x,x); }
        public uint2 yy { get => new uint2(y,y); }
        public uint2 zz { get => new uint2(z,z); }
        
        public uint2 xy { get => new uint2(x,y); set { x = value.x; y = value.y; } }
        public uint2 xz { get => new uint2(x,z); set { x = value.x; z = value.y; } }
        public uint2 yx { get => new uint2(y,x); set { y = value.x; x = value.y; } }
        public uint2 yz { get => new uint2(y,z); set { y = value.x; z = value.y; } }
        public uint2 zy { get => new uint2(z,y); set { z = value.x; y = value.y; } }
        public uint2 zx { get => new uint2(z,x); set { z = value.x; x = value.y; } }
        
        public uint3 xxx { get => new uint3(x,x,x); }
        public uint3 xxy { get => new uint3(x,x,y); }
        public uint3 xxz { get => new uint3(x,x,z); }
        public uint3 xyx { get => new uint3(x,y,x); }
        public uint3 xyy { get => new uint3(x,y,y); }
        public uint3 xzx { get => new uint3(x,z,x); }
        public uint3 xzz { get => new uint3(x,z,z); }
        public uint3 yxx { get => new uint3(y,x,x); }
        public uint3 yxy { get => new uint3(y,x,y); }
        public uint3 yyx { get => new uint3(y,y,x); }
        public uint3 yyy { get => new uint3(y,y,y); }
        public uint3 yyz { get => new uint3(y,y,z); }
        public uint3 yzy { get => new uint3(y,z,y); }
        public uint3 yzz { get => new uint3(y,z,z); }
        public uint3 zxx { get => new uint3(z,x,x); }
        public uint3 zxz { get => new uint3(z,x,z); }
        public uint3 zyy { get => new uint3(z,y,y); }
        public uint3 zyz { get => new uint3(z,y,z); }
        public uint3 zzx { get => new uint3(z,z,x); }
        public uint3 zzy { get => new uint3(z,z,y); }
        public uint3 zzz { get => new uint3(z,z,z); }

        public uint3 xyz { get => new uint3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public uint3 xzy { get => new uint3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public uint3 yxz { get => new uint3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public uint3 yzx { get => new uint3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public uint3 zxy { get => new uint3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public uint3 zyx { get => new uint3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }

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
        
        public uint2 xx { get => new uint2(x,x); }
        public uint2 xy { get => new uint2(x,y); set { x = value.x; y = value.y; } }
        public uint2 xz { get => new uint2(x,z); set { x = value.x; z = value.y; } }
        public uint2 xw { get => new uint2(x,w); set { x = value.x; w = value.y; } }
        public uint2 yx { get => new uint2(y,x); set { y = value.x; x = value.y; } }
        public uint2 yy { get => new uint2(y,y); }
        public uint2 yz { get => new uint2(y,z); set { y = value.x; z = value.y; } }
        public uint2 yw { get => new uint2(y,w); set { y = value.x; w = value.y; } }
        public uint2 zx { get => new uint2(z,x); set { z = value.x; x = value.y; } }
        public uint2 zy { get => new uint2(z,y); set { z = value.x; y = value.y; } }
        public uint2 zz { get => new uint2(z,z); }
        public uint2 zw { get => new uint2(z,w); set { z = value.x; w = value.y; } }
        public uint2 wx { get => new uint2(w,x); set { w = value.x; x = value.y; } }
        public uint2 wy { get => new uint2(w,y); set { w = value.x; y = value.y; } }
        public uint2 wz { get => new uint2(w,z); set { w = value.x; z = value.y; } }
        public uint2 ww { get => new uint2(w,w); }
        
        public uint3 xxx { get => new uint3(x,x,x); }
        public uint3 xxy { get => new uint3(x,x,y); }
        public uint3 xxz { get => new uint3(x,x,z); }
        public uint3 xxw { get => new uint3(x,x,w); }
        public uint3 xyx { get => new uint3(x,y,x); }
        public uint3 xyy { get => new uint3(x,y,y); }
        public uint3 xyz { get => new uint3(x,y,z); set { x = value.x; y = value.y; z = value.z; } }
        public uint3 xyw { get => new uint3(x,y,w); set { x = value.x; y = value.y; w = value.z; } }
        public uint3 xzx { get => new uint3(x,z,x); }
        public uint3 xzy { get => new uint3(x,z,y); set { x = value.x; z = value.y; y = value.z; } }
        public uint3 xzz { get => new uint3(x,z,z); }
        public uint3 xzw { get => new uint3(x,z,w); set { x = value.x; z = value.y; w = value.z; } }
        public uint3 xwx { get => new uint3(x,w,x); }
        public uint3 xwy { get => new uint3(x,w,y); set { x = value.x; w = value.y; y = value.z; } }
        public uint3 xwz { get => new uint3(x,w,z); set { x = value.x; w = value.y; z = value.z; } }
        public uint3 xww { get => new uint3(x,w,w); }
        public uint3 yxx { get => new uint3(y,x,x); }
        public uint3 yxy { get => new uint3(y,x,y); }
        public uint3 yxz { get => new uint3(y,x,z); set { y = value.x; x = value.y; z = value.z; } }
        public uint3 yxw { get => new uint3(y,x,w); set { y = value.x; x = value.y; w = value.z; } }
        public uint3 yyx { get => new uint3(y,y,x); }
        public uint3 yyy { get => new uint3(y,y,y); }
        public uint3 yyz { get => new uint3(y,y,z); }
        public uint3 yyw { get => new uint3(y,y,w); }
        public uint3 yzx { get => new uint3(y,z,x); set { y = value.x; z = value.y; x = value.z; } }
        public uint3 yzy { get => new uint3(y,z,y); }
        public uint3 yzz { get => new uint3(y,z,z); }
        public uint3 yzw { get => new uint3(y,z,w); set { y = value.x; z = value.y; w = value.z; } }
        public uint3 ywx { get => new uint3(y,w,x); set { y = value.x; w = value.y; x = value.z; } }
        public uint3 ywy { get => new uint3(y,w,y); }
        public uint3 ywz { get => new uint3(y,w,z); set { y = value.x; w = value.y; z = value.z; } }
        public uint3 yww { get => new uint3(y,w,w); }
        public uint3 zxx { get => new uint3(z,x,x); }
        public uint3 zxy { get => new uint3(z,x,y); set { z = value.x; x = value.y; y = value.z; } }
        public uint3 zxz { get => new uint3(z,x,z); }
        public uint3 zxw { get => new uint3(z,x,w); set { z = value.x; x = value.y; w = value.z; } }
        public uint3 zyx { get => new uint3(z,y,x); set { z = value.x; y = value.y; x = value.z; } }
        public uint3 zyy { get => new uint3(z,y,y); }
        public uint3 zyz { get => new uint3(z,y,z); }
        public uint3 zyw { get => new uint3(z,y,w); set { z = value.x; y = value.y; w = value.z; } }
        public uint3 zzx { get => new uint3(z,z,x); }
        public uint3 zzy { get => new uint3(z,z,y); }
        public uint3 zzz { get => new uint3(z,z,z); }
        public uint3 zzw { get => new uint3(z,z,w); }
        public uint3 zwx { get => new uint3(z,w,x); set { z = value.x; w = value.y; x = value.z; } }
        public uint3 zwy { get => new uint3(z,w,y); set { z = value.x; w = value.y; y = value.z; } }
        public uint3 zwz { get => new uint3(z,w,z); }
        public uint3 zww { get => new uint3(z,w,w); }
        public uint3 wxx { get => new uint3(w,x,x); }
        public uint3 wxy { get => new uint3(w,x,y); set { w = value.x; x = value.y; y = value.z; } }
        public uint3 wxz { get => new uint3(w,x,z); set { w = value.x; x = value.y; z = value.z; } }
        public uint3 wxw { get => new uint3(w,x,w); }
        public uint3 wyx { get => new uint3(w,y,x); set { w = value.x; y = value.y; x = value.z; } }
        public uint3 wyy { get => new uint3(w,y,y); }
        public uint3 wyz { get => new uint3(w,y,z); set { w = value.x; y = value.y; z = value.z; } }
        public uint3 wyw { get => new uint3(w,y,w); }
        public uint3 wzx { get => new uint3(w,z,x); set { w = value.x; z = value.y; x = value.z; } }
        public uint3 wzy { get => new uint3(w,z,y); set { w = value.x; z = value.y; y = value.z; } }
        public uint3 wzz { get => new uint3(w,z,z); }
        public uint3 wzw { get => new uint3(w,z,w); }
        public uint3 wwx { get => new uint3(w,w,x); }
        public uint3 wwy { get => new uint3(w,w,y); }
        public uint3 wwz { get => new uint3(w,w,z); }
        public uint3 www { get => new uint3(w,w,w); }
        
        public uint4 xxxx { get => new uint4(x,x,x,x); }
        public uint4 xxxy { get => new uint4(x,x,x,y); }
        public uint4 xxxz { get => new uint4(x,x,x,z); }
        public uint4 xxxw { get => new uint4(x,x,x,w); }
        public uint4 xxyx { get => new uint4(x,x,y,x); }
        public uint4 xxyy { get => new uint4(x,x,y,y); }
        public uint4 xxyz { get => new uint4(x,x,y,z); }
        public uint4 xxyw { get => new uint4(x,x,y,w); }
        public uint4 xxzx { get => new uint4(x,x,z,x); }
        public uint4 xxzy { get => new uint4(x,x,z,y); }
        public uint4 xxzz { get => new uint4(x,x,z,z); }
        public uint4 xxzw { get => new uint4(x,x,z,w); }
        public uint4 xxwx { get => new uint4(x,x,w,x); }
        public uint4 xxwy { get => new uint4(x,x,w,y); }
        public uint4 xxwz { get => new uint4(x,x,w,z); }
        public uint4 xxww { get => new uint4(x,x,w,w); }
        public uint4 xyxx { get => new uint4(x,y,x,x); }
        public uint4 xyxy { get => new uint4(x,y,x,y); }
        public uint4 xyxz { get => new uint4(x,y,x,z); }
        public uint4 xyxw { get => new uint4(x,y,x,w); }
        public uint4 xyyx { get => new uint4(x,y,y,x); }
        public uint4 xyyy { get => new uint4(x,y,y,y); }
        public uint4 xyyz { get => new uint4(x,y,y,z); }
        public uint4 xyyw { get => new uint4(x,y,y,w); }
        public uint4 xyzx { get => new uint4(x,y,z,x); }
        public uint4 xyzy { get => new uint4(x,y,z,y); }
        public uint4 xyzz { get => new uint4(x,y,z,z); }
        public uint4 xyzw { get => new uint4(x,y,z,w); set { x = value.x; y = value.y; z = value.z; w = value.w; } }
        public uint4 xywx { get => new uint4(x,y,w,x); }
        public uint4 xywy { get => new uint4(x,y,w,y); }
        public uint4 xywz { get => new uint4(x,y,w,z); set { x = value.x; y = value.y; w = value.z; z = value.w; } }
        public uint4 xyww { get => new uint4(x,y,w,w); }
        public uint4 xzxx { get => new uint4(x,z,x,x); }
        public uint4 xzxy { get => new uint4(x,z,x,y); }
        public uint4 xzxz { get => new uint4(x,z,x,z); }
        public uint4 xzxw { get => new uint4(x,z,x,w); }
        public uint4 xzyx { get => new uint4(x,z,y,x); }
        public uint4 xzyy { get => new uint4(x,z,y,y); }
        public uint4 xzyz { get => new uint4(x,z,y,z); }
        public uint4 xzyw { get => new uint4(x,z,y,w); set { x = value.x; z = value.y; y = value.z; w = value.w; } }
        public uint4 xzzx { get => new uint4(x,z,z,x); }
        public uint4 xzzy { get => new uint4(x,z,z,y); }
        public uint4 xzzz { get => new uint4(x,z,z,z); }
        public uint4 xzzw { get => new uint4(x,z,z,w); }
        public uint4 xzwx { get => new uint4(x,z,w,x); }
        public uint4 xzwy { get => new uint4(x,z,w,y); set { x = value.x; z = value.y; w = value.z; y = value.w; } }
        public uint4 xzwz { get => new uint4(x,z,w,z); }
        public uint4 xzww { get => new uint4(x,z,w,w); }
        public uint4 xwxx { get => new uint4(x,w,x,x); }
        public uint4 xwxy { get => new uint4(x,w,x,y); }
        public uint4 xwxz { get => new uint4(x,w,x,z); }
        public uint4 xwxw { get => new uint4(x,w,x,w); }
        public uint4 xwyx { get => new uint4(x,w,y,x); }
        public uint4 xwyy { get => new uint4(x,w,y,y); }
        public uint4 xwyz { get => new uint4(x,w,y,z); set { x = value.x; w = value.y; y = value.z; z = value.w; } }
        public uint4 xwyw { get => new uint4(x,w,y,w); }
        public uint4 xwzx { get => new uint4(x,w,z,x); }
        public uint4 xwzy { get => new uint4(x,w,z,y); set { x = value.x; w = value.y; z = value.z; y = value.w; } }
        public uint4 xwzz { get => new uint4(x,w,z,z); }
        public uint4 xwzw { get => new uint4(x,w,z,w); }
        public uint4 xwwx { get => new uint4(x,w,w,x); }
        public uint4 xwwy { get => new uint4(x,w,w,y); }
        public uint4 xwwz { get => new uint4(x,w,w,z); }
        public uint4 xwww { get => new uint4(x,w,w,w); }
        public uint4 yxxx { get => new uint4(y,x,x,x); }
        public uint4 yxxy { get => new uint4(y,x,x,y); }
        public uint4 yxxz { get => new uint4(y,x,x,z); }
        public uint4 yxxw { get => new uint4(y,x,x,w); }
        public uint4 yxyx { get => new uint4(y,x,y,x); }
        public uint4 yxyy { get => new uint4(y,x,y,y); }
        public uint4 yxyz { get => new uint4(y,x,y,z); }
        public uint4 yxyw { get => new uint4(y,x,y,w); }
        public uint4 yxzx { get => new uint4(y,x,z,x); }
        public uint4 yxzy { get => new uint4(y,x,z,y); }
        public uint4 yxzz { get => new uint4(y,x,z,z); }
        public uint4 yxzw { get => new uint4(y,x,z,w); set { y = value.x; x = value.y; z = value.z; w = value.w; } }
        public uint4 yxwx { get => new uint4(y,x,w,x); }
        public uint4 yxwy { get => new uint4(y,x,w,y); }
        public uint4 yxwz { get => new uint4(y,x,w,z); set { y = value.x; x = value.y; w = value.z; z = value.w; } }
        public uint4 yxww { get => new uint4(y,x,w,w); }
        public uint4 yyxx { get => new uint4(y,y,x,x); }
        public uint4 yyxy { get => new uint4(y,y,x,y); }
        public uint4 yyxz { get => new uint4(y,y,x,z); }
        public uint4 yyxw { get => new uint4(y,y,x,w); }
        public uint4 yyyx { get => new uint4(y,y,y,x); }
        public uint4 yyyy { get => new uint4(y,y,y,y); }
        public uint4 yyyz { get => new uint4(y,y,y,z); }
        public uint4 yyyw { get => new uint4(y,y,y,w); }
        public uint4 yyzx { get => new uint4(y,y,z,x); }
        public uint4 yyzy { get => new uint4(y,y,z,y); }
        public uint4 yyzz { get => new uint4(y,y,z,z); }
        public uint4 yyzw { get => new uint4(y,y,z,w); }
        public uint4 yywx { get => new uint4(y,y,w,x); }
        public uint4 yywy { get => new uint4(y,y,w,y); }
        public uint4 yywz { get => new uint4(y,y,w,z); }
        public uint4 yyww { get => new uint4(y,y,w,w); }
        public uint4 yzxx { get => new uint4(y,z,x,x); }
        public uint4 yzxy { get => new uint4(y,z,x,y); }
        public uint4 yzxz { get => new uint4(y,z,x,z); }
        public uint4 yzxw { get => new uint4(y,z,x,w); set { y = value.x; z = value.y; x = value.z; w = value.w; } }
        public uint4 yzyx { get => new uint4(y,z,y,x); }
        public uint4 yzyy { get => new uint4(y,z,y,y); }
        public uint4 yzyz { get => new uint4(y,z,y,z); }
        public uint4 yzyw { get => new uint4(y,z,y,w); }
        public uint4 yzzx { get => new uint4(y,z,z,x); }
        public uint4 yzzy { get => new uint4(y,z,z,y); }
        public uint4 yzzz { get => new uint4(y,z,z,z); }
        public uint4 yzzw { get => new uint4(y,z,z,w); }
        public uint4 yzwx { get => new uint4(y,z,w,x); set { y = value.x; z = value.y; w = value.z; x = value.w; } }
        public uint4 yzwy { get => new uint4(y,z,w,y); }
        public uint4 yzwz { get => new uint4(y,z,w,z); }
        public uint4 yzww { get => new uint4(y,z,w,w); }
        public uint4 ywxx { get => new uint4(y,w,x,x); }
        public uint4 ywxy { get => new uint4(y,w,x,y); }
        public uint4 ywxz { get => new uint4(y,w,x,z); set { y = value.x; w = value.y; x = value.z; z = value.w; } }
        public uint4 ywxw { get => new uint4(y,w,x,w); }
        public uint4 ywyx { get => new uint4(y,w,y,x); }
        public uint4 ywyy { get => new uint4(y,w,y,y); }
        public uint4 ywyz { get => new uint4(y,w,y,z); }
        public uint4 ywyw { get => new uint4(y,w,y,w); }
        public uint4 ywzx { get => new uint4(y,w,z,x); set { y = value.x; w = value.y; z = value.z; x = value.w; } }
        public uint4 ywzy { get => new uint4(y,w,z,y); }
        public uint4 ywzz { get => new uint4(y,w,z,z); }
        public uint4 ywzw { get => new uint4(y,w,z,w); }
        public uint4 ywwx { get => new uint4(y,w,w,x); }
        public uint4 ywwy { get => new uint4(y,w,w,y); }
        public uint4 ywwz { get => new uint4(y,w,w,z); }
        public uint4 ywww { get => new uint4(y,w,w,w); }
        public uint4 zxxx { get => new uint4(z,x,x,x); }
        public uint4 zxxy { get => new uint4(z,x,x,y); }
        public uint4 zxxz { get => new uint4(z,x,x,z); }
        public uint4 zxxw { get => new uint4(z,x,x,w); }
        public uint4 zxyx { get => new uint4(z,x,y,x); }
        public uint4 zxyy { get => new uint4(z,x,y,y); }
        public uint4 zxyz { get => new uint4(z,x,y,z); }
        public uint4 zxyw { get => new uint4(z,x,y,w); set { z = value.x; x = value.y; y = value.z; w = value.w; } }
        public uint4 zxzx { get => new uint4(z,x,z,x); }
        public uint4 zxzy { get => new uint4(z,x,z,y); }
        public uint4 zxzz { get => new uint4(z,x,z,z); }
        public uint4 zxzw { get => new uint4(z,x,z,w); }
        public uint4 zxwx { get => new uint4(z,x,w,x); }
        public uint4 zxwy { get => new uint4(z,x,w,y); set { z = value.x; x = value.y; w = value.z; y = value.w; } }
        public uint4 zxwz { get => new uint4(z,x,w,z); }
        public uint4 zxww { get => new uint4(z,x,w,w); }
        public uint4 zyxx { get => new uint4(z,y,x,x); }
        public uint4 zyxy { get => new uint4(z,y,x,y); }
        public uint4 zyxz { get => new uint4(z,y,x,z); }
        public uint4 zyxw { get => new uint4(z,y,x,w); set { z = value.x; y = value.y; x = value.z; w = value.w; } }
        public uint4 zyyx { get => new uint4(z,y,y,x); }
        public uint4 zyyy { get => new uint4(z,y,y,y); }
        public uint4 zyyz { get => new uint4(z,y,y,z); }
        public uint4 zyyw { get => new uint4(z,y,y,w); }
        public uint4 zyzx { get => new uint4(z,y,z,x); }
        public uint4 zyzy { get => new uint4(z,y,z,y); }
        public uint4 zyzz { get => new uint4(z,y,z,z); }
        public uint4 zyzw { get => new uint4(z,y,z,w); }
        public uint4 zywx { get => new uint4(z,y,w,x); set { z = value.x; y = value.y; w = value.z; x = value.w; } }
        public uint4 zywy { get => new uint4(z,y,w,y); }
        public uint4 zywz { get => new uint4(z,y,w,z); }
        public uint4 zyww { get => new uint4(z,y,w,w); }
        public uint4 zzxx { get => new uint4(z,z,x,x); }
        public uint4 zzxy { get => new uint4(z,z,x,y); }
        public uint4 zzxz { get => new uint4(z,z,x,z); }
        public uint4 zzxw { get => new uint4(z,z,x,w); }
        public uint4 zzyx { get => new uint4(z,z,y,x); }
        public uint4 zzyy { get => new uint4(z,z,y,y); }
        public uint4 zzyz { get => new uint4(z,z,y,z); }
        public uint4 zzyw { get => new uint4(z,z,y,w); }
        public uint4 zzzx { get => new uint4(z,z,z,x); }
        public uint4 zzzy { get => new uint4(z,z,z,y); }
        public uint4 zzzz { get => new uint4(z,z,z,z); }
        public uint4 zzzw { get => new uint4(z,z,z,w); }
        public uint4 zzwx { get => new uint4(z,z,w,x); }
        public uint4 zzwy { get => new uint4(z,z,w,y); }
        public uint4 zzwz { get => new uint4(z,z,w,z); }
        public uint4 zzww { get => new uint4(z,z,w,w); }
        public uint4 zwxx { get => new uint4(z,w,x,x); }
        public uint4 zwxy { get => new uint4(z,w,x,y); set { z = value.x; w = value.y; x = value.z; y = value.w; } }
        public uint4 zwxz { get => new uint4(z,w,x,z); }
        public uint4 zwxw { get => new uint4(z,w,x,w); }
        public uint4 zwyx { get => new uint4(z,w,y,x); set { z = value.x; w = value.y; y = value.z; x = value.w; } }
        public uint4 zwyy { get => new uint4(z,w,y,y); }
        public uint4 zwyz { get => new uint4(z,w,y,z); }
        public uint4 zwyw { get => new uint4(z,w,y,w); }
        public uint4 zwzx { get => new uint4(z,w,z,x); }
        public uint4 zwzy { get => new uint4(z,w,z,y); }
        public uint4 zwzz { get => new uint4(z,w,z,z); }
        public uint4 zwzw { get => new uint4(z,w,z,w); }
        public uint4 zwwx { get => new uint4(z,w,w,x); }
        public uint4 zwwy { get => new uint4(z,w,w,y); }
        public uint4 zwwz { get => new uint4(z,w,w,z); }
        public uint4 zwww { get => new uint4(z,w,w,w); }
        public uint4 wxxx { get => new uint4(w,x,x,x); }
        public uint4 wxxy { get => new uint4(w,x,x,y); }
        public uint4 wxxz { get => new uint4(w,x,x,z); }
        public uint4 wxxw { get => new uint4(w,x,x,w); }
        public uint4 wxyx { get => new uint4(w,x,y,x); }
        public uint4 wxyy { get => new uint4(w,x,y,y); }
        public uint4 wxyz { get => new uint4(w,x,y,z); set { w = value.x; x = value.y; y = value.z; z = value.w; } }
        public uint4 wxyw { get => new uint4(w,x,y,w); }
        public uint4 wxzx { get => new uint4(w,x,z,x); }
        public uint4 wxzy { get => new uint4(w,x,z,y); set { w = value.x; x = value.y; z = value.z; y = value.w; } }
        public uint4 wxzz { get => new uint4(w,x,z,z); }
        public uint4 wxzw { get => new uint4(w,x,z,w); }
        public uint4 wxwx { get => new uint4(w,x,w,x); }
        public uint4 wxwy { get => new uint4(w,x,w,y); }
        public uint4 wxwz { get => new uint4(w,x,w,z); }
        public uint4 wxww { get => new uint4(w,x,w,w); }
        public uint4 wyxx { get => new uint4(w,y,x,x); }
        public uint4 wyxy { get => new uint4(w,y,x,y); }
        public uint4 wyxz { get => new uint4(w,y,x,z); set { w = value.x; y = value.y; x = value.z; z = value.w; } }
        public uint4 wyxw { get => new uint4(w,y,x,w); }
        public uint4 wyyx { get => new uint4(w,y,y,x); }
        public uint4 wyyy { get => new uint4(w,y,y,y); }
        public uint4 wyyz { get => new uint4(w,y,y,z); }
        public uint4 wyyw { get => new uint4(w,y,y,w); }
        public uint4 wyzx { get => new uint4(w,y,z,x); set { w = value.x; y = value.y; z = value.z; x = value.w; } }
        public uint4 wyzy { get => new uint4(w,y,z,y); }
        public uint4 wyzz { get => new uint4(w,y,z,z); }
        public uint4 wyzw { get => new uint4(w,y,z,w); }
        public uint4 wywx { get => new uint4(w,y,w,x); }
        public uint4 wywy { get => new uint4(w,y,w,y); }
        public uint4 wywz { get => new uint4(w,y,w,z); }
        public uint4 wyww { get => new uint4(w,y,w,w); }
        public uint4 wzxx { get => new uint4(w,z,x,x); }
        public uint4 wzxy { get => new uint4(w,z,x,y); set { w = value.x; z = value.y; x = value.z; y = value.w; } }
        public uint4 wzxz { get => new uint4(w,z,x,z); }
        public uint4 wzxw { get => new uint4(w,z,x,w); }
        public uint4 wzyx { get => new uint4(w,z,y,x); set { w = value.x; z = value.y; y = value.z; x = value.w; } }
        public uint4 wzyy { get => new uint4(w,z,y,y); }
        public uint4 wzyz { get => new uint4(w,z,y,z); }
        public uint4 wzyw { get => new uint4(w,z,y,w); }
        public uint4 wzzx { get => new uint4(w,z,z,x); }
        public uint4 wzzy { get => new uint4(w,z,z,y); }
        public uint4 wzzz { get => new uint4(w,z,z,z); }
        public uint4 wzzw { get => new uint4(w,z,z,w); }
        public uint4 wzwx { get => new uint4(w,z,w,x); }
        public uint4 wzwy { get => new uint4(w,z,w,y); }
        public uint4 wzwz { get => new uint4(w,z,w,z); }
        public uint4 wzww { get => new uint4(w,z,w,w); }
        public uint4 wwxx { get => new uint4(w,w,x,x); }
        public uint4 wwxy { get => new uint4(w,w,x,y); }
        public uint4 wwxz { get => new uint4(w,w,x,z); }
        public uint4 wwxw { get => new uint4(w,w,x,w); }
        public uint4 wwyx { get => new uint4(w,w,y,x); }
        public uint4 wwyy { get => new uint4(w,w,y,y); }
        public uint4 wwyz { get => new uint4(w,w,y,z); }
        public uint4 wwyw { get => new uint4(w,w,y,w); }
        public uint4 wwzx { get => new uint4(w,w,z,x); }
        public uint4 wwzy { get => new uint4(w,w,z,y); }
        public uint4 wwzz { get => new uint4(w,w,z,z); }
        public uint4 wwzw { get => new uint4(w,w,z,w); }
        public uint4 wwwx { get => new uint4(w,w,w,x); }
        public uint4 wwwy { get => new uint4(w,w,w,y); }
        public uint4 wwwz { get => new uint4(w,w,w,z); }
        public uint4 wwww { get => new uint4(w,w,w,w); }

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
