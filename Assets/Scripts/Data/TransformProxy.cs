
using Types;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class TransformProxy : MonoBehaviour
{
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            uint thisTransformId = _entityAssembly.GetTransformId(this);
            if (thisTransformId == 0)
            {
                thisTransformId = _entityAssembly.RegisterTransformProxy(this);
                _entityProxy.transformId = thisTransformId;
                entityId = _entityProxy.entityId;
                position = transform.position;
                rotation = transform.rotation;
                scale = transform.localScale;
            }
        }    
    }

    void OnDestroy()
    {
        _entityProxy.transformId = 0;
    }
    
    void OnDrawGizmos()
    {
        if (!_entityProxy)
        {
            Awake();
        }
        if (_entityAssembly)
        {
            const float SolidDistanceThreshold = 150.0f;
            const float WireDistanceThreshold = 75.0f;
            
            Gizmos.color = _entityProxy.GetTeamColor();
            Gizmos.matrix = Matrix4x4.Translate(position.ToVector3()) * Matrix4x4.Rotate(rotation.ToQuaternion());
            float distance = Vector3.Distance(position.ToVector3(), Camera.current.transform.position);
            
            if (distance < WireDistanceThreshold)
            {
                Gizmos.DrawWireCube(Vector3.zero, scale.ToVector3());
            }
            else if(distance < SolidDistanceThreshold)
            {
                Gizmos.DrawCube(Vector3.zero, scale.ToVector3());
            }
        }
    }
    
    public uint entityId
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                return thisTransform.entityId;
            }
            else
            {
                return 0;
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                thisTransform.entityId = value;
                _entityAssembly.SetTransform(thisTransformId, thisTransform);
            }
        }
    }

    public double3 position
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                return thisTransform.position;
            }
            else
            {
                return new double3(0,0,0);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                thisTransform.position = value;
                _entityAssembly.SetTransform(thisTransformId, thisTransform);
            }
        }
    }
    
    public float4 rotation
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                return thisTransform.rotation;
            }
            else
            {
                return new float4(0,0,0,1);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                thisTransform.rotation = value;
                _entityAssembly.SetTransform(thisTransformId, thisTransform);
            }
        }
    }
    
    public float3 scale
    {
        get
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                return thisTransform.scale;
            }
            else
            {
                return new float3(1,1,1);
            }
        }
        set
        {
            if (_entityAssembly)
            {
                uint thisTransformId = _entityAssembly.GetTransformId(this);
                Structs.Transform thisTransform = _entityAssembly.GetTransform(thisTransformId);
                thisTransform.scale = value;
                _entityAssembly.SetTransform(thisTransformId, thisTransform);
            }
        }
    }
}
