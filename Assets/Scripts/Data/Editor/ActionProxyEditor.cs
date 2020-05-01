
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ActionProxy))]
public class ActionProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ActionProxy actionProxy = target as ActionProxy;

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("actionId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetPosition");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("actionTimeout");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( actionProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.ActionID = (uint)EditorGUILayout.IntField((int)actionProxy.ActionID);
            if (actionProxy.actionId != actionProxy.ActionID)
            {
                actionProxy.actionId = actionProxy.ActionID;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.TargetEntityID = (uint)EditorGUILayout.IntField((int)actionProxy.TargetEntityID);
            if (actionProxy.targetEntityId != actionProxy.TargetEntityID)
            {
                actionProxy.targetEntityId = actionProxy.TargetEntityID;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            actionProxy.TargetPosition = EditorGUILayout.Vector3Field("",actionProxy.TargetPosition.ToVector3());
            if (ComputeShaderEmulator.length(actionProxy.TargetPosition - actionProxy.targetPosition) > Mathf.Epsilon)
            {
                actionProxy.targetPosition = actionProxy.TargetPosition;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.ActionTimeout = (uint)EditorGUILayout.FloatField( actionProxy.ActionTimeout );
            if (actionProxy.actionTimeout != actionProxy.ActionTimeout)
            {
                actionProxy.actionTimeout = actionProxy.ActionTimeout;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(actionProxy);
            EditorSceneManager.MarkSceneDirty(actionProxy.gameObject.scene);
        }
    }
}
