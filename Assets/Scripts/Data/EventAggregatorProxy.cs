
using System.Collections.Generic;
using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[RequireComponent(typeof(EntityProxy))]
public class EventAggregatorProxy : ComponentProxy
{
    private EntityAssembly _entityAssembly;
    private EntityProxy _entityProxy;

    private class EventMessage
    {
        public float offset;
        public float duration;
        public float remainingTime;
        public string message;

        public EventMessage(float offset, float duration, string message)
        {
            this.offset = offset;
            this.duration = duration;
            this.remainingTime = this.duration;
            this.message = message;
        }
    };

    private class ShootingTracer
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float duration;
        public float remainingTime;
        
        public ShootingTracer(Vector3 startPos, Vector3 endPos, float duration)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.duration = duration;
            this.remainingTime = this.duration;
        }
    };

    private List<ShootingTracer> _shootingTracers = new List<ShootingTracer>();
    private List<EventMessage> _eventMessages = new List<EventMessage>();
    
    protected override void Awake()
    {
        _entityProxy = GetComponent<EntityProxy>();
        _entityAssembly = FindObjectOfType<EntityAssembly>();
        if (_entityAssembly)
        {
            if (_entityAssembly.GetEntityId(this) == 0)
            {
                _entityAssembly.RegisterEventAggregatorProxy(_entityProxy.entityId, this);
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
            _entityAssembly.UnregisterEventAggregatorProxy( _entityAssembly.GetEntityId(this), this );
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
            uint thisEntityId = _entityAssembly.GetEntityId(this);
            int eventId = firstEventId;
            while (eventId > 0)
            {
                uint id = ComputeShaderEmulator._eventBuffer[eventId].id;
                uint otherEntityId = ComputeShaderEmulator._eventBuffer[eventId].entityId;
                var param = ComputeShaderEmulator._eventBuffer[eventId].param;
                if (id == ComputeShaderEmulator.EVENT_TEST_SHOOTING)
                {
                    var startPos = ComputeShaderEmulator._transformBuffer[thisEntityId].position.ToVector3();
                    var endPos = ComputeShaderEmulator._transformBuffer[otherEntityId].position.ToVector3();
                    float distance = Vector3.Distance(startPos, endPos);
                    float duration = distance / 333.0f;
                    var shootingTracer = new ShootingTracer( startPos, endPos, duration );
                    _shootingTracers.Add(shootingTracer);
                }
                eventId = ComputeShaderEmulator._eventBuffer[eventId].nextEventId;
            }

            for (int i = _shootingTracers.Count - 1; i >= 0; i--)
            {
                _shootingTracers[i].remainingTime -= Time.deltaTime;
                if (_shootingTracers[i].remainingTime < 0.0f)
                {
                    _shootingTracers.RemoveAt(i);
                }
                else
                {
                    float t = Mathf.Clamp01( 1.0f - _shootingTracers[i].remainingTime / _shootingTracers[i].duration);
                    float startT = Mathf.Clamp01(t + 0.1f);
                    float endT = Mathf.Clamp01(t);
                    Vector3 tracerStartPos = Vector3.Lerp(_shootingTracers[i].startPos, _shootingTracers[i].endPos, startT);
                    Vector3 tracerEndPos = Vector3.Lerp(_shootingTracers[i].startPos, _shootingTracers[i].endPos, endT);

                    Gizmos.color = Color.white;
                    Gizmos.DrawLine( tracerStartPos, tracerEndPos );
                }
            }
        }
    }
    
    private static Structs.EventAggregator _dummy = new Structs.EventAggregator();

    private Structs.EventAggregator _component
    {
        get
        {
            if (_entityAssembly)
            {
                uint entityId = _entityAssembly.GetEntityId(this);
                if (entityId != 0)
                {
                    return _entityAssembly.GetEventAggregator(entityId);
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
                    _entityAssembly.SetEventAggregator(entityId, value);
                }
            }
        }
    }
    
    public int eventCount
    {
        get { return _component.eventCount; }
    }
    
    public int firstEventId
    {
        get { return _component.firstEventId; }
    }
}
