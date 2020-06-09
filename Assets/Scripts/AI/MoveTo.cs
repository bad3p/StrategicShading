using System.Runtime.Remoting.Channels;
using Types;
using Structs;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MoveTo : BehaviourTreeNode
{
    public float Radius = 25.0f;
    public float TargetPositionErrorThreshold = 1.0f;
    
    #region MonoBehaviour
    void OnDrawGizmos()
    {
#if UNITY_EDITOR        
        BehaviourTree behaviourTree = GetComponentInParent<BehaviourTree>();
        if (!behaviourTree)
        {
            return;
        }

        if (behaviourTree.EntityID == 0)
        {
            return;
        }

        EntityAssembly entityAssembly = FindObjectOfType<EntityAssembly>();
        if (!entityAssembly)
        {
            return;
        }

        EntityProxy entityProxy = entityAssembly.GetEntityProxy(behaviourTree.EntityID);
        if (!entityProxy)
        {
            return;
        }

        Gizmos.color = entityProxy.GetTeamColor();
        Gizmos.matrix = Matrix4x4.Translate( transform.position );
            
        const int NumPoints = 32;
        for (int i = 0; i < NumPoints; i++)
        {
            float j = (i > 0) ? (i - 1) : (NumPoints - 1);

            float angleI = i * 360.0f / NumPoints * Mathf.Deg2Rad;
            float angleJ = j * 360.0f / NumPoints * Mathf.Deg2Rad;
                
            Gizmos.DrawLine
            (
                new Vector3( Mathf.Cos(angleI), 0, Mathf.Sin(angleI) ) * Radius,
                new Vector3( Mathf.Cos(angleJ), 0, Mathf.Sin(angleJ) ) * Radius
            );
        }

        if (EditorApplication.isPlaying)
        {
        }
        else
        {
        }
#endif
    }
    #endregion
    
    #region BehaviourTreeNode
    public override void Initiate(BehaviourTreeNode parentNode)
    {
        entityId = parentNode.entityId;
        if (entityId > 0 && entityId <= entityCount)
        {
            status = Status.Running;
        }
        else
        {
            status = Status.Failure;
        }
    }

    public override void Run()
    {
        if (status == Status.Running)
        {
            if ( ComputeShaderEmulator.HasComponents(entityId, ComputeShaderEmulator.HIERARCHY) )
            {
                double3 centerOfHierarchy = GetCenterOfHierarchy(entityId);
                uint nearestEntityId = GetNearestEntityInHierarchy(entityId, transform.position);
                float nearestDistance = ComputeShaderEmulator.distance( transformBuffer[nearestEntityId].position, transform.position );

                double3 movementDir = centerOfHierarchy - transform.position;
                float movementDist = ComputeShaderEmulator.length(movementDir);
                if (movementDist > ComputeShaderEmulator.FLOAT_EPSILON)
                {
                    movementDir *= 1.0f / movementDist;
                }
                
                float firstChildDistance = ComputeShaderEmulator.max(0.0f, nearestDistance - 10);

                float3 firstChildForward = -movementDir;
                float3 firstChildUp = new float3(0,1,0);
                float3 firstChildRight = ComputeShaderEmulator.cross(firstChildUp,firstChildForward);
                firstChildUp = ComputeShaderEmulator.cross(firstChildForward,firstChildRight);
                
                double3 nextSiblingTargetPosition = transform.position + movementDir * firstChildDistance;
                float4 nextSiblingTargetRotation = ComputeShaderEmulator.quaternionFromBasis(firstChildRight, firstChildUp, firstChildForward);
                
                Debug.DrawLine
                (
                    transform.position,
                    nextSiblingTargetPosition.ToVector3(),
                    Color.white
                );

                uint firstChildEntityId = hierarchyBuffer[entityId].firstChildEntityId;
                if (firstChildEntityId > 0)
                {
                    if ( ComputeShaderEmulator.HasComponents(firstChildEntityId, ComputeShaderEmulator.HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT) )
                    {
                        bool allChildrenArrived = true;
                        float4 targetVelocityByDistance = new float4( 5.0f, 0.5f, 25.0f, 1.0f );

                        uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(firstChildEntityId);
                        if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                        {
                            ComputeShaderEmulator.AdjustMovement(firstChildEntityId, TargetPositionErrorThreshold, nextSiblingTargetPosition, nextSiblingTargetRotation, targetVelocityByDistance);
                            if (allChildrenArrived)
                            {
                                allChildrenArrived = ComputeShaderEmulator.IsMovementCompleted(firstChildEntityId);
                            }

                            double3 offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[firstChildEntityId].rotation);
                            double offsetLength = ComputeShaderEmulator.length(transformBuffer[firstChildEntityId].scale);
                            double3 offset = offsetDir * offsetLength;
                            nextSiblingTargetPosition = transformBuffer[firstChildEntityId].position + offset;
                            nextSiblingTargetRotation = transformBuffer[firstChildEntityId].rotation;
                            targetVelocityByDistance = new float4( 1.0f, 0.5f, 10.0f, 1.0f );
                        }

                        if (hierarchyBuffer[firstChildEntityId].nextSiblingEntityId > 0)
                        {
                            uint nextSiblingEntityId = firstChildEntityId;
                            while (hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0)
                            {
                                nextSiblingEntityId = hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
                                if( ComputeShaderEmulator.HasComponents(nextSiblingEntityId, ComputeShaderEmulator.HIERARCHY_TRANSFORM_PERSONNEL_MOVEMENT) )
                                {
                                    suppression = ComputeShaderEmulator.GetPersonnelSuppression(nextSiblingEntityId);
                                    if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                                    {
                                        ComputeShaderEmulator.AdjustMovement(nextSiblingEntityId, TargetPositionErrorThreshold, nextSiblingTargetPosition, nextSiblingTargetRotation, targetVelocityByDistance);
                                        if (allChildrenArrived)
                                        {
                                            allChildrenArrived = allChildrenArrived && ComputeShaderEmulator.IsMovementCompleted(nextSiblingEntityId);
                                        }

                                        double3 offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[nextSiblingEntityId].rotation);
                                        double offsetLength = ComputeShaderEmulator.length(transformBuffer[nextSiblingEntityId].scale);
                                        double3 offset = offsetDir * offsetLength;
                                        nextSiblingTargetPosition = transformBuffer[nextSiblingEntityId].position + offset;
                                        nextSiblingTargetRotation = transformBuffer[nextSiblingEntityId].rotation;
                                        targetVelocityByDistance = new float4( 1.0f, 0.5f, 10.0f, 1.0f );
                                    }
                                }
                                else
                                {
                                    Debug.LogError( "[MoveTo] failed, inconsistent entity (missing Transform or Movement component)!" );
                                    status = Status.Failure;
                                }
                            }
                        }

                        if (allChildrenArrived)
                        {
                            Debug.Log( "[MoveTo] entity \"" + entityId + "\" to " + transform.position + " is successful!" );
                            status = Status.Success;
                        }
                    }
                    else
                    {
                        Debug.LogError( "[MoveTo] failed, not implemented for units above squad!" );
                        status = Status.Failure;    
                    }
                }
                else
                {
                    Debug.LogError( "[MoveTo] failed, hierarchy.firstChildEntityId == 0" );
                    status = Status.Failure;    
                }
            }
            else
            {
                Debug.LogError( "[MoveTo] failed, entity.hierarchyId == 0" );
                status = Status.Failure;    
            }
        }
    }
    #endregion
}
