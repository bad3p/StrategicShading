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
        if (!behaviourTree.EntityProxy)
        {
            return;
        }
        if (behaviourTree.EntityProxy.entityId == 0)
        {
            return;
        }
        
        Gizmos.color = behaviourTree.EntityProxy.GetTeamColor();
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
                var entity = _entityAssembly.GetEntity( entityId );
                if (entity.hierarchyId > 0)
                {
                    var hierarchy = _entityAssembly.GetHierarchy( entity.hierarchyId );
                    if (hierarchy.firstChildEntityId > 0)
                    {
                        var firstChildEntity = _entityAssembly.GetEntity(hierarchy.firstChildEntityId);
                        if (firstChildEntity.transformId > 0 && firstChildEntity.movementId > 0)
                        {
                            var firstChildHierarchy = _entityAssembly.GetHierarchy(firstChildEntity.hierarchyId);
                            var firstChildTransform = _entityAssembly.GetTransform(firstChildEntity.transformId);
                            var firstChildMovement = _entityAssembly.GetMovement(firstChildEntity.movementId);

                            bool allChildrenArrived = true;
                            float3 targetPositionError = firstChildMovement.targetPosition - this.transform.position;
                            if (ComputeShaderEmulator.length(targetPositionError) > Mathf.Epsilon)
                            {
                                firstChildMovement.targetVelocity = 1.4f; // TODO: configure
                                firstChildMovement.targetPosition = this.transform.position;
                                _entityAssembly.SetMovement(firstChildEntity.movementId, firstChildMovement);
                                allChildrenArrived &= false;
                            }
                            
                            float targetRotationError = ComputeShaderEmulator.sigangle(firstChildTransform.rotation, this.transform.rotation);
                            if (Mathf.Abs(targetRotationError) > Mathf.Epsilon)
                            {
                                firstChildMovement.targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                firstChildMovement.targetRotation = this.transform.rotation;
                                _entityAssembly.SetMovement(firstChildEntity.movementId, firstChildMovement);
                                allChildrenArrived &= false;
                            }

                            float3 movementDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), firstChildTransform.rotation);
                            float offsetLength = ComputeShaderEmulator.length(firstChildTransform.scale);
                            double3 offset = movementDir * offsetLength;
                            double3 nextSiblingTargetPosition = firstChildTransform.position + offset;
                            float4 nextSiblingTargetRotation = firstChildTransform.rotation;

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
                                        targetRotationError = ComputeShaderEmulator.sigangle(nextSiblingTransform.rotation, this.transform.rotation);

                                        if (ComputeShaderEmulator.length(targetPositionError) > Mathf.Epsilon)
                                        {
                                            nextSiblingMovement.targetVelocity = 1.4f; // TODO: configure
                                            nextSiblingMovement.targetPosition = nextSiblingTargetPosition;
                                            _entityAssembly.SetMovement(nextSiblingEntity.movementId, nextSiblingMovement);
                                            allChildrenArrived &= false;
                                        }
                                        
                                        if (Mathf.Abs(targetRotationError) > Mathf.Epsilon)
                                        {
                                            nextSiblingMovement.targetAngularVelocity = ComputeShaderEmulator.radians(45.0f); // TODO: configure
                                            nextSiblingMovement.targetRotation = nextSiblingTargetRotation;
                                            _entityAssembly.SetMovement(nextSiblingEntity.movementId, nextSiblingMovement);
                                            allChildrenArrived &= false;
                                        }

                                        movementDir = ComputeShaderEmulator.rotate(new float3(0, 0, -1), nextSiblingTransform.rotation);
                                        offsetLength = ComputeShaderEmulator.length(nextSiblingTransform.scale);
                                        offset = movementDir * offsetLength;
                                        nextSiblingTargetPosition = nextSiblingTransform.position + offset;
                                        nextSiblingTargetRotation = nextSiblingTransform.rotation;
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
                                Debug.Log( "[MoveTo] entity \"" + _entityAssembly.GetEntityProxy(entityId).name + "\" to " + transform.position + " is successful!" );
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
