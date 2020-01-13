
struct FirearmsData
{
    int3 distance;
    int3 firepower;    
};

struct UnitData
{
    int type;
    int maxPersonnel;
    int maxAmmo;
    int maxPrimaryFirearms;
    int maxSecondaryFirearms;
    int primaryFirearmsDataID;
    int secondaryFirearmsDataID;
};

struct Unit
{
    int unitDataID;
    int carrierUnitID;
    int team;
    int ready;
    int wounded;
    int level;
    int experience;
    int morale;
    int fitness;
    int ammo;
    int primaryFirearms;
    int secondaryFirearms;
    float3 pos;
    float3 ext;
    float3 vel;
};

struct Group
{
    int team;
    int hqID;
    int3 subID;
};
