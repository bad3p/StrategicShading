using Types;

namespace Structs
{
    public struct FirearmsData
    {
        public float spread;
        public float3 distance;
        public float3 firepower;
    };

    public struct UnitData
    {
        public int type;
        public int maxPersonnel;
        public int maxAmmo;
        public int maxPrimaryFirearms;
        public int maxSecondaryFirearms;
        public int primaryFirearmsDataID;
        public int secondaryFirearmsDataID;
    };

    public struct Unit
    {
        public int unitDataID;
        public int carrierUnitID;
        public int teamID;
        public int ready;
        public int wounded;
        public int experience;
        public int morale;
        public int fitness;
        public int ammo;
        public int primaryFirearms;
        public int secondaryFirearms;
        public double3 pos;
        public float3 dir;
        public float3 ext;
        public float3 vel;
    };

    public struct Group
    {
        public int teamID;
        public int hqID;
        public int4 subID;
    };    
}
