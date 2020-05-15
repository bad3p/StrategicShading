using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Deploy : BehaviourTreeNode
{
    public float FrontlineWidth = 25.0f;
    
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

        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2,
            transform.position + transform.right * FrontlineWidth / 2
        );
        
        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2,
            transform.position + transform.forward * FrontlineWidth / 2
        );
        
        Gizmos.DrawLine
        (
            transform.position + transform.right * FrontlineWidth / 2,
            transform.position + transform.forward * FrontlineWidth / 2
        );

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
                uint entityChildCount = GetEntityChildCount(entityId);
                
                double3 nextSiblingTargetPosition;
                double3 deploymentStep;
                float4 nextSiblingTargetRotation = this.transform.rotation;

                if (entityChildCount > 1)
                {
                    nextSiblingTargetPosition = this.transform.position - this.transform.right * FrontlineWidth / 2;
                    deploymentStep = this.transform.right * FrontlineWidth / (entityChildCount-1);
                }
                else
                {
                    nextSiblingTargetPosition = this.transform.position;
                    deploymentStep = new float3(0,0,0);
                }

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
                            
                            double3 targetPositionError = movementBuffer[firstChildEntityId].targetPosition - nextSiblingTargetPosition;
                            if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                            {
                                movementBuffer[firstChildEntityId].targetVelocity = 1.4f; // TODO: configure
                                movementBuffer[firstChildEntityId].targetPosition = nextSiblingTargetPosition;
                            }
                            
                            float targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[firstChildEntityId].rotation, this.transform.rotation);
                            if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                            {
                                movementBuffer[firstChildEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                movementBuffer[firstChildEntityId].targetRotation = this.transform.rotation;
                            }
                            
                            bool allChildrenArrived = true;
                            float3 currentPositionError = movementBuffer[firstChildEntityId].targetPosition - transformBuffer[firstChildEntityId].position;
                            if (ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length(transformBuffer[firstChildEntityId].scale))
                            {
                                allChildrenArrived = false;
                            }

                            nextSiblingTargetPosition = nextSiblingTargetPosition + deploymentStep;

                            if (hierarchyBuffer[firstChildEntityId].nextSiblingEntityId > 0)
                            {
                                uint nextSiblingEntityId = firstChildEntityId;
                                while (hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0)
                                {
                                    nextSiblingEntityId = hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
                                    if ((descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM &&
                                        (descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.MOVEMENT) == ComputeShaderEmulator.MOVEMENT)
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

                                        if ( ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length(transformBuffer[nextSiblingEntityId].scale) )
                                        {
                                            allChildrenArrived = false;
                                        }

                                        nextSiblingTargetPosition = nextSiblingTargetPosition + deploymentStep;
                                    }
                                    else
                                    {
                                        Debug.LogError( "[Deploy] failed, inconsistent entity (missing Transform or Movement component)!" );
                                        status = Status.Failure;
                                    }
                                }
                            }

                            if (allChildrenArrived)
                            {
                                Debug.Log( "[Deploy] entity \"" + entityId + "\" to " + transform.position + " is successful!" );
                                status = Status.Success;
                            }
                        }
                        else
                        {
                            Debug.LogError( "[Deploy] failed, not implemented for units above squad!" );
                            status = Status.Failure;    
                        }
                    }
                    else
                    {
                        Debug.LogError( "[Deploy] failed, hierarchy.firstChildEntityId == 0" );
                        status = Status.Failure;    
                    }
                }
                else
                {
                    Debug.LogError( "[Deploy] failed, entity.hierarchyId == 0" );
                    status = Status.Failure;    
                }
            }
            else
            {
                Debug.LogError( "[Deploy] failed, entityId == 0" );
                status = Status.Failure;
            }
        }
    }
    #endregion
}
