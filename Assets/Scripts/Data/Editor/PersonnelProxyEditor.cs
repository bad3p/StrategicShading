
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PersonnelProxy))]
public class PersonnelProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PersonnelProxy personnelProxy = target as PersonnelProxy;

        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();
        
        if (entityAssembly && entityAssembly.GetEntityId(personnelProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("descId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("morale");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("fitness");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("pose");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("count");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("status");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                personnelProxy.DescID = personnelProxy.descId;
                if (entityAssembly)
                {
                    if (personnelProxy.DescID < entityAssembly.PersonnelNameBuffer.Count)
                    {
                        EditorGUILayout.LabelField(entityAssembly.PersonnelNameBuffer[(int)personnelProxy.DescID]);        
                    }
                    else
                    {
                        EditorGUILayout.LabelField(personnelProxy.DescID.ToString());    
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(personnelProxy.DescID.ToString());
                }
            }
            else
            {
                if (entityAssembly)
                {
                    personnelProxy.DescID = (uint)EditorGUILayout.Popup((int)personnelProxy.DescID, entityAssembly.PersonnelNameBuffer.ToArray());
                    if (personnelProxy.descId != personnelProxy.DescID)
                    {
                        personnelProxy.descId = personnelProxy.DescID;
                        personnelProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(personnelProxy);
                    }
                }
                else
                {
                    personnelProxy.DescID = (uint) EditorGUILayout.IntField((int) personnelProxy.DescID);
                    if (personnelProxy.descId != personnelProxy.DescID)
                    {
                        personnelProxy.descId = personnelProxy.DescID;
                        personnelProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(personnelProxy);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                personnelProxy.Morale = personnelProxy.morale;
                EditorGUILayout.LabelField(personnelProxy.Morale.ToString("F2"));
            }
            else
            {
                personnelProxy.Morale = EditorGUILayout.FloatField(personnelProxy.Morale);
                if (Mathf.Abs(personnelProxy.Morale - personnelProxy.morale) > Mathf.Epsilon)
                {
                    personnelProxy.morale = personnelProxy.Morale;
                }                
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                personnelProxy.Fitness = personnelProxy.fitness;
                EditorGUILayout.LabelField(personnelProxy.Fitness.ToString("F2"));
            }
            else
            {
                personnelProxy.Fitness = EditorGUILayout.FloatField(personnelProxy.Fitness);
                if (Mathf.Abs(personnelProxy.Fitness - personnelProxy.fitness) > Mathf.Epsilon)
                {
                    personnelProxy.fitness = personnelProxy.Fitness;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                personnelProxy.Pose = personnelProxy.pose;
                EditorGUILayout.LabelField(personnelProxy.Count.ToString());
            }
            else
            {
                personnelProxy.Pose = (uint) EditorGUILayout.IntField((int) personnelProxy.Pose);
                if (personnelProxy.Pose > ComputeShaderEmulator.PERSONNEL_POSE_STANDING)
                {
                    personnelProxy.Pose = ComputeShaderEmulator.PERSONNEL_POSE_STANDING;
                }
                if (personnelProxy.Pose != personnelProxy.pose)
                {
                    personnelProxy.pose = personnelProxy.Pose;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                personnelProxy.Count = personnelProxy.count;
                EditorGUILayout.LabelField(personnelProxy.Count.ToString());
            }
            else
            {
                personnelProxy.Count = (uint) EditorGUILayout.IntField((int) personnelProxy.Count);
                if (personnelProxy.Count != personnelProxy.count)
                {
                    personnelProxy.count = personnelProxy.Count;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(personnelProxy.status.ToString("X8"));
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
