
using System.Collections.Generic;
using UnityEngine;
using Types;

[ExecuteInEditMode]
public class EntityProxy : MonoBehaviour
{
    public uint TeamId = 0;
    
    private EntityAssembly _entityAssembly;
    private bool _isInitialized = false;

    private int _hullFrame = 0;
    private List<float2> _pointCloud = new List<float2>();
    private List<float2> _hull = new List<float2>();
    private int _hullHash = 0;
    private int _meshHash = 0;
    private Mesh _mesh = null;

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

            ComponentProxy[] componentProxies = GetComponents<ComponentProxy>();
            for (int i = 0; i < componentProxies.Length; i++)
            {
                componentProxies[i].AwakeImmediate();
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
            UpdateConvexHull();

            if (transformId == 0)
            {
                UpdateMesh();

                if (_mesh)
                {
                    Color teamColor = GetTeamColor();
                    Color solidColor = teamColor;
                    Color wireColor = teamColor;

                    float distance = Vector3.Distance(_mesh.bounds.center, Camera.current.transform.position);
                    float size = _mesh.bounds.extents.magnitude;

                    AnimationCurve solidAlpha = new AnimationCurve
                    (
                        new Keyframe[]
                        {
                            new Keyframe(size, 0.0f),
                            new Keyframe(size * 2, 1.0f)
                        }
                    );

                    solidColor.a = solidAlpha.Evaluate(distance);

                    if (solidColor.a > 0)
                    {
                        Gizmos.color = solidColor;
                        Gizmos.DrawMesh(_mesh);
                    }

                    Gizmos.color = wireColor;
                    for (int i1 = 0; i1 < _hull.Count; i1++)
                    {
                        int i0 = (i1 == 0) ? _hull.Count - 1 : i1 - 1;
                        Vector3 v0 = new Vector3(_hull[i0].x, 0, _hull[i0].y);
                        Vector3 v1 = new Vector3(_hull[i1].x, 0, _hull[i1].y);
                        Gizmos.DrawLine(v0, v1);
                    }
                }
            }
        }
    }

    void UpdateMesh()
    {
        if (_hullHash != _meshHash)
        {
            _meshHash = _hullHash;
            
            if (_mesh == null)
            {
                _mesh = new Mesh();
            }

            if (_hull.Count == 0)
            {
                _mesh = null;
                return;
            }

            Vector3[] vertices = new Vector3[_hull.Count];
            Vector3[] normals = new Vector3[_hull.Count];
            for (int i = 0; i < _hull.Count; i++)
            {
                vertices[i].Set( _hull[i].x, 0.0f, _hull[i].y );
                normals[i].Set( 0, 1, 0 );
            }

            _mesh.triangles = new int[0];
            _mesh.vertices = vertices;
            _mesh.normals = normals;
            
            List<int> indices = new List<int>();
            for (int i = 0; i < _hull.Count; i++)
            {
                indices.Add( i );
            }

            List<int> triangulation = new List<int>();

            while (indices.Count > 3)
            {
                int earIndex = -1;
                float triangleArea = float.MaxValue;

                for (int i = 0; i < indices.Count; i++)
                {
                    int prevIndex = ( i == 0 ) ? indices.Count-1 : i - 1;
                    int nextIndex = ( i == indices.Count-1 ) ? 0 : i + 1;

                    float2 e1 = _hull[indices[i]] - _hull[indices[prevIndex]];
                    float e1mag = Mathf.Sqrt(e1.x * e1.x + e1.y * e1.y);
                    if (e1mag > 0)
                    {
                        e1 = e1 * 1.0f / e1mag;
                    }
                    
                    float2 e2 = _hull[indices[nextIndex]] - _hull[indices[i]];
                    float e2mag = Mathf.Sqrt(e2.x * e2.x + e2.y * e2.y);
                    if (e2mag > 0)
                    {
                        e2 = e2 * 1.0f / e2mag;
                    }
                    
                    float e1e2dot = (e1.x*e2.x + e1.y*e2.y);
                    
                    if( e1e2dot >= 0.9959f )
                    {
                        continue;
                    }
                    
                    float area = Geometry.TriangleArea( _hull[indices[prevIndex]], _hull[indices[i]], _hull[indices[nextIndex]] );
                    if( area < triangleArea )
                    {
                        earIndex = i;
                        triangleArea = area;
                    }
                }
                
                if( earIndex == -1 )
                {
                    break;
                }
                
                int leftIndex = ( earIndex == 0 ) ? indices.Count-1 : earIndex - 1;
                int rightIndex = ( earIndex == indices.Count-1 ) ? 0 : earIndex + 1;
                
                triangulation.Add( indices[rightIndex] );
                triangulation.Add( indices[earIndex] );
                triangulation.Add( indices[leftIndex] );
                
                
                indices.RemoveAt( earIndex );
            }
            if( indices.Count == 3 )
            {
                triangulation.Add( indices[2] );
                triangulation.Add( indices[1] );
                triangulation.Add( indices[0] );
            }

            _mesh.triangles = triangulation.ToArray();
        }
    }

    void UpdateConvexHull()
    {
        if (!_entityAssembly)
        {
            _pointCloud.Clear();
            _hull.Clear();
            return;
        }
        if (_hullFrame != Time.frameCount)
        {
            _hullFrame = Time.frameCount;
            
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

                _hull.Clear();
                _hull.AddRange( _pointCloud );
            }
            else if (hierarchyId > 0)
            {
                HierarchyProxy hierarchyProxy = _entityAssembly.GetHierarchyProxy(hierarchyId);

                uint childEntityId = hierarchyProxy.firstChildEntityId;
                while (childEntityId > 0)
                {
                    EntityProxy childEntityProxy = _entityAssembly.GetEntityProxy(childEntityId);
                    childEntityProxy.UpdateConvexHull();
                    _pointCloud.AddRange( childEntityProxy._hull );
                    
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

                if (_pointCloud.Count > 0)
                {
                    _hull = Geometry.ConvexHull(_pointCloud);
                }
                else
                {
                    _hull.Clear();
                }

                _hullHash = 17;
                unchecked
                {
                    for (int i = 0; i < _hull.Count; i++)
                    {
                        float2 p = _hull[i];
                        _hullHash = _hullHash * 23 + p.x.GetHashCode();
                        _hullHash = _hullHash * 23 + p.y.GetHashCode();
                    }
                }
            }
        }
    }

    public Mesh mesh
    {
        get { return _mesh; }
    }

    public float2 hullCenter
    {
        get
        {
            if (_hull.Count > 0)
            {
                float2 hullCenter = _hull[0];
                for (int i = 1; i < _hull.Count; i++)
                {
                    hullCenter += _hull[i];
                }

                hullCenter *= 1.0f / _hull.Count;
                return hullCenter;
            }
            else
            {
                return new float2(0,0);
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
    
    public uint movementId
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
                return entity.movementId;
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
                entity.movementId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
    
    public uint firepowerId
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
                return entity.firepowerId;
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
                entity.firepowerId = value;
                _entityAssembly.SetEntity(thisEntityId, entity);
            }
        }
    }
}
