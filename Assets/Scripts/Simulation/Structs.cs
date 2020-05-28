
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
        public float3 linearVelocitySlow; // { crawling, crouching, standing }
        public float3 linearVelocityFast;
        public float angularVelocity;
        public float3 fitnessConsumptionRateSlow;
        public float3 fitnessConsumptionRateFast;
        public float3 fitnessThreshold; // { wind, weakened, exhausted }
        public float3 fitnessRecoveryRate;
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
        public uint status; // [POSE_BITS][PERSONNEL_BITS]...[PERSONNEL_BITS]
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
        public float4 targetVelocityByDistance;
        public float3 deltaPosition;
    };
    
    public struct Firepower
    {
        public uint targetEntityId;
        public uint ammunitionBudget;
    };
}
