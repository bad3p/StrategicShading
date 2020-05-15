
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(PersonnelProxy))]
public class PersonnelProxy : ComponentProxy
{
    public float Morale = 600.0f;
    public float Fitness = 14400.0f;
    public uint Count = 12;
    public uint Wounded = 0;
    public uint Killed = 0;
    
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
                _entityAssembly.RegisterPersonnelProxy(_entityProxy.entityId, this);
                morale = Morale;
                fitness = Fitness;
                count = Count;
                wounded = Wounded;
                killed = Killed;
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
            _entityAssembly.UnregisterPersonnelProxy( _entityAssembly.GetEntityId(this), this );
        }
    }
    
    void OnDrawGizmos()
    {
        if (!_entityProxy)
        {
            Awake();
        }
        if (_entityAssembly && _entityAssembly.GetEntityId(this) != 0)
        {
            Gizmos.color = _entityProxy.GetTeamColor();

            TransformProxy transformProxy = GetComponent<TransformProxy>();
            if (transformProxy)
            {
                const float DistanceThreshold = 100.0f;
                
                if (Vector3.Distance(transformProxy.position.ToVector3(), Camera.current.transform.position) > DistanceThreshold)
                {
                    return;
                }
                
                Gizmos.matrix = Matrix4x4.Translate(transformProxy.position.ToVector3()) * Matrix4x4.Rotate(transformProxy.rotation.ToQuaternion());

                float3 scale = transformProxy.scale;
                
                int rowSize = Mathf.Min( (int)count, Mathf.CeilToInt( Mathf.Sqrt(count) * scale.x / scale.z ));
                int colSize = Mathf.CeilToInt(count / rowSize);
                if (rowSize * colSize < count)
                {
                    colSize++;
                }

                float xStep = scale.x / rowSize;
                float zStep = scale.z / colSize;
                
                for (int i = 0; i < count; i++)
                {
                    int z = i / rowSize;
                    int x = i - z * rowSize;
                    float3 p = new float3
                    (
                        -scale.x / 2 + x * xStep + xStep / 2,
                        0,
                        -scale.z / 2 + z * zStep + zStep / 2
                    );
                    
                    Gizmos.DrawCube(p.ToVector3(), new Vector3( 0.25f,2.0f,0.25f) );
                }
            }
        }
    }
    
    private static Structs.Personnel _dummy = new Structs.Personnel();

    private Structs.Personnel _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetPersonnel(entityId);
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
                    _entityAssembly.SetPersonnel(entityId, value);
                }
            }
        }
    }

    public float morale
    {
        get { return _component.morale; }
        set
        {
            var temp = _component;
            temp.morale = value;
            _component = temp;
        }
    }
    
    public float fitness
    {
        get { return _component.fitness; }
        set
        {
            var temp = _component;
            temp.fitness = value;
            _component = temp;
        }
    }
    
    public uint count
    {
        get { return _component.count; }
        set
        {
            var temp = _component;
            temp.count = value;
            _component = temp;
        }
    }
    
    public uint wounded
    {
        get { return _component.wounded; }
        set
        {
            var temp = _component;
            temp.wounded = value;
            _component = temp;
        }
    }
    
    public uint killed
    {
        get { return _component.killed; }
        set
        {
            var temp = _component;
            temp.killed = value;
            _component = temp;
        }
    }
}
