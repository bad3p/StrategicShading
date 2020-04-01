using System;
using Types;
using Structs;

public partial class Simulation
{
    public static int rngIndex(int threadIndex)
    {
        return threadIndex % _rngCount;
    }
    
    public static int mod(int n) 
    {
        return ((n % _rngMax) + _rngMax) % _rngMax;
    }

    public static int rng(int rngIndex)
    {
        int result = 0;
        int prevPos = 1;
        int nextPos = 0;
        int exchangedPos = 0;

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
                int p0 = rngIndex * (_rngStateLength+1) + 1; 
                int p1 = (prevPos + 1) % _rngStateLength;
                int p2 = (prevPos + _rngStateLength / 2 + _rngStateLength / 11) % _rngStateLength;
                result = mod( _rngState[p0+p1] - _rngState[p0+p2] );
                _rngState[p0 + prevPos] = result;
            }
        }
        return result; 
    }

    public static float rngRange(float min, float max, int rngIndex)
    {
        return min + (max - min) * ((float)( rng( rngIndex ) )) / _rngMax; 
    }

    public static float lcgBoxMuller(float mu, float sigma, int lcgIndex)
    {
        /*
        float u1 = 0.0f;
        float u2 = 0.0f;
    
        while( u1 <= FLOAT_EPSILON )
        {
            u1 = lcgRange( 0.0f, 1.0f, lcgIndex );
            u2 = lcgRange( 0.0f, 1.0f, lcgIndex );
        }
    
        float z0 = sqrt(-2.0f * log(u1)) * cos(FLOAT_2PI * u2);
        float z1 = sqrt(-2.0f * log(u1)) * sin(FLOAT_2PI * u2);
	
        return z0 * sigma + mu;
        */

        return rngRange(0.0f, 1.0f, lcgIndex);
        return rngRange(0.0f, 1.0f, lcgIndex) * rngRange(-1.0f, 1.0f, lcgIndex);
    } 
}
