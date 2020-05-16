
using UnityEngine;

#if UNITY_EDITOR
using System.Collections.Generic;
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
    
#if UNITY_EDITOR
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
            Random.InitState( RngSeed );
        }
        
        int[] rngStateData = new int[RngCount*(RngStateLength+1)];
        for (int i = 0; i < RngCount; i++)
        {
            rngStateData[i * (RngStateLength + 1)] = 0;
            for (int j = 0; j < RngStateLength; j++)
            {
                rngStateData[i * (RngStateLength + 1) + j + 1] = Random.Range( 0, RngMaxUniform );
            }
        }
        
        ComputeShaderEmulator._rngMax = RngMaxUniform;
        ComputeShaderEmulator._rngCount = RngCount;
        ComputeShaderEmulator._rngStateLength = RngStateLength;
        ComputeShaderEmulator._rngState = new int[rngStateData.Length];
        rngStateData.CopyTo(ComputeShaderEmulator._rngState, 0);

        ComputeShaderEmulator._entityCount = _descBuffer.Count; 
        InitBuffer(_descBuffer, ref ComputeShaderEmulator._descBuffer);
        InitBuffer(_transformBuffer, ref ComputeShaderEmulator._transformBuffer);
        InitBuffer(_hierarchyBuffer, ref ComputeShaderEmulator._hierarchyBuffer);
        InitBuffer(_personnelBuffer, ref ComputeShaderEmulator._personnelBuffer);
        InitBuffer(_firearmsBuffer, ref ComputeShaderEmulator._firearmsBuffer);
        InitBuffer(_movementBuffer, ref ComputeShaderEmulator._movementBuffer);
        InitBuffer(_firepowerBuffer, ref ComputeShaderEmulator._firepowerBuffer);
    }
    
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }

        ComputeShaderEmulator._dT = Time.deltaTime;
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateMovement, (uint)_movementBuffer.Count / 256 + 1, 1, 1 );

        SyncBuffers(ref ComputeShaderEmulator._descBuffer, _descBuffer);
        SyncBuffers(ref ComputeShaderEmulator._transformBuffer, _transformBuffer);
        SyncBuffers(ref ComputeShaderEmulator._hierarchyBuffer, _hierarchyBuffer);
        SyncBuffers(ref ComputeShaderEmulator._personnelBuffer, _personnelBuffer);
        SyncBuffers(ref ComputeShaderEmulator._firearmsBuffer, _firearmsBuffer);
        SyncBuffers(ref ComputeShaderEmulator._movementBuffer, _movementBuffer);
        SyncBuffers(ref ComputeShaderEmulator._firepowerBuffer, _firepowerBuffer);
    }
#endif    
}
