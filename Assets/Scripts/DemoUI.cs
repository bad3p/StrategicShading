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
    private ComputeBuffer _lcgState = null;
    
    void Awake()
    {
        _mapImageMaterial = new Material( Shader.Find("Unlit/Texture") );
        MapImage.material = _mapImageMaterial;
        MapImage.SetMaterialDirty();
        
        _renderTexture = new RenderTexture( 256, 128, 0, RenderTextureFormat.RFloat );		
        _renderTexture.enableRandomWrite = true;
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.Create();

        const int LCGCount = 256;
        int[] lcgState = new int[LCGCount];
        for (int i = 0; i < LCGCount; i++)
        {
            while (true)
            {
                int state = Random.Range(0, int.MaxValue);
                
                bool found = false;
                for (int j = 0; j < i; j++)
                {
                    if (lcgState[j] == state)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    lcgState[i] = state;
                    break;
                }
            }
        }
        
        _lcgState = new ComputeBuffer( LCGCount, sizeof(int) );
        _lcgState.SetData( lcgState );

        TestRng();
    }

    void TestRng()
    {
        Simulation._outRenderTextureWidth = 128;
        Simulation._outRenderTextureHeight = 128;
        Simulation._outRenderTexture = new Types.RWTexture2D<float>();
        Simulation._lcgCount = 1;
        Simulation._lcgState = new Types.RWStructuredBuffer<int>() { 23642 };
        Simulation.Dispatch( Simulation.GenerateRandomNumbers, 128, 128, 1, 1 );

        const float HistogramMin = -1.0f;
        const float HistogramMax = 1.0f;
        const float HistogramRange = (HistogramMax - HistogramMin);
        int[] histogram = new int[20];
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
            s += "[" + (HistogramMin + i * slotRange).ToString("F3") + " ... " +
                 (HistogramMin + (i + 1) * slotRange).ToString("F3") + "] = " + histogram[i].ToString() + "\n";
        }
        Debug.Log(s);
    }

    void Update()
    {
        const int GroupSize = 256;
        
        int generateRandomNumbers = SimulationComputeShader.FindKernel("GenerateRandomNumbers");
        SimulationComputeShader.SetBuffer(generateRandomNumbers, "_lcgState", _lcgState);
        SimulationComputeShader.SetTexture(generateRandomNumbers, "_outRenderTexture", _renderTexture);
        SimulationComputeShader.SetInt("_outRenderTextureWidth", _renderTexture.width);
        SimulationComputeShader.SetInt("_outRenderTextureHeight", _renderTexture.height);
        SimulationComputeShader.SetInt("_lcgCount", _lcgState.count);
        SimulationComputeShader.Dispatch(generateRandomNumbers, ((_renderTexture.width * _renderTexture.height ) / GroupSize) + 1, 1, 1);
        
        _mapImageMaterial.mainTexture = _renderTexture;
    }
}
