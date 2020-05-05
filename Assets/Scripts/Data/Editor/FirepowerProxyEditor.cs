﻿
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
        if (!firepowerProxy)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
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
            EditorGUILayout.LabelField( firepowerProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firepowerProxy.TargetEntityID = (uint)EditorGUILayout.IntField((int)firepowerProxy.TargetEntityID);
            if (firepowerProxy.targetEntityId != firepowerProxy.TargetEntityID)
            {
                firepowerProxy.targetEntityId = firepowerProxy.TargetEntityID;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firepowerProxy.AmmunitionBudget = (uint)EditorGUILayout.IntField((int)firepowerProxy.AmmunitionBudget);
            if (firepowerProxy.ammunitionBudget != firepowerProxy.AmmunitionBudget)
            {
                firepowerProxy.ammunitionBudget = firepowerProxy.AmmunitionBudget;
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
