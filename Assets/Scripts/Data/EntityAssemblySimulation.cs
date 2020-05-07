
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class EntityAssembly : MonoBehaviour
{
#if UNITY_EDITOR    
    void Start()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }
    }
    
    void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            return;
        }

        ComputeShaderEmulator._dT = Time.deltaTime;

        ComputeShaderEmulator._entityCount = _entityBuffer.Count;
        ComputeShaderEmulator._transformCount = _transformBuffer.Count;
        ComputeShaderEmulator._hierarchyCount = _hierarchyBuffer.Count;
        ComputeShaderEmulator._personnelCount = _personnelBuffer.Count;
        ComputeShaderEmulator._firearmsCount = _firearmsBuffer.Count;
        ComputeShaderEmulator._movementCount = _movementBuffer.Count;
        ComputeShaderEmulator._firepowerCount = _firepowerBuffer.Count;
        
        ComputeShaderEmulator._entityBuffer.Clear();
        ComputeShaderEmulator._entityBuffer.AddRange( _entityBuffer );
        
        ComputeShaderEmulator._transformBuffer.Clear();
        ComputeShaderEmulator._transformBuffer.AddRange( _transformBuffer );
        
        ComputeShaderEmulator._hierarchyBuffer.Clear();
        ComputeShaderEmulator._hierarchyBuffer.AddRange( _hierarchyBuffer );
        
        ComputeShaderEmulator._personnelBuffer.Clear();
        ComputeShaderEmulator._personnelBuffer.AddRange( _personnelBuffer );
        
        ComputeShaderEmulator._firearmsBuffer.Clear();
        ComputeShaderEmulator._firearmsBuffer.AddRange( _firearmsBuffer );
        
        ComputeShaderEmulator._movementBuffer.Clear();
        ComputeShaderEmulator._movementBuffer.AddRange( _movementBuffer );
        
        ComputeShaderEmulator._firepowerBuffer.Clear();
        ComputeShaderEmulator._firepowerBuffer.AddRange( _firepowerBuffer );
        
        ComputeShaderEmulator.Dispatch( ComputeShaderEmulator.UpdateMovement, (uint)_movementBuffer.Count / 256 + 1, 1, 1 );
        
        _entityBuffer.Clear();
        _entityBuffer.AddRange( ComputeShaderEmulator._entityBuffer );
        
        _transformBuffer.Clear();
        _transformBuffer.AddRange( ComputeShaderEmulator._transformBuffer );
        
        _hierarchyBuffer.Clear();
        _hierarchyBuffer.AddRange( ComputeShaderEmulator._hierarchyBuffer );
        
        _personnelBuffer.Clear();
        _personnelBuffer.AddRange( ComputeShaderEmulator._personnelBuffer );
        
        _firearmsBuffer.Clear();
        _firearmsBuffer.AddRange( ComputeShaderEmulator._firearmsBuffer );
        
        _movementBuffer.Clear();
        _movementBuffer.AddRange( ComputeShaderEmulator._movementBuffer );
        
        _firepowerBuffer.Clear();
        _firepowerBuffer.AddRange( ComputeShaderEmulator._firepowerBuffer );        
    }
#endif    
}
