using System;
using Types;
using Structs;

public partial class Simulation
{
    public const int RAND_MAX_32 = (int)((1U << 31) - 1);
    public const int RAND_MAX = (int)((1U << 15) - 1);

    int lcgIndex(int threadIndex)
    {
        return threadIndex % _lcgCount;
    }

    int lcg(int lcgIndex)
    {
        int result = 0;
        int prev = 1;
        int next = 1;

        while( result != prev )
        {
            prev = _lcgState[lcgIndex];
            next = ((prev * 214013 + 2531011) & RAND_MAX_32) >> 16;
            _lcgState[lcgIndex] = next;
            result = prev;
        }
        return next; 
    }

    float lcgRange(float min, float max, int lcgIndex)
    {
        return min + (max - min) * ((float)( lcg( lcgIndex ) )) / RAND_MAX; 
    }

    float lcgBoxMuller(float mu, float sigma, int lcgIndex)
    {
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
    } 
}
