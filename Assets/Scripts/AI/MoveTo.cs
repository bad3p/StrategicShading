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
                if ( (descBuffer[entityId] & ComputeShaderEmulator.HIERARCHY) == ComputeShaderEmulator.HIERARCHY )
                {
                    uint firstChildEntityId = hierarchyBuffer[entityId].firstChildEntityId;
                    if (firstChildEntityId > 0)
                    {
                        if ( (descBuffer[firstChildEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM &&
                             (descBuffer[firstChildEntityId] & ComputeShaderEmulator.MOVEMENT) == ComputeShaderEmulator.MOVEMENT )
                        {
                            const float TargetPositionErrorThreshold = 0.1f;
                            const float TargetRotationErrorThreshold = Mathf.Deg2Rad * 1.0f;
                            
                            double3 targetPositionError = movementBuffer[firstChildEntityId].targetPosition - this.transform.position;
                            if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                            {
                                movementBuffer[firstChildEntityId].targetVelocity = 1.4f; // TODO: configure
                                movementBuffer[firstChildEntityId].targetPosition = this.transform.position;
                            }
                            
                            float targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[firstChildEntityId].rotation, this.transform.rotation);
                            if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                            {
                                movementBuffer[firstChildEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                movementBuffer[firstChildEntityId].targetRotation = this.transform.rotation;
                            }
                            
                            bool allChildrenArrived = true;
                            float3 currentPositionError = movementBuffer[firstChildEntityId].targetPosition - transformBuffer[firstChildEntityId].position;
                            if (ComputeShaderEmulator.length(currentPositionError) > Radius)
                            {
                                allChildrenArrived = false;
                            }

                            float3 offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[firstChildEntityId].rotation);
                            float offsetLength = ComputeShaderEmulator.length(transformBuffer[firstChildEntityId].scale);
                            double3 offset = (offsetDir * offsetLength);
                            double3 nextSiblingTargetPosition = transformBuffer[firstChildEntityId].position + offset;
                            float4 nextSiblingTargetRotation = transformBuffer[firstChildEntityId].rotation;

                            if (hierarchyBuffer[firstChildEntityId].nextSiblingEntityId > 0)
                            {
                                uint nextSiblingEntityId = firstChildEntityId;
                                while (hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0)
                                {
                                    nextSiblingEntityId = hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
                                    if ( (descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM &&
                                         (descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.MOVEMENT) == ComputeShaderEmulator.MOVEMENT )
                                    {
                                        targetPositionError = movementBuffer[nextSiblingEntityId].targetPosition - nextSiblingTargetPosition;
                                        targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[nextSiblingEntityId].rotation, nextSiblingTargetRotation);
                                        currentPositionError = movementBuffer[nextSiblingEntityId].targetPosition - transformBuffer[nextSiblingEntityId].position;

                                        if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                                        {
                                            movementBuffer[nextSiblingEntityId].targetVelocity = 1.4f; // TODO: configure
                                            movementBuffer[nextSiblingEntityId].targetPosition = nextSiblingTargetPosition;
                                        }
                                        
                                        if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                                        {
                                            movementBuffer[nextSiblingEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                            movementBuffer[nextSiblingEntityId].targetRotation = nextSiblingTargetRotation;
                                        }

                                        if ( ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length( transformBuffer[nextSiblingEntityId].scale) )
                                        {
                                            allChildrenArrived = false;
                                        }

                                        offsetDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), transformBuffer[nextSiblingEntityId].rotation);
                                        offsetLength = ComputeShaderEmulator.length(transformBuffer[nextSiblingEntityId].scale);
                                        offset = offsetDir * offsetLength;
                                        nextSiblingTargetPosition = transformBuffer[nextSiblingEntityId].position + offset;
                                        nextSiblingTargetRotation = transformBuffer[nextSiblingEntityId].rotation;
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
