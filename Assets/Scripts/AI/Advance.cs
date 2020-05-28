using System;
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Advance : BehaviourTreeNode
{
    public float FrontlineWidth = 25.0f;
    public float AdvanceDistance = 10.0f;
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
            transform.position - transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 8,
            transform.position + transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 8
        );
        
        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2,
            transform.position - transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth
        );
        
        Gizmos.DrawLine
        (
            transform.position + transform.right * FrontlineWidth / 2,
            transform.position + transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth
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

            double3 fp0 = this.transform.position - this.transform.right * FrontlineWidth / 2;
            double3 fp1 = this.transform.position + this.transform.right * FrontlineWidth / 2;
            double nearestDistance = GetNearestDistanceFromChildEntityToLine(entityId, fp0.xz, fp1.xz);
            double targetDistance = Math.Max(0, nearestDistance - AdvanceDistance);
            double3 targetPositionOffset = this.transform.forward * (float) targetDistance;
            
            Debug.DrawLine( this.transform.position, this.transform.position - targetPositionOffset.ToVector3() );
            
            double3 nextSiblingTargetPosition;
            double3 advancementStep;

            if (entityChildCount > 1)
            {
                nextSiblingTargetPosition = this.transform.position - this.transform.right * FrontlineWidth / 2;
                nextSiblingTargetPosition -= targetPositionOffset;
                advancementStep = this.transform.right * FrontlineWidth / (entityChildCount-1);
            }
            else
            {
                nextSiblingTargetPosition = this.transform.position;
                advancementStep = new float3(0,0,0);
            }

            if ( ComputeShaderEmulator.HasComponents(entityId, ComputeShaderEmulator.HIERARCHY) )
            {
                uint firstChildEntityId = hierarchyBuffer[entityId].firstChildEntityId;
                if (firstChildEntityId > 0)
                {
                    if( ComputeShaderEmulator.HasComponents(firstChildEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK) )
                    {
                        bool allChildrenArrived = true;
                        float4 targetVelocityByDistance = new float4( AdvanceDistance + 5.0f, 0.5f, AdvanceDistance + 10.0f, 1.0f );
                        
                        uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(firstChildEntityId);
                        if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                        {
                            ComputeShaderEmulator.AdjustMovement(firstChildEntityId, TargetPositionErrorThreshold, nextSiblingTargetPosition, this.transform.rotation, targetVelocityByDistance);
                            if (allChildrenArrived)
                            {
                                allChildrenArrived = ComputeShaderEmulator.IsMovementCompleted(firstChildEntityId);
                            }
                            nextSiblingTargetPosition = nextSiblingTargetPosition + advancementStep;
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
                                        nextSiblingTargetPosition = nextSiblingTargetPosition + advancementStep;
                                    }
                                }
                                else
                                {
                                    Debug.LogError( "[Advance] failed, inconsistent entity (missing Transform or Movement component)!" );
                                    status = Status.Failure;
                                    break;
                                }
                            }
                        }

                        if (allChildrenArrived)
                        {
                            Debug.Log( "[Advance] entity \"" + entityId + "\" to " + transform.position + " is successful!" );
                            status = Status.Success;
                        }
                    }
                    else
                    {
                        Debug.LogError( "[Advance] failed, not implemented for units above squad!" );
                        status = Status.Failure;    
                    }
                }
                else
                {
                    Debug.LogError( "[Advance] failed, hierarchy.firstChildEntityId == 0" );
                    status = Status.Failure;    
                }
            }
            else
            {
                Debug.LogError( "[Advance] failed, entity.hierarchyId == 0" );
                status = Status.Failure;    
            }
        }        
    }
    #endregion
}
