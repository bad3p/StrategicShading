﻿
using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public partial class EntityAssembly : MonoBehaviour
{
    private List<uint> _descBuffer = new List<uint>() { 0 };
    private Dictionary<uint, EntityProxy> _entityProxyById = new Dictionary<uint,EntityProxy>();
    private Dictionary<EntityProxy, uint> _idByEntityProxy = new Dictionary<EntityProxy,uint>();
    
    private List<Structs.Transform> _transformBuffer = new List<Structs.Transform>() { new Structs.Transform() };
    private Dictionary<uint, TransformProxy> _transformProxyById = new Dictionary<uint, TransformProxy>();
    private Dictionary<TransformProxy, uint> _idByTransformProxy = new Dictionary<TransformProxy,uint>();
    
    private List<Structs.Hierarchy> _hierarchyBuffer = new List<Structs.Hierarchy>() { new Structs.Hierarchy() };
    private Dictionary<uint, HierarchyProxy> _hierarchyProxyById = new Dictionary<uint, HierarchyProxy>();
    private Dictionary<HierarchyProxy, uint> _idByHierarchyProxy = new Dictionary<HierarchyProxy,uint>();
    
    private List<Structs.Personnel> _personnelBuffer = new List<Structs.Personnel>() { new Structs.Personnel() };
    private Dictionary<uint, PersonnelProxy> _personnelProxyById = new Dictionary<uint, PersonnelProxy>();
    private Dictionary<PersonnelProxy, uint> _idByPersonnelProxy = new Dictionary<PersonnelProxy,uint>();
    
    private List<Structs.Firearms> _firearmsBuffer = new List<Structs.Firearms>() { new Structs.Firearms() };
    private Dictionary<uint, FirearmsProxy> _firearmsProxyById = new Dictionary<uint, FirearmsProxy>();
    private Dictionary<FirearmsProxy, uint> _idByFirearmsProxy = new Dictionary<FirearmsProxy,uint>();
    
    private List<Structs.Movement> _movementBuffer = new List<Structs.Movement>() { new Structs.Movement() };
    private Dictionary<uint, MovementProxy> _movementProxyById = new Dictionary<uint, MovementProxy>();
    private Dictionary<MovementProxy, uint> _idByMovementProxy = new Dictionary<MovementProxy,uint>();
    
    private List<Structs.Firepower> _firepowerBuffer = new List<Structs.Firepower>() { new Structs.Firepower() };
    private Dictionary<uint, FirepowerProxy> _firepowerProxyById = new Dictionary<uint, FirepowerProxy>();
    private Dictionary<FirepowerProxy, uint> _idByFirepowerProxy = new Dictionary<FirepowerProxy,uint>();
    
    #region Generics
    uint GetComponentBitMask<P>(P proxy) where P : ComponentProxy
    {
        if (proxy is TransformProxy)
        {
            return ComputeShaderEmulator.TRANSFORM;
        }
        else if (proxy is HierarchyProxy)
        {
            return ComputeShaderEmulator.HIERARCHY;
        }
        else if (proxy is PersonnelProxy)
        {
            return ComputeShaderEmulator.PERSONNEL;
        }
        else if (proxy is FirearmsProxy)
        {
            return ComputeShaderEmulator.FIREARMS;
        }
        else if (proxy is MovementProxy)
        {
            return ComputeShaderEmulator.MOVEMENT;
        }
        else if (proxy is FirepowerProxy)
        {
            return ComputeShaderEmulator.FIREPOWER;
        }
        else
        {
            return 0;
        }
    }
    
    uint GetComponentEntityId<P>(P proxy, Dictionary<P,uint> idByProxy)
    {
        uint entityId = 0;
        if (idByProxy.TryGetValue(proxy, out entityId))
        {
            return entityId;
        }
        return 0;
    }
    
    P GetComponentProxy<P>(uint entityId, Dictionary<uint,P> proxyById) where P : Component
    {
        P proxy = null;
        if (proxyById.TryGetValue(entityId, out proxy))
        {
            return proxy;
        }
        return null as P;
    }
    
    void RegisterComponentProxy<P>(uint entityId, P proxy, Dictionary<P,uint> idByProxy, Dictionary<uint,P> proxyById) where P : ComponentProxy
    {
        uint componentBitMask = GetComponentBitMask(proxy);
        if (componentBitMask == 0)
        {
            Debug.LogError("[EntityAssembly] RegisterComponentProxy<" + typeof(P).Name + ">() failed, component bitmask is 0!" );
            return;
        }
        
        if ( GetComponentEntityId( proxy, idByProxy ) == 0 )
        {
            proxyById.Add( entityId, proxy );
            idByProxy.Add( proxy, entityId );
            SetEntityDesc(entityId, GetEntityDesc(entityId) | componentBitMask);
        }
        else
        {
            Debug.LogError("[EntityAssembly] RegisterComponentProxy<" + typeof(P).Name + ">() failed, entity " + entityId + " already have this component!");
        }
    }
    
    void UnregisterComponentProxy<P>(uint entityId, P proxy, Dictionary<P,uint> idByProxy, Dictionary<uint,P> proxyById) where P : ComponentProxy
    {
        uint componentBitMask = GetComponentBitMask(proxy);
        if (componentBitMask == 0)
        {
            Debug.LogError("[EntityAssembly] RegisterComponentProxy<" + typeof(P).Name + ">() failed, component bitmask is 0!" );
            return;
        }
        
        if ( GetComponentEntityId( proxy, idByProxy ) != 0 )
        {
            proxyById.Remove( entityId );
            idByProxy.Remove( proxy );
            SetEntityDesc(entityId, GetEntityDesc(entityId) & ~componentBitMask);
        }
        else
        {
            Debug.LogError("[EntityAssembly] UnregisterComponentProxy<" + typeof(P).Name + ">() failed, entity " + entityId + " have no such component!");
        }
    }
    
    S GetComponent<P,S>(uint entityId, List<S> structBuffer) where S : struct
    {
        if (entityId > 0 && entityId < structBuffer.Count)
        {
            return structBuffer[(int)entityId];
        }
        else
        {
            Debug.LogError("[EntityAssembly] GetStruct<" + typeof(P).Name + "," + typeof(S).Name + ">() failed, invalid entityId: " + entityId + "!");
            return new S();
        }
    }
    
    public void SetComponent<P,S>(uint entityId, S s, List<S> structBuffer) where S : struct
    {
        if (entityId > 0 && entityId < structBuffer.Count)
        {
            structBuffer[(int) entityId] = s;
        }
        else
        {
            Debug.LogError("[EntityAssembly] SetEntity<" + typeof(P).Name + "," + typeof(S).Name + ">() failed, invalid proxiId: " + entityId + "!");
        }
    }
    #endregion
    
    #region Buffers
    public List<uint> descBuffer
    {
        get { return _descBuffer; }
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
        uint entityId = 0;
        if (_idByEntityProxy.TryGetValue(entityProxy, out entityId))
        {
            return entityId;
        }
        return 0;
    }
    
    public EntityProxy GetEntityProxy(uint entityId)
    {
        EntityProxy entityProxy = null;
        if (_entityProxyById.TryGetValue(entityId, out entityProxy))
        {
            return entityProxy;
        }
        return null as EntityProxy;
    }

    public uint RegisterEntityProxy(EntityProxy entityProxy)
    {
        uint entityId = GetEntityId( entityProxy );
        if (entityId == 0)
        {
            entityId = (uint) descBuffer.Count;
            _entityProxyById.Add( entityId, entityProxy );
            _idByEntityProxy.Add( entityProxy, entityId );
            _descBuffer.Add( 0 );
            _transformBuffer.Add( new Structs.Transform() );
            _hierarchyBuffer.Add( new Structs.Hierarchy() );
            _personnelBuffer.Add( new Structs.Personnel() );
            _firearmsBuffer.Add( new Structs.Firearms() );
            _movementBuffer.Add( new Structs.Movement() );
            _firepowerBuffer.Add( new Structs.Firepower() );
        }
        return entityId;
    }
    
    public void UnregisterEntityProxy(EntityProxy entityProxy)
    {
        uint entityId = GetEntityId( entityProxy );
        if (entityId != 0)
        {
            _entityProxyById.Remove(entityId);
            _idByEntityProxy.Remove(entityProxy);
        }
    }
    
    public uint GetEntityDesc(uint entityId)
    {
        if (entityId > 0 && entityId < _descBuffer.Count)
        {
            return _descBuffer[(int)entityId];
        }
        else
        {
            return 0;
        }
    }
    
    public void SetEntityDesc(uint entityId, uint entityDesc)
    {
        if (entityId > 0 && entityId < _descBuffer.Count)
        {
            _descBuffer[(int)entityId] = entityDesc;
        }
    }
    #endregion
    
    #region Transforms
    public uint GetEntityId(TransformProxy transformProxy)
    {
        return GetComponentEntityId( transformProxy, _idByTransformProxy );
    }
    
    public TransformProxy GetTransformProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _transformProxyById );
    }
    
    public void RegisterTransformProxy(uint entityId, TransformProxy transformProxy)
    {
        RegisterComponentProxy( entityId, transformProxy, _idByTransformProxy, _transformProxyById );
    }
    
    public void UnregisterTransformProxy(uint entityId, TransformProxy transformProxy)
    {
        UnregisterComponentProxy( entityId, transformProxy, _idByTransformProxy, _transformProxyById );
    }
    
    public Structs.Transform GetTransform(uint entityId)
    {
        return GetComponent<TransformProxy,Structs.Transform>(entityId, _transformBuffer);
    }
    
    public void SetTransform(uint entityId, Structs.Transform t)
    {
        SetComponent<TransformProxy,Structs.Transform>(entityId, t, _transformBuffer);
    }
    #endregion
    
    #region Hierarchies
    public uint GetEntityId(HierarchyProxy hierarchyProxy)
    {
        return GetComponentEntityId( hierarchyProxy, _idByHierarchyProxy );
    }
    
    public HierarchyProxy GetHierarchyProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _hierarchyProxyById );
    }
    
    public void RegisterHierarchyProxy(uint entityId, HierarchyProxy hierarchyProxy)
    {
        RegisterComponentProxy( entityId, hierarchyProxy, _idByHierarchyProxy, _hierarchyProxyById );
    }
    
    public void UnregisterHierarchyProxy(uint entityId, HierarchyProxy hierarchyProxy)
    {
        UnregisterComponentProxy( entityId, hierarchyProxy, _idByHierarchyProxy, _hierarchyProxyById );
    }
    
    public Structs.Hierarchy GetHierarchy(uint entityId)
    {
        return GetComponent<HierarchyProxy,Structs.Hierarchy>(entityId, _hierarchyBuffer);
    }
    
    public void SetHierarchy(uint entityId, Structs.Hierarchy h)
    {
        SetComponent<HierarchyProxy,Structs.Hierarchy>(entityId, h, _hierarchyBuffer);
    }
    #endregion
    
    #region Personnel
    public uint GetEntityId(PersonnelProxy personnelProxy)
    {
        return GetComponentEntityId( personnelProxy, _idByPersonnelProxy );
    }
    
    public PersonnelProxy GetPersonnelProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _personnelProxyById );
    }
    
    public void RegisterPersonnelProxy(uint entityId, PersonnelProxy personnelProxy)
    {
        RegisterComponentProxy( entityId, personnelProxy, _idByPersonnelProxy, _personnelProxyById );
    }
    
    public void UnregisterPersonnelProxy(uint entityId, PersonnelProxy personnelProxy)
    {
        UnregisterComponentProxy( entityId, personnelProxy, _idByPersonnelProxy, _personnelProxyById );
    }
    
    public Structs.Personnel GetPersonnel(uint entityId)
    {
        return GetComponent<PersonnelProxy,Structs.Personnel>(entityId, _personnelBuffer);
    }
    
    public void SetPersonnel(uint entityId, Structs.Personnel p)
    {
        SetComponent<PersonnelProxy,Structs.Personnel>(entityId, p, _personnelBuffer);
    }
    #endregion
    
    #region Firearms
    public uint GetEntityId(FirearmsProxy firearmsProxy)
    {
        return GetComponentEntityId( firearmsProxy, _idByFirearmsProxy );
    }
    
    public FirearmsProxy GetFirearmsProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _firearmsProxyById );
    }
    
    public void RegisterFirearmsProxy(uint entityId, FirearmsProxy firearmsProxy)
    {
        RegisterComponentProxy( entityId, firearmsProxy, _idByFirearmsProxy, _firearmsProxyById );
    }
    
    public void UnregisterFirearmsProxy(uint entityId, FirearmsProxy firearmsProxy)
    {
        UnregisterComponentProxy( entityId, firearmsProxy, _idByFirearmsProxy, _firearmsProxyById );
    }
    
    public Structs.Firearms GetFirearms(uint entityId)
    {
        return GetComponent<FirearmsProxy,Structs.Firearms>(entityId, _firearmsBuffer);
    }
    
    public void SetFirearms(uint entityId, Structs.Firearms f)
    {
        SetComponent<FirearmsProxy,Structs.Firearms>(entityId, f, _firearmsBuffer);
    }
    #endregion
    
    #region Movement
    public uint GetEntityId(MovementProxy movementProxy)
    {
        return GetComponentEntityId( movementProxy, _idByMovementProxy );
    }
    
    public MovementProxy GetMovementProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _movementProxyById );
    }
    
    public void RegisterMovementProxy(uint entityId, MovementProxy movementProxy)
    {
        RegisterComponentProxy( entityId, movementProxy, _idByMovementProxy, _movementProxyById );
    }
    
    public void UnregisterMovementProxy(uint entityId, MovementProxy movementProxy)
    {
        UnregisterComponentProxy( entityId, movementProxy, _idByMovementProxy, _movementProxyById );
    }
    
    public Structs.Movement GetMovement(uint entityId)
    {
        return GetComponent<MovementProxy,Structs.Movement>(entityId, _movementBuffer);
    }
    
    public void SetMovement(uint entityId, Structs.Movement m)
    {
        SetComponent<MovementProxy,Structs.Movement>(entityId, m, _movementBuffer);
    }
    #endregion
    
    #region Firepower
    public uint GetEntityId(FirepowerProxy firepowerProxy)
    {
        return GetComponentEntityId( firepowerProxy, _idByFirepowerProxy );
    }
    
    public FirepowerProxy GetFirepowerProxy(uint entityId)
    {
        return GetComponentProxy( entityId, _firepowerProxyById );
    }
    
    public void RegisterFirepowerProxy(uint entityId, FirepowerProxy firepowerProxy)
    {
        RegisterComponentProxy( entityId, firepowerProxy, _idByFirepowerProxy, _firepowerProxyById );
    }
    
    public void UnregisterFirepowerProxy(uint entityId, FirepowerProxy firepowerProxy)
    {
        UnregisterComponentProxy( entityId, firepowerProxy, _idByFirepowerProxy, _firepowerProxyById );
    }
    
    public Structs.Firepower GetFirepower(uint entityId)
    {
        return GetComponent<FirepowerProxy,Structs.Firepower>(entityId, _firepowerBuffer);
    }
    
    public void SetFirepower(uint entityId, Structs.Firepower f)
    {
        SetComponent<FirepowerProxy,Structs.Firepower>(entityId, f, _firepowerBuffer);
    }
    #endregion
}
