
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
            EditorGUILayout.LabelField("targetPosition");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetRotation");
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
            if (EditorApplication.isPlaying)
            {
                movementProxy.TargetPosition = movementProxy.targetPosition;
                EditorGUILayout.LabelField(movementProxy.TargetPosition.x.ToString("F2") + ", " + movementProxy.TargetPosition.y.ToString("F2") + ", " + movementProxy.TargetPosition.z.ToString("F2"));    
            }
            else
            {
                movementProxy.TargetPosition = EditorGUILayout.Vector3Field("", movementProxy.TargetPosition.ToVector3());
                if (ComputeShaderEmulator.length(movementProxy.TargetPosition - movementProxy.targetPosition) > Mathf.Epsilon)
                {
                    movementProxy.targetPosition = movementProxy.TargetPosition;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                movementProxy.TargetRotation = movementProxy.targetRotation;
                EditorGUILayout.LabelField(movementProxy.TargetRotation.x.ToString("F2") + ", " + movementProxy.TargetRotation.y.ToString("F2") + ", " + movementProxy.TargetRotation.z.ToString("F2") + ", " + movementProxy.TargetRotation.w.ToString("F2"));
            }
            else
            {
                movementProxy.TargetRotation = EditorGUILayout.Vector4Field("", movementProxy.TargetRotation.ToVector4());
                if (ComputeShaderEmulator.length(movementProxy.TargetRotation - movementProxy.targetRotation) > Mathf.Epsilon)
                {
                    movementProxy.targetRotation = movementProxy.TargetRotation;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(movementProxy.TargetVelocity.ToString("F2"));
            }
            else
            {
                movementProxy.TargetVelocity = movementProxy.targetVelocity;
                movementProxy.TargetVelocity = EditorGUILayout.FloatField(movementProxy.TargetVelocity);
                if (Mathf.Abs(movementProxy.TargetVelocity - movementProxy.targetVelocity) > Mathf.Epsilon)
                {
                    movementProxy.targetVelocity = movementProxy.TargetVelocity;
                }
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
