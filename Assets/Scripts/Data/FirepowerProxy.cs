
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class FirepowerProxy : ComponentProxy
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
            uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
            if (thisFirepowerId == 0)
            {
                thisFirepowerId = _entityAssembly.RegisterFirepowerProxy(this);
                _entityProxy.firepowerId = thisFirepowerId;
                entityId = _entityProxy.entityId;
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
        _entityProxy.firepowerId = 0;
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
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                return thisFirepower.entityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                thisFirepower.entityId = value;
                _entityAssembly.SetFirepower(thisFirepowerId, thisFirepower);
            }
        }
    }
    
    public uint targetEntityId
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                return thisFirepower.targetEntityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                thisFirepower.targetEntityId = value;
                _entityAssembly.SetFirepower(thisFirepowerId, thisFirepower);
            }
        }
    }
    
    public uint ammunitionBudget
    {
        get
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                return thisFirepower.ammunitionBudget;
            }
            else
            {
                return 0; 
            }
        }
        set
        {
            if (_entityAssembly && _entityAssembly.GetFirepowerId(this) != 0)
            {
                uint thisFirepowerId = _entityAssembly.GetFirepowerId(this);
                Structs.Firepower thisFirepower = _entityAssembly.GetFirepower(thisFirepowerId);
                thisFirepower.ammunitionBudget = value;
                _entityAssembly.SetFirepower(thisFirepowerId, thisFirepower);
            }
        }
    }
}
