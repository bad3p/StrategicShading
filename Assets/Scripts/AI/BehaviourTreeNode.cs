
using System;
using UnityEngine;

public abstract class BehaviourTreeNode : MonoBehaviour
{
    public enum Status
    {
        Running,
        Success,
        Failure
    }
    
    #region ETCBinding
    private uint _entityId = 0;
    private EntityAssembly _entityAssembly = null;

    public uint entityId
    {
        get { return _entityId; }
        protected set { _entityId = value; }
    }
    
    public EntityAssembly entityAssembly
    {
        get { return _entityAssembly; }
        protected set { _entityAssembly = value; }
    }
    #endregion
    
    #region ETCAccess
    protected bool TryGetEntity(uint eId, ref Structs.Entity e)
    {
        if (_entityAssembly && eId > 0)
        {
            e = _entityAssembly.GetEntity(eId);
            return true;
        }
        return false;
    }
    protected bool TryGetTransform(ref Structs.Entity e, ref Structs.Transform t)
    {
        if (_entityAssembly && e.transformId > 0)
        {
            t = _entityAssembly.GetTransform(e.transformId);
            return true;
        }
        return false;
    }
    protected bool TryGetHierarchy(ref Structs.Entity e, ref Structs.Hierarchy h)
    {
        if (_entityAssembly && e.hierarchyId > 0)
        {
            h = _entityAssembly.GetHierarchy(e.hierarchyId);
            return true;
        }
        return false;
    }
    protected bool TryGetPersonnel(ref Structs.Entity e, ref Structs.Personnel p)
    {
        if (_entityAssembly && e.personnelId > 0)
        {
            p = _entityAssembly.GetPersonnel(e.personnelId);
            return true;
        }
        return false;
    }
    protected bool TryGetFirearms(ref Structs.Entity e, ref Structs.Firearms f)
    {
        if (_entityAssembly && e.firearmsId > 0)
        {
            f = _entityAssembly.GetFirearms(e.personnelId);
            return true;
        }
        return false;
    }
    protected bool TryGetMovement(ref Structs.Entity e, ref Structs.Movement m)
    {
        if (_entityAssembly && e.movementId > 0)
        {
            m = _entityAssembly.GetMovement(e.movementId);
            return true;
        }
        return false;
    }
    protected bool TryGetFirepower(ref Structs.Entity e, ref Structs.Firepower f)
    {
        if (_entityAssembly && e.firepowerId > 0)
        {
            f = _entityAssembly.GetFirepower(e.firepowerId);
            return true;
        }
        return false;
    }
    protected void SetTransform(ref Structs.Entity e, ref Structs.Transform t)
    {
        if (_entityAssembly && e.transformId > 0)
        {
            _entityAssembly.SetTransform(e.transformId, t);
        }
    }
    protected void SetPersonnel(ref Structs.Entity e, ref Structs.Personnel p)
    {
        if (_entityAssembly && e.personnelId > 0)
        {
            _entityAssembly.SetPersonnel(e.personnelId, p);
        }
    }
    protected void SetFirearms(ref Structs.Entity e, ref Structs.Firearms f)
    {
        if (_entityAssembly && e.firearmsId > 0)
        {
            _entityAssembly.SetFirearms(e.firearmsId, f);
        }
    }
    protected void SetMovement(ref Structs.Entity e, ref Structs.Movement m)
    {
        if (_entityAssembly && e.movementId > 0)
        {
            _entityAssembly.SetMovement(e.movementId, m);
        }
    }
    protected void SetFirepower(ref Structs.Entity e, ref Structs.Firepower f)
    {
        if (_entityAssembly && e.firepowerId > 0)
        {
            _entityAssembly.SetFirepower(e.firepowerId, f);
        }
    }
    #endregion
    
    #region ETCHelpers
    protected bool TryGetFirstChildEntity(ref Structs.Hierarchy hierarchy, ref Structs.Entity firstChildEntity)
    {
        return TryGetEntity(hierarchy.firstChildEntityId, ref firstChildEntity);
    }
    protected bool TryGetNextSiblingEntity(ref Structs.Hierarchy hierarchy, ref Structs.Entity nextSiblingEntity)
    {
        return TryGetEntity(hierarchy.nextSiblingEntityId, ref nextSiblingEntity);
    }
    protected bool TryGetParentEntity(ref Structs.Hierarchy hierarchy, ref Structs.Entity parentEntity)
    {
        return TryGetEntity(hierarchy.parentEntityId, ref parentEntity);
    }
    protected uint GetChildCount(ref Structs.Hierarchy hierarchy)
    {
        Structs.Entity childEntity = new Structs.Entity();
        Structs.Hierarchy childHierarchy = new Structs.Hierarchy();
        if ( TryGetFirstChildEntity(ref hierarchy, ref childEntity) && TryGetHierarchy(ref childEntity, ref childHierarchy) )
        {
            uint result = 1;
            while (childHierarchy.nextSiblingEntityId > 0)
            {
                result++;
                if (!TryGetNextSiblingEntity(ref hierarchy, ref childEntity) && TryGetHierarchy(ref childEntity, ref childHierarchy))
                {
                    break;
                }
            }

            return result;
        }
        return 0;
    }
    #endregion
    
    #region Status
    private Status _status = Status.Running;
    public Status status
    {
        get { return _status; }
        protected set { _status = value;  }
    }
    #endregion
    
    #region Abstraction
    public abstract void Initiate(BehaviourTreeNode parentNode);
    public abstract void Run();
    #endregion
}