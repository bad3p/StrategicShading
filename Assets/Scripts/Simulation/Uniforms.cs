using Types;
using Structs;

public partial class ComputeShaderEmulator
{
    public const uint TRANSFORM = 0x00000001;
    public const uint HIERARCHY = 0x00000002;
    public const uint PERSONNEL = 0x00000004;
    public const uint FIREARMS = 0x00000008;
    public const uint MOVEMENT = 0x00000010;
    public const uint FIREPOWER = 0x00000020;
    public const uint TEAM_BITMASK = 0xC0000000;
    public const uint TEAM_SHIFT = 30;
    
    public const uint PERSONNEL_POSE_BITMASK = 0xC0000000;
    public const uint PERSONNEL_POSE_SHIFT = 30;
    public const uint PERSONNEL_POSE_STANDING = 3;
    public const uint PERSONNEL_POSE_CROUCHING = 2;
    public const uint PERSONNEL_POSE_LAYING = 1;
    public const uint PERSONNEL_POSE_HIDING = 0;
    public const uint MAX_PERSONNEL = 10;
    public const uint PERSONNEL_STATUS_BITMASK = 0x3;
    public const uint PERSONNEL_STATUS_HEALTHY = 3;
    public const uint PERSONNEL_STATUS_WOUNDED = 2;
    public const uint PERSONNEL_STATUS_KILLED = 1;
    public const uint PERSONNEL_STATUS_ABSENT = 0;
    
    public const float PERSONNEL_MORALE_MAX = 600.0f;
    public const float PERSONNEL_MORALE_MIN = 1.0f;
    
    public const float PERSONNEL_FITNESS_MAX = 14400.0f;
    public const float PERSONNEL_FITNESS_MIN = 1.0f;
    
    public const float FLOAT_EPSILON = 1.19e-07f;
    public const double DOUBLE_EPSILON = .22e-16;

    public const float FLOAT_PI = 3.1415927f;
    public const double DOUBLE_PI = 3.1415926535897931;

    public const float FLOAT_2PI = 2*3.1415927f;
    public const double DOUBLE_2PI = 2*3.1415926535897931;

    public static uint _rngMax;
    public static uint _rngCount;
    public static uint _rngStateLength;
    public static uint[] _rngState = new uint[0];
}
