
using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
using Structs;
using UnityEditor;
#endif

public partial class EntityAssembly : MonoBehaviour
{
    [Header("CPU Emulation")]
    public int NumCPUThreads = 1;
    
    [Header("RNG")]
    public bool RngSeedFromTimer = false;
    public int RngSeed = 0;
    public int RngMaxUniform = 1000000;
    public int RngCount = 256;
    public int RngStateLength = 55;
    
    #region FeedbackEvents
    private class EventMessage
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float duration;
        public float remainingTime;
        public string message;

        public EventMessage(Vector3 startPos, Vector3 endPos, float duration, string message)
        {
            this.startPos = startPos;
            this.endPos = endPos;
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
        public uint numTracers;
        
        public ShootingTracer(Vector3 startPos, Vector3 endPos, float duration, uint numTracers)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.duration = duration;
            this.remainingTime = this.duration;
            this.numTracers = numTracers;
        }
    };
    
    private List<ShootingTracer> _shootingTracers = new List<ShootingTracer>();
    private List<EventMessage> _eventMessages = new List<EventMessage>();
    #endregion
    
#if UNITY_EDITOR
    static void InitArrayHeaderBuffer(ref ArrayHeader[] arrayHeaderBuffer, uint entityCount)
    {
        arrayHeaderBuffer = new ArrayHeader[ComputeShaderEmulator.ARRAY_COUNT];
        for (uint i = 0; i < ComputeShaderEmulator.ARRAY_COUNT; i++)
        {
            arrayHeaderBuffer[i].capacity = (int)entityCount;
            arrayHeaderBuffer[i].count = 0;    
        }
    }
    
    static void InitBuffer<T>(ref T[] dstBuffer, uint length)
    {
        dstBuffer = new T[length];
    }
    
    static void InitBuffer<T>(T[] srcBuffer, ref T[] dstBuffer)
    {
        dstBuffer = new T[srcBuffer.Length];
        srcBuffer.CopyTo(dstBuffer, 0);
    }
    
    static void InitBuffer<T>(List<T> srcBuffer, ref T[] dstBuffer)
    {
        dstBuffer = new T[srcBuffer.Count];
        srcBuffer.CopyTo(dstBuffer, 0);
    }

    static void SyncBuffers<T>(ref T[] srcBuffer, List<T> dstBuffer)
    {
        dstBuffer.Clear();
        dstBuffer.AddRange( srcBuffer );
    }
    
    static void ClearArrayHeaderBuffer(ref ArrayHeader[] arrayHeaderBuffer)
    {
        for (uint i = 0; i < arrayHeaderBuffer.Length; i++)
        {
            arrayHeaderBuffer[i].count = 1; // 0 is reserved index    
        }
    }    

    static uint SelectedEntityId()
    {
        if (Selection.activeGameObject)
        {
            EntityProxy selectedEntityProxy = Selection.activeGameObject.GetComponent<EntityProxy>();
            if (selectedEntityProxy)
            {
                return selectedEntityProxy.entityId;
            }
        }
        return 0;
    }
    
    void Start()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
        
        if (RngSeedFromTimer)
        {
            Random.InitState( (int)System.DateTime.Now.Ticks );
        }
        else
        {
            Random.InitState( (int)RngSeed );
        }
        
        int[] rngStateData = new int[RngCount*(RngStateLength+1)];
        for (int i = 0; i < RngCount; i++)
        {
            rngStateData[i * (RngStateLength + 1)] = 0;
            for (int j = 0; j < RngStateLength; j++)
            {
                rngStateData[i * (RngStateLength + 1) + j + 1] = (int)Random.Range( 0, (int)RngMaxUniform );
            }
        }

        ComputeShaderEmulator.NumCPUThreads = NumCPUThreads;
        
        ComputeShaderEmulator._rngMax = RngMaxUniform;
        ComputeShaderEmulator._rngCount = RngCount;
        ComputeShaderEmulator._rngStateLength = RngStateLength;
        ComputeShaderEmulator._rngState = new int[rngStateData.Length];
        rngStateData.CopyTo(ComputeShaderEmulator._rngState, 0);
        
        InitBuffer(_descBuffer, ref ComputeShaderEmulator._descBuffer);
        InitBuffer(_transformBuffer, ref ComputeShaderEmulator._transformBuffer);
        InitBuffer(_hierarchyBuffer, ref ComputeShaderEmulator._hierarchyBuffer);
        InitBuffer(_personnelBuffer, ref ComputeShaderEmulator._personnelBuffer);
        InitBuffer(_firearmsBuffer, ref ComputeShaderEmulator._firearmBuffer);
        InitBuffer(_movementBuffer, ref ComputeShaderEmulator._movementBuffer);
        InitBuffer(_targetingBuffer, ref ComputeShaderEmulator._targetingBuffer);
        InitBuffer(_eventAggregatorBuffer, ref ComputeShaderEmulator._eventAggregatorBuffer);
        InitBuffer(FirearmDescBuffer, ref ComputeShaderEmulator._firearmDescBuffer);
        InitBuffer(PersonnelDescBuffer, ref ComputeShaderEmulator._personnelDescBuffer);
        InitArrayHeaderBuffer(ref ComputeShaderEmulator._arrayHeaderBuffer, (uint)_descBuffer.Count);
        InitBuffer(ref ComputeShaderEmulator._eventBuffer, (uint)_descBuffer.Count);
        InitBuffer(ref ComputeShaderEmulator._feedbackEventBuffer, (uint)_descBuffer.Count);
        ComputeShaderEmulator._firearmDescCount = (uint) FirearmDescBuffer.Length;
        ComputeShaderEmulator._personnelDescCount = (uint) PersonnelDescBuffer.Length;
        ComputeShaderEmulator._entityCount = (uint)_descBuffer.Count;
        ComputeShaderEmulator._selectedEntityId = SelectedEntityId();
    }
    
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }

        const uint ThreadGroupSizeX = 256;
        uint threadGroupsX = ComputeShaderEmulator._entityCount / ThreadGroupSizeX + 1;
        
        ComputeShaderEmulator._firepower = Firepower;
        ComputeShaderEmulator._killProbability = KillProbability;
        ComputeShaderEmulator._woundProbability = WoundProbability;
        ComputeShaderEmulator._moraleDamage = MoraleDamage;
        ComputeShaderEmulator._killMoraleDamage = KillMoraleDamage;
        ComputeShaderEmulator._woundMoraleDamage = WoundMoraleDamage;

        ComputeShaderEmulator._dT = Time.deltaTime;
        ComputeShaderEmulator._entityCount = (uint)_descBuffer.Count;
        ComputeShaderEmulator._selectedEntityId = SelectedEntityId();

        ClearArrayHeaderBuffer(ref ComputeShaderEmulator._arrayHeaderBuffer);
        
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.ProcessEvents, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.Cleanup, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateMovement, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdatePersonnel, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateTargeting, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateFirearms, threadGroupsX, 1, 1 );

        SyncBuffers(ref ComputeShaderEmulator._descBuffer, _descBuffer);
        SyncBuffers(ref ComputeShaderEmulator._transformBuffer, _transformBuffer);
        SyncBuffers(ref ComputeShaderEmulator._hierarchyBuffer, _hierarchyBuffer);
        SyncBuffers(ref ComputeShaderEmulator._personnelBuffer, _personnelBuffer);
        SyncBuffers(ref ComputeShaderEmulator._firearmBuffer, _firearmsBuffer);
        SyncBuffers(ref ComputeShaderEmulator._movementBuffer, _movementBuffer);
        SyncBuffers(ref ComputeShaderEmulator._targetingBuffer, _targetingBuffer);
        SyncBuffers(ref ComputeShaderEmulator._eventAggregatorBuffer, _eventAggregatorBuffer);

        ProcessFeedbackEvents();
    }

    void ProcessFeedbackEvents()
    {
        int numFeedbackEvents = ComputeShaderEmulator._arrayHeaderBuffer[ComputeShaderEmulator.ARRAY_FEEDBACK_EVENT].count;

        for (int eventId = 0; eventId < numFeedbackEvents; eventId++)
        {
            uint id = ComputeShaderEmulator._feedbackEventBuffer[eventId].id;
            uint entityId0 = ComputeShaderEmulator._feedbackEventBuffer[eventId].entityId0;
            uint entityId1 = ComputeShaderEmulator._feedbackEventBuffer[eventId].entityId1;
            var param = ComputeShaderEmulator._feedbackEventBuffer[eventId].param;
            if (id == ComputeShaderEmulator.FEEDBACK_EVENT_SHOOTING)
            {
                const float TracerVelocity = 266.0f;
                
                var startPos = ComputeShaderEmulator._transformBuffer[entityId0].position.ToVector3();
                var endPos = ComputeShaderEmulator._transformBuffer[entityId1].position.ToVector3();
                float distance = Vector3.Distance(startPos, endPos);
                float duration = distance / TracerVelocity;
                var shootingTracer = new ShootingTracer(startPos, endPos, duration, (uint)param.x);
                _shootingTracers.Add(shootingTracer);
            }
            else if (id == ComputeShaderEmulator.FEEDBACK_EVENT_AIMING || 
                     id == ComputeShaderEmulator.FEEDBACK_EVENT_RELOADING )
            {
                const float TextOffset = 5.0f;
                const float TextDuration = 1.0f;

                string text = "";
                switch (id)
                {
                case ComputeShaderEmulator.FEEDBACK_EVENT_AIMING: 
                    text = "Aiming";
                    break;
                case ComputeShaderEmulator.FEEDBACK_EVENT_RELOADING: 
                    text = "Reloading";
                    break;
                default:
                    break;
                }
                
                var startPos = ComputeShaderEmulator._transformBuffer[entityId0].position.ToVector3();
                var endPos = startPos + Vector3.up * TextOffset;
                var eventMessage = new EventMessage( startPos, endPos, TextDuration, text );
                _eventMessages.Add(eventMessage);
            }
            else if (id == ComputeShaderEmulator.FEEDBACK_EVENT_FIREARM_DAMAGE)
            {
                const float TextOffset = 5.0f;
                const float TextDuration = 2.0f;

                string text = "";
                if (param.x > 0.0f)
                {
                    text += "-" + param.x.ToString("F0") + "w";
                }

                if (param.y > 0.0f)
                {
                    if (text.Length > 0)
                    {
                        text += ", ";
                    }
                    text += "-" + param.y.ToString("F0") + "k";
                }

                if (text.Length > 0)
                {
                    text += ", ";
                }
                text += "-" + param.z.ToString("F0") + "m";

                var startPos = ComputeShaderEmulator._transformBuffer[entityId0].position.ToVector3();
                var endPos = startPos + Vector3.up * TextOffset;
                var eventMessage = new EventMessage( startPos, endPos, TextDuration, text );
                _eventMessages.Add(eventMessage);
            }
        }        
    }

    void OnDrawGizmos()
    {
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
                float startT = Mathf.Clamp01(t + 0.125f);
                float endT = Mathf.Clamp01(t);
                Vector3 tracerStartPos = Vector3.Lerp(_shootingTracers[i].startPos, _shootingTracers[i].endPos, startT);
                Vector3 tracerEndPos = Vector3.Lerp(_shootingTracers[i].startPos, _shootingTracers[i].endPos, endT);
                Vector3 tracerDir = tracerEndPos - tracerStartPos;
                float tracerLength = tracerDir.magnitude;
                tracerDir = tracerDir.normalized;

                Gizmos.color = Color.white;

                uint numTracers = (uint)Mathf.Max(_shootingTracers[i].numTracers, 1);
                float bulletLength = tracerLength / numTracers;
                float gapLength = bulletLength * 0.333f;
                bulletLength -= gapLength;

                Vector3 prevPos = tracerStartPos;
                Vector3 currentPos = tracerStartPos;
                for (uint j = 0; j < numTracers; j++ )
                {
                    currentPos += tracerDir * bulletLength;
                    Gizmos.DrawLine(prevPos, currentPos);

                    currentPos += tracerDir * gapLength;
                    prevPos = currentPos;
                }
            }
        }

        for (int i = _eventMessages.Count - 1; i >= 0; i--)
        {
            _eventMessages[i].remainingTime -= Time.deltaTime;
            if (_eventMessages[i].remainingTime < 0.0f)
            {
                _eventMessages.RemoveAt(i);
            }
            else
            {
                float t = Mathf.Clamp01( 1.0f - _eventMessages[i].remainingTime / _eventMessages[i].duration);
                
                Handles.color = Color.Lerp( new Color( 1,1,1,1 ), new Color( 1,1,1,0 ), t );
                Handles.Label( Vector3.Lerp(_eventMessages[i].startPos, _eventMessages[i].endPos, t), _eventMessages[i].message );
            }
        }
    }
#endif    
}
