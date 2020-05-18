using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Assault : BehaviourTreeNode
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
        const uint MOVABLE_MASK = ComputeShaderEmulator.TRANSFORM | ComputeShaderEmulator.MOVEMENT;
        if ((descBuffer[entityId] & MOVABLE_MASK) == MOVABLE_MASK)
        {
            movementBuffer[entityId].targetPosition = transformBuffer[entityId].position;
            movementBuffer[entityId].targetRotation = transformBuffer[entityId].rotation;
            movementBuffer[entityId].targetVelocity = 0;
            movementBuffer[entityId].targetAngularVelocity = 0;
        }
        else
        {
            Debug.LogWarning( "[Assault] Halt() callback is not implemented for units!" );
        }
    }

    private int _leapfrogIndex = 0; 

    private void IssueOrders()
    {
        uint entityChildCount = GetEntityChildCount(entityId);

        const float LeapfrogDistance = 25.0f;
        
        double3 fp0 = this.transform.position - this.transform.right * FrontlineWidth / 2;
        double3 fp1 = this.transform.position + this.transform.right * FrontlineWidth / 2;
        float nearestDistance = (float)GetNearestDistanceFromChildEntityToLine(entityId, fp0.xz, fp1.xz);
        float targetDistance = nearestDistance - LeapfrogDistance;
        targetDistance = (targetDistance < 0) ? 0 : targetDistance;

        int childIndex = 0;
        double3 targetPosition = this.transform.position - this.transform.right * FrontlineWidth / 2 - transform.forward * targetDistance;
        double3 targetPositionOffset = this.transform.right * FrontlineWidth / Mathf.Max(entityChildCount-1, 1);
        ForEveryChildEntity(entityId, (childEntityId) =>
        {
            if (childIndex % 2 == _leapfrogIndex)
            {
                movementBuffer[childEntityId].targetPosition = targetPosition;
                movementBuffer[childEntityId].targetVelocity = (15.0f * 1000.0f) / (60.0f * 60.0f);
                movementBuffer[childEntityId].targetRotation = this.transform.rotation;
                movementBuffer[childEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f);
            }
            else
            {
                movementBuffer[childEntityId].targetPosition = transformBuffer[childEntityId].position;
                movementBuffer[childEntityId].targetVelocity = 0;
            }
            
            childIndex++;
            targetPosition += targetPositionOffset;
        });

        _leapfrogIndex = _leapfrogIndex == 0 ? 1 : 0;
    }

    private bool CheckOrders()
    {
        bool result = true;
        ForEveryChildEntity(entityId, (childEntityId) =>
        {
            if (movementBuffer[childEntityId].targetVelocity > 0)
            {
                double3 currentPositionError = movementBuffer[childEntityId].targetPosition - transformBuffer[childEntityId].position;
                float currentPositionErrorMagnitude = ComputeShaderEmulator.length(currentPositionError);
                if (currentPositionErrorMagnitude > ComputeShaderEmulator.length(transformBuffer[childEntityId].scale))
                {
                    result = false;
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
