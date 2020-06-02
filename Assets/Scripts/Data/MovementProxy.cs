
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
    public float4 TargetVelocityByDistance = new float4(0,0,0,1);
    
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
                targetVelocityByDistance = TargetVelocityByDistance;
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

            bool isMoving = (Mathf.Abs(targetVelocityByDistance.y) > Mathf.Epsilon) ||
                            (Mathf.Abs(targetVelocityByDistance.w) > Mathf.Epsilon);
            
            if ( isMoving && (_entityProxy.entityDesc & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM)
            {
                TransformProxy transformProxy = _entityAssembly.GetTransformProxy(_entityProxy.entityId);
                if (transformProxy)
                {
                    Vector3 p0 = targetPosition.ToVector3();
                    Vector3 p1 = transformProxy.position.ToVector3();

                    float distance = Vector3.Distance(p0, p1);
                    Vector3 dir = (p1 - p0).normalized;
                    while (distance > 0)
                    {
                        const float DashLength = 1.0f;
                        const float GapLength = 1.0f;
                        
                        p1 = p0 + dir * Mathf.Min(DashLength, distance);
                        Gizmos.DrawLine(p0, p1);
                        p0 = p1;
                        distance -= Mathf.Min(DashLength, distance);
                        
                        p1 = p0 + dir * Mathf.Min(GapLength, distance);
                        p0 = p1;
                        distance -= Mathf.Min(DashLength, distance);
                    }
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
    
    public float4 targetVelocityByDistance
    {
        get { return _component.targetVelocityByDistance; }
        set
        {
            var temp = _component;
            temp.targetVelocityByDistance = TargetVelocityByDistance;
            _component = temp;
        }
    }
    
    public float3 deltaPosition
    {
        get { return _component.deltaPosition; }
    }
}
