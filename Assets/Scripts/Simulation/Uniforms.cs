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
    
    public const uint PERSONNEL_EXPOSURE_BITMASK = 0xC0000000;
    public const uint PERSONNEL_EXPOSURE_SHIFT = 30;
    public const uint PERSONNEL_EXPOSURE_STAND = 3;
    public const uint PERSONNEL_EXPOSURE_CROUCH = 2;
    public const uint PERSONNEL_EXPOSURE_CRAWL = 1;
    public const uint PERSONNEL_EXPOSURE_HIDE = 0;
    public const uint MAX_PERSONNEL = 10;
    public const uint PERSONNEL_STATUS_BITMASK = 0x3;
    public const uint PERSONNEL_STATUS_HEALTHY = 3;
    public const uint PERSONNEL_STATUS_WOUNDED = 2;
    public const uint PERSONNEL_STATUS_KILLED = 1;
    public const uint PERSONNEL_STATUS_ABSENT = 0;
    
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
