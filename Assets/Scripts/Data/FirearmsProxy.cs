
using Types;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(FirearmsProxy))]
public class FirearmsProxy : MonoBehaviour
{
    public uint Ammo = 30;
    public float4 Distance = new float4( 50, 125, 250, 500 );
    public float4 Firepower= new float4( 99, 66, 33, 13 );
    public float Timeout = 0;
    
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    void Awake()
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
                timeout = Timeout;
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
    
    public float timeout
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisFirearmsId = _entityAssembly.GetFirearmsId(this);
                Structs.Firearms thisFirearms = _entityAssembly.GetFirearms(thisFirearmsId);
                return thisFirearms.timeout;
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
                thisFirearms.timeout = value;
                _entityAssembly.SetFirearms(thisFirearmsId, thisFirearms);
            }
        }
    }
}
