
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityAssembly))]
public class EntityAssemblyEditor : Editor
{
    // GUI helpers
    
    private static GUIStyle _headerLabel = null;
    private static GUIStyle _headerTextField = null;
    private static GUIStyle _headerButton = null;
    private static GUIStyle _headerButtonArea = null;
    private static GUIStyle _headerIndentArea = null;
    private static GUIStyle _itemArea = null;

    private static bool _showFirepower = false;
    
    private static bool _showFirearmDescs = false;
    private static bool _showPersonnelDescs = false;
    private static bool _showBuildingDescs = false;
    
    private HashSet<int> _expandedFirearmDescIds = new HashSet<int>();
    private HashSet<int> _expandedPersonnelDescIds = new HashSet<int>();
    private HashSet<int> _expandedBuildingDescIds = new HashSet<int>(); 
    
    private static void RemoveAt<T>(ref T[] arr, int index) where T : new()
    {
	    if (index >= 0 && index < arr.Length)
	    {
		    for (int i = index + 1; i < arr.Length; i++)
		    {
			    arr[i - 1] = arr[i];
		    }
		    System.Array.Resize<T>(ref arr, arr.Length - 1);
	    }
    }
    
    private static void RemoveAt(ref string[] arr, int index)
    {
	    if (index >= 0 && index < arr.Length)
	    {
		    for (int i = index + 1; i < arr.Length; i++)
		    {
			    arr[i - 1] = arr[i];
		    }
		    System.Array.Resize<string>(ref arr, arr.Length - 1);
	    }
    }
	
    private static void InsertAt<T>(ref T[] arr, int index) where T : new()
    {
	    if (index >= 0 && index < arr.Length)
	    {
		    System.Array.Resize<T>(ref arr, arr.Length + 1);
		    for (int i = arr.Length-1; i > index; i--)
		    {
			    arr[i] = arr[i-1];
		    }			
	    }
    }
    
    private static void InsertAt(ref string[] arr, int index)
    {
	    if (index >= 0 && index < arr.Length)
	    {
		    System.Array.Resize<string>(ref arr, arr.Length + 1);
		    for (int i = arr.Length-1; i > index; i--)
		    {
			    arr[i] = arr[i-1];
		    }			
	    }
    }

    private static void Add<T>(ref T[] arr) where T : new()
    {
	    System.Array.Resize<T>(ref arr, arr.Length + 1);
	    arr[arr.Length-1] = new T(); 
    }
    
    private static void Add(ref string[] arr)
    {
	    System.Array.Resize<string>(ref arr, arr.Length + 1);
	    arr[arr.Length-1] = "New Item"; 
    }
    
