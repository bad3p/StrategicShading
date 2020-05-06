
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class MovementProxy : ComponentProxy
{
    public uint TargetEntityID = 0;
    public double3 TargetPosition = new double3(0,0,0);
    public float TargetVelocity = 0;
    
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisMovementId = _entityAssembly.GetMovementId(this);
            if (thisMovementId == 0)
            {
                thisMovementId = _entityAssembly.RegisterMovementProxy(this);
                _entityProxy.movementId = thisMovementId;
                entityId = _entityProxy.entityId;
                targetEntityId = TargetEntityID;
                targetPosition = TargetPosition;
                targetVelocity = TargetVelocity;
            }
        }    
    }

    void OnDestroy()
    {
#if UNITY_EDITOR
        if( EditorApplication.isPlayingOrWillChangePlaymode )
        {
            return;
        }
#endif        
        _entityProxy.movementId = 0;
    }

    void OnDrawGizmosSelected()
    {
        if (!_entityProxy)
        {
            Awake();
        }
        if (_entityAssembly)
        {
            Gizmos.color = _entityProxy.GetTeamColor();
            if (targetVelocity > 0 && _entityProxy.transformId > 0)
            {
                TransformProxy transformProxy = _entityAssembly.GetTransformProxy(_entityProxy.transformId);
                if (transformProxy)
                {
                    if (targetEntityId == 0)
                    {
                        Gizmos.DrawLine( transformProxy.position.ToVector3(), targetPosition.ToVector3() );
                    }
                    else
                    {
                        EntityProxy targetEntityProxy = _entityAssembly.GetEntityProxy(targetEntityId);
                        if (targetEntityProxy.transformId > 0)
                        {
                            TransformProxy targetTransformProxy = _entityAssembly.GetTransformProxy(targetEntityProxy.transformId);

                            float4 targetRotation = targetTransformProxy.rotation;
                            float3 targetX = ComputeShaderEmulator.rotate(new float3(1, 0, 0), targetRotation);
                            float3 targetY = ComputeShaderEmulator.rotate(new float3(0, 1, 0), targetRotation);
                            float3 targetZ = ComputeShaderEmulator.rotate(new float3(0, 0, 1), targetRotation);

                            float3 targetOffset = targetX * (float)targetPosition.x +
                                                  targetY * (float)targetPosition.y +
                                                  targetZ * (float)targetPosition.z;

                            double3 absolutePosition = targetTransformProxy.position + new double3( targetOffset.x, targetOffset.y, targetOffset.z );
                            Gizmos.DrawLine( transformProxy.position.ToVector3(), absolutePosition.ToVector3() );
                        }
                    }
                }
            }
        }
    }
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.entityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.entityId = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public uint targetEntityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetEntityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetEntityId = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public double3 targetPosition
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetPosition;
            }
            else
            {
                return new double3(0,0,0);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetPosition = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public float targetVelocity
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetVelocity;
            }
            else
            {
                return 0.0f;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetVelocity = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
}
