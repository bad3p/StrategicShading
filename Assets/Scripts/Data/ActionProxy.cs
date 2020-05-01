
using Types;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class ActionProxy : ComponentProxy
{
    public uint ActionID = 0;
    public uint TargetEntityID = 0;
    public double3 TargetPosition = new double3(0,0,0);
    public float ActionTimeout = 0;
    
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisActionId = _entityAssembly.GetActionId(this);
            if (thisActionId == 0)
            {
                thisActionId = _entityAssembly.RegisterActionProxy(this);
                _entityProxy.actionId = thisActionId;
                entityId = _entityProxy.entityId;
                actionId = ActionID;
                targetEntityId = TargetEntityID;
                targetPosition = TargetPosition;
                actionTimeout = ActionTimeout;            
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
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.entityId;
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
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                thisAction.entityId = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }

    public uint actionId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.actionId;
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
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                thisAction.actionId = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public uint targetEntityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.targetEntityId;
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
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                thisAction.targetEntityId = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public double3 targetPosition
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.targetPosition;
            }
            else
            {
                return new double3(0,0,0);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                thisAction.targetPosition = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public float actionTimeout
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.actionTimeout;
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
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                thisAction.actionTimeout = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
}
