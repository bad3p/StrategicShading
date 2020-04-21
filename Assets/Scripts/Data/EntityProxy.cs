
using UnityEngine;

[ExecuteInEditMode]
public class EntityProxy : MonoBehaviour
{
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
            }
        }
        _isInitialized = true;
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
}
