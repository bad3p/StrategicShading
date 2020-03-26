using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        _renderTexture = new RenderTexture( 1024, 512, 0, RenderTextureFormat.RFloat );		
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
