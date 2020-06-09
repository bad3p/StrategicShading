
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class TransformProxy : ComponentProxy
{
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;
    
    private AnimationCurve _solidAlpha = new AnimationCurve
    (
        new Keyframe[]
        {
            new Keyframe(75.0f, 0.0f),
            new Keyframe(100.0f, 1.0f),
            new Keyframe(125.0f, 1.0f),
            new Keyframe(150.0f, 0.0f)
        }
    );
            
    private AnimationCurve _wireAlpha = new AnimationCurve
    (
        new Keyframe[]
        {
            new Keyframe(75.0f, 1.0f),
            new Keyframe(100.0f, 0.0f),
        }
    );
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            if (_entityAssembly.GetEntityId(this) == 0)
            {
                _entityAssembly.RegisterTransformProxy(_entityProxy.entityId,this);
                position = transform.position;
                rotation = transform.rotation;
                scale = transform.localScale;
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
            _entityAssembly.UnregisterTransformProxy( _entityAssembly.GetEntityId(this), this );
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
            Color teamColor = _entityProxy.GetTeamColor();
            Color solidColor = teamColor;
            Color wireColor = teamColor;

            float distance = Vector3.Distance(position.ToVector3(), Camera.current.transform.position);

            float size = ComputeShaderEmulator.length(scale);

            _solidAlpha.keys = new Keyframe[]
            {
                new Keyframe(size * 15.0f, 0.0f),
                new Keyframe(size * 25.0f, 1.0f),
                new Keyframe(size * 35.0f, 1.0f),
                new Keyframe(size * 50.0f, 0.0f)
            };
            _wireAlpha.keys = new Keyframe[]
            {
                new Keyframe(size * 15.0f, 1.0f),
                new Keyframe(size * 25.0f, 0.0f)
            };

            solidColor.a = _solidAlpha.Evaluate(distance);
            wireColor.a = _wireAlpha.Evaluate(distance);
            
            Gizmos.matrix = Matrix4x4.Translate(position.ToVector3()) * Matrix4x4.Rotate(rotation.ToQuaternion());
            
            Gizmos.color = wireColor;
            if (wireColor.a > 0)
            {
                Gizmos.DrawWireCube(Vector3.zero, scale.ToVector3());
            }
            
            Gizmos.color = solidColor;
            if(solidColor.a > 0)
            {
                Gizmos.DrawCube(Vector3.zero, scale.ToVector3());
            }
        }
    }

    private static Structs.Transform _dummy = new Structs.Transform();

    private Structs.Transform _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetTransform(entityId);
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
                    _entityAssembly.SetTransform(entityId, value);
                }
            }
        }
    }

    public double3 position
    {
        get { return _component.position; }
        set
        {
            var temp = _component;
            temp.position = value;
            _component = temp;
        }
    }
    
    public float4 rotation
    {
        get { return _component.rotation; }
        set
        {
            var temp = _component;
            temp.rotation = value;
            _component = temp;
        }
    }
    
    public float3 scale
    {
        get { return _component.scale; }
        set
        {
            var temp = _component;
            temp.scale = value;
            _component = temp;
        }
    }
}
