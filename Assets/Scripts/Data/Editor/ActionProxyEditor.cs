
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(ActionProxy))]
public class ActionProxyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ActionProxy actionProxy = target as ActionProxy;
        if (!actionProxy)
        {
            return;
        }

        EditorGUILayout.BeginHorizontal();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("entityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("moveTargetEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("moveTargetVector");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("moveTargetValue");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("attackTargetEntityId");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("attackTargetVector");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("attackTargetValue");
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField( actionProxy.entityId.ToString() );
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.MoveTargetEntityID = (uint)EditorGUILayout.IntField((int)actionProxy.MoveTargetEntityID);
            if (actionProxy.moveTargetEntityId != actionProxy.MoveTargetEntityID)
            {
                actionProxy.moveTargetEntityId = actionProxy.MoveTargetEntityID;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            actionProxy.MoveTargetVector = EditorGUILayout.Vector3Field("",actionProxy.MoveTargetVector.ToVector3());
            if (ComputeShaderEmulator.length(actionProxy.MoveTargetVector - actionProxy.moveTargetVector) > Mathf.Epsilon)
            {
                actionProxy.moveTargetVector = actionProxy.MoveTargetVector;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.MoveTargetValue = EditorGUILayout.FloatField( actionProxy.MoveTargetValue );
            if (Mathf.Abs(actionProxy.moveTargetValue - actionProxy.MoveTargetValue) > Mathf.Epsilon)
            {
                actionProxy.moveTargetValue = actionProxy.MoveTargetValue;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.AttackTargetEntityID = (uint)EditorGUILayout.IntField((int)actionProxy.AttackTargetEntityID);
            if (actionProxy.attackTargetEntityId != actionProxy.AttackTargetEntityID)
            {
                actionProxy.attackTargetEntityId = actionProxy.AttackTargetEntityID;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            actionProxy.AttackTargetVector = EditorGUILayout.Vector3Field("",actionProxy.AttackTargetVector.ToVector3());
            if (ComputeShaderEmulator.length(actionProxy.AttackTargetVector - actionProxy.attackTargetVector) > Mathf.Epsilon)
            {
                actionProxy.attackTargetVector = actionProxy.AttackTargetVector;
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            actionProxy.AttackTargetValue = EditorGUILayout.FloatField( actionProxy.AttackTargetValue );
            if (Mathf.Abs(actionProxy.attackTargetValue - actionProxy.AttackTargetValue) > Mathf.Epsilon)
            {
                actionProxy.attackTargetValue = actionProxy.AttackTargetValue;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(actionProxy);
            EditorSceneManager.MarkSceneDirty(actionProxy.gameObject.scene);
        }
    }
}
