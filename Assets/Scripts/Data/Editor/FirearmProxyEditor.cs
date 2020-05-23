
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(FirearmProxy))]
public class FirearmProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FirearmProxy firearmProxy = target as FirearmProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(firearmProxy) == 0)
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
            EditorGUILayout.LabelField("ammo");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("stateId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("stateTimeout");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmProxy.DescID = firearmProxy.descId;
                if (entityAssembly)
                {
                    if (firearmProxy.DescID < entityAssembly.FirearmNameBuffer.Count)
                    {
                        EditorGUILayout.LabelField(entityAssembly.FirearmNameBuffer[(int)firearmProxy.DescID]);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(firearmProxy.DescID.ToString());
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(firearmProxy.DescID.ToString());
                }
            }
            else
            {
                if (entityAssembly)
                {
                    firearmProxy.DescID = (uint)EditorGUILayout.Popup((int)firearmProxy.DescID, entityAssembly.FirearmNameBuffer.ToArray());
                    if (firearmProxy.descId != firearmProxy.DescID)
                    {
                        firearmProxy.descId = firearmProxy.DescID;
                        firearmProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(firearmProxy);
                    }
                }
                else
                {
                    firearmProxy.DescID = (uint) EditorGUILayout.IntField((int) firearmProxy.DescID);
                    if (firearmProxy.descId != firearmProxy.DescID)
                    {
                        firearmProxy.descId = firearmProxy.DescID;
                        firearmProxy.ValidateDataConsistency();
                        EditorUtility.SetDirty(firearmProxy);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmProxy.Ammo = firearmProxy.ammo;
                EditorGUILayout.LabelField(firearmProxy.Ammo.ToString());
            }
            else
            {
                firearmProxy.Ammo = (uint) EditorGUILayout.IntField((int) firearmProxy.Ammo);
                if (firearmProxy.ammo != firearmProxy.Ammo)
                {
                    firearmProxy.ammo = firearmProxy.Ammo;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmProxy.StateID = firearmProxy.stateId;
                EditorGUILayout.LabelField(firearmProxy.StateID.ToString());
            }
            else
            {
                firearmProxy.StateID = (uint) EditorGUILayout.IntField((int) firearmProxy.StateID);
                if (firearmProxy.StateID != firearmProxy.stateId)
                {
                    firearmProxy.stateId = firearmProxy.StateID;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(firearmProxy.StateTimeout.ToString("F2"));    
            }
            else
            {
                firearmProxy.StateTimeout = firearmProxy.stateTimeout;
                firearmProxy.StateTimeout = (uint) EditorGUILayout.FloatField(firearmProxy.StateTimeout);
                if (Mathf.Abs(firearmProxy.StateTimeout - firearmProxy.stateTimeout) > Mathf.Epsilon)
                {
                    firearmProxy.stateTimeout = firearmProxy.StateTimeout;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(firearmProxy);
            EditorSceneManager.MarkSceneDirty(firearmProxy.gameObject.scene);
        }
    }
}
