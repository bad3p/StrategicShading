
using Types;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(MovementProxy))]
public class MovementProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MovementProxy movementProxy = target as MovementProxy;
        if (!movementProxy)
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
            EditorGUILayout.LabelField("targetPosition");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetVelocity");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( movementProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            movementProxy.TargetEntityID = (uint)EditorGUILayout.IntField((int)movementProxy.TargetEntityID);
            if (movementProxy.targetEntityId != movementProxy.TargetEntityID)
            {
                movementProxy.targetEntityId = movementProxy.TargetEntityID;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            movementProxy.TargetPosition = EditorGUILayout.Vector3Field("",movementProxy.TargetPosition.ToVector3());
            if (ComputeShaderEmulator.length(movementProxy.TargetPosition - movementProxy.targetPosition) > Mathf.Epsilon)
            {
                movementProxy.targetPosition = movementProxy.TargetPosition;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            movementProxy.TargetVelocity = EditorGUILayout.FloatField( movementProxy.TargetVelocity );
            if (Mathf.Abs(movementProxy.TargetVelocity - movementProxy.targetVelocity) > Mathf.Epsilon)
            {
                movementProxy.targetVelocity = movementProxy.TargetVelocity;
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(movementProxy);
            EditorSceneManager.MarkSceneDirty(movementProxy.gameObject.scene);
        }
    }
}
