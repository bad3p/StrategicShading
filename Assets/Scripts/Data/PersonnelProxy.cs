
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(PersonnelProxy))]
public class PersonnelProxy : ComponentProxy
{
    public uint DescID = 0;
    public float Morale = 600.0f;
    public float Fitness = 14400.0f;
    public uint Exposure = 0;
    public uint Count = 0;
    
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
                descId = DescID;
                morale = Morale;
                fitness = Fitness;
                exposure = Exposure;
                ValidateStatus();
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
                const float DistanceThreshold = 125.0f;
                
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

    private void ValidateStatus()
    {
        if (!_entityAssembly)
        {
            Awake();
        }
        if (!_entityAssembly)
        {
            return;
        }
            
        uint maxPersonnel = 0;
        uint descId = _component.descId;
        if (descId > 0 && descId < _entityAssembly.PersonnelDescBuffer.Count)
        {
            maxPersonnel = _entityAssembly.PersonnelDescBuffer[(int)descId].maxPersonnel;
        }
        
        maxPersonnel = maxPersonnel < ComputeShaderEmulator.MAX_PERSONNEL
            ? maxPersonnel
            : ComputeShaderEmulator.MAX_PERSONNEL; 
            
        var temp = _component;
        for (uint i = 0; i < maxPersonnel; i++)
        {
            uint personnelStatus = temp.status >> (int)(i*2);
            personnelStatus &= ComputeShaderEmulator.PERSONNEL_STATUS_BITMASK;
            if( personnelStatus == ComputeShaderEmulator.PERSONNEL_STATUS_ABSENT )
            {
                personnelStatus = ComputeShaderEmulator.PERSONNEL_STATUS_HEALTHY;
            }
            
            temp.status &= ~(ComputeShaderEmulator.PERSONNEL_STATUS_BITMASK << (int)(i*2));
            temp.status |= personnelStatus << (int)(i*2);
        }
        for (uint i = maxPersonnel; i < ComputeShaderEmulator.MAX_PERSONNEL; i++)
        {
            temp.status &= ~(ComputeShaderEmulator.PERSONNEL_STATUS_BITMASK << (int)(i*2));
        }
        
        _component = temp;
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
        ValidateStatus();
        Count = count;
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
    
    public uint status
    {
        get { return _component.status; }
    }

    public uint exposure
    {
        get
        {
            return _component.status >> (int)ComputeShaderEmulator.PERSONNEL_EXPOSURE_SHIFT;
        }
        set
        {
            var temp = _component;
            temp.status &= ~ComputeShaderEmulator.PERSONNEL_EXPOSURE_BITMASK;
            temp.status |= value << (int)ComputeShaderEmulator.PERSONNEL_EXPOSURE_SHIFT;
            _component = temp;
        }
    }

    public uint count
    {
        get
        {
            if (!_entityAssembly)
            {
                Awake();
            }
            
            uint maxPersonnel = 0;
            uint descId = _component.descId;
            if (descId > 0 && descId < _entityAssembly.PersonnelDescBuffer.Count)
            {
                maxPersonnel = _entityAssembly.PersonnelDescBuffer[(int)descId].maxPersonnel;
            }

            maxPersonnel = maxPersonnel < ComputeShaderEmulator.MAX_PERSONNEL
                ? maxPersonnel
                : ComputeShaderEmulator.MAX_PERSONNEL; 
            
            var temp = _component;
            uint result = 0;
            for (uint i = 0; i < maxPersonnel; i++)
            {
                uint personnelStatus = temp.status >> (int)(i * 2);
                personnelStatus &= ComputeShaderEmulator.PERSONNEL_STATUS_BITMASK;
                if( personnelStatus == ComputeShaderEmulator.PERSONNEL_STATUS_HEALTHY )
                {
                    result++;
                }
            }

            return result;
        }
        set
        {
            if (!_entityAssembly)
            {
                Awake();
            }
            
            uint maxPersonnel = 0;
            uint descId = _component.descId;
            if (descId > 0 && descId < _entityAssembly.PersonnelDescBuffer.Count)
            {
                maxPersonnel = _entityAssembly.PersonnelDescBuffer[(int)descId].maxPersonnel;
            }

            maxPersonnel = maxPersonnel < ComputeShaderEmulator.MAX_PERSONNEL
                ? maxPersonnel
                : ComputeShaderEmulator.MAX_PERSONNEL; 
            
            var temp = _component;
            for (uint i = 0; i < maxPersonnel; i++)
            {
                uint personnelStatus = ComputeShaderEmulator.PERSONNEL_STATUS_ABSENT;
                if( i < value )
                {
                    personnelStatus = ComputeShaderEmulator.PERSONNEL_STATUS_HEALTHY;
                }
                else
                {
                    personnelStatus = ComputeShaderEmulator.PERSONNEL_STATUS_KILLED;
                }
                temp.status &= ~(ComputeShaderEmulator.PERSONNEL_STATUS_BITMASK << (int)(i*2));
                temp.status |= personnelStatus << (int)(i*2);
            }

            _component = temp;
            ValidateStatus();
        }
    }
}
