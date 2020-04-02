using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DemoUI : MonoBehaviour
{
    [Header("UI")]
    public Image MapImage;
    [Header("Resources")]
    public ComputeShader SimulationComputeShader;
    
    private Material      _mapImageMaterial = null;
    private RenderTexture _renderTexture = null;
    
    private int _rngMax;
    private int _rngCount;
    private int _rngStateLength;
    private ComputeBuffer _rngState = null;

    void Awake()
    {
        _mapImageMaterial = new Material( Shader.Find("Unlit/Texture") );
        MapImage.material = _mapImageMaterial;
        MapImage.SetMaterialDirty();
        
        _renderTexture = new RenderTexture( 4096, 2048, 0, RenderTextureFormat.RFloat );		
        _renderTexture.enableRandomWrite = true;
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.Create();

        _rngMax = 1000000;
        _rngCount = 256;
        _rngStateLength = 55;
        int[] rngStateData = new int[_rngCount*(_rngStateLength+1)];
        for (int i = 0; i < _rngCount; i++)
        {
            rngStateData[i * (_rngStateLength + 1)] = 0;
            for (int j = 0; j < _rngStateLength; j++)
            {
                rngStateData[i * (_rngStateLength + 1) + j + 1] = Random.Range( 0, _rngMax );
            }
        }
        
        _rngState = new ComputeBuffer( _rngCount*(_rngStateLength+1), sizeof(int) );
        _rngState.SetData( rngStateData );

        // TestRng();
    }

    void TestRng()
    {
        Random.InitState( (int)DateTime.Now.Ticks );
        
        Simulation._outRenderTextureWidth = 128;
        Simulation._outRenderTextureHeight = 128;
        Simulation._outRenderTexture = new Types.RWTexture2D<float>();

        Simulation._rngMax = 100000;
        Simulation._rngCount = 1;
        Simulation._rngStateLength = 55;
        Simulation._rngState = new Types.RWStructuredBuffer<int>();

        for (int rngIndex = 0; rngIndex < Simulation._rngCount; rngIndex++)
        {
            // position
            Simulation._rngState.Add(0);
            // state
            for (int stateIndex = 0; stateIndex < Simulation._rngStateLength; stateIndex++)
            {
                Simulation._rngState.Add(Random.Range(0, Simulation._rngMax));
            }
        }
        
        
        Simulation.Dispatch( Simulation.GenerateRandomNumbers, 128, 128, 1, 1 );

        const float HistogramMin = -1.0f;
        const float HistogramMax = 1.0f;
        const float HistogramRange = (HistogramMax - HistogramMin);
        int[] histogram = new int[21];
        float slotRange = HistogramRange / histogram.Length;

        for (int x = 0; x < Simulation._outRenderTextureWidth; x++)
        {
            for (int y = 0; y < Simulation._outRenderTextureWidth; y++)
            {
                Types.int2 xy = new Types.int2(x, y);
                if (Simulation._outRenderTexture.ContainsKey(xy))
                {
                    float value = Simulation._outRenderTexture[xy];

                    for (int i = 0; i < histogram.Length; i++)
                    {
                        if (value >= HistogramMin + i * slotRange && value < HistogramMin + (i + 1) * slotRange)
                        {
                            histogram[i]++;
                        }
                    }
                }
            }
        }

        string s = "";
        for (int i = 0; i < histogram.Length; i++)
        {
            s += histogram[i].ToString() + "\n";
        }
        Debug.Log(s);
    }

    void Update()
    {
        const int GroupSize = 256;
        
        int generateRandomNumbers = SimulationComputeShader.FindKernel("GenerateRandomNumbers");
        SimulationComputeShader.SetInt("_outRenderTextureWidth", _renderTexture.width);
        SimulationComputeShader.SetInt("_outRenderTextureHeight", _renderTexture.height);
        SimulationComputeShader.SetInt("_rngMax", _rngMax);
        SimulationComputeShader.SetInt("_rngCount", _rngCount);
        SimulationComputeShader.SetInt("_rngStateLength", _rngStateLength);
        SimulationComputeShader.SetBuffer(generateRandomNumbers, "_rngState", _rngState);
        SimulationComputeShader.SetTexture(generateRandomNumbers, "_outRenderTexture", _renderTexture);
        SimulationComputeShader.Dispatch(generateRandomNumbers, ((_renderTexture.width * _renderTexture.height ) / GroupSize) + 1, 1, 1);
        
        _mapImageMaterial.mainTexture = _renderTexture;
    }
}
