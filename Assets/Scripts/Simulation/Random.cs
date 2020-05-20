using System;
using Types;
using Structs;

public partial class ComputeShaderEmulator
{
    public static uint rngIndex(uint threadIndex)
    {
        return threadIndex % _rngCount;
    }
    
    public static uint rngMod(uint n) 
    {
        return ((n % _rngMax) + _rngMax) % _rngMax;
    }

    public static uint rng(uint rngIndex)
    {
        uint result = 0;
        uint prevPos = 1;
        uint nextPos = 0;
        uint exchangedPos = 0;

        while( prevPos != exchangedPos )
        {
            prevPos = _rngState[rngIndex*(_rngStateLength+1)];
            nextPos = (prevPos + 1) % _rngStateLength;
            
            // InterlockedCompareExchange( _rngState[rngIndex*(_rngStateLength+1)], prevPos, nextPos, exchangedPos );
            _rngState[rngIndex * (_rngStateLength+1)] = nextPos;
            exchangedPos = prevPos;

            if (exchangedPos == prevPos)
            {
                // int temp = mod(state[(pos + 1) % 55] - state[(pos + 32) % 55]);
                uint p0 = rngIndex * (_rngStateLength+1) + 1; 
                uint p1 = (prevPos + 1) % _rngStateLength;
                uint p2 = (prevPos + _rngStateLength / 2 + _rngStateLength / 11) % _rngStateLength;
                result = rngMod( _rngState[p0+p1] - _rngState[p0+p2] );
                _rngState[p0 + prevPos] = result;
            }
        }
        return result; 
    }

    public static float rngRange(float min, float max, uint rngIndex)
    {
        return min + (max - min) * ((float)( rng( rngIndex ) )) / _rngMax; 
    }

    public static float rngNormal(uint rngIndex)
    {
        return rngRange(0.0f, 1.0f, rngIndex) * rngRange( -1.0f, 1.0f, rngIndex );
    } 
}
