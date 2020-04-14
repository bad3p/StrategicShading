using Types;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InfantryEntity))]
[CanEditMultipleObjects]
public class InfantryEditor : Editor
{
    private static bool ValidateDataIDs(InfantryEntity infantryEntity, InfantryDataProvider infantryDataProvider, FirearmsDataProvider firearmsDataProvider)
    {
        var infantryDataID = infantryEntity.infantry.infantryDataID;

        if (infantryDataID >= infantryDataProvider.InfantryData.Length || infantryDataID >= infantryDataProvider.InfantryName.Length)
        {
            return false;
        }
        
        var infantryData = infantryDataProvider.InfantryData[infantryEntity.infantry.infantryDataID];
        
        var firearmsDataID = infantryData.firearmsDataID;
        if (firearmsDataID > firearmsDataProvider.FirearmsData.Length || firearmsDataID > firearmsDataProvider.FirearmsName.Length)
        {
            return false;
        }

        return true;
    }

    public override void OnInspectorGUI()
    {
        InfantryEntity infantryEntity = target as InfantryEntity;
        serializedObject.Update();
        
        InfantryDataProvider infantryDataProvider = FindObjectOfType<InfantryDataProvider>();
        FirearmsDataProvider firearmsDataProvider = FindObjectOfType<FirearmsDataProvider>();

        if (infantryDataProvider && firearmsDataProvider && ValidateDataIDs(infantryEntity, infantryDataProvider, firearmsDataProvider) )
        {
            var infantryDataID = infantryEntity.infantry.infantryDataID;
            var infantryData = infantryDataProvider.InfantryData[infantryDataID];
            var firearmsDataID = infantryData.firearmsDataID;
            var firearmsData = firearmsDataProvider.FirearmsData[firearmsDataID];
        
            EditorGUILayout.LabelField( infantryDataProvider.InfantryName[infantryDataID] );
            EditorGUILayout.LabelField( firearmsDataProvider.FirearmsName[firearmsDataID] );
            
            infantryEntity.infantry.morale = infantryData.maxMorale;
            infantryEntity.infantry.fitness = infantryData.maxFitness;
            infantryEntity.infantry.clipAmmo = firearmsData.maxClipAmmo;
            infantryEntity.infantry.beltAmmo = infantryData.maxBeltAmmo;
        }
        else
        {
            EditorGUILayout.LabelField( "Invalid data IDs!" );
        }

        infantryEntity.SynchronizeWithTransform();
        
        serializedObject.ApplyModifiedProperties();

        DrawDefaultInspector();
    }
}
