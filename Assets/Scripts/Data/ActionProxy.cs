
using System;
using Types;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class ActionProxy : ComponentProxy
{
    public uint MoveTargetEntityID = 0;
    public double3 MoveTargetVector = new double3(0,0,0);
    public float MoveTargetValue = 0;
    public uint AttackTargetEntityID = 0;
    public float3 AttackTargetVector = new float3(0,0,0);
    public float AttackTargetValue = 0;
    
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
                moveTargetEntityId = MoveTargetEntityID;
                moveTargetVector = MoveTargetVector;
                moveTargetValue = MoveTargetValue;
                attackTargetEntityId = AttackTargetEntityID;
                attackTargetVector = AttackTargetVector;
                attackTargetValue = AttackTargetValue;
            }
        }    
    }

    void OnDestroy()
    {
        _entityProxy.actionId = 0;
    }

    void OnDrawGizmosSelected()
    {
        if (!_entityProxy)
        {
            Awake();
        }
        if (_entityAssembly)
        {
            float2 hullCenter2D = _entityProxy.hullCenter;
            float3 hullCenter = new float3( hullCenter2D.x, 0, hullCenter2D.y );

            Gizmos.color = _entityProxy.GetTeamColor();
            if (moveTargetEntityId == 0)
            {
                float3 pos = moveTargetVector.ToVector3();
                Gizmos.DrawLine( hullCenter.ToVector3(), pos.ToVector3() );
            }
            else
            {
                
            }
        }
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

    public uint moveTargetEntityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.moveTargetEntityId;
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
                thisAction.moveTargetEntityId = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public double3 moveTargetVector
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.moveTargetVector;
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
                thisAction.moveTargetVector = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public float moveTargetValue
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.moveTargetValue;
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
                thisAction.moveTargetValue = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public uint attackTargetEntityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.attackTargetEntityId;
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
                thisAction.attackTargetEntityId = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public float3 attackTargetVector
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.attackTargetVector;
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
                thisAction.attackTargetVector = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
    
    public float attackTargetValue
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisActionId = _entityAssembly.GetActionId(this);
                Structs.Action thisAction = _entityAssembly.GetAction(thisActionId);
                return thisAction.attackTargetValue;
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
                thisAction.attackTargetValue = value;
                _entityAssembly.SetAction(thisActionId, thisAction);
            }
        }
    }
}
