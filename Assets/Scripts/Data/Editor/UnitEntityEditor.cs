using Types;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitEntity))]
[CanEditMultipleObjects]
public class UnitEntityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UnitEntity unitEntity = target as UnitEntity;
        serializedObject.Update();

        unitEntity.SynchronizeWithChildren();
        
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Synchronize hierarchy"))
        {
            unitEntity.SynchronizeSubHierarchy();
        }

        DrawDefaultInspector();
    }
}
