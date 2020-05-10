
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class MovementProxy : ComponentProxy
{
    public double3 TargetPosition = new double3(0,0,0);
    public float4 TargetRotation = new float4(0,0,0,1);
    public float TargetVelocity = 0;
    public float TargetAngularVelocity = 0;
    
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
                targetPosition = TargetPosition;
                targetRotation = TargetRotation;
                targetVelocity = TargetVelocity;
                targetAngularVelocity = TargetAngularVelocity;
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
                    Gizmos.DrawLine( transformProxy.position.ToVector3(), targetPosition.ToVector3() );
                }
            }
        }
    }
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
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
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.entityId = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public double3 targetPosition
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
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
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetPosition = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public float4 targetRotation
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetRotation;
            }
            else
            {
                return new float4(0,0,0,1);
            }
        }
        set
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetRotation = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public float targetVelocity
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
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
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetVelocity = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public float targetAngularVelocity
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetAngularVelocity;
            }
            else
            {
                return 0.0f;
            }
        }
        set
        {
            if (_entityAssembly && _entityAssembly.GetMovementId(this) != 0)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetAngularVelocity = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
}
