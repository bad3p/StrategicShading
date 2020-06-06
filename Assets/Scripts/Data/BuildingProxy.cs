
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class BuildingProxy : ComponentProxy
{
    public uint DescID = 0;
    public float Integrity = 0.0f;
    
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
                _entityAssembly.RegisterBuildingProxy(_entityProxy.entityId, this);
                descId = DescID;
                integrity = Integrity;
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
            _entityAssembly.UnregisterBuildingProxy( _entityAssembly.GetEntityId(this), this );
        }
    }
    
    private static Structs.Building _dummy = new Structs.Building();

    private Structs.Building _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetBuilding(entityId);
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
                    _entityAssembly.SetBuilding(entityId, value);
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

        if (descId < _entityAssembly.BuildingDescBuffer.Length)
        {
            var buildingDesc = _entityAssembly.BuildingDescBuffer[(int) descId];
            Integrity = buildingDesc.maxIntegrity;
            integrity = Integrity;
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

    public float integrity
    {
        get { return _component.integrity; }
        set
        {
            var temp = _component;
            temp.integrity = value;
            _component = temp;
        }
    }
}
