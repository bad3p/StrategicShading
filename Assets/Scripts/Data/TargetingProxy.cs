
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class TargetingProxy : ComponentProxy
{
    public float3 Direction = new float3(0,0,0);
    public float Arc = 0.0f;
    
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
                _entityAssembly.RegisterTargetingProxy(_entityProxy.entityId, this);
                direction = Direction;
                arc = Arc;
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
            _entityAssembly.UnregisterTargetingProxy( _entityAssembly.GetEntityId(this), this );
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
        }
    }
    
    private static Structs.Targeting _dummy = new Structs.Targeting();

    private Structs.Targeting _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetTargeting(entityId);
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
                    _entityAssembly.SetTargeting(entityId, value);
                }
            }
        }
    }

    public float3 front
    {
        get { return _component.front; }
        set
        {
            var temp = _component;
            temp.front = value;
            _component = temp;
        }
    }
    
    public float3 direction
    {
        get { return _component.direction; }
        set
        {
            var temp = _component;
            temp.direction = value;
            _component = temp;
        }
    }
    
    public float arc
    {
        get { return _component.arc; }
        set
        {
            var temp = _component;
            temp.arc = value;
            _component = temp;
        }
    }
}
