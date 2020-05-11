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
        if (!behaviourTree.EntityProxy)
        {
            return;
        }
        if (behaviourTree.EntityProxy.entityId == 0)
        {
            return;
        }
        
        Gizmos.color = behaviourTree.EntityProxy.GetTeamColor();

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
    private EntityAssembly _entityAssembly = null;
    
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
        
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (!_entityAssembly)
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

                var entity = _entityAssembly.GetEntity( entityId );
                if (entity.hierarchyId > 0)
                {
                    var hierarchy = _entityAssembly.GetHierarchy( entity.hierarchyId );
                    if (hierarchy.firstChildEntityId > 0)
                    {
                        var firstChildEntity = _entityAssembly.GetEntity(hierarchy.firstChildEntityId);
                        if (firstChildEntity.transformId > 0 && firstChildEntity.movementId > 0)
                        {
                            const float TargetPositionErrorThreshold = 0.1f;
                            const float TargetRotationErrorThreshold = Mathf.Deg2Rad * 1.0f;
                            
                            var firstChildHierarchy = _entityAssembly.GetHierarchy(firstChildEntity.hierarchyId);
                            var firstChildTransform = _entityAssembly.GetTransform(firstChildEntity.transformId);
                            var firstChildMovement = _entityAssembly.GetMovement(firstChildEntity.movementId);
                            
                            double3 targetPositionError = firstChildMovement.targetPosition - nextSiblingTargetPosition;
                            if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                            {
                                firstChildMovement.targetVelocity = 1.4f; // TODO: configure
                                firstChildMovement.targetPosition = nextSiblingTargetPosition;
                                _entityAssembly.SetMovement(firstChildEntity.movementId, firstChildMovement);
                            }
                            
                            float targetRotationError = ComputeShaderEmulator.sigangle(firstChildTransform.rotation, this.transform.rotation);
                            if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                            {
                                firstChildMovement.targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                firstChildMovement.targetRotation = this.transform.rotation;
                                _entityAssembly.SetMovement(firstChildEntity.movementId, firstChildMovement);
                            }
                            
                            bool allChildrenArrived = true;
                            float3 currentPositionError = firstChildMovement.targetPosition - firstChildTransform.position;
                            if (ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length(firstChildTransform.scale))
                            {
                                allChildrenArrived = false;
                            }

                            nextSiblingTargetPosition = nextSiblingTargetPosition + deploymentStep;

                            if (firstChildHierarchy.nextSiblingEntityId > 0)
                            {
                                var nextSiblingEntity = firstChildEntity;
                                var nextSiblingHierarchy = firstChildHierarchy;
                                while (nextSiblingHierarchy.nextSiblingEntityId > 0)
                                {
                                    nextSiblingEntity = _entityAssembly.GetEntity(nextSiblingHierarchy.nextSiblingEntityId);
                                    nextSiblingHierarchy = _entityAssembly.GetHierarchy(nextSiblingEntity.hierarchyId);
                                    if (nextSiblingEntity.transformId > 0 && nextSiblingEntity.movementId > 0)
                                    {
                                        var nextSiblingTransform = _entityAssembly.GetTransform(nextSiblingEntity.transformId);
                                        var nextSiblingMovement = _entityAssembly.GetMovement(nextSiblingEntity.movementId);

                                        targetPositionError = nextSiblingMovement.targetPosition - nextSiblingTargetPosition;
                                        targetRotationError = ComputeShaderEmulator.sigangle(nextSiblingTransform.rotation, nextSiblingTargetRotation);
                                        currentPositionError = nextSiblingMovement.targetPosition - nextSiblingTransform.position;

                                        if (ComputeShaderEmulator.length(targetPositionError) > TargetPositionErrorThreshold)
                                        {
                                            nextSiblingMovement.targetVelocity = 1.4f; // TODO: configure
                                            nextSiblingMovement.targetPosition = nextSiblingTargetPosition;
                                            _entityAssembly.SetMovement(nextSiblingEntity.movementId, nextSiblingMovement);
                                        }
                                        
                                        if (Mathf.Abs(targetRotationError) > TargetRotationErrorThreshold)
                                        {
                                            nextSiblingMovement.targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                            nextSiblingMovement.targetRotation = nextSiblingTargetRotation;
                                            _entityAssembly.SetMovement(nextSiblingEntity.movementId, nextSiblingMovement);
                                        }

                                        if ( ComputeShaderEmulator.length(currentPositionError) > ComputeShaderEmulator.length(nextSiblingTransform.scale) )
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
                                Debug.Log( "[Deploy] entity \"" + _entityAssembly.GetEntityProxy(entityId).name + "\" to " + transform.position + " is successful!" );
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
    
    public uint GetEntityChildCount(uint entityId)
    {
        if (_entityAssembly)
        {
            var entity = _entityAssembly.GetEntity(entityId);
            if (entity.hierarchyId > 0)
            {
                var hierarchy = _entityAssembly.GetHierarchy( entity.hierarchyId );
                if (hierarchy.firstChildEntityId > 0)
                {
                    uint result = 1;
                    
                    var childEntity = _entityAssembly.GetEntity(hierarchy.firstChildEntityId);
                    var childHierarchy = _entityAssembly.GetHierarchy(childEntity.hierarchyId);

                    while (childHierarchy.nextSiblingEntityId > 0)
                    {
                        result++;
                        childEntity = _entityAssembly.GetEntity(childHierarchy.nextSiblingEntityId);
                        childHierarchy = _entityAssembly.GetHierarchy(childEntity.hierarchyId);
                    }

                    return result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
    #endregion
}
