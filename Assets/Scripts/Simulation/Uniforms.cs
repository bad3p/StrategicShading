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
    
    public const float FLOAT_EPSILON = 1.19e-07f;
    public const double DOUBLE_EPSILON = .22e-16;

    public const float FLOAT_PI = 3.1415927f;
    public const double DOUBLE_PI = 3.1415926535897931;

    public const float FLOAT_2PI = 2*3.1415927f;
    public const double DOUBLE_2PI = 2*3.1415926535897931;

    public static int _rngMax;
    public static int _rngCount;
    public static int _rngStateLength;
    public static int[] _rngState = new int[0];
}