    private static void SupportCustomStyles()
	{
		_headerLabel = null;
		_headerTextField = null;
		_headerButton = null;
		_headerButtonArea = null;
		_headerIndentArea = null;
		_itemArea = null;
		
		if( _headerLabel == null )
		{
			_headerLabel = new GUIStyle(GUI.skin.box);
			_headerLabel.alignment = TextAnchor.MiddleCenter;
			_headerLabel.fontStyle = FontStyle.Bold;
			_headerLabel.stretchHeight = false;
			_headerLabel.stretchWidth = true;
			_headerLabel.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
			_headerLabel.fixedHeight = 24;
		}
		
		if( _headerTextField == null )
		{
			_headerTextField = new GUIStyle(GUI.skin.box);
			_headerTextField.alignment = TextAnchor.MiddleLeft;
			_headerTextField.fontStyle = FontStyle.Bold;
			_headerTextField.stretchHeight = false;
			_headerTextField.stretchWidth = true;
			_headerTextField.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
			_headerTextField.fixedHeight = 24;
		}

		if( _headerButton == null )
		{
			_headerButton = new GUIStyle(GUI.skin.button);
			_headerButton.alignment = TextAnchor.MiddleCenter;
			_headerButton.fontStyle = FontStyle.Bold;
			_headerButton.stretchHeight = false;
			_headerButton.stretchWidth = false;
			_headerButton.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
			_headerButton.fixedWidth = 16;
			_headerButton.fixedWidth = 16;
		}

		if(_headerButtonArea == null)
		{
			_headerButtonArea = new GUIStyle(GUI.skin.box);
			_headerButtonArea.alignment = TextAnchor.MiddleCenter;
			_headerButtonArea.fontStyle = FontStyle.Bold;
			_headerButtonArea.stretchHeight = false;
			_headerButtonArea.stretchWidth = false;
			_headerButtonArea.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
			_headerButtonArea.fixedHeight = 24;
			_headerButtonArea.fixedWidth = 24;
		}
		
		if(_headerIndentArea == null)
		{
			_headerIndentArea = new GUIStyle(GUI.skin.scrollView);
			_headerIndentArea.alignment = TextAnchor.MiddleCenter;
			_headerIndentArea.fontStyle = FontStyle.Bold;
			_headerIndentArea.stretchHeight = false;
			_headerIndentArea.stretchWidth = false;
			_headerIndentArea.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
			_headerIndentArea.fixedHeight = 24;
			_headerIndentArea.fixedWidth = 48;
		}

		if(_itemArea == null)
		{
			_itemArea = new GUIStyle(GUI.skin.box);
			_itemArea.alignment = TextAnchor.MiddleCenter;
			_itemArea.fontStyle = FontStyle.Bold;
			_itemArea.stretchHeight = false;
			_itemArea.stretchWidth = true;
			_itemArea.normal.textColor = Color.Lerp(Color.white, Color.black, 0.75f);
		}
	}
    
    private void DrawHeaderLabel(string text)
    {
	    EditorGUILayout.BeginHorizontal();
	    {
		    GUILayout.Box( text, _headerLabel );
	    }
	    EditorGUILayout.EndHorizontal();
    }
    
    private void DrawInsertButton(System.Action onInsertButtonPressed)
    {
	    EditorGUILayout.BeginHorizontal();
	    {
		    for (int i = 0; i < 4; i++)
		    {
			    EditorGUILayout.BeginVertical();
			    EditorGUILayout.EndVertical();					
		    }
		    EditorGUILayout.BeginVertical();
		    {
			    if (GUILayout.Button("Insert"))
			    {
				    onInsertButtonPressed();
			    }
		    }
		    EditorGUILayout.EndVertical();
	    }
	    EditorGUILayout.EndHorizontal();
    }
	
    private void DrawRemoveButton(System.Action onRemoveButtonPressed)
    {
	    EditorGUILayout.BeginHorizontal();
	    {
		    for (int i = 0; i < 4; i++)
		    {
			    EditorGUILayout.BeginVertical();
			    EditorGUILayout.EndVertical();					
		    }
		    EditorGUILayout.BeginVertical();
		    {
			    if (GUILayout.Button("Remove"))
			    {
				    onRemoveButtonPressed();
			    }
		    }
		    EditorGUILayout.EndVertical();
	    }
	    EditorGUILayout.EndHorizontal();
    }
    
    private bool DrawDropDownToggle(string text, bool flag)
    {
	    EditorGUILayout.BeginHorizontal();
	    {
		    EditorGUILayout.BeginVertical(_headerButtonArea);
		    if (GUILayout.Button(flag ? "+" : "-", _headerButton))
		    {
			    flag = !flag;
			    GUI.FocusControl(null);
		    }
		    EditorGUILayout.EndVertical();
			
		    EditorGUILayout.BeginVertical();			
		    GUILayout.Box( text, _headerLabel );
		    EditorGUILayout.EndVertical();
	    }
	    EditorGUILayout.EndHorizontal();
	    return flag;
    }
    
