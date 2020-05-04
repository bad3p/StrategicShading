
using System;
using Types;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class MovementProxy : ComponentProxy
{
    public uint TargetEntityID = 0;
    public double3 TargetPosition = new double3(0,0,0);
    public float TargetVelocity = 0;
    
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisMovementId = _entityAssembly.GetMovementId(this);
            if (thisMovementId == 0)
            {
                thisMovementId = _entityAssembly.RegisterMovementProxy(this);
                _entityProxy.movementId = thisMovementId;
                entityId = _entityProxy.entityId;
                targetEntityId = TargetEntityID;
                targetPosition = TargetPosition;
                targetVelocity = TargetVelocity;
            }
        }    
    }

    void OnDestroy()
    {
        _entityProxy.movementId = 0;
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
            if (targetEntityId == 0)
            {
                float3 pos = targetPosition.ToVector3();
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
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.entityId;
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
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.entityId = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public uint targetEntityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetEntityId;
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
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetEntityId = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public double3 targetPosition
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetPosition;
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
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetPosition = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
    
    public float targetVelocity
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                return thisMovement.targetVelocity;
            }
            else
            {
                return 0.0f;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisMovementId = _entityAssembly.GetMovementId(this);
                Structs.Movement thisMovement = _entityAssembly.GetMovement(thisMovementId);
                thisMovement.targetVelocity = value;
                _entityAssembly.SetMovement(thisMovementId, thisMovement);
            }
        }
    }
}
