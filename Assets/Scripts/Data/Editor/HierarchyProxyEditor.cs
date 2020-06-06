
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HierarchyProxy))]
public class HierarchyProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        HierarchyProxy hierarchyProxy = target as HierarchyProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(hierarchyProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("parentEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firstChildEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("nextSiblingEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("joinEntityId");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( hierarchyProxy.parentEntityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( hierarchyProxy.firstChildEntityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( hierarchyProxy.nextSiblingEntityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( hierarchyProxy.joinEntityId.ToString() );
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }
}
