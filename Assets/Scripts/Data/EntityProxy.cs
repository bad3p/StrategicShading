
using UnityEngine;

[ExecuteInEditMode]
public class EntityProxy : MonoBehaviour
{
    public uint TeamId = 0;
    
    private EntityAssembly _entityAssembly;
    private bool _isInitialized = false;
    
    void Awake()
    {
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisEntityId = _entityAssembly.GetEntityId(this);
            if (thisEntityId == 0)
            {
                thisEntityId = _entityAssembly.RegisterEntityProxy(this);
                teamId = TeamId;
            }
        }
        _isInitialized = true;
    }
    
    public Color GetTeamColor()
    {
        switch (teamId)
        {
            case 0: 
                return Color.black;
                break;
            case 1: 
                return Color.red;
                break;
            case 2:
                return Color.blue;
                break;
            case 3:
                return Color.Lerp( Color.green, Color.black, 0.125f );
                break;
            default:
                return Color.white;
                break;
        }
    }

    public uint entityId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                return _entityAssembly.GetEntityId(this);
            }
            else
            {
                return 0;
            }
        }
    }
    
    public uint teamId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                return entity.teamId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                entity.teamId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }

    public uint transformId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                return entity.transformId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                entity.transformId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
    
    public uint hierarchyId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                return entity.hierarchyId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                entity.hierarchyId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
    
    public uint personnelId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                return entity.personnelId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                entity.personnelId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
    
    public uint firearmsId
    {
        get
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                return entity.firearmsId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_isInitialized)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisEntityId = _entityAssembly.GetEntityId(this);
                Structs.Entity entity = _entityAssembly.GetEntity(thisEntityId);
                entity.firearmsId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
}
