
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
            uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
            if (thisHierarchyId == 0)
            {
                thisHierarchyId = _entityAssembly.RegisterHierarchyProxy(this);
                _entityProxy.hierarchyId = thisHierarchyId;
                entityId = _entityProxy.entityId;
                firstChildEntityId = 0;
                nextSiblingEntityId = 0;
                if (transform.parent)
                {
                    HierarchyProxy parentHierarchyProxy = transform.parent.GetComponent<HierarchyProxy>();
                    if (parentHierarchyProxy)
                    {
                        parentEntityId = parentHierarchyProxy.entityId;
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
            EntityProxy oldParentEntityProxy = _entityAssembly.GetEntityProxy(parentEntityId);
            HierarchyProxy oldParentHierarchyProxy = _entityAssembly.GetHierarchyProxy(oldParentEntityProxy.hierarchyId);
            if (oldParentHierarchyProxy)
            {
                oldParentHierarchyProxy.DisconnectChild(this);
            }
        }
        
        _entityProxy.hierarchyId = 0;
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
                EntityProxy oldParentEntityProxy = _entityAssembly.GetEntityProxy(parentEntityId);
                HierarchyProxy oldParentHierarchyProxy = _entityAssembly.GetHierarchyProxy(oldParentEntityProxy.hierarchyId);
                oldParentHierarchyProxy.DisconnectChild( this );
            }

            if (transform.parent)
            {
                HierarchyProxy newParentHierarchyProxy = transform.parent.GetComponent<HierarchyProxy>();
                if (newParentHierarchyProxy)
                {
                    parentEntityId = newParentHierarchyProxy.entityId;
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
            childHierarchyProxy.parentEntityId = entityId;
            if (firstChildEntityId == 0)
            {
                firstChildEntityId = childHierarchyProxy.entityId;
            }
            else
            {
                EntityProxy siblingEntityProxy = _entityAssembly.GetEntityProxy(firstChildEntityId);
                HierarchyProxy siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingEntityProxy.hierarchyId);
                while (siblingHierarchyProxy.nextSiblingEntityId > 0)
                {
                    siblingEntityProxy = _entityAssembly.GetEntityProxy(siblingHierarchyProxy.nextSiblingEntityId);
                    siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingEntityProxy.hierarchyId);
                }
                siblingHierarchyProxy.nextSiblingEntityId = childHierarchyProxy.entityId;
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
            if (firstChildEntityId == childHierarchyProxy.entityId)
            {
                firstChildEntityId = childHierarchyProxy.nextSiblingEntityId;
                childHierarchyProxy.nextSiblingEntityId = 0;
            }
            else
            {
                EntityProxy siblingEntityProxy = _entityAssembly.GetEntityProxy(firstChildEntityId);
                HierarchyProxy siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingEntityProxy.hierarchyId);
                while (siblingHierarchyProxy.nextSiblingEntityId != childHierarchyProxy.entityId && siblingHierarchyProxy.nextSiblingEntityId != 0)
                {
                    siblingEntityProxy = _entityAssembly.GetEntityProxy(siblingHierarchyProxy.nextSiblingEntityId);
                    siblingHierarchyProxy = _entityAssembly.GetHierarchyProxy(siblingEntityProxy.hierarchyId);
                }
                if (siblingHierarchyProxy.nextSiblingEntityId == childHierarchyProxy.entityId)
                {
                    siblingHierarchyProxy.nextSiblingEntityId = childHierarchyProxy.nextSiblingEntityId;
                    childHierarchyProxy.nextSiblingEntityId = 0;
                }
                else
                {
                    Debug.Log("[HierarchyProxy] DisconnectChild() failed, unable to locate entity " + childHierarchyProxy.entityId + " in hierarchy!");
                }
            }
            childHierarchyProxy.parentEntityId = 0;
        }
    }

    public uint entityId
    {
        get
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly && _entityAssembly.GetHierarchyId(this) != 0)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                return thisHierarchy.entityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                thisHierarchy.entityId = value;
                _entityAssembly.SetHierarchy(thisHierarchyId, thisHierarchy);
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
                    
                    EntityProxy childEntityProxy =  _entityAssembly.GetEntityProxy(firstChildEntityId);
                    HierarchyProxy childHierarchyProxy = _entityAssembly.GetHierarchyProxy(childEntityProxy.hierarchyId);
                    uint childRank = childHierarchyProxy.rank;
                    maxChildRank = maxChildRank < childRank ? childRank : maxChildRank;

                    while (childHierarchyProxy.nextSiblingEntityId > 0)
                    {
                        childEntityProxy =  _entityAssembly.GetEntityProxy(childHierarchyProxy.nextSiblingEntityId);
                        childHierarchyProxy = _entityAssembly.GetHierarchyProxy(childEntityProxy.hierarchyId);
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
        get
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly && _entityAssembly.GetHierarchyId(this) != 0)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                return thisHierarchy.parentEntityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                thisHierarchy.parentEntityId = value;
                _entityAssembly.SetHierarchy(thisHierarchyId, thisHierarchy);
            }
        }
    }
    
    public uint firstChildEntityId
    {
        get
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly && _entityAssembly.GetHierarchyId(this) != 0)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                return thisHierarchy.firstChildEntityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                thisHierarchy.firstChildEntityId = value;
                _entityAssembly.SetHierarchy(thisHierarchyId, thisHierarchy);
            }
        }
    }
    
    public uint nextSiblingEntityId
    {
        get
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly && _entityAssembly.GetHierarchyId(this) != 0)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                return thisHierarchy.nextSiblingEntityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (!_entityProxy)
            {
                Awake();
            }
            if (_entityAssembly)
            {
                uint thisHierarchyId = _entityAssembly.GetHierarchyId(this);
                Structs.Hierarchy thisHierarchy = _entityAssembly.GetHierarchy(thisHierarchyId);
                thisHierarchy.nextSiblingEntityId = value;
                _entityAssembly.SetHierarchy(thisHierarchyId, thisHierarchy);
            }
        }
    }
}
