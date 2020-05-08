
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EntityProxy))]
public class EntityProxyEditor : Editor
{
    void OnSceneGUI()
    {
        EntityProxy entityProxy = target as EntityProxy;
        if (entityProxy.mesh)
        {
            float distance = Vector3.Distance(entityProxy.mesh.bounds.center, Camera.current.transform.position);
            Handles.Label( entityProxy.mesh.bounds.center, distance.ToString("F3"));
        }
    }

    public override void OnInspectorGUI()
    {
        EntityProxy entityProxy = target as EntityProxy;

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("teamId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("transformId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("hierarchyId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("personnelId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firearmsId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("movementId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firepowerId");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField( entityProxy.TeamId.ToString() );    
            }
            else
            {
                entityProxy.TeamId = (uint) EditorGUILayout.IntField((int) entityProxy.TeamId);
                if (entityProxy.teamId != entityProxy.TeamId)
                {
                    entityProxy.teamId = entityProxy.TeamId;
                }
            }

            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.transformId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.hierarchyId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.personnelId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.firearmsId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.movementId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( entityProxy.firepowerId.ToString() );
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();

        if (!EditorApplication.isPlaying)
        {
            if (entityProxy.transformId == 0 && entityProxy.hierarchyId > 0 && entityProxy.transform.hasChanged)
            {
                UpdateTransforms(entityProxy);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(entityProxy);
            EditorSceneManager.MarkSceneDirty(entityProxy.gameObject.scene);
        }
    }

    static void UpdateTransforms(EntityProxy entityProxy)
    {
        if (entityProxy.transformId > 0)
        {
            TransformProxy transformProxy = entityProxy.GetComponent<TransformProxy>();
            transformProxy.position = transformProxy.transform.position;
            transformProxy.rotation = transformProxy.transform.rotation;
            transformProxy.scale = transformProxy.transform.localScale;
        }

        int childCount = entityProxy.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = entityProxy.transform.GetChild(i);
            EntityProxy childEntityProxy = childTransform.GetComponent<EntityProxy>();
            if (childEntityProxy)
            {
                UpdateTransforms(childEntityProxy);
            }
        }
    }
}
