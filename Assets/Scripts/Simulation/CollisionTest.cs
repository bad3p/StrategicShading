using UnityEngine;
using Types;

public class CollisionTest : MonoBehaviour
{
    public GameObject RayStart;
    public GameObject RayEnd;

    void OnDrawGizmos()
    {
        if (RayStart && RayEnd)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine( RayStart.transform.position, RayEnd.transform.position );
            
            double3 globalBoxPosition = this.transform.position;
            float4 globalBoxRotation = this.transform.rotation;
            float3 globalBoxScale = this.transform.localScale;
            
            double3 globalRayStart = RayStart.transform.position;
            double3 globalRayEnd = RayEnd.transform.position;
            
            float3 localRayStart = globalRayStart - globalBoxPosition;
            float3 localRayEnd = globalRayEnd - globalBoxPosition;

            float4 invBoxRotation = ComputeShaderEmulator.quaternionInverse(globalBoxRotation);
            localRayStart = ComputeShaderEmulator.rotate(localRayStart, invBoxRotation);
            localRayEnd = ComputeShaderEmulator.rotate(localRayEnd, invBoxRotation);

            float3 localRayDir = ComputeShaderEmulator.normalize(localRayEnd - localRayStart);
            
            float3 localBoxSup = globalBoxScale * 0.5f;
            float3 localBoxInf = -localBoxSup;
            float3 localHit = new float3(0.0f,0.0f,0.0f);
            if (ComputeShaderEmulator.CollideRayAABB(localRayStart, localRayDir, localBoxInf, localBoxSup, out localHit))
            {
                localHit = ComputeShaderEmulator.rotate(localHit, globalBoxRotation);
                double3 globalHit = localHit;
                globalHit = globalHit + globalBoxPosition;
                Gizmos.DrawSphere( globalHit.ToVector3(), 0.05f );
            }
        }
    }
}
