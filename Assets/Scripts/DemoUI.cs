﻿
using Types;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ComputeShaderEngine))]
public class DemoUI : MonoBehaviour
{
    [Header("UI")]
    public Image MapImage;

    private ComputeShaderEngine _computeShaderEngine = null;
    private Material      _mapImageMaterial = null;
    private RenderTexture _renderTexture = null;
    private RWTexture2D<float> _rwTexture2D = null;

    void Awake()
    {
        _computeShaderEngine = GetComponent<ComputeShaderEngine>();
        
        _mapImageMaterial = new Material( Shader.Find("Unlit/Texture") );
        MapImage.material = _mapImageMaterial;
        MapImage.SetMaterialDirty();
        
        _renderTexture = new RenderTexture( 256, 128, 0, RenderTextureFormat.RFloat );		
        _renderTexture.enableRandomWrite = true;
        _renderTexture.filterMode = FilterMode.Point;
        _renderTexture.Create();
        
        _rwTexture2D = new RWTexture2D<float>( 256, 128 );
    }

    void Update()
    {
        _computeShaderEngine.GenerateRandomNumbers( _renderTexture );
        _computeShaderEngine.GenerateRandomNumbers( _rwTexture2D );
        _mapImageMaterial.mainTexture = _renderTexture;
    }
}
