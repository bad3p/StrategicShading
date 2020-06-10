
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
    public uint RngSeed = 0;
    public uint RngMaxUniform = 1000000;
    public uint RngCount = 256;
    public uint RngStateLength = 55;
    
#if UNITY_EDITOR
    static void InitArrayHeaderBuffer(ref ArrayHeader[] arrayHeaderBuffer, uint length, int capacity)
    {
        arrayHeaderBuffer = new ArrayHeader[length];
        for (uint i = 0; i < length; i++)
        {
            arrayHeaderBuffer[i].capacity = (i == 0) ? 0 : capacity;
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
            arrayHeaderBuffer[i].count = 1; // 0 is also reserved    
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
        
        uint[] rngStateData = new uint[RngCount*(RngStateLength+1)];
        for (int i = 0; i < RngCount; i++)
        {
            rngStateData[i * (RngStateLength + 1)] = 0;
            for (int j = 0; j < RngStateLength; j++)
            {
                rngStateData[i * (RngStateLength + 1) + j + 1] = (uint)Random.Range( 0, (int)RngMaxUniform );
            }
        }

        ComputeShaderEmulator.NumCPUThreads = NumCPUThreads;
        
        ComputeShaderEmulator._rngMax = RngMaxUniform;
        ComputeShaderEmulator._rngCount = RngCount;
        ComputeShaderEmulator._rngStateLength = RngStateLength;
        ComputeShaderEmulator._rngState = new uint[rngStateData.Length];
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
        InitArrayHeaderBuffer(ref ComputeShaderEmulator._arrayHeaderBuffer, ComputeShaderEmulator.EVENT_LAST + 1, (int)(_descBuffer.Count * _descBuffer.Count));
        InitBuffer(ref ComputeShaderEmulator._firearmEventBuffer, (uint)(_descBuffer.Count * _descBuffer.Count)); 
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

        ComputeShaderEmulator._dT = Time.deltaTime;
        ComputeShaderEmulator._entityCount = (uint)_descBuffer.Count;
        ComputeShaderEmulator._selectedEntityId = SelectedEntityId();

        ClearArrayHeaderBuffer(ref ComputeShaderEmulator._arrayHeaderBuffer);
        
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.Cleanup, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateMovement, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdatePersonnel, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateTargeting, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateFirearms, threadGroupsX, 1, 1 );
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateJoinRequests, threadGroupsX, 1, 1 );

        SyncBuffers(ref ComputeShaderEmulator._descBuffer, _descBuffer);
        SyncBuffers(ref ComputeShaderEmulator._transformBuffer, _transformBuffer);
        SyncBuffers(ref ComputeShaderEmulator._hierarchyBuffer, _hierarchyBuffer);
        SyncBuffers(ref ComputeShaderEmulator._personnelBuffer, _personnelBuffer);
        SyncBuffers(ref ComputeShaderEmulator._firearmBuffer, _firearmsBuffer);
        SyncBuffers(ref ComputeShaderEmulator._movementBuffer, _movementBuffer);
        SyncBuffers(ref ComputeShaderEmulator._targetingBuffer, _targetingBuffer);
        SyncBuffers(ref ComputeShaderEmulator._eventAggregatorBuffer, _eventAggregatorBuffer);
    }
#endif    
}
