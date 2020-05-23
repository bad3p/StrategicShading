
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TransformProxy))]
public class TransformProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TransformProxy transformProxy = target as TransformProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(transformProxy) == 0)
        {
            return;
        }

        if (!EditorApplication.isPlaying)
        {
            transformProxy.position = transformProxy.transform.position;
            transformProxy.rotation = transformProxy.transform.rotation;
            transformProxy.scale = transformProxy.transform.localScale;
        }
        else
        {
            transformProxy.transform.position = transformProxy.position.ToVector3();
            transformProxy.transform.rotation = transformProxy.rotation.ToQuaternion();
            transformProxy.transform.localScale = transformProxy.scale.ToVector3();
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("position");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("rotation");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("scale");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( transformProxy.position.ToVector3().ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( transformProxy.rotation.ToQuaternion().ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( transformProxy.scale.ToVector3().ToString() );
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }
}
