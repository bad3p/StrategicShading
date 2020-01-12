using UnityEngine;

namespace Structs
{

public struct FirearmsData
{
    public Vector3Int distance;
    public Vector3Int firepower;

    public const int Stride = 24;
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

    public const int Stride = 28;
};

public struct Unit
{
    public int unitDataID;
    public int carrierUnitID;
    public int ready;
    public int wounded;
    public int level;
    public int experience;
    public int morale;
    public int fitness;
    public int ammo;
    public int primaryFirearms;
    public int secondaryFirearms;
    public Vector3 pos;
    public Vector3 ext;
    public Vector3 vel;

    public const int Stride = 80;
};

public struct Group
{
    public int hqID;
    public Vector3Int subID;

    public const int Stride = 16;
};


}
