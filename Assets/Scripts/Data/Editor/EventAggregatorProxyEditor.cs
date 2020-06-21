
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
                EditorGUILayout.LabelField("firstEventId");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(eventAggregatorProxy.eventCount.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(eventAggregatorProxy.firstEventId.ToString());
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();

        if (EditorApplication.isPlaying)
        {
            int eventId = eventAggregatorProxy.firstEventId;
            if (eventId > 0)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("entityId");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.LabelField("param");
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndHorizontal();

                while (eventId > 0)
                {
                    uint entityId = ComputeShaderEmulator._eventBuffer[eventId].entityId;
                    var param = ComputeShaderEmulator._eventBuffer[eventId].param;
                    eventId = ComputeShaderEmulator._eventBuffer[eventId].nextEventId;
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(entityId.ToString());
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.LabelField(param.x.ToString("F2") + ", " + param.y.ToString("F2") + ", " + param.z.ToString("F2") + ", " + param.w.ToString("F2"));
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
