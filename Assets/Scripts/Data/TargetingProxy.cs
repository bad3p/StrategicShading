
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class TargetingProxy : ComponentProxy
{
    public uint TargetEntityID = 0;
    public uint AmmunitionBudget = 0;
    
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
                targetEntityId = TargetEntityID;
                ammunitionBudget = AmmunitionBudget;
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

    public uint targetEntityId
    {
        get { return _component.targetEntityId; }
        set
        {
            var temp = _component;
            temp.targetEntityId = value;
            _component = temp;
        }
    }
    
    public uint ammunitionBudget
    {
        get { return _component.ammunitionBudget; }
        set
        {
            var temp = _component;
            temp.ammunitionBudget = value;
            _component = temp;
        }
    }
}
