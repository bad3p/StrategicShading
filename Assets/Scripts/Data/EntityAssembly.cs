
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public partial class EntityAssembly : MonoBehaviour
{
    private List<Structs.Entity> _entityBuffer = new List<Structs.Entity>() { new Structs.Entity() };
    private Dictionary<EntityProxy, uint> _entityProxyToEntityId = new Dictionary<EntityProxy, uint>();
    
    private List<Structs.Transform> _transformBuffer = new List<Structs.Transform>() { new Structs.Transform() };
    private Dictionary<TransformProxy, uint> _transformProxyToEntityId = new Dictionary<TransformProxy, uint>();
    
    private List<Structs.Hierarchy> _hierarchyBuffer = new List<Structs.Hierarchy>() { new Structs.Hierarchy() };
    private Dictionary<HierarchyProxy, uint> _hierarchyProxyToEntityId = new Dictionary<HierarchyProxy, uint>();
    
    private List<Structs.Personnel> _personnelBuffer = new List<Structs.Personnel>() { new Structs.Personnel() };
    private Dictionary<PersonnelProxy, uint> _personnelProxyToEntityId = new Dictionary<PersonnelProxy, uint>();
    
    private List<Structs.Firearms> _firearmsBuffer = new List<Structs.Firearms>() { new Structs.Firearms() };
    private Dictionary<FirearmsProxy, uint> _firearmsProxyToEntityId = new Dictionary<FirearmsProxy, uint>();
    
    private List<Structs.Movement> _movementBuffer = new List<Structs.Movement>() { new Structs.Movement() };
    private Dictionary<MovementProxy, uint> _movementProxyToEntityId = new Dictionary<MovementProxy, uint>();
    
    private List<Structs.Firepower> _firepowerBuffer = new List<Structs.Firepower>() { new Structs.Firepower() };
    private Dictionary<FirepowerProxy, uint> _firepowerProxyToEntityId = new Dictionary<FirepowerProxy, uint>();
    
    #region Generics
    public uint GetProxyId<P>(P proxy, Dictionary<P,uint> proxyToId)
    {
        uint proxyId = 0;
        if (proxyToId.TryGetValue(proxy, out proxyId))
        {
            return proxyId;
        }
        return 0;
    }
    
    public P GetProxy<P>(uint proxyId, Dictionary<P,uint> proxyToId) where P : Component
    {
        foreach (var keyValuePair in proxyToId)
        {
            if (keyValuePair.Value == proxyId)
            {
                return keyValuePair.Key;
            }
        }

        return null;
    }
    
    public uint RegisterProxy<P,S>(P proxy, List<S> structBuffer, Dictionary<P,uint> proxyToId) where S : struct
    {
        uint proxyId = GetProxyId<P>(proxy, proxyToId);
        if (proxyId == 0)
        {
            proxyId = (uint) structBuffer.Count;
            proxyToId.Add( proxy, proxyId );
            structBuffer.Add( new S() );
        }
        return proxyId;
    }
    
    public S GetStruct<P,S>(uint proxyId, List<S> structBuffer) where S : struct
    {
        if (proxyId > 0 && proxyId < structBuffer.Count)
        {
            return structBuffer[(int)proxyId];
        }
        else
        {
            Debug.LogError("[EntityAssembly] GetStruct<" + typeof(P).Name + "," + typeof(S).Name + ">() failed, invalid proxyId: " + proxyId + "!");
            return new S();
        }
    }
    
    public void SetStruct<P,S>(uint proxyId, S s, List<S> structBuffer) where S : struct
    {
        if (proxyId > 0 && proxyId < structBuffer.Count)
        {
            structBuffer[(int) proxyId] = s;
        }
        else
        {
            Debug.LogError("[EntityAssembly] SetEntity<" + typeof(P).Name + "," + typeof(S).Name + ">() failed, invalid proxiId: " + proxyId + "!");
        }
    }
    #endregion
    
    #region Buffers
    public List<Structs.Entity> entityBuffer
    {
        get { return _entityBuffer; }
    }

    public List<Structs.Transform> transformBuffer
    {
        get { return _transformBuffer;  }
    }

    public List<Structs.Hierarchy> hierarchyBuffer
    {
        get { return _hierarchyBuffer; }
    }
    public List<Structs.Personnel> personnelBuffer
    {
        get { return _personnelBuffer;  }
    }

    public List<Structs.Firearms> firearmsBuffer
    {
        get { return _firearmsBuffer; }
    }

    public List<Structs.Movement> movementBuffer
    {
        get { return _movementBuffer;  }
    }
    #endregion
    
    #region Entities
    public uint GetEntityId(EntityProxy entityProxy)
    {
        return GetProxyId( entityProxy, _entityProxyToEntityId );
    }
    
    public EntityProxy GetEntityProxy(uint entityId)
    {
        return GetProxy( entityId, _entityProxyToEntityId );
    }

    public uint RegisterEntityProxy(EntityProxy entityProxy)
    {
        return RegisterProxy( entityProxy, _entityBuffer, _entityProxyToEntityId );
    }
    
    public Structs.Entity GetEntity(uint entityId)
    {
        return GetStruct<EntityProxy,Structs.Entity>(entityId, _entityBuffer);
    }
    
    public void SetEntity(uint entityId, Structs.Entity entity)
    {
        SetStruct<EntityProxy,Structs.Entity>(entityId, entity, _entityBuffer);
    }
    #endregion
    
    #region Transforms
    public uint GetTransformId(TransformProxy transformProxy)
    {
        return GetProxyId( transformProxy, _transformProxyToEntityId );
    }
    
    public TransformProxy GetTransformProxy(uint transformId)
    {
        return GetProxy( transformId, _transformProxyToEntityId );
    }
    
    public uint RegisterTransformProxy(TransformProxy transformProxy)
    {
        return RegisterProxy( transformProxy, _transformBuffer, _transformProxyToEntityId );
    }
    
    public Structs.Transform GetTransform(uint transformId)
    {
        return GetStruct<TransformProxy,Structs.Transform>(transformId, _transformBuffer);
    }
    
    public void SetTransform(uint transformId, Structs.Transform t)
    {
        SetStruct<TransformProxy,Structs.Transform>(transformId, t, _transformBuffer);
    }
    #endregion
    
    #region Hierarchies
    public uint GetHierarchyId(HierarchyProxy hierarchyProxy)
    {
        return GetProxyId( hierarchyProxy, _hierarchyProxyToEntityId );
    }
    
    public HierarchyProxy GetHierarchyProxy(uint hierarchyId)
    {
        return GetProxy( hierarchyId, _hierarchyProxyToEntityId );
    }
    
    public uint RegisterHierarchyProxy(HierarchyProxy hierarchyProxy)
    {
        return RegisterProxy( hierarchyProxy, _hierarchyBuffer, _hierarchyProxyToEntityId );
    }
    
    public Structs.Hierarchy GetHierarchy(uint hierarchyId)
    {
        return GetStruct<HierarchyProxy,Structs.Hierarchy>(hierarchyId, _hierarchyBuffer);
    }
    
    public void SetHierarchy(uint hierarchyId, Structs.Hierarchy h)
    {
        SetStruct<HierarchyProxy,Structs.Hierarchy>(hierarchyId, h, _hierarchyBuffer);
    }
    #endregion
    
    #region Personnel
    public uint GetPersonnelId(PersonnelProxy personnelProxy)
    {
        return GetProxyId( personnelProxy, _personnelProxyToEntityId );
    }
    
    public PersonnelProxy GetPersonnelProxy(uint personnelId)
    {
        return GetProxy( personnelId, _personnelProxyToEntityId );
    }
    
    public uint RegisterPersonnelProxy(PersonnelProxy personnelProxy)
    {
        return RegisterProxy( personnelProxy, _personnelBuffer, _personnelProxyToEntityId );
    }
    
    public Structs.Personnel GetPersonnel(uint personnelId)
    {
        return GetStruct<PersonnelProxy,Structs.Personnel>(personnelId, _personnelBuffer);
    }
    
    public void SetPersonnel(uint personnelId, Structs.Personnel p)
    {
        SetStruct<PersonnelProxy,Structs.Personnel>(personnelId, p, _personnelBuffer);
    }
    #endregion
    
    #region Firearms
    public uint GetFirearmsId(FirearmsProxy firearmsProxy)
    {
        return GetProxyId( firearmsProxy, _firearmsProxyToEntityId );
    }
    
    public FirearmsProxy GetFirearmsProxy(uint firearmsId)
    {
        return GetProxy( firearmsId, _firearmsProxyToEntityId );
    }
    
    public uint RegisterFirearmsProxy(FirearmsProxy firearmsProxy)
    {
        return RegisterProxy( firearmsProxy, _firearmsBuffer, _firearmsProxyToEntityId );
    }
    
    public Structs.Firearms GetFirearms(uint firearmsId)
    {
        return GetStruct<FirearmsProxy,Structs.Firearms>(firearmsId, _firearmsBuffer);
    }
    
    public void SetFirearms(uint firearmsId, Structs.Firearms f)
    {
        SetStruct<FirearmsProxy,Structs.Firearms>(firearmsId, f, _firearmsBuffer);
    }
    #endregion
    
    #region Movement
    public uint GetMovementId(MovementProxy movementProxy)
    {
        return GetProxyId( movementProxy, _movementProxyToEntityId );
    }
    
    public MovementProxy GetMovementProxy(uint movementId)
    {
        return GetProxy( movementId, _movementProxyToEntityId );
    }
    
    public uint RegisterMovementProxy(MovementProxy movementProxy)
    {
        return RegisterProxy( movementProxy, _movementBuffer, _movementProxyToEntityId );
    }
    
    public Structs.Movement GetMovement(uint movementId)
    {
        return GetStruct<MovementProxy,Structs.Movement>(movementId, _movementBuffer);
    }
    
    public void SetMovement(uint movementId, Structs.Movement m)
    {
        SetStruct<MovementProxy,Structs.Movement>(movementId, m, _movementBuffer);
    }
    #endregion
    
    #region Firepower
    public uint GetFirepowerId(FirepowerProxy firepowerProxy)
    {
        return GetProxyId( firepowerProxy, _firepowerProxyToEntityId );
    }
    
    public FirepowerProxy GetFirepowerProxy(uint firepowerId)
    {
        return GetProxy( firepowerId, _firepowerProxyToEntityId );
    }
    
    public uint RegisterFirepowerProxy(FirepowerProxy firepowerProxy)
    {
        return RegisterProxy( firepowerProxy, _firepowerBuffer, _firepowerProxyToEntityId );
    }
    
    public Structs.Firepower GetFirepower(uint firepowerId)
    {
        return GetStruct<FirepowerProxy,Structs.Firepower>(firepowerId, _firepowerBuffer);
    }
    
    public void SetFirepower(uint firepowerId, Structs.Firepower f)
    {
        SetStruct<FirepowerProxy,Structs.Firepower>(firepowerId, f, _firepowerBuffer);
    }
    #endregion
}
