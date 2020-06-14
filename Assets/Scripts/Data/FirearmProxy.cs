
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
                
                if( descId > 0 && descId < _entityAssembly.FirearmDescBuffer.Length )
                {                
                    ammo = _entityAssembly.FirearmDescBuffer[descId].maxAmmo;
                    clipAmmo = _entityAssembly.FirearmDescBuffer[descId].maxClipAmmo;
                }
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
    
    public uint clipAmmo
    {
        get { return _component.clipAmmo; }
        set
        {
            var temp = _component;
            temp.clipAmmo = value;
            _component = temp;
        }
    }
    
    public uint status
    {
        get { return _component.status; }
    }
    
    public float timeout
    {
        get { return _component.timeout; }
    }
}
