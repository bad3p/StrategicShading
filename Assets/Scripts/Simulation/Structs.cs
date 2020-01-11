
using UnityEngine;

public struct FirepowerData
{
    public Vector3Int distance;
    public Vector3Int firepower;
    
    public const int SizeOf = 12*3; // ComputeShader stride
}

public struct UnitData
{
    public int type;
    public int maxPersonnel;
    public int firepowerDataID;
    public int maxAmmo;

    public const int SizeOf = 4*4; // ComputeShader stride
};

public struct Unit
{
    public int unitDataID;
    public int parentUnitID;
    public int ready;
    public int wounded;
    public int level;
    public int experience;
    public int morale;
    public int fitness;
    public int ammo;
    public Vector3 pos;
    public Vector3 ext;

    public const int SizeOf = 4*9+12*2; // ComputeShader stride
};

public struct Group
{
    public int hqID;
    public Vector3Int subID;
    
    public const int SizeOf = 4*1+12*1; // ComputeShader stride
};