
using Types;

namespace Structs
{
    // FLYWEIGHT
    
    [System.Serializable]
    public struct FirearmDesc
    {
        public uint maxAmmo;
        public float4 distance;
        public float4 firepower;
        public float aimingTime;
        public float reloadingTime;
    };
    
    [System.Serializable]
    public struct PersonnelDesc
    {
        public uint maxPersonnel;
        public float3 linearVelocity; // { crawl, walk, run }
        public float angularVelocity;
        public float3 fitnessConsumptionRate;
        public float fitnessRecoveryRate;
        public float4 moraleThreshold; // { shaken, pinned, panic, broken }
        public float4 moraleRecoveryRate;
    };
    
    // COMPONENTS
    
    public struct Transform
    {
        public double3 position;
        public float4 rotation;
        public float3 scale;
    };
    
    public struct Hierarchy
    {
        public uint parentEntityId;
        public uint firstChildEntityId;
        public uint nextSiblingEntityId;
    };

    public struct Personnel
    {
        public uint descId;
        public float morale;
        public float fitness;
        public uint status; // [EXPOSURE_BITS][PERSONNEL_BITS]
    };
    
    public struct Firearm
    {
        public uint descId;
        public uint ammo;
        public uint stateId;
        public float stateTimeout;
    };
    
    public struct Movement
    {
        public double3 targetPosition;
        public float4 targetRotation;
        public float targetVelocity;
        public float targetAngularVelocity;
    };
    
    public struct Firepower
    {
        public uint targetEntityId;
        public uint ammunitionBudget;
    };
}
