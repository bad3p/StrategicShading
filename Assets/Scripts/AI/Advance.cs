using System;
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Advance : BehaviourTreeNode
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
            uint entityChildCount = GetEntityChildCount(entityId);

            const float RunavayDistance = 10.0f;

            double3 fp0 = this.transform.position - this.transform.right * FrontlineWidth / 2;
            double3 fp1 = this.transform.position + this.transform.right * FrontlineWidth / 2;
            double nearestDistance = GetNearestDistanceFromChildEntityToLine(entityId, fp0.xz, fp1.xz);
            double targetDistance = Math.Max(0, nearestDistance - RunavayDistance);
            double3 targetPositionOffset = this.transform.forward * (float) targetDistance;
            
            Debug.DrawLine( this.transform.position, this.transform.position - targetPositionOffset.ToVector3() );
            
            double3 nextSiblingTargetPosition;
            double3 advancementStep;
            float4 nextSiblingTargetRotation = this.transform.rotation;

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
            
            float4 velocityByCurrentPositionError = new float4
            (
                RunavayDistance,
                1.4f,
                RunavayDistance * 2,
                4.17f
            );

            if ( (descBuffer[entityId] & ComputeShaderEmulator.HIERARCHY) == ComputeShaderEmulator.HIERARCHY)
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
                        float targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[firstChildEntityId].rotation, this.transform.rotation);
                        float3 currentPositionError = nextSiblingTargetPosition - transformBuffer[firstChildEntityId].position;

                        if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                        {
                            float targetVelocity = ComputeShaderEmulator.lerpargs(velocityByCurrentPositionError, ComputeShaderEmulator.length(currentPositionError));
                            movementBuffer[firstChildEntityId].targetVelocity = targetVelocity; // TODO: configure
                            movementBuffer[firstChildEntityId].targetPosition = nextSiblingTargetPosition;
                        }

                        if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                        {
                            movementBuffer[firstChildEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                            movementBuffer[firstChildEntityId].targetRotation = this.transform.rotation;
                        }
                        
                        currentPositionError = nextSiblingTargetPosition - transformBuffer[firstChildEntityId].position;
                        float currentPositionErrorMagnitude = ComputeShaderEmulator.length(currentPositionError);
                        
                        bool allChildrenArrived = true;
                        if (currentPositionErrorMagnitude > ComputeShaderEmulator.length(transformBuffer[firstChildEntityId].scale))
                        {
                            allChildrenArrived = false;
                        }

                        nextSiblingTargetPosition = nextSiblingTargetPosition + advancementStep;

                        if (hierarchyBuffer[firstChildEntityId].nextSiblingEntityId > 0)
                        {
                            uint nextSiblingEntityId = firstChildEntityId;
                            while (hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId > 0)
                            {
                                nextSiblingEntityId = hierarchyBuffer[nextSiblingEntityId].nextSiblingEntityId;
                                if ( (descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM &&
                                     (descBuffer[nextSiblingEntityId] & ComputeShaderEmulator.MOVEMENT) == ComputeShaderEmulator.MOVEMENT)
                                {
                                    targetPositionError = movementBuffer[nextSiblingEntityId].targetPosition - nextSiblingTargetPosition;
                                    targetRotationError = ComputeShaderEmulator.sigangle(transformBuffer[nextSiblingEntityId].rotation, nextSiblingTargetRotation);

                                    if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                                    {
                                        currentPositionError = nextSiblingTargetPosition - transformBuffer[nextSiblingEntityId].position;
                                        currentPositionErrorMagnitude = ComputeShaderEmulator.length(currentPositionError);
                                        float targetVelocity = ComputeShaderEmulator.lerpargs(velocityByCurrentPositionError, currentPositionErrorMagnitude);
                                        movementBuffer[nextSiblingEntityId].targetVelocity = targetVelocity; // TODO: configure
                                        movementBuffer[nextSiblingEntityId].targetPosition = nextSiblingTargetPosition;
                                    }
                                    
                                    if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                                    {
                                        movementBuffer[nextSiblingEntityId].targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                        movementBuffer[nextSiblingEntityId].targetRotation = nextSiblingTargetRotation;
                                    }
                                    
                                    currentPositionError = nextSiblingTargetPosition - transformBuffer[nextSiblingEntityId].position;
                                    currentPositionErrorMagnitude = ComputeShaderEmulator.length(currentPositionError);

                                    if ( currentPositionErrorMagnitude > ComputeShaderEmulator.length(transformBuffer[nextSiblingEntityId].scale) )
                                    {
                                        allChildrenArrived = false;
                                    }

                                    nextSiblingTargetPosition = nextSiblingTargetPosition + advancementStep;
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
