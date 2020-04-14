using Types;
using UnityEditor;

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

        DrawDefaultInspector();
    }
}
