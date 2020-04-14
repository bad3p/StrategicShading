using System;
using Types;

namespace Structs
{
    [Serializable]
    public struct FirearmsData
    {
        public float spread;
        public float recoilTime;
        public float reloadingTime; 
        public int maxClipAmmo;
    };

    [Serializable]
    public struct InfantryData
    {
        public int firearmsDataID;
        public int maxBeltAmmo;
        public float maxMorale;
        public float moraleRecoveryRate;
        public float moraleSpreadPenalty;
        public float maxFitness;
        public float fitnessRecoveryRate;
        public float fitnessSpreadPenalty;
        public float4 movementVel;
        public float4 movementCost;
    };
    
    [Serializable]
    public struct Entity
    {
        public int typeID;
        public int instanceID;
        public double3 worldPos;
        public float4 worldRot;
        public float3 extent;
    };

    [Serializable]
    public struct Infantry
    {
        public int entityID;
        public int infantryDataID;
        public float morale;
        public float fitness;
        public int clipAmmo;
        public int beltAmmo;
        public int firearmsActionID;
        public float firearmsActionTime;
    };

    [Serializable]
    public struct Unit
    {
        public int entityID;
        public int parentUnitID;
        public int subTypeID;
        public int subArrayID;
    };
    
    [Serializable]
    public struct Array
    {
        public int capacity;
        public int count;
        public int offset;
    };
}
