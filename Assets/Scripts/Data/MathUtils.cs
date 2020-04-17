
using UnityEngine;

public static class MathUtils
{
    public static Matrix4x4 MatrixFromBasis(Vector3 x, Vector3 y, Vector3 z)
    {
        Matrix4x4 m = new Matrix4x4();
        m.m00 = x.x; m.m01 = y.x; m.m02 = z.x; m.m03 = 0.0f;
        m.m10 = x.y; m.m11 = y.y; m.m12 = z.y; m.m13 = 0.0f;
        m.m20 = x.z; m.m21 = y.z; m.m22 = z.z; m.m23 = 0.0f;
        m.m30 = 0.0f; m.m31 = 0.0f; m.m32 = 0.0f; m.m33 = 1.0f;
        return m;
    }
    
    public static void Encapsulate(ref Vector3 inf, ref Vector3 sup, ref Vector3 point)
    {
        inf.x = Mathf.Min(inf.x, point.x);
        inf.y = Mathf.Min(inf.y, point.y);
        inf.z = Mathf.Min(inf.z, point.z);
        sup.x = Mathf.Max(sup.x, point.x);
        sup.y = Mathf.Max(sup.y, point.y);
        sup.z = Mathf.Max(sup.z, point.z);
    }
}

public static class Matrix4x4Ext
{
   
    public static Quaternion ToQuaternion(this Matrix4x4 m) 
    {
        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
        q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
        q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2; 
        q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 
        q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
        q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
        q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );
        return q;
    }    
}

public static class Vector3Ext
{
}