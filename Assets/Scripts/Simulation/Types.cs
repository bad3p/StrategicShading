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
