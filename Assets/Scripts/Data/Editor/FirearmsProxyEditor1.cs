
using UnityEngine;
using UnityEditor;

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
            EditorGUILayout.LabelField("timeout");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( firearmsProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firearmsProxy.Ammo = (uint)EditorGUILayout.IntField((int)firearmsProxy.Ammo);
            if (firearmsProxy.ammo != firearmsProxy.Ammo)
            {
                firearmsProxy.ammo = firearmsProxy.Ammo;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firearmsProxy.Distance = EditorGUILayout.Vector4Field("",firearmsProxy.Distance.ToVector4());
            if (ComputeShaderEmulator.length(firearmsProxy.Distance - firearmsProxy.distance) > Mathf.Epsilon)
            {
                firearmsProxy.distance = firearmsProxy.Distance;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firearmsProxy.Firepower = EditorGUILayout.Vector4Field("",firearmsProxy.Firepower.ToVector4());
            if (ComputeShaderEmulator.length(firearmsProxy.Firepower - firearmsProxy.firepower) > Mathf.Epsilon)
            {
                firearmsProxy.firepower = firearmsProxy.Firepower;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            firearmsProxy.Timeout = EditorGUILayout.FloatField(firearmsProxy.Timeout);
            if (Mathf.Abs(firearmsProxy.Timeout - firearmsProxy.timeout) > Mathf.Epsilon)
            {
                firearmsProxy.timeout = firearmsProxy.Timeout;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
    }
}
