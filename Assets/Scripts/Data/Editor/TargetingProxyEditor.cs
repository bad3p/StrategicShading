
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
            EditorGUILayout.LabelField("arc");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("front");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("numEnemies");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("numAllies");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firearmTargetIds");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firearmTargetWeights");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(targetingProxy.arc.x.ToString("F2") + ", " + targetingProxy.arc.y.ToString("F2") + ", " + targetingProxy.arc.y.ToString("F2"));
            }
            else
            {
                targetingProxy.Arc = EditorGUILayout.Vector4Field("", targetingProxy.Arc.ToVector4());
                if (ComputeShaderEmulator.distance(targetingProxy.Arc, targetingProxy.arc) > ComputeShaderEmulator.FLOAT_EPSILON)
                {
                    targetingProxy.arc = targetingProxy.Arc;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(targetingProxy.front.x.ToString("F2") + ", " + targetingProxy.front.y.ToString("F2") + ", " + targetingProxy.front.z.ToString("F2"));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(targetingProxy.numEnemies.ToString());
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(targetingProxy.numAllies.ToString());
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(targetingProxy.firearmTargetIds.x.ToString() + ", " + targetingProxy.firearmTargetIds.y.ToString() + ", " + targetingProxy.firearmTargetIds.z.ToString() + ", " + targetingProxy.firearmTargetIds.w.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(targetingProxy.firearmTargetWeights.x.ToString("F2") + ", " + targetingProxy.firearmTargetWeights.y.ToString("F2") + ", " + targetingProxy.firearmTargetWeights.z.ToString("F2") + ", " + targetingProxy.firearmTargetWeights.w.ToString("F2") );
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
