﻿using Types;
using Structs;

public partial class Simulation
{
    public const float FLOAT_EPSILON = 1.19e-07f;
    public const double DOUBLE_EPSILON = .22e-16;

    public const float FLOAT_PI = 3.1415927f;
    public const double DOUBLE_PI = 3.1415926535897931;

    public const float FLOAT_2PI = 2*3.1415927f;
    public const double DOUBLE_2PI = 2*3.1415926535897931;

    public int _lcgCount;
    public RWStructuredBuffer<int> _lcgState;    
}