
using Types;
using UnityEngine;

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
            uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
            if (thisFirearmsId == 0)
            {
                thisFirearmsId = _entityAssembly.RegisterFirearmsProxy(this);
                _entityProxy.firearmsId = thisFirearmsId;
                entityId = _entityProxy.entityId;
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
        _entityProxy.firearmsId = 0;
    }
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.entityId;
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
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.entityId = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }

    public uint ammo
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.ammo;
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
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.ammo = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
    
    public float4 distance
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.distance;
            }
            else
            {
                return new float4(0,0,0,0);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.distance = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
    
    public float4 firepower
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.firepower;
            }
            else
            {
                return new float4(0,0,0,0);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.firepower = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
    
    public uint stateId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.stateId;
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
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.stateId = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
    
    public float stateTimeout
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.stateTimeout;
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
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                thisFirearms.stateTimeout = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
}
