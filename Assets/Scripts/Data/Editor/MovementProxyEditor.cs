
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
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(movementProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetPosition");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetRotation");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("targetVelocityByDistance");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("deltaPosition");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
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
                movementProxy.TargetVelocityByDistance = movementProxy.targetVelocityByDistance;
                EditorGUILayout.LabelField(movementProxy.TargetVelocityByDistance.x.ToString("F2") + ", " + movementProxy.TargetVelocityByDistance.y.ToString("F2") + ", " + movementProxy.TargetVelocityByDistance.z.ToString("F2") + ", " + movementProxy.TargetVelocityByDistance.w.ToString("F2"));
            }
            else
            {
                movementProxy.TargetVelocityByDistance = EditorGUILayout.Vector4Field("", movementProxy.TargetVelocityByDistance.ToVector4());
                if (ComputeShaderEmulator.length(movementProxy.TargetVelocityByDistance - movementProxy.targetVelocityByDistance) > Mathf.Epsilon)
                {
                    movementProxy.targetVelocityByDistance = movementProxy.TargetVelocityByDistance;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(movementProxy.deltaPosition.x.ToString("F2") + ", " + movementProxy.deltaPosition.y.ToString("F2") + ", " + movementProxy.deltaPosition.z.ToString("F2"));
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
