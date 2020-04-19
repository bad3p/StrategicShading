using Types;
using UnityEngine;

public class GenericEntity : MonoBehaviour 
{
    public Structs.Entity entity;
    
    void SynchronizeSubHierarchy(GenericEntity genericEntity)
    {
        var t = genericEntity.transform;
        int childCount = t.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                SynchronizeSubHierarchy(childEntity);
            }
        }
        
        if (genericEntity is UnitEntity)
        {
            genericEntity.SynchronizeWithChildren( false );
        }
        else
        {
            genericEntity.SynchronizeWithTransform( false );
        }
    }

    public void SynchronizeSubHierarchy()
    {
        SynchronizeSubHierarchy( this );
    }

    public void SynchronizeWithTransform(bool updateParent = true)
    {
        var t = transform;
        entity.worldPos = t.position;
        entity.worldRot = t.rotation;
        entity.extent = t.localScale * 0.5f;

        if (updateParent)
        {
            UnitEntity parentUnitEntity = t.parent.GetComponent<UnitEntity>();
            if (parentUnitEntity)
            {
                parentUnitEntity.SynchronizeWithChildren();
            }
        }
    }
    
    public void SynchronizeWithChildren(bool updateParent = true)
    {
        var t = transform;

        int childCount = t.childCount;

        if (childCount == 0)
        {
            entity.worldPos = t.position;
            entity.worldRot = t.rotation;
            entity.extent = new float3( 0, 0, 0 );
            return;
        }

        float3 pos = new float3( 0, 0, 0 );
        float3 forward = new float3( 0, 0, 0 );
        int numChildEntities = 0;
        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                Quaternion childWorldRot = childEntity.entity.worldRot.ToQuaternion();
                forward += (childWorldRot * Vector3.forward).normalized;
                float3 worldPos = childEntity.entity.worldPos;
                pos += worldPos;
                numChildEntities++;
            }
        }
        forward = ComputeShaderEmulator.normalize( forward );
        
        float3 up = new float3(0,1,0);
        float3 right = ComputeShaderEmulator.cross(up, forward);
        up = ComputeShaderEmulator.cross(forward, right);

        entity.worldRot = ComputeShaderEmulator.quaternionFromBasis( right, up, forward );
        entity.worldPos *= 1.0f / numChildEntities;

        float4x4 entityToWorldMatrix = ComputeShaderEmulator.trs( entity.worldPos, entity.worldRot, new float3(1,1,1) );
        float4x4 worldToEntityMatrix = ComputeShaderEmulator.inverse(entityToWorldMatrix);
        
        float3 entityInf = new float3( float.MaxValue, float.MaxValue, float.MaxValue );
        float3 entitySup = new float3( float.MinValue, float.MinValue, float.MinValue );

        for (int i = 0; i < childCount; i++)
        {
            GenericEntity childEntity = t.GetChild(i).GetComponent<GenericEntity>();
            if (childEntity)
            {
                float3 localOBBVertex = new float3( -childEntity.entity.extent.x, -childEntity.entity.extent.y, -childEntity.entity.extent.z );
                for (int vid = 0; vid < 8; vid++)
                {
                    switch (vid)
                    {
                        case 1: localOBBVertex.z *= -1; break;
                        case 2: localOBBVertex.x *= -1; break;
                        case 3: localOBBVertex.z *= -1; break;
                        case 4: localOBBVertex.x *= -1; localOBBVertex.y *= -1; break;
                        case 5: localOBBVertex.z *= -1; break;
                        case 6: localOBBVertex.x *= -1; break;
                        case 7: localOBBVertex.z *= -1; break;
                    }

                    float3 worldOBBVertex = ComputeShaderEmulator.rotate( localOBBVertex, childEntity.entity.worldRot.ToQuaternion() );
                    float3 entityWorldPos = childEntity.entity.worldPos;
                    worldOBBVertex += entityWorldPos;
                    
                    float3 localPoint = ComputeShaderEmulator.mulp(worldOBBVertex, worldToEntityMatrix);
                    entityInf = ComputeShaderEmulator.min( entityInf, localPoint );
                    entitySup = ComputeShaderEmulator.max( entitySup, localPoint );
                }
            }
        }

        entity.worldPos += entityToWorldMatrix.ToMatrix4x4().MultiplyVector( (entityInf + (entitySup - entityInf) * 0.5f).ToVector3() );
        entity.extent = (entitySup - entityInf) * 0.5f;

        if (updateParent && t.parent)
        {
            UnitEntity parentUnitEntity = t.parent.GetComponent<UnitEntity>();
            if (parentUnitEntity)
            {
                parentUnitEntity.SynchronizeWithChildren();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (ComputeShaderEmulator.length(entity.worldRot) <= ComputeShaderEmulator.FLOAT_EPSILON)
        {
            return;
        }
        
        Gizmos.matrix = Matrix4x4.TRS( entity.worldPos.ToVector3(), entity.worldRot.ToQuaternion(), entity.extent.ToVector3() * 2.0f );
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}