    private bool DrawDropDownTextField(string text, bool flag, out string modifiedText)
    {
	    EditorGUILayout.BeginHorizontal();
	    {
		    EditorGUILayout.BeginVertical(_headerIndentArea);
		    EditorGUILayout.EndVertical();
		    
		    EditorGUILayout.BeginVertical(_headerButtonArea);
		    if (GUILayout.Button(flag ? "+" : "-", _headerButton))
		    {
			    flag = !flag;
		    }
		    EditorGUILayout.EndVertical();
			
		    EditorGUILayout.BeginVertical();			
		    modifiedText = EditorGUILayout.TextField(text, _headerTextField);
		    EditorGUILayout.EndVertical();
	    }
	    EditorGUILayout.EndHorizontal();
	    return flag;
    }
    
    private void DrawItems<T>(string header, ref bool flag, ref string[] itemNames, ref T[] items, Func<T,KeyValuePair<T,bool>> drawFunc, Func<int,bool> isExpandedFunc, Action<int,bool> setExpandedFunc) where T : new()
    {
	    flag = DrawDropDownToggle( header, flag );
	    if (flag)
	    {
		    int removedItemID = -1;
			
		    //EditorGUI.indentLevel++;
		    for (int i = 1; i < items.Length; i++)
		    {
			    //itemNames[i] = GUILayout.TextField( itemNames[i] );

			    bool prevIsExpanded = isExpandedFunc(i);
			    string modifiedText = itemNames[i];
			    bool newIsExpanded = DrawDropDownTextField(itemNames[i], prevIsExpanded, out modifiedText);
			    if (itemNames[i] != modifiedText)
			    {
				    itemNames[i] = modifiedText;
			    }
			    if (prevIsExpanded != newIsExpanded)
			    {
				    setExpandedFunc(i, newIsExpanded);
				    GUI.FocusControl(null);
			    }

			    if (newIsExpanded)
			    {
				    EditorGUI.indentLevel++;
				    EditorGUILayout.Space();
				    KeyValuePair<T, bool> drawResult = drawFunc(items[i]);
				    items[i] = drawResult.Key;
				    if (drawResult.Value)
				    {
					    removedItemID = i;
				    }
				    EditorGUI.indentLevel--;
			    }
		    }
		    EditorGUILayout.Space();

		    EditorGUILayout.BeginHorizontal();
		    {
			    if (GUILayout.Button("Add"))
			    {
				    Add<T>(ref items);
				    Add(ref itemNames);
			    }
		    }
		    EditorGUILayout.EndHorizontal();

		    if (removedItemID >= 0)
		    {
			    RemoveAt<T>( ref items, removedItemID );
			    RemoveAt( ref itemNames, removedItemID );
		    }
	    }
    }
    
