
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
            EditorGUILayout.LabelField("front");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("direction");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("arc");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(targetingProxy.front.x.ToString("F2") + ", " + targetingProxy.front.y.ToString("F2") + ", " + targetingProxy.front.y.ToString("F2"));
            }
            else
            {
                EditorGUILayout.LabelField(targetingProxy.front.x.ToString("F2") + ", " + targetingProxy.front.y.ToString("F2") + ", " + targetingProxy.front.y.ToString("F2"));
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(targetingProxy.direction.x.ToString("F2") + ", " + targetingProxy.direction.y.ToString("F2") + ", " + targetingProxy.direction.y.ToString("F2"));
            }
            else
            {
                targetingProxy.Direction = EditorGUILayout.Vector3Field("", targetingProxy.Direction.ToVector3());
                if (ComputeShaderEmulator.distance(targetingProxy.Direction, targetingProxy.direction) > ComputeShaderEmulator.FLOAT_EPSILON)
                {
                    targetingProxy.direction = targetingProxy.Direction;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(targetingProxy.arc.ToString("F2"));
            }
            else
            {
                targetingProxy.Arc = EditorGUILayout.FloatField("", targetingProxy.Arc);
                if (Mathf.Abs(targetingProxy.Arc - targetingProxy.arc) > Mathf.Epsilon)
                {
                    targetingProxy.arc = targetingProxy.Arc;
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
