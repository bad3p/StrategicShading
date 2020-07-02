
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
            EditorGUILayout.LabelField("clipAmmo");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("status");
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("timeout");
            EditorGUILayout.EndHorizontal();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("firepower");
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                if (entityAssembly)
                {
                    if (firearmProxy.descId < entityAssembly.FirearmNameBuffer.Length)
                    {
                        EditorGUILayout.LabelField(entityAssembly.FirearmNameBuffer[(int)firearmProxy.descId]);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(firearmProxy.descId.ToString());
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
                    firearmProxy.DescID = (uint)EditorGUILayout.Popup((int)firearmProxy.DescID, entityAssembly.FirearmNameBuffer);
                    if (firearmProxy.descId != firearmProxy.DescID)
                    {
                        firearmProxy.descId = firearmProxy.DescID;
                        var firearmDesc = entityAssembly.FirearmDescBuffer[firearmProxy.descId];
                        firearmProxy.ammo = firearmDesc.maxAmmo;
                        firearmProxy.clipAmmo = firearmDesc.maxClipAmmo;                        
                        EditorUtility.SetDirty(firearmProxy);
                    }
                }
                else
                {
                    firearmProxy.DescID = (uint) EditorGUILayout.IntField((int) firearmProxy.DescID);
                    if (firearmProxy.descId != firearmProxy.DescID)
                    {
                        firearmProxy.descId = firearmProxy.DescID;
                        firearmProxy.ammo = 1;
                        firearmProxy.clipAmmo = 1;
                        EditorUtility.SetDirty(firearmProxy);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(firearmProxy.ammo.ToString());            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(firearmProxy.clipAmmo.ToString());            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            {
                uint status = firearmProxy.status;

                string statusString = "";

                if ((status & ComputeShaderEmulator.FIREARM_READY_BIT) == ComputeShaderEmulator.FIREARM_READY_BIT)
                {
                    statusString += "[RDY]";
                }
                else
                {
                    statusString += "[N/RDY]";
                }

                if ((status & ComputeShaderEmulator.FIREARM_JAMMED_BIT) == ComputeShaderEmulator.FIREARM_JAMMED_BIT)
                {
                    statusString += "[JAM]";
                }
                else
                {
                    statusString += "[N/JAM]";
                }

                switch (status & ~ComputeShaderEmulator.FIREARM_FLAGS_BITMASK)
                {
                    case ComputeShaderEmulator.FIREARM_STATE_MOUNTING:
                        statusString += "[MOUNTING]";
                        break;
                    case ComputeShaderEmulator.FIREARM_STATE_UNMOUNTING:
                        statusString += "[UNMOUNTING]";
                        break;
                    case ComputeShaderEmulator.FIREARM_STATE_AIMING:
                        statusString += "[AIMING]";
                        break;
                    case ComputeShaderEmulator.FIREARM_STATE_RELOADING:
                        statusString += "[RELOADING]";
                        break;
                    case ComputeShaderEmulator.FIREARM_STATE_UNJAMMING:
                        statusString += "[UNJAMMING]";
                        break;
                    default:
                        statusString += "[IDLE]";
                        break;
                }

                EditorGUILayout.LabelField(statusString);
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(firearmProxy.timeout.ToString("F2"));    
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                uint entityId = entityAssembly.GetEntityId(firearmProxy);
                uint targetEntityId = ComputeShaderEmulator._targetingBuffer[entityId].firearmTargetIds.x;
                if (targetEntityId > 0)
                {
                    var entityPosition = ComputeShaderEmulator._transformBuffer[entityId].position;
                    var targetEntityPosition = ComputeShaderEmulator._transformBuffer[targetEntityId].position;
                    float distanceToTarget = ComputeShaderEmulator.distance( entityPosition, targetEntityPosition );
                    float firepower = ComputeShaderEmulator.GetFirearmFirepower(entityId, distanceToTarget);
                    EditorGUILayout.LabelField(firepower.ToString("F2"));
                }
                else
                {
                    EditorGUILayout.LabelField("-");
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
