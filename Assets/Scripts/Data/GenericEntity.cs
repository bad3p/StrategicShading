using Types;
using UnityEngine;

public class GenericEntity : MonoBehaviour 
{
    public Structs.Entity entity;

    public static Quaternion ToQuaternion(float4 value)
    {
        return new Quaternion( value.x, value.y, value.z, value.w );
    }
    
    public static Quaternion ToQuaternion(double4 value)
    {
        return new Quaternion( (float)value.x, (float)value.y, (float)value.z, (float)value.w );
    }
    
    public static Vector3 ToVector3(float3 value)
    {
        return new Vector3( value.x, value.y, value.z );
    }
    
    public static Vector3 ToVector3(double3 value)
    {
        return new Vector3( (float)value.x, (float)value.y, (float)value.z );
    }
    
    public static Quaternion ToQuaternion(Matrix4x4 m) 
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

    public void SynchronizeWithTransform()
    {
        var t = transform;
        entity.worldPos = new double3( t.position.x, t.position.y, t.position.z );
        entity.worldRot = new float4( t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w );
        entity.extent = new float3( 0.5f * t.localScale.x, 0.5f * t.localScale.y, 0.5f * t.localScale.z );
        
        _z = t.forward;
        _y = t.up;
        _x = t.right;
    }

    public void SynchronizeWithChildren()
    {
        var t = transform;

        int childCount = t.childCount;

        Vector3 forward = Vector3.zero;
        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                Quaternion childWorldRot = ToQuaternion(childEntity.entity.worldRot);
                forward += (childWorldRot * Vector3.forward).normalized;
            }
        }
        forward.Normalize();
        
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.Cross(up, forward);
        up = Vector3.Cross(forward, right);

        _z = forward;
        _y = up;
        _x = right;
        
        Matrix4x4 m = new Matrix4x4();
        m.m00 = _x.x; m.m01 = _y.x; m.m02 = _z.x; m.m03 = 0.0f;
        m.m10 = _x.y; m.m11 = _y.y; m.m12 = _z.y; m.m13 = 0.0f;
        m.m20 = _x.z; m.m21 = _y.z; m.m22 = _z.z; m.m23 = 0.0f;
        m.m30 = 0.0f; m.m31 = 0.0f; m.m32 = 0.0f; m.m33 = 1.0f;

        Quaternion q = ToQuaternion(m);
        
        entity.worldRot = new float4( q.x, q.y, q.z, q.w );
        
        Vector3 worldPos = Vector3.zero;
        int numChildEntities = 0;
        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                worldPos.x += (float) childEntity.entity.worldPos.x;
                worldPos.y += (float) childEntity.entity.worldPos.y;
                worldPos.z += (float) childEntity.entity.worldPos.z;
                numChildEntities++;
            }
        }
        worldPos *= 1.0f / numChildEntities;
        
        entity.worldPos = new float3( worldPos.x, worldPos.y, worldPos.z );

        Matrix4x4 entityToWorldMatrix = Matrix4x4.TRS
        (
            ToVector3(entity.worldPos),
            ToQuaternion(entity.worldRot),
            Vector3.one
        );

        Matrix4x4 worldToEntityMatrix = Matrix4x4.Inverse(entityToWorldMatrix);
        
        Vector3 entityInf = Vector3.zero;
        Vector3 entitySup = Vector3.zero;

        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                Vector3 childEntityWorldPos = ToVector3(childEntity.entity.worldPos);
                Vector3 localPos = worldToEntityMatrix.MultiplyPoint(childEntityWorldPos);
                entityInf.x = Mathf.Min(entityInf.x, localPos.x);
                entityInf.y = Mathf.Min(entityInf.y, localPos.y);
                entityInf.z = Mathf.Min(entityInf.z, localPos.z);
                entitySup.x = Mathf.Max(entitySup.x, localPos.x);
                entitySup.y = Mathf.Max(entitySup.y, localPos.y);
                entitySup.z = Mathf.Max(entitySup.z, localPos.z);
            }
        }
        
        entity.extent = new float3
        (
            Mathf.Max( entitySup.x, -entityInf.x ),
            Mathf.Max( entitySup.y, -entityInf.y ),
            Mathf.Max( entitySup.z, -entityInf.z )
        );
    }

    private Vector3 _z = Vector3.forward;
    private Vector3 _y = Vector3.up;
    private Vector3 _x = Vector3.right;

    void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS( ToVector3(entity.worldPos), Quaternion.identity, Vector3.one );
        Gizmos.color = Color.red;
        Gizmos.DrawLine( Vector3.zero, _x );
        Gizmos.color = Color.green;
        Gizmos.DrawLine( Vector3.zero, _y );
        Gizmos.color = Color.blue;
        Gizmos.DrawLine( Vector3.zero, _z );
        
        Gizmos.matrix = Matrix4x4.TRS( ToVector3(entity.worldPos), ToQuaternion(entity.worldRot), ToVector3(entity.extent) * 2.0f );
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}