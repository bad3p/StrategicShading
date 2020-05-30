
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityAssembly))]
public class EntityAssemblyEditor : Editor
{
    // GUI helpers
    
    private static GUIStyle _headerLabel = null;
    private static GUIStyle _headerButton = null;
    private static GUIStyle _headerButtonArea = null;
    private static GUIStyle _itemArea = null;
    
    private static bool _showFirearmDescs = false;
    private static bool _showPersonnelDescs = false;
    
    private static void SupportCustomStyles()
	{
		_headerLabel = null;
		_headerButton = null;
		_headerButtonArea = null;
		_itemArea = null;
		
		if( _headerLabel == null )
		{
			_headerLabel = new GUIStyle(GUI.skin.box);
			_headerLabel.alignment = TextAnchor.MiddleCenter;
			_headerLabel.fontStyle = FontStyle.Bold;
			_headerLabel.stretchHeight = false;
			_headerLabel.stretchWidth = true;
			_headerLabel.normal.textColor = Color.Lerp(Color.white, Color.gray, 0.75f);
			_headerLabel.fixedHeight = 24;
		}
		
		if( _headerButton == null )
		{
			_headerButton = new GUIStyle(GUI.skin.button);
			_headerButton.alignment = TextAnchor.MiddleCenter;
			_headerButton.fontStyle = FontStyle.Bold;
			_headerButton.stretchHeight = false;
			_headerButton.stretchWidth = false;
			_headerButton.normal.textColor = Color.Lerp(Color.white, Color.gray, 0.75f);
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
			_headerButtonArea.normal.textColor = Color.Lerp(Color.white, Color.gray, 0.75f);
			_headerButtonArea.fixedHeight = 24;
			_headerButtonArea.fixedWidth = 24;
		}

		if(_itemArea == null)
		{
			_itemArea = new GUIStyle(GUI.skin.box);
			_itemArea.alignment = TextAnchor.MiddleCenter;
			_itemArea.fontStyle = FontStyle.Bold;
			_itemArea.stretchHeight = false;
			_itemArea.stretchWidth = true;
			_itemArea.normal.textColor = Color.Lerp(Color.white, Color.gray, 0.75f);
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
		    }
		    EditorGUILayout.EndVertical();
			
		    EditorGUILayout.BeginVertical();			
		    GUILayout.Box( text, _headerLabel );
		    EditorGUILayout.EndVertical();
	    }
	    EditorGUILayout.EndHorizontal();
	    return flag;
    }
    
    private void DrawItems<T>(string header, ref bool flag, List<string> itemNames, List<T> items, System.Func<T,bool> drawFunc) where T : new()
    {
	    flag = DrawDropDownToggle( header, flag );
	    if (flag)
	    {
		    int removedItemID = -1;
			
		    EditorGUI.indentLevel++;
		    for (int i = 0; i < items.Count; i++)
		    {
			    itemNames[i] = GUILayout.TextField( itemNames[i] );
			    if (drawFunc(items[i]))
			    {
				    removedItemID = i;
			    }
		    }
		    EditorGUI.indentLevel--;
		    EditorGUILayout.Space();

		    EditorGUILayout.BeginHorizontal();
		    {
			    if (GUILayout.Button("Add"))
			    {
				    items.Add(new T());
			    }
		    }
		    EditorGUILayout.EndHorizontal();

		    if (removedItemID >= 0)
		    {
			    items.RemoveAt( removedItemID );
		    }
	    }
    }
    
    private void DrawFirearmDescs()
    {
	    Func<Structs.FirearmDesc,bool> DrawItem = (firearmDesc) =>
	    {
		    bool result = false;

		    firearmDesc.maxAmmo = (uint)EditorGUILayout.IntField("maxAmmo", (int)firearmDesc.maxAmmo); 
		    firearmDesc.distance = EditorGUILayout.Vector4Field("distance", firearmDesc.distance.ToVector4());
		    firearmDesc.firepower = EditorGUILayout.Vector4Field("firepower",firearmDesc.firepower.ToVector4());
		    firearmDesc.aimingTime = EditorGUILayout.FloatField("aimingTime", firearmDesc.aimingTime);
		    firearmDesc.reloadingTime = EditorGUILayout.FloatField("reloadingTime", firearmDesc.reloadingTime);

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
					    result = true;
				    }
			    }
			    EditorGUILayout.EndVertical();
		    }
		    EditorGUILayout.EndHorizontal();
		    EditorGUILayout.Space();
		    return result;
	    };
		
	    DrawItems( "Firearm", ref _showFirearmDescs, _entityAssembly.FirearmNameBuffer, _entityAssembly.FirearmDescBuffer, DrawItem );
    }
    
    private void DrawPersonnelDescs()
    {
	    Func<Structs.PersonnelDesc,bool> DrawItem = (presonnelDesc) =>
	    {
		    bool result = false;

		    presonnelDesc.maxPersonnel = (uint)EditorGUILayout.IntField("maxPersonnel", (int)presonnelDesc.maxPersonnel);
		    presonnelDesc.linearVelocitySlow = EditorGUILayout.Vector3Field("linearVelocitySlow", presonnelDesc.linearVelocitySlow.ToVector3());
		    presonnelDesc.linearVelocityFast = EditorGUILayout.Vector3Field("linearVelocityFast", presonnelDesc.linearVelocityFast.ToVector3());
		    presonnelDesc.angularVelocity = EditorGUILayout.FloatField("angularVelocity", presonnelDesc.angularVelocity);
		    presonnelDesc.fitnessConsumptionRateSlow = EditorGUILayout.Vector3Field("fitnessConsumptionRateSlow", presonnelDesc.fitnessConsumptionRateSlow.ToVector3());
		    presonnelDesc.fitnessConsumptionRateFast = EditorGUILayout.Vector3Field("fitnessConsumptionRateFast", presonnelDesc.fitnessConsumptionRateFast.ToVector3());
		    presonnelDesc.fitnessThreshold = EditorGUILayout.Vector3Field("fitnessThreshold", presonnelDesc.fitnessThreshold.ToVector3());
		    presonnelDesc.fitnessRecoveryRate = EditorGUILayout.Vector3Field("fitnessRecoveryRate", presonnelDesc.fitnessRecoveryRate.ToVector3());
		    presonnelDesc.moraleThreshold = EditorGUILayout.Vector4Field("moraleThreshold", presonnelDesc.moraleThreshold.ToVector4());
		    presonnelDesc.moraleRecoveryRate = EditorGUILayout.Vector4Field("moraleRecoveryRate", presonnelDesc.moraleRecoveryRate.ToVector4());

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
					    result = true;
				    }
			    }
			    EditorGUILayout.EndVertical();
		    }
		    EditorGUILayout.EndHorizontal();
		    EditorGUILayout.Space();
		    return result;
	    };
		
	    DrawItems( "Personnel", ref _showPersonnelDescs, _entityAssembly.PersonnelNameBuffer, _entityAssembly.PersonnelDescBuffer, DrawItem );
    }
    
    // expanded list items
    
    private HashSet<int> _expandedFirearmDescIds = new HashSet<int>();
    private HashSet<int> _expandedPersonnelDescIds = new HashSet<int>();
    
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
    
    // Editor
    
    EntityAssembly _entityAssembly = null;
    
    public override void OnInspectorGUI()
    {
        _entityAssembly = target as EntityAssembly;
        if (!_entityAssembly)
        {
            return;
        }

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
            (_entityAssembly.FirearmDescBuffer.Count != _entityAssembly.FirearmNameBuffer.Count) ||
            (_entityAssembly.PersonnelDescBuffer.Count != _entityAssembly.PersonnelNameBuffer.Count);

        // user have to manually resolve conflicts
        
        if (drawDefaultInspector)
        {
            DrawDefaultInspector();
            return;
        }
        
        // draw flyweights

        SupportCustomStyles();
        DrawHeaderLabel("Flyweights");
        DrawFirearmDescs();
        DrawPersonnelDescs();
        
        EditorUtility.SetDirty( _entityAssembly );
    }

}
