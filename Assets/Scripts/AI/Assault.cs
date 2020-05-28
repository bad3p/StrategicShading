using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Assault : BehaviourTreeNode
{
    public float FrontlineWidth = 25.0f;
    public float LeapfrogDistance = 25.0f;
    
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
            transform.position - transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 16,
            transform.position + transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 16
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
    private static void Halt(uint entityId)
    {
        if (ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.MOVABLE_MASK))
        {
            ComputeShaderEmulator.StopMovement(entityId);
        }
        else
        {
            Debug.LogWarning( "[Assault] Halt() callback is not implemented for units!" );
        }
    }

    private int _leapfrogIndex = 0; 

    private void IssueOrders()
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
        float nearestDistance = (float)GetNearestDistanceFromChildEntityToLine(entityId, fp0.xz, fp1.xz);
        float targetDistance = nearestDistance - LeapfrogDistance;
        targetDistance = (targetDistance < 0) ? 0 : targetDistance;

        float averageDistance = 0;
        float averageThreshold = 0;
        int childIndex = 0;
        double3 targetPosition = this.transform.position - this.transform.right * FrontlineWidth / 2 - transform.forward * targetDistance;
        double3 targetPositionOffset = this.transform.right * FrontlineWidth / Mathf.Max(entityChildCount-1, 1);
        ForEveryChildEntity(entityId, (childEntityId) =>
        {
            if( ComputeShaderEmulator.HasComponents(childEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK) )
            {
                uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(childEntityId);
                if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                {
                    if (childIndex % 2 == _leapfrogIndex)
                    {
                        float4 targetVelocityByDistance = new float4( 1.0f, 0.5f, 3.0f, 1.0f );
                        ComputeShaderEmulator.StartMovement(childEntityId, targetPosition, this.transform.rotation, targetVelocityByDistance);
                    }

                    childIndex++;
                    targetPosition += targetPositionOffset;
                    averageDistance += (float) GetDistanceFromEntityToLine(childEntityId, fp0.xz, fp1.xz);
                    averageThreshold += ComputeShaderEmulator.length(transformBuffer[childEntityId].scale);
                }
            }
        });

        if (childIndex > 0)
        {
            averageDistance *= 1.0f / childIndex;
            averageThreshold *= 1.0f / childIndex;
            
            if (averageDistance < averageThreshold)
            {
                Debug.Log( "[Assault] entity \"" + entityId + "\" to " + transform.position + " is successful!" );
                status = Status.Success;
            }
        }
        else
        {
            if (averageDistance < averageThreshold)
            {
                Debug.Log( "[Assault] entity \"" + entityId + "\" to " + transform.position + " is failed!" );
                status = Status.Failure;
            }
        }

        _leapfrogIndex = _leapfrogIndex == 0 ? 1 : 0;
    }

    private bool CheckOrders()
    {
        bool result = true;
        ForEveryChildEntity(entityId, (childEntityId) =>
        {
            if (ComputeShaderEmulator.HasComponents(childEntityId, ComputeShaderEmulator.MOVABLE_PERSONNEL_MASK))
            {
                uint suppression = ComputeShaderEmulator.GetPersonnelSuppression(childEntityId);
                if (suppression < ComputeShaderEmulator.SUPPRESSION_PINNED)
                {
                    if (!ComputeShaderEmulator.IsMovementCompleted(childEntityId))
                    {
                        result = false;
                    }
                }
            }
        });
        return result;
    }
    
    public override void Initiate(BehaviourTreeNode parentNode)
    {
        entityId = parentNode.entityId;
        if (entityId > 0 && entityId <= entityCount)
        {
            status = Status.Running;
            ForEveryChildEntity(entityId, Halt);
            IssueOrders();
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
            if (CheckOrders())
            {
                IssueOrders();
            }
        }
    }
    #endregion
}
