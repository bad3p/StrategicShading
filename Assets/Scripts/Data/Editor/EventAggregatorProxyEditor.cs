
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EventAggregatorProxy))]
public class EventAggregatorProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EventAggregatorProxy eventAggregatorProxy = target as EventAggregatorProxy;
        
        EntityAssembly entityAssembly = GameObject.FindObjectOfType<EntityAssembly>();

        if (entityAssembly && entityAssembly.GetEntityId(eventAggregatorProxy) == 0)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("eventCount");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("firearmEventIndex");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(eventAggregatorProxy.eventCount.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(eventAggregatorProxy.firearmEventIndex.ToString());
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        if (EditorApplication.isPlaying)
        {
            int eventIndex = eventAggregatorProxy.firearmEventIndex;
            if (eventIndex > 0)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("entityId");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("firepower");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();

                while (eventIndex > 0)
                {
                    uint entityId = ComputeShaderEmulator._firearmEventBuffer[eventIndex].entityId;
                    float firepower = ComputeShaderEmulator._firearmEventBuffer[eventIndex].firepower;
                    eventIndex = ComputeShaderEmulator._firearmEventBuffer[eventIndex].nextIndex;
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(entityId.ToString());
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(firepower.ToString("F2"));
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(eventAggregatorProxy);
            EditorSceneManager.MarkSceneDirty(eventAggregatorProxy.gameObject.scene);
        }
    }
}
