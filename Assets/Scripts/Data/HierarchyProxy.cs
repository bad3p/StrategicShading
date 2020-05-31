
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class HierarchyProxy : ComponentProxy
{
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            // get entityId ahead of time
            uint thisEntityId = _entityProxy.entityId;
            if (_entityAssembly.GetEntityId(this) == 0)
            {
                _entityAssembly.RegisterHierarchyProxy(thisEntityId, this);
                firstChildEntityId = 0;
                nextSiblingEntityId = 0;
                if (transform.parent)
                {
                    HierarchyProxy parentHierarchyProxy = transform.parent.GetComponent<HierarchyProxy>();
                    if (parentHierarchyProxy)
                    {
                        parentEntityId = _entityAssembly.GetEntityId(parentHierarchyProxy);
                        parentHierarchyProxy.ConnectChild(this);
                    }
                    else
                    {
                        parentEntityId = 0;
                    }
                }
                else
                {
                    parentEntityId = 0;
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
        
        if (!_entityProxy)
        {
            Awake();
        }

        if (parentEntityId != 0)
        {
            HierarchyProxy oldParentHierarchyProxy = _entityAssembly.GetHierarchyProxy(parentEntityId);
            if (oldParentHierarchyProxy)
            {
                oldParentHierarchyProxy.DisconnectChild(this);
            }
        }
        
        if (_entityAssembly && _entityAssembly.GetEntityId(this) != 0)
        {
            _entityAssembly.UnregisterHierarchyProxy( _entityAssembly.GetEntityId(this), this );
        }
    }

    void OnTransformParentChanged()
    {
        if (!_entityProxy)
        {
            Awake();
        }

        if (_entityAssembly)
        {
            if (parentEntityId != 0)
            {
                HierarchyProxy oldParentHierarchyProxy = _entityAssembly.GetHierarchyProxy(parentEntityId);
                oldParentHierarchyProxy.DisconnectChild( this );
            }

            if (transform.parent)
            {
                HierarchyProxy newParentHierarchyProxy = transform.parent.GetComponent<HierarchyProxy>();
                if (newParentHierarchyProxy)
                {
                    parentEntityId = _entityAssembly.GetEntityId(newParentHierarchyProxy);
                    newParentHierarchyProxy.ConnectChild(this);
                }
                else
                {
                    parentEntityId = 0;
                }
            }
            else
            {
                parentEntityId = 0;
            }
        }
    }

    public void ConnectChild(HierarchyProxy childHierarchyProxy)
    {
        if (!_entityProxy)
        {
            Awake();
        }
        
        if (_entityAssembly)
        {
            childHierarchyProxy.parentEntityId = _entityAssembly.GetEntityId(this);
            if (firstChildEntityId == 0)
            {
                firstChildEntityId = _entityAssembly.GetEntityId(childHierarchyProxy);
            }
            else
            {
                HierarchyProxy siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(firstChildEntityId);
                while (siblingHierarchyProxy.nextSiblingEntityId > 0)
                {
                    siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingHierarchyProxy.nextSiblingEntityId);
                }
                siblingHierarchyProxy.nextSiblingEntityId = _entityAssembly.GetEntityId(childHierarchyProxy);
            }
        }
    }

    public void DisconnectChild(HierarchyProxy childHierarchyProxy)
    {
        if (!_entityProxy)
        {
            Awake();
        }
        
        if (_entityAssembly)
        {
            if (firstChildEntityId == _entityAssembly.GetEntityId(childHierarchyProxy))
            {
                firstChildEntityId = childHierarchyProxy.nextSiblingEntityId;
                childHierarchyProxy.nextSiblingEntityId = 0;
            }
            else
            {
                HierarchyProxy siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(firstChildEntityId);
                while (siblingHierarchyProxy.nextSiblingEntityId != _entityAssembly.GetEntityId(childHierarchyProxy) && siblingHierarchyProxy.nextSiblingEntityId != 0)
                {
                    siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingHierarchyProxy.nextSiblingEntityId);
                }
                if (siblingHierarchyProxy.nextSiblingEntityId == _entityAssembly.GetEntityId(childHierarchyProxy))
                {
                    siblingHierarchyProxy.nextSiblingEntityId = childHierarchyProxy.nextSiblingEntityId;
                    childHierarchyProxy.nextSiblingEntityId = 0;
                }
                else
                {
                    Debug.Log("[HierarchyProxy] DisconnectChild() failed, unable to locate entity " + _entityAssembly.GetEntityId(childHierarchyProxy) + " in hierarchy!");
                }
            }
            childHierarchyProxy.parentEntityId = 0;
        }
    }
    
    private static Structs.Hierarchy _dummy = new Structs.Hierarchy();

    private Structs.Hierarchy _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetHierarchy(entityId);
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
                    _entityAssembly.SetHierarchy(entityId, value);
                }
            }
        }
    }

    public uint rank
    {
        get
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                if (firstChildEntityId == 0)
                {
                    return 0;
                }
                else
                {
                    uint maxChildRank = 0;
                    
                    HierarchyProxy childHierarchyProxy = _entityAssembly.GetHierarchyProxy(firstChildEntityId);
                    uint childRank = childHierarchyProxy.rank;
                    maxChildRank = maxChildRank < childRank ? childRank : maxChildRank;

                    while (childHierarchyProxy.nextSiblingEntityId != 0)
                    {
                        childHierarchyProxy = _entityAssembly.GetHierarchyProxy(childHierarchyProxy.nextSiblingEntityId);
                        childRank = childHierarchyProxy.rank;
                        maxChildRank = maxChildRank < childRank ? childRank : maxChildRank;
                    }

                    return maxChildRank + 1;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    public uint parentEntityId
    {
        get { return _component.parentEntityId; }
        set
        {
            var temp = _component;
            temp.parentEntityId = value;
            _component = temp;
        }
    }
    
    public uint firstChildEntityId
    {
        get { return _component.firstChildEntityId; }
        set
        {
            var temp = _component;
            temp.firstChildEntityId = value;
            _component = temp;
        }
    }
    
    public uint nextSiblingEntityId
    {
        get { return _component.nextSiblingEntityId; }
        set
        {
            var temp = _component;
            temp.nextSiblingEntityId = value;
            _component = temp;
        }
    }
    
    public uint joinEntityId
    {
        get { return _component.joinEntityId; }
        set
        {
            var temp = _component;
            temp.joinEntityId = value;
            _component = temp;
        }
    }
}