    private void DrawFirearmDescs()
    {
	    Func<Structs.FirearmDesc,KeyValuePair<Structs.FirearmDesc,bool>> DrawItem = (firearmDesc) =>
	    {
		    Structs.FirearmDesc modifiedItem = new Structs.FirearmDesc();

		    modifiedItem.crew = (uint)EditorGUILayout.IntField("crew", (int)firearmDesc.crew);
		    modifiedItem.maxAmmo = (uint)EditorGUILayout.IntField("maxAmmo", (int)firearmDesc.maxAmmo);
		    modifiedItem.maxClipAmmo = (uint)EditorGUILayout.IntField("maxClipAmmo", (int)firearmDesc.maxClipAmmo);
		    modifiedItem.maxBurstAmmo = (uint)EditorGUILayout.IntField("maxBurstAmmo", (int)firearmDesc.maxBurstAmmo);
		    modifiedItem.distance = EditorGUILayout.Vector4Field("distance", firearmDesc.distance.ToVector4());
		    modifiedItem.firepower = EditorGUILayout.Vector4Field("firepower",firearmDesc.firepower.ToVector4());
		    modifiedItem.mountingTime = EditorGUILayout.FloatField("mountingTime", firearmDesc.mountingTime);
		    modifiedItem.aimingTime = EditorGUILayout.FloatField("aimingTime", firearmDesc.aimingTime);
		    modifiedItem.reloadingTime = EditorGUILayout.FloatField("reloadingTime", firearmDesc.reloadingTime);
		    modifiedItem.unjammingTime = EditorGUILayout.FloatField("unjammingTime", firearmDesc.unjammingTime);

		    bool deleteItem = false;
		    EditorGUILayout.BeginHorizontal();
		    {
			    for (int i = 0; i < 4; i++)
			    {
				    EditorGUILayout.BeginVertical();
				    EditorGUILayout.EndVertical();					
			    }
			    EditorGUILayout.BeginVertical();
			    {
				    if (GUILayout.Button("Remove"))
				    {
					    deleteItem = true;
				    }
			    }
			    EditorGUILayout.EndVertical();
		    }
		    EditorGUILayout.EndHorizontal();
		    EditorGUILayout.Space();

		    return new KeyValuePair<Structs.FirearmDesc, bool>(modifiedItem, deleteItem);
	    };

	    Func<int, bool> IsExpanded = (firearmDescId) => IsFirearmDescExpanded(firearmDescId);
	    Action<int, bool> SetExpanded = (firearmDescId, flag) => SetFirearmDescExpanded(firearmDescId, flag);
		
	    DrawItems( "Firearm", ref _showFirearmDescs, ref _entityAssembly.FirearmNameBuffer, ref _entityAssembly.FirearmDescBuffer, DrawItem, IsExpanded, SetExpanded );
    }
    
    private void DrawPersonnelDescs()
    {
	    Func<Structs.PersonnelDesc,KeyValuePair<Structs.PersonnelDesc,bool>> DrawItem = (presonnelDesc) =>
	    {
		    Structs.PersonnelDesc modifiedItem = new Structs.PersonnelDesc();

		    modifiedItem.maxPersonnel = (uint)EditorGUILayout.IntField("maxPersonnel", (int)presonnelDesc.maxPersonnel);
		    modifiedItem.linearVelocitySlow = EditorGUILayout.Vector3Field("linearVelocitySlow", presonnelDesc.linearVelocitySlow.ToVector3());
		    modifiedItem.linearVelocityFast = EditorGUILayout.Vector3Field("linearVelocityFast", presonnelDesc.linearVelocityFast.ToVector3());
		    modifiedItem.angularVelocity = EditorGUILayout.FloatField("angularVelocity", presonnelDesc.angularVelocity);
		    modifiedItem.fitnessConsumptionRateSlow = EditorGUILayout.Vector3Field("fitnessConsumptionRateSlow", presonnelDesc.fitnessConsumptionRateSlow.ToVector3());
		    modifiedItem.fitnessConsumptionRateFast = EditorGUILayout.Vector3Field("fitnessConsumptionRateFast", presonnelDesc.fitnessConsumptionRateFast.ToVector3());
		    modifiedItem.fitnessThreshold = EditorGUILayout.Vector3Field("fitnessThreshold", presonnelDesc.fitnessThreshold.ToVector3());
		    modifiedItem.fitnessRecoveryRate = EditorGUILayout.Vector3Field("fitnessRecoveryRate", presonnelDesc.fitnessRecoveryRate.ToVector3());
		    modifiedItem.moraleThreshold = EditorGUILayout.Vector4Field("moraleThreshold", presonnelDesc.moraleThreshold.ToVector4());
		    modifiedItem.moraleRecoveryRate = EditorGUILayout.Vector4Field("moraleRecoveryRate", presonnelDesc.moraleRecoveryRate.ToVector4());

		    bool deleteItem = false;
		    EditorGUILayout.BeginHorizontal();
		    {
			    for (int i = 0; i < 4; i++)
			    {
				    EditorGUILayout.BeginVertical();
				    EditorGUILayout.EndVertical();					
			    }
			    EditorGUILayout.BeginVertical();
			    {
				    if (GUILayout.Button("Remove"))
				    {
					    deleteItem = true;
				    }
			    }
			    EditorGUILayout.EndVertical();
		    }
		    EditorGUILayout.EndHorizontal();
		    EditorGUILayout.Space();
		    
		    return new KeyValuePair<Structs.PersonnelDesc, bool>(modifiedItem, deleteItem);
	    };
	    
	    Func<int, bool> IsExpanded = (personnelDescId) => IsPersonnelDescExpanded(personnelDescId);
	    Action<int, bool> SetExpanded = (personnelDescId, flag) => SetPersonnelDescExpanded(personnelDescId, flag);
		
	    DrawItems( "Personnel", ref _showPersonnelDescs, ref _entityAssembly.PersonnelNameBuffer, ref _entityAssembly.PersonnelDescBuffer, DrawItem, IsExpanded, SetExpanded );
    }
    
