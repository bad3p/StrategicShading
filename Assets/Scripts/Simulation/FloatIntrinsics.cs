using Types;
using UnityEngine;
using UnityEngine.Networking;

public partial class ComputeShaderEmulator
{
    public static float degrees(float radians)
    {
        return radians * 57.29578f;
    }
    
    public static float radians(float degrees)
    {
        return degrees / 57.29578f;
    }
    
    public static float saturate(float value)
    {
        return Mathf.Clamp(value, 0.0f, 1.0f);
    }
    
    public static float clamp(float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
    
    public static float smoothstep(float edge0, float edge1, float x)
    {
        float t = clamp((x - edge0) / (edge1 - edge0), 0.0f, 1.0f);
        return t * t * (3.0f - 2.0f * t);
    }
    
    public static float abs(float v)
    {
        return Mathf.Abs(v); 
    }
    
    public static float sign(float v)
    {
        return Mathf.Sign(v); 
    }
    
    public static float max(float a, float b)
    {
        return Mathf.Max(a,b); 
    }
    
    public static float2 max(float2 a, float2 b)
    {
        return new float2( Mathf.Max(a.x,b.x), Mathf.Max(a.y,b.y) ); 
    }
    
    public static float3 max(float3 a, float3 b)
    {
        return new float3( Mathf.Max(a.x,b.x), Mathf.Max(a.y,b.y), Mathf.Max(a.z,b.z) ); 
    }
    
    public static float4 max(float4 a, float4 b)
    {
        return new float4( Mathf.Max(a.x,b.x), Mathf.Max(a.y,b.y), Mathf.Max(a.z,b.z), Mathf.Max(a.w,b.w) ); 
    }
    
    public static float min(float a, float b)
    {
        return Mathf.Min(a,b); 
    }
    
    public static float2 min(float2 a, float2 b)
    {
        return new float2( Mathf.Min(a.x,b.x), Mathf.Min(a.y,b.y) ); 
    }
    
    public static float3 min(float3 a, float3 b)
    {
        return new float3( Mathf.Min(a.x,b.x), Mathf.Min(a.y,b.y), Mathf.Min(a.z,b.z) ); 
    }
    
    public static float4 min(float4 a, float4 b)
    {
        return new float4( Mathf.Min(a.x,b.x), Mathf.Min(a.y,b.y), Mathf.Min(a.z,b.z), Mathf.Min(a.w,b.w) ); 
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
    
    public static float2 lerp(float2 a, float2 b, float t)
    {
        return new float2( Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t) );
    }
    
    public static float3 lerp(float3 a, float3 b, float t)
    {
        return new float3( Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t), Mathf.Lerp(a.z, b.z, t) );
    }
    
    public static float2 normalize(float2 v)
    {
        float vlen = length(v);
        return (vlen > FLOAT_EPSILON) ? (v * 1.0f / vlen) : v;
    }
    
