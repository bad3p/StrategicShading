using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Deploy : BehaviourTreeNode
{
    public float FrontlineWidth = 25.0f;
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
            uint entityChildCount = GetEntityFilteredChildCount(entityId, (childEntityId) =>
            {
                if (!ComputeShaderEmulator.HasComponents(childEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK))
                {
                    return false;
                }
                uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(childEntityId);
                return (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED);
            });
            
            double3 nextSiblingTargetPosition;
            double3 deploymentStep;

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

            if ( ComputeShaderEmulator.HasComponents(entityId, ComputeShaderEmulator.HIERARCHY) )
            {
                uint firstChildEntityId = hierarchyBuffer[entityId].firstChildEntityId;
                if (firstChildEntityId > 0)
                {
                    if ( ComputeShaderEmulator.HasComponents(firstChildEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK) )
                    {
                        bool allChildrenArrived = true;
                        float4 targetVelocityByDistance = new float4( 5.0f, 0.5f, 10.0f, 1.0f );
                        
                        uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(firstChildEntityId);
                        if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                        {
                            ComputeShaderEmulator.AdjustMovement(firstChildEntityId, TargetPositionErrorThreshold, nextSiblingTargetPosition, this.transform.rotation, targetVelocityByDistance);
                            if (allChildrenArrived)
                            {
                                allChildrenArrived = ComputeShaderEmulator.IsMovementCompleted(firstChildEntityId);
                            }
                            nextSiblingTargetPosition = nextSiblingTargetPosition + deploymentStep;
                        }

                        if (hierarchyBuffer[firstChildEntityId].nextSiblingEntityId > 0)
                        {
                            uint nextSiblingEntityId = firstChildEntityId;
                            while (hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0)
                            {
                                nextSiblingEntityId = hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
                                if( ComputeShaderEmulator.HasComponents(nextSiblingEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK) )
                                {
                                    suppression = ComputeShaderEmulator.GetPersonnelSuppression(nextSiblingEntityId);
                                    if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                                    {
                                        ComputeShaderEmulator.AdjustMovement(nextSiblingEntityId, TargetPositionErrorThreshold, nextSiblingTargetPosition, this.transform.rotation, targetVelocityByDistance);
                                        if (allChildrenArrived)
                                        {
                                            allChildrenArrived = allChildrenArrived && ComputeShaderEmulator.IsMovementCompleted(nextSiblingEntityId);
                                        }
                                        nextSiblingTargetPosition = nextSiblingTargetPosition + deploymentStep;
                                    }
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
    }
    #endregion
}
