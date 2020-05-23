
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class FirearmProxy : ComponentProxy
{
    public uint DescID = 0;
    public uint Ammo = 30;
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
                descId = DescID;
                ammo = Ammo;
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
    
    private static Structs.Firearm _dummy = new Structs.Firearm();

    private Structs.Firearm _component
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

    public void ValidateDataConsistency()
    {
        if (!_entityAssembly)
        {
            Awake();
        }
        if (!_entityAssembly)
        {
            return;
        }

        if (descId < _entityAssembly.FirearmDescBuffer.Count)
        {
            var firearmDesc = _entityAssembly.FirearmDescBuffer[(int) descId];
            Ammo = firearmDesc.maxAmmo;
            ammo = Ammo;
        }
    }
    
    public uint descId
    {
        get { return _component.descId; }
        set
        {
            var temp = _component;
            temp.descId = value;
            _component = temp;
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
