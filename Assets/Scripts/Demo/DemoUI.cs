using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DemoUI : MonoBehaviour
{
    [Header("UI")]
    public Image MapImage;

    [Header("Setup")] 
    public string MapName;

    private Scene? _mapScene = null;
    
    private Simulation.FirearmsData[] _firearmsData = new Simulation.FirearmsData[0];
    private Dictionary<Demo.FirearmsData, int> _firearmsDataIndices = new Dictionary<Demo.FirearmsData, int>();
    
    #region SceneLoading
    void LoadFirearmsData()
    {
        Demo.FirearmsData[] firearmsData = GameObject.FindObjectsOfType<Demo.FirearmsData>();

        _firearmsData = new Simulation.FirearmsData[firearmsData.Length];
        for (int i = 0; i < firearmsData.Length; i++)
        {
            _firearmsDataIndices.Add( firearmsData[i], i );

            _firearmsData[i].distance = firearmsData[i].distance;
            _firearmsData[i].firepower = firearmsData[i].firepower;
        }
    }
    #endregion
    
    #region MonoBehaviour
    void Awake()
    {
        if (MapName.Length > 0)
        {
            LoadSceneParameters loadSceneParameters = new LoadSceneParameters();
            loadSceneParameters.loadSceneMode = LoadSceneMode.Additive;
            loadSceneParameters.localPhysicsMode = LocalPhysicsMode.Physics3D;
            _mapScene = SceneManager.LoadScene( MapName, loadSceneParameters );
        }

        if (_mapScene.HasValue && _mapScene.Value.isLoaded)
        {
            LoadFirearmsData();
        }
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion
}
