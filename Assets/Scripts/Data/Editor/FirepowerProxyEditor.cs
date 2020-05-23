
using Types;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(FirepowerProxy))]
public class FirepowerProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FirepowerProxy firepowerProxy = target as FirepowerProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(firepowerProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ammunitionBudget");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firepowerProxy.TargetEntityID = firepowerProxy.targetEntityId;
                EditorGUILayout.LabelField(firepowerProxy.TargetEntityID.ToString());
            }
            else
            {
                firepowerProxy.TargetEntityID = (uint) EditorGUILayout.IntField((int) firepowerProxy.TargetEntityID);
                if (firepowerProxy.targetEntityId != firepowerProxy.TargetEntityID)
                {
                    firepowerProxy.targetEntityId = firepowerProxy.TargetEntityID;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firepowerProxy.AmmunitionBudget = firepowerProxy.ammunitionBudget;
                EditorGUILayout.LabelField(firepowerProxy.AmmunitionBudget.ToString());
            }
            else
            {
                firepowerProxy.AmmunitionBudget = (uint) EditorGUILayout.IntField((int) firepowerProxy.AmmunitionBudget);
                if (firepowerProxy.ammunitionBudget != firepowerProxy.AmmunitionBudget)
                {
                    firepowerProxy.ammunitionBudget = firepowerProxy.AmmunitionBudget;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(firepowerProxy);
            EditorSceneManager.MarkSceneDirty(firepowerProxy.gameObject.scene);
        }
    }
}
