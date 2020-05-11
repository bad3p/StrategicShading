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
        if (entityId > 0)
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
            if (entityId > 0)
            {
                uint hierarchyId = entityBuffer[entityId].hierarchyId; 
                if (hierarchyId > 0)
                {
                    uint firstChildEntityId = hierarchyBuffer[hierarchyId].firstChildEntityId;
                    if (firstChildEntityId > 0)
                    {
                        uint firstChildTransformId = entityBuffer[firstChildEntityId].transformId;
                        uint firstChildMovementId = entityBuffer[firstChildEntityId].movementId;
                        uint firstChildHierarchyId = entityBuffer[firstChildEntityId].hierarchyId;
                        if (firstChildTransformId > 0 && firstChildMovementId > 0)
                        {
                            const float TargetPositionErrorThreshold = 0.1f;
                            const float TargetRotationErrorThreshold = Mathf.Deg2Rad * 1.0f;
                            
                            double3 targetPositionError = movementBuffer[firstChildMovementId].targetPosition - this.transform.position;
                            if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                            {
                                movementBuffer[firstChildMovementId].targetVelocity = 1.4f; // TODO: configure
                                movementBuffer[firstChildMovementId].targetPosition = this.transform.position;
                            }
                            
                            float targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[firstChildTransformId].rotation, this.transform.rotation);
                            if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                            {
                                movementBuffer[firstChildMovementId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                movementBuffer[firstChildMovementId].targetRotation = this.transform.rotation;
                            }
                            
                            bool allChildrenArrived = true;
                            float3 currentPositionError = movementBuffer[firstChildMovementId].targetPosition - transformBuffer[firstChildTransformId].position;
                            if (ComputeShaderEmulator.length(currentPositionError) > Radius)
                            {
                                allChildrenArrived = false;
                            }

                            float3 offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[firstChildTransformId].rotation);
                            float offsetLength = ComputeShaderEmulator.length(transformBuffer[firstChildTransformId].scale);
                            double3 offset = (offsetDir * offsetLength);
                            double3 nextSiblingTargetPosition = transformBuffer[firstChildTransformId].position + offset;
                            float4 nextSiblingTargetRotation = transformBuffer[firstChildTransformId].rotation;

                            if (hierarchyBuffer[firstChildHierarchyId].nextSiblingEntityId > 0)
                            {
                                uint nextSiblingEntityId = firstChildEntityId;
                                uint nextSiblingHierarchyId = firstChildHierarchyId;
                                while (hierarchyBuffer[nextSiblingHierarchyId].nextSiblingEntityId > 0)
                                {
                                    nextSiblingEntityId = hierarchyBuffer[nextSiblingHierarchyId].nextSiblingEntityId;
                                    nextSiblingHierarchyId = entityBuffer[nextSiblingEntityId].hierarchyId;
                                    
                                    uint nextSiblingTransformId = entityBuffer[nextSiblingEntityId].transformId;
                                    uint nextSiblingMovementId = entityBuffer[nextSiblingEntityId].movementId;
                                    
                                    if (nextSiblingTransformId > 0 && nextSiblingMovementId > 0)
                                    {
                                        targetPositionError = movementBuffer[nextSiblingMovementId].targetPosition - nextSiblingTargetPosition;
                                        targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[nextSiblingTransformId].rotation, nextSiblingTargetRotation);
                                        currentPositionError = movementBuffer[nextSiblingMovementId].targetPosition - transformBuffer[nextSiblingTransformId].position;

                                        if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                                        {
                                            movementBuffer[nextSiblingMovementId].targetVelocity = 1.4f; // TODO: configure
                                            movementBuffer[nextSiblingMovementId].targetPosition = nextSiblingTargetPosition;
                                        }
                                        
                                        if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                                        {
                                            movementBuffer[nextSiblingMovementId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                            movementBuffer[nextSiblingMovementId].targetRotation = nextSiblingTargetRotation;
                                        }

                                        if ( ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length( transformBuffer[nextSiblingTransformId].scale) )
                                        {
                                            allChildrenArrived = false;
                                        }

                                        offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[nextSiblingTransformId].rotation);
                                        offsetLength = ComputeShaderEmulator.length(transformBuffer[nextSiblingTransformId].scale);
                                        offset = offsetDir * offsetLength;
                                        nextSiblingTargetPosition = transformBuffer[nextSiblingTransformId].position + offset;
                                        nextSiblingTargetRotation = transformBuffer[nextSiblingTransformId].rotation;
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
            else
            {
                Debug.LogError( "[MoveTo] failed, entityId == 0" );
                status = Status.Failure;
            }
        }
    }
    #endregion
}