    private void DrawBuildingDescs()
    {
	    Func<Structs.BuildingDesc,KeyValuePair<Structs.BuildingDesc,bool>> DrawItem = (buildingDesc) =>
	    {
		    Structs.BuildingDesc modifiedItem = new Structs.BuildingDesc();

		    modifiedItem.maxIntegrity = (uint)EditorGUILayout.FloatField("maxIntegrity", (int)buildingDesc.maxIntegrity);
		    modifiedItem.armor = EditorGUILayout.FloatField("armor", buildingDesc.armor);
		    modifiedItem.material = (uint)EditorGUILayout.IntField("material", (int)buildingDesc.material);

		    bool deleteItem = false;
		    EditorGUILayout.BeginHorizontal();
		    {
			    for (int i = 0; i < 4; i++)
			    {
				    EditorGUILayout.BeginVertical();
				    EditorGUILayout.EndVertical();					
			    }
			    EditorGUILayout.BeginVertical();
			    {
				    if (GUILayout.Button("Remove"))
				    {
					    deleteItem = true;
				    }
			    }
			    EditorGUILayout.EndVertical();
		    }
		    EditorGUILayout.EndHorizontal();
		    EditorGUILayout.Space();
		    
		    return new KeyValuePair<Structs.BuildingDesc, bool>(modifiedItem, deleteItem);
	    };
	    
	    Func<int, bool> IsExpanded = (buildingDescId) => IsBuildingDescExpanded(buildingDescId);
	    Action<int, bool> SetExpanded = (buildingDescId, flag) => SetBuildingDescExpanded(buildingDescId, flag);
		
	    DrawItems( "Buildings", ref _showBuildingDescs, ref _entityAssembly.BuildingNameBuffer, ref _entityAssembly.BuildingDescBuffer, DrawItem, IsExpanded, SetExpanded );
    }
    
    private bool IsFirearmDescExpanded(int firearmDescId)
    {
        return _expandedFirearmDescIds.Contains(firearmDescId);
    }

    private void SetFirearmDescExpanded(int firearmDescId, bool isExpanded)
    {
        bool isCurrentlyExpanded = IsFirearmDescExpanded(firearmDescId);
        if (isExpanded != isCurrentlyExpanded)
        {
            if (isExpanded)
            {
                _expandedFirearmDescIds.Add(firearmDescId);
            }
            else
            {
                _expandedFirearmDescIds.Remove(firearmDescId);
            }
        }
    }
    
    private bool IsPersonnelDescExpanded(int personnelDescId)
    {
        return _expandedPersonnelDescIds.Contains(personnelDescId);
    }

    private void SetPersonnelDescExpanded(int personnelDescId, bool isExpanded)
    {
        bool isCurrentlyExpanded = IsPersonnelDescExpanded(personnelDescId);
        if (isExpanded != isCurrentlyExpanded)
        {
            if (isExpanded)
            {
                _expandedPersonnelDescIds.Add(personnelDescId);
            }
            else
            {
                _expandedPersonnelDescIds.Remove(personnelDescId);
            }
        }
    }
    
