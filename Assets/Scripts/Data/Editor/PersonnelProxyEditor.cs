
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PersonnelProxy))]
public class PersonnelProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PersonnelProxy personnelProxy = target as PersonnelProxy;

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("morale");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("fitness");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("count");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("wounded");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("killed");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( personnelProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            personnelProxy.Morale = EditorGUILayout.FloatField(personnelProxy.Morale);
            if (Mathf.Abs(personnelProxy.Morale - personnelProxy.morale) > Mathf.Epsilon)
            {
                personnelProxy.morale = personnelProxy.Morale;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            personnelProxy.Fitness = EditorGUILayout.FloatField(personnelProxy.Fitness);
            if (Mathf.Abs(personnelProxy.Fitness - personnelProxy.fitness) > Mathf.Epsilon)
            {
                personnelProxy.fitness = personnelProxy.Fitness;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            personnelProxy.Count = (uint)EditorGUILayout.IntField((int)personnelProxy.Count);
            if (personnelProxy.Count != personnelProxy.count)
            {
                personnelProxy.count = personnelProxy.Count;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            personnelProxy.Wounded = (uint)EditorGUILayout.IntField((int)personnelProxy.Wounded);
            if (personnelProxy.Wounded != personnelProxy.wounded)
            {
                personnelProxy.wounded = personnelProxy.Wounded;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            personnelProxy.Killed = (uint)EditorGUILayout.IntField((int)personnelProxy.Killed);
            if (personnelProxy.Killed != personnelProxy.killed)
            {
                personnelProxy.killed = personnelProxy.Killed;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(personnelProxy);
            EditorSceneManager.MarkSceneDirty(personnelProxy.gameObject.scene);
        }
    }
}
