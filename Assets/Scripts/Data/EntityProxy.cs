
using System.Collections.Generic;
using UnityEngine;
using Types;

[ExecuteInEditMode]
public class EntityProxy : MonoBehaviour
{
    public uint TeamId = 0;
    
    private EntityAssembly _entityAssembly;
    private bool _isInitialized = false;

    private int _concaveHullFrame = 0;
    private List<float2> _pointCloud = new List<float2>();
    private List<float2> _concaveHull = new List<float2>();
    
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

    void OnDrawGizmos()
    {
        if (!_isInitialized)
        {
            Awake();
        }
        if (_entityAssembly)
        {
            UpdateConcaveHull();

            if (transformId == 0)
            {
                Gizmos.color = GetTeamColor();
                for (int i = 0; i < _pointCloud.Count; i++)
                {
                    Vector3 v = new Vector3(_pointCloud[i].x, 0, _pointCloud[i].y);
                    Gizmos.DrawSphere(v, 0.5f);
                }
                for (int i1 = 0; i1 < _concaveHull.Count; i1++)
                {
                    int i0 = (i1 == 0) ? _concaveHull.Count - 1 : i1 - 1;
                    Vector3 v0 = new Vector3(_concaveHull[i0].x, 0, _concaveHull[i0].y);
                    Vector3 v1 = new Vector3(_concaveHull[i1].x, 0, _concaveHull[i1].y);
                    Gizmos.DrawLine(v0, v1);
                }
            }
        }
    }

    void UpdateConcaveHull()
    {
        if (!_entityAssembly)
        {
            _pointCloud.Clear();
            _concaveHull.Clear();
            return;
        }
        if (_concaveHullFrame != Time.frameCount)
        {
            _concaveHullFrame = Time.frameCount;
            
            _pointCloud.Clear();
            
            if (transformId > 0)
            {
                TransformProxy transformProxy = _entityAssembly.GetTransformProxy(transformId);
                
                Matrix4x4 entityMatrix = Matrix4x4.Translate(transformProxy.position.ToVector3()) * Matrix4x4.Rotate(transformProxy.rotation.ToQuaternion());
                Vector3 scale = transformProxy.scale.ToVector3() * 0.5f;

                Vector3 vertex = new Vector3(-scale.x, 0, -scale.z);
                vertex = entityMatrix.MultiplyPoint(vertex);
                _pointCloud.Add( new float2(vertex.x, vertex.z) );
                
                vertex = new Vector3(-scale.x, 0, scale.z);
                vertex = entityMatrix.MultiplyPoint(vertex);
                _pointCloud.Add( new float2(vertex.x, vertex.z) );
                
                vertex = new Vector3(scale.x, 0, scale.z);
                vertex = entityMatrix.MultiplyPoint(vertex);
                _pointCloud.Add( new float2(vertex.x, vertex.z) );
                
                vertex = new Vector3(scale.x, 0, -scale.z);
                vertex = entityMatrix.MultiplyPoint(vertex);
                _pointCloud.Add( new float2(vertex.x, vertex.z) );

                _concaveHull.Clear();
                _concaveHull.AddRange( _pointCloud );
            }
            else if (hierarchyId > 0)
            {
                HierarchyProxy hierarchyProxy = _entityAssembly.GetHierarchyProxy(hierarchyId);

                uint childEntityId = hierarchyProxy.firstChildEntityId;
                while (childEntityId > 0)
                {
                    EntityProxy childEntityProxy = _entityAssembly.GetEntityProxy(childEntityId);
                    childEntityProxy.UpdateConcaveHull();
                    _pointCloud.AddRange( childEntityProxy._concaveHull );
                    
                    uint childHierarchyId = childEntityProxy.hierarchyId;
                    if (childHierarchyId > 0)
                    {
                        HierarchyProxy childHierarchyProxy = _entityAssembly.GetHierarchyProxy(childHierarchyId);
                        childEntityId = childHierarchyProxy.nextSiblingEntityId;
                    }
                    else
                    {
                        break;
                    }
                }
                
                bool result = false;
                var temp = new List<float2>();
                temp.AddRange(_pointCloud);
                _concaveHull = ConcaveHull.KNearestHull(temp, Mathf.Min(temp.Count,5), ref result);
            }
        }
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
