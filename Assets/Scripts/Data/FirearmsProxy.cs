
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class FirearmsProxy : ComponentProxy
{
    public uint Ammo = 30;
    public float4 Distance = new float4( 50, 125, 250, 500 );
    public float4 Firepower= new float4( 99, 66, 33, 13 );
    public uint StateID = 0;
    public float StateTimeout = 0.0f;
    
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
                _entityAssembly.RegisterFirearmsProxy(_entityProxy.entityId, this);
                ammo = Ammo;
                distance = Distance;
                firepower = Firepower;
                stateId = StateID;
                stateTimeout = StateTimeout;
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
            _entityAssembly.UnregisterFirearmsProxy( _entityAssembly.GetEntityId(this), this );
        }
    }
    
    private static Structs.Firearms _dummy = new Structs.Firearms();

    private Structs.Firearms _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetFirearms(entityId);
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
                    _entityAssembly.SetFirearms(entityId, value);
                }
            }
        }
    }

    public uint ammo
    {
        get { return _component.ammo; }
        set
        {
            var temp = _component;
            temp.ammo = value;
            _component = temp;
        }
    }
    
    public float4 distance
    {
        get { return _component.distance; }
        set
        {
            var temp = _component;
            temp.distance = value;
            _component = temp;
        }
    }
    
    public float4 firepower
    {
        get { return _component.firepower; }
        set
        {
            var temp = _component;
            temp.firepower = value;
            _component = temp;
        }
    }
    
    public uint stateId
    {
        get { return _component.stateId; }
        set
        {
            var temp = _component;
            temp.stateId = value;
            _component = temp;
        }
    }
    
    public float stateTimeout
    {
        get { return _component.stateTimeout; }
        set
        {
            var temp = _component;
            temp.stateTimeout = value;
            _component = temp;
        }
    }
}
