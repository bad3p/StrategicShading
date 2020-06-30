
using Types;

namespace Structs
{
    // GLOBAL
    
    public struct Environment
    {
        public uint date; // [YEAR][MONTH][DAY] 
        public uint time; // [MSEC]
        public float2 wind;
        public float temperature;
        public float precipitation;
        public float fog;
    };
    
    // FLYWEIGHT
    
    [System.Serializable]
    public struct FirearmDesc
    {
        public uint crew;
        public uint maxAmmo;
        public uint maxClipAmmo;
        public uint maxBurstAmmo;
        public float4 distance;
        public float4 firepower;
        public float mountingTime;
        public float aimingTime;
        public float reloadingTime;
        public float unjammingTime;
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
    
    [System.Serializable]
    public struct BuildingDesc
    {
        public float maxIntegrity;
        public float armor;
        public uint material;
    };
    
    // EVENTS
    
    public struct ArrayHeader
    {
        public int capacity;
        public int count;
    };

    public struct Event
    {
        public uint id;
        public uint entityId;
        public float4 param;
        public int nextEventId;
    };
    
    public struct FeedbackEvent
    {
        public uint id;
        public uint entityId0;
        public uint entityId1;
        public float4 param;
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
        public uint joinEntityId;
        public uint indoorEntityId;
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
        public uint clipAmmo;
        public uint status; // [READY_BIT][JAMMED_BIT][STATE_BITS]
        public float timeout;
    };
    
    public struct Movement
    {
        public double3 targetPosition;
        public float4 targetRotation;
        public float4 targetVelocityByDistance;
        public float3 deltaPosition;
    };
    
    public struct Targeting
    {
        public float4 arc;
        public float3 front;
        public uint numEnemies;
        public uint numAllies;
        public uint4 firearmTargetIds;
        public float4 firearmTargetWeights;
    };
    
    public struct EventAggregator
    {
        public int eventCount;
        public int firstEventId;
    };
    
    public struct Building
    {
        public uint descId;
        public float integrity;
    };
}
