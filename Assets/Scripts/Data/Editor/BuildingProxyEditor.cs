
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(BuildingProxy))]
public class BuildingProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BuildingProxy buildingProxy = target as BuildingProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(buildingProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("descId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("integrity");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                if (entityAssembly)
                {
                    if (buildingProxy.descId < entityAssembly.BuildingNameBuffer.Length)
                    {
                        EditorGUILayout.LabelField(entityAssembly.BuildingNameBuffer[(int)buildingProxy.descId]);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(buildingProxy.descId.ToString());
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(buildingProxy.DescID.ToString());
                }
            }
            else
            {
                if (entityAssembly)
                {
                    buildingProxy.DescID = (uint)EditorGUILayout.Popup((int)buildingProxy.DescID, entityAssembly.BuildingNameBuffer);
                    if (buildingProxy.descId != buildingProxy.DescID)
                    {
                        buildingProxy.descId = buildingProxy.DescID;
                        buildingProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(buildingProxy);
                    }
                }
                else
                {
                    buildingProxy.DescID = (uint) EditorGUILayout.IntField((int) buildingProxy.DescID);
                    if (buildingProxy.descId != buildingProxy.DescID)
                    {
                        buildingProxy.descId = buildingProxy.DescID;
                        buildingProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(buildingProxy);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(buildingProxy.integrity.ToString());
            }
            else
            {
                buildingProxy.Integrity = EditorGUILayout.FloatField( buildingProxy.Integrity);
                if (buildingProxy.integrity != buildingProxy.Integrity)
                {
                    buildingProxy.integrity = buildingProxy.Integrity;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(buildingProxy);
            EditorSceneManager.MarkSceneDirty(buildingProxy.gameObject.scene);
        }
    }
}