    public static float3 normalize(float3 v)
    {
        float vlen = length(v);
        return (vlen > FLOAT_EPSILON) ? (v * 1.0f / vlen) : v;
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

    public static float dot(float2 v1, float2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }
    
    public static float dot(float3 v1, float3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }
    
    public static float dot(float4 v1, float4 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z + v1.w * v2.w;
    }
    
    public static float3 cross(float3 a, float3 b)
    {
        return new float3
        (
            a.y*b.z-a.z*b.y,
            -(a.x*b.z-a.z*b.x),
            a.x*b.y-a.y*b.x
        );
    }
    
    public static float4 quaternionInverse(float4 q)
    {
        float dotqq = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
        float rcpdotqq = 1.0f / dotqq;
        return new float4( -q.x * rcpdotqq, -q.y * rcpdotqq, -q.z * rcpdotqq, q.w * rcpdotqq );
    }

    public static float4 quaternionFromBasis(float3 x, float3 y, float3 z)
    {
        float4 q = new float4();
        q.w = sqrt( max( 0, 1 + x.x + y.y + z.z ) ) / 2; 
        q.x = sqrt( max( 0, 1 + x.x - y.y - z.z ) ) / 2; 
        q.y = sqrt( max( 0, 1 - x.x + y.y - z.z ) ) / 2; 
        q.z = sqrt( max( 0, 1 - x.x - y.y + z.z ) ) / 2; 
        q.x *= sign( q.x * ( y.z - z.y ) );
        q.y *= sign( q.y * ( z.x - x.z ) );
        q.z *= sign( q.z * ( x.y - y.x ) );
        return q;
    }
    
    public static float4 quaternionFromAsixAngle(float angle, float3 axis)
    {
        float4 q = new float4();

        float halfAngle = angle / 2;
        float sinHalfAngle = sin(halfAngle);
        float cosHalfAngle = cos(halfAngle);
        
        q.x = axis.x * sinHalfAngle;
        q.y = axis.y * sinHalfAngle;
        q.z = axis.z * sinHalfAngle;
        q.w = cosHalfAngle;
        return q;
    }

    public static float4x4 trs(float3 t, float4 q, float3 s)
    {
        float4x4 sm = new float4x4
        (
            new float4( s.x, 0, 0, 0 ),
            new float4( 0, s.y, 0, 0 ),
            new float4( 0, 0, s.z, 0 ),
            new float4( 0, 0, 0, 1 )
        );

        float3 x = rotate(new float3(1, 0, 0), q);
        float3 y = rotate(new float3(0, 1, 0), q);
        float3 z = rotate(new float3(0, 0, 1), q);

        float4x4 m = matrixFromBasis(x, y, z);

        m = m * sm;

        m._m03 = t.x;
        m._m13 = t.y;
        m._m23 = t.z;
        m._m33 = 1.0f;
        return m;
    }
    
    public static float4x4 matrixFromBasis(float3 x, float3 y, float3 z)
    {
        float4x4 m = new float4x4();
        m._m00 = x.x; m._m01 = y.x; m._m02 = z.x; m._m03 = 0.0f;
        m._m10 = x.y; m._m11 = y.y; m._m12 = z.y; m._m13 = 0.0f;
        m._m20 = x.z; m._m21 = y.z; m._m22 = z.z; m._m23 = 0.0f;
        m._m30 = 0.0f; m._m31 = 0.0f; m._m32 = 0.0f; m._m33 = 1.0f;
        return m;
    }

    public static float4x4 matrixFromQuaternion(float4 q)
    {
        float num1 = q.x * 2f;
        float num2 = q.y * 2f;
        float num3 = q.z * 2f;
        float num4 = q.x * num1;
        float num5 = q.y * num2;
        float num6 = q.z * num3;
        float num7 = q.x * num2;
        float num8 = q.x * num3;
        float num9 = q.y * num3;
        float num10 = q.w * num1;
        float num11 = q.w * num2;
        float num12 = q.w * num3;
        float4x4 matrix4x4;
        matrix4x4._m00 = (float) (1.0 - ((double) num5 + (double) num6));
        matrix4x4._m10 = num7 + num12;
        matrix4x4._m20 = num8 - num11;
        matrix4x4._m30 = 0.0f;
        matrix4x4._m01 = num7 - num12;
        matrix4x4._m11 = (float) (1.0 - ((double) num4 + (double) num6));
        matrix4x4._m21 = num9 + num10;
        matrix4x4._m31 = 0.0f;
        matrix4x4._m02 = num8 + num11;
        matrix4x4._m12 = num9 - num10;
        matrix4x4._m22 = (float) (1.0 - ((double) num4 + (double) num5));
        matrix4x4._m32 = 0.0f;
        matrix4x4._m03 = 0.0f;
        matrix4x4._m13 = 0.0f;
        matrix4x4._m23 = 0.0f;
        matrix4x4._m33 = 1f;
        return matrix4x4;
    }
    
    public static float4x4 inverse(float4x4 m)
    {
        //return m.ToMatrix4x4().inverse;
        
        float n11 = m._m00, n12 = m._m10, n13 = m._m20, n14 = m._m30;
        float n21 = m._m01, n22 = m._m11, n23 = m._m21, n24 = m._m31;
        float n31 = m._m02, n32 = m._m12, n33 = m._m22, n34 = m._m32;
        float n41 = m._m03, n42 = m._m13, n43 = m._m23, n44 = m._m33;

        float t11 = n23 * n34 * n42 - n24 * n33 * n42 + n24 * n32 * n43 - n22 * n34 * n43 - n23 * n32 * n44 + n22 * n33 * n44;
        float t12 = n14 * n33 * n42 - n13 * n34 * n42 - n14 * n32 * n43 + n12 * n34 * n43 + n13 * n32 * n44 - n12 * n33 * n44;
        float t13 = n13 * n24 * n42 - n14 * n23 * n42 + n14 * n22 * n43 - n12 * n24 * n43 - n13 * n22 * n44 + n12 * n23 * n44;
        float t14 = n14 * n23 * n32 - n13 * n24 * n32 - n14 * n22 * n33 + n12 * n24 * n33 + n13 * n22 * n34 - n12 * n23 * n34;

        float det = n11 * t11 + n21 * t12 + n31 * t13 + n41 * t14;
        float idet = 1.0f / det;

        float4x4 ret;

        ret._m00 = t11 * idet;
        ret._m01 = (n24 * n33 * n41 - n23 * n34 * n41 - n24 * n31 * n43 + n21 * n34 * n43 + n23 * n31 * n44 - n21 * n33 * n44) * idet;
        ret._m02 = (n22 * n34 * n41 - n24 * n32 * n41 + n24 * n31 * n42 - n21 * n34 * n42 - n22 * n31 * n44 + n21 * n32 * n44) * idet;
        ret._m03 = (n23 * n32 * n41 - n22 * n33 * n41 - n23 * n31 * n42 + n21 * n33 * n42 + n22 * n31 * n43 - n21 * n32 * n43) * idet;

        ret._m10 = t12 * idet;
        ret._m11 = (n13 * n34 * n41 - n14 * n33 * n41 + n14 * n31 * n43 - n11 * n34 * n43 - n13 * n31 * n44 + n11 * n33 * n44) * idet;
        ret._m12 = (n14 * n32 * n41 - n12 * n34 * n41 - n14 * n31 * n42 + n11 * n34 * n42 + n12 * n31 * n44 - n11 * n32 * n44) * idet;
        ret._m13 = (n12 * n33 * n41 - n13 * n32 * n41 + n13 * n31 * n42 - n11 * n33 * n42 - n12 * n31 * n43 + n11 * n32 * n43) * idet;

        ret._m20 = t13 * idet;
        ret._m21 = (n14 * n23 * n41 - n13 * n24 * n41 - n14 * n21 * n43 + n11 * n24 * n43 + n13 * n21 * n44 - n11 * n23 * n44) * idet;
        ret._m22 = (n12 * n24 * n41 - n14 * n22 * n41 + n14 * n21 * n42 - n11 * n24 * n42 - n12 * n21 * n44 + n11 * n22 * n44) * idet;
        ret._m23 = (n13 * n22 * n41 - n12 * n23 * n41 - n13 * n21 * n42 + n11 * n23 * n42 + n12 * n21 * n43 - n11 * n22 * n43) * idet;

        ret._m30 = t14 * idet;
        ret._m31 = (n13 * n24 * n31 - n14 * n23 * n31 + n14 * n21 * n33 - n11 * n24 * n33 - n13 * n21 * n34 + n11 * n23 * n34) * idet;
        ret._m32 = (n14 * n22 * n31 - n12 * n24 * n31 - n14 * n21 * n32 + n11 * n24 * n32 + n12 * n21 * n34 - n11 * n22 * n34) * idet;
        ret._m33 = (n12 * n23 * n31 - n13 * n22 * n31 + n13 * n21 * n32 - n11 * n23 * n32 - n12 * n21 * n33 + n11 * n22 * n33) * idet;

        return ret;  
    }
    
    public static float4 mul(float4 vec, float4x4 mat)
    {
        return new float4
        (
            vec.x*mat._m00 + vec.y*mat._m01 + vec.z*mat._m02 + vec.w*mat._m03,
            vec.x*mat._m10 + vec.y*mat._m11 + vec.z*mat._m12 + vec.w*mat._m13,
            vec.x*mat._m20 + vec.y*mat._m21 + vec.z*mat._m22 + vec.w*mat._m23,
            vec.x*mat._m30 + vec.y*mat._m31 + vec.z*mat._m32 + vec.w*mat._m33
        );
    }
    
    public static float3 transformVector(float3 v, float4x4 mat)
    {
        float4 temp = new float4( v.x, v.y, v.z, 0.0f );
        temp = mul(temp, mat);
        return new float3(temp.x, temp.y, temp.z);
    }
    
    public static float3 transformPoint(float3 p, float4x4 mat)
    {
        float4 temp = new float4( p.x, p.y, p.z, 1.0f );
        temp = mul(temp, mat);
        return new float3(temp.x, temp.y, temp.z);
    }
    
    public static float4 transformQuaternion(float4 lhs, float4 rhs)
    {
        return new float4
        (
            (lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y), 
            (lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z), 
            (lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x), 
            (lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z)
        ); 
    }
    
    public static float3 rotate(float3 v, float4 q)
    {
        float3 u = new float3(q.x, q.y, q.z);
        float s = q.w;
        return u * 2.0f * dot(u, v) +
               v * (s*s - dot(u, u)) +
               cross(u, v) * 2.0f * s;
    }
    
    public static float angle(float2 from, float2 to)
    {
        float fromSqrMagnitude = dot(from, from);
        float toSqrMagnitude = dot(to, to);
        
        float num = Mathf.Sqrt( fromSqrMagnitude * toSqrMagnitude );
        if (num < Mathf.Epsilon)
        {
            return 0.0f;
        }

        return Mathf.Acos( Mathf.Clamp( dot(from, to) / num, -1f, 1f) );
    }

    public static float sigangle(float2 from, float2 to)
    {
        return angle(from, to) * Mathf.Sign( from.x * to.y - from.y * to.x );
    }
    
    public static float angle(float3 from, float3 to)
    {
        float fromSqrMagnitude = dot(from, from);
        float toSqrMagnitude = dot(to, to);
        float num = sqrt( fromSqrMagnitude * toSqrMagnitude);
        if (num < FLOAT_EPSILON)
        {
            return 0.0f;
        }
        return Mathf.Acos( clamp( dot(from, to) / num, -1f, 1f) );
    }
    
    public static float sigangle(float3 from, float3 to, float3 axis)
    {
        float a = angle(from, to);
        float cx = (from.y * to.z - from.z * to.y);
        float cy = (from.z * to.x - from.x * to.z);
        float cz = (from.x * to.y - from.y * to.x);
        float c = sign( axis.x * cx + axis.y * cy + axis.z * cz);
        return a * c;
    }
    
    public static float sigangle(float4 from, float4 to)
    {
        float3 fromForward = new float3(0, 0, 1);
        fromForward = rotate(fromForward, from);
        float3 toForward = new float3(0, 0, 1);
        toForward = rotate(toForward, to);
        float3 axis = normalize(cross(fromForward, toForward));
        float angle = sigangle(fromForward, toForward, axis);
        return angle;
    }
    
    public static float lerpargs(float4 args, float xz)
    {
        float t = clamp((xz - args.x) / (args.z - args.x), 0.0f, 1.0f);
        return lerp( args.y, args.w, t );
    }
}