    private bool IsBuildingDescExpanded(int buildingDescId)
    {
	    return _expandedBuildingDescIds.Contains(buildingDescId);
    }

    private void SetBuildingDescExpanded(int buildingDescId, bool isExpanded)
    {
	    bool isCurrentlyExpanded = IsBuildingDescExpanded(buildingDescId);
	    if (isExpanded != isCurrentlyExpanded)
	    {
		    if (isExpanded)
		    {
			    _expandedBuildingDescIds.Add(buildingDescId);
		    }
		    else
		    {
			    _expandedBuildingDescIds.Remove(buildingDescId);
		    }
	    }
    }
    
    // Editor
    
    EntityAssembly _entityAssembly = null;
    
    public override void OnInspectorGUI()
    {
	    SupportCustomStyles();
	    
        _entityAssembly = target as EntityAssembly;
        if (!_entityAssembly)
        {
            return;
        }
        
        DrawHeaderLabel("Runtime");

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityCount");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(_entityAssembly.descBuffer.Count.ToString());
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();

        bool drawDefaultInspector =
            (_entityAssembly.FirearmDescBuffer.Length != _entityAssembly.FirearmNameBuffer.Length) ||
            (_entityAssembly.PersonnelDescBuffer.Length != _entityAssembly.PersonnelNameBuffer.Length);

        // user have to manually resolve conflicts
        
        if (drawDefaultInspector)
        {
            DrawDefaultInspector();
            return;
        }
        
        // draw globals
        
        DrawHeaderLabel("Globals");
        
        _showFirepower = DrawDropDownToggle( "Firepower", _showFirepower );
        if (_showFirepower)
        {
	        _entityAssembly.Firepower = EditorGUILayout.Vector4Field("firepower", _entityAssembly.Firepower.ToVector4());
	        _entityAssembly.KillProbability = EditorGUILayout.Vector4Field("killProbability", _entityAssembly.KillProbability.ToVector4());
	        _entityAssembly.WoundProbability = EditorGUILayout.Vector4Field("woundProbability", _entityAssembly.WoundProbability.ToVector4());
	        _entityAssembly.MoraleDamage = EditorGUILayout.Vector4Field("moraleDamage", _entityAssembly.MoraleDamage.ToVector4());
	        _entityAssembly.KillMoraleDamage = EditorGUILayout.FloatField("killMoraleDamage", _entityAssembly.KillMoraleDamage);
	        _entityAssembly.WoundMoraleDamage = EditorGUILayout.FloatField("woundMoraleDamage", _entityAssembly.WoundMoraleDamage); 
        }
        
        // draw flyweights

        DrawHeaderLabel("Flyweights");
        DrawFirearmDescs();
        DrawPersonnelDescs();
        DrawBuildingDescs();
        
        DrawHeaderLabel("Simulation");
        _entityAssembly.NumCPUThreads = EditorGUILayout.IntField("NumCPUThreads", _entityAssembly.NumCPUThreads);
        _entityAssembly.RngSeedFromTimer = EditorGUILayout.Toggle("RngSeedFromTimer", _entityAssembly.RngSeedFromTimer);
        _entityAssembly.RngSeed = EditorGUILayout.IntField("RngSeed", _entityAssembly.RngSeed);
        _entityAssembly.RngMaxUniform = EditorGUILayout.IntField("RngMaxUniform", _entityAssembly.RngMaxUniform);
        _entityAssembly.RngCount = EditorGUILayout.IntField("RngCount", _entityAssembly.RngCount);
        _entityAssembly.RngStateLength = EditorGUILayout.IntField("RngStateLength", _entityAssembly.RngStateLength);

        EditorUtility.SetDirty( _entityAssembly );	    
    }

}
