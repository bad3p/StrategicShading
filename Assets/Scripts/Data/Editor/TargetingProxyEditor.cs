
using Types;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(TargetingProxy))]
public class TargetingProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TargetingProxy targetingProxy = target as TargetingProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(targetingProxy) == 0)
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
                targetingProxy.TargetEntityID = targetingProxy.targetEntityId;
                EditorGUILayout.LabelField(targetingProxy.TargetEntityID.ToString());
            }
            else
            {
                targetingProxy.TargetEntityID = (uint) EditorGUILayout.IntField((int) targetingProxy.TargetEntityID);
                if (targetingProxy.targetEntityId != targetingProxy.TargetEntityID)
                {
                    targetingProxy.targetEntityId = targetingProxy.TargetEntityID;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                targetingProxy.AmmunitionBudget = targetingProxy.ammunitionBudget;
                EditorGUILayout.LabelField(targetingProxy.AmmunitionBudget.ToString());
            }
            else
            {
                targetingProxy.AmmunitionBudget = (uint) EditorGUILayout.IntField((int) targetingProxy.AmmunitionBudget);
                if (targetingProxy.ammunitionBudget != targetingProxy.AmmunitionBudget)
                {
                    targetingProxy.ammunitionBudget = targetingProxy.AmmunitionBudget;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(targetingProxy);
            EditorSceneManager.MarkSceneDirty(targetingProxy.gameObject.scene);
        }
    }
}
