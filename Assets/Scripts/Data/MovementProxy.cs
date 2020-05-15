
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
            if (_entityAssembly.GetEntityId(this) == 0)
            {
                _entityAssembly.RegisterMovementProxy(_entityProxy.entityId, this);
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
        if (_entityAssembly && _entityAssembly.GetEntityId(this) != 0)
        {
            _entityAssembly.UnregisterMovementProxy( _entityAssembly.GetEntityId(this), this );
        }
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
            if (targetVelocity > 0 && (_entityProxy.entityDesc & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM)
            {
                TransformProxy transformProxy = _entityAssembly.GetTransformProxy(_entityProxy.entityId);
                if (transformProxy)
                {
                    Gizmos.DrawLine( transformProxy.position.ToVector3(), targetPosition.ToVector3() );
                }
            }
        }
    }

    private static Structs.Movement _dummy = new Structs.Movement();

    private Structs.Movement _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetMovement(entityId);
                }
            }
            return _dummy;
        }
        set
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    _entityAssembly.SetMovement(entityId, value);
                }
            }
        }
    }
    
    public double3 targetPosition
    {
        get { return _component.targetPosition; }
        set
        {
            var temp = _component;
            temp.targetPosition = value;
            _component = temp;
        }
    }
    
    public float4 targetRotation
    {
        get { return _component.targetRotation; }
        set
        {
            var temp = _component;
            temp.targetRotation = value;
            _component = temp;
        }
    }
    
    public float targetVelocity
    {
        get { return _component.targetVelocity; }
        set
        {
            var temp = _component;
            temp.targetVelocity = value;
            _component = temp;
        }
    }
    
    public float targetAngularVelocity
    {
        get { return _component.targetAngularVelocity; }
        set
        {
            var temp = _component;
            temp.targetAngularVelocity = value;
            _component = temp;
        }
    }
}
