
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(FirearmsProxy))]
public class FirearmsProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FirearmsProxy firearmsProxy = target as FirearmsProxy;

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ammo");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("distance");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("firepower");
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
            EditorGUILayout.LabelField( firearmsProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmsProxy.Ammo = firearmsProxy.ammo;
                EditorGUILayout.LabelField(firearmsProxy.Ammo.ToString());
            }
            else
            {
                firearmsProxy.Ammo = (uint) EditorGUILayout.IntField((int) firearmsProxy.Ammo);
                if (firearmsProxy.ammo != firearmsProxy.Ammo)
                {
                    firearmsProxy.ammo = firearmsProxy.Ammo;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmsProxy.Distance = firearmsProxy.distance;
                EditorGUILayout.LabelField(firearmsProxy.Distance.x.ToString("F1") + " " + firearmsProxy.Distance.y.ToString("F1") + " " + firearmsProxy.Distance.z.ToString("F1") + " " + firearmsProxy.Distance.w.ToString("F1"));
            }
            else
            {
                firearmsProxy.Distance = EditorGUILayout.Vector4Field("", firearmsProxy.Distance.ToVector4());
                if (ComputeShaderEmulator.length(firearmsProxy.Distance - firearmsProxy.distance) > Mathf.Epsilon)
                {
                    firearmsProxy.distance = firearmsProxy.Distance;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(firearmsProxy.Firepower.x.ToString("F1") + " " + firearmsProxy.Firepower.y.ToString("F1") + " " + firearmsProxy.Firepower.z.ToString("F1") + " " + firearmsProxy.Firepower.w.ToString("F1"));
            }
            else
            {
                firearmsProxy.Firepower = firearmsProxy.firepower;
                firearmsProxy.Firepower = EditorGUILayout.Vector4Field("", firearmsProxy.Firepower.ToVector4());
                if (ComputeShaderEmulator.length(firearmsProxy.Firepower - firearmsProxy.firepower) > Mathf.Epsilon)
                {
                    firearmsProxy.firepower = firearmsProxy.Firepower;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                firearmsProxy.StateID = firearmsProxy.stateId;
                EditorGUILayout.LabelField(firearmsProxy.StateID.ToString());
            }
            else
            {
                firearmsProxy.StateID = (uint) EditorGUILayout.IntField((int) firearmsProxy.StateID);
                if (firearmsProxy.StateID != firearmsProxy.stateId)
                {
                    firearmsProxy.stateId = firearmsProxy.StateID;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField(firearmsProxy.StateTimeout.ToString("F2"));    
            }
            else
            {
                firearmsProxy.StateTimeout = firearmsProxy.stateTimeout;
                firearmsProxy.StateTimeout = (uint) EditorGUILayout.FloatField(firearmsProxy.StateTimeout);
                if (Mathf.Abs(firearmsProxy.StateTimeout - firearmsProxy.stateTimeout) > Mathf.Epsilon)
                {
                    firearmsProxy.stateTimeout = firearmsProxy.StateTimeout;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(firearmsProxy);
            EditorSceneManager.MarkSceneDirty(firearmsProxy.gameObject.scene);
        }
    }
}
