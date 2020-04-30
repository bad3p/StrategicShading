//#define OPTIMIZED

using Types;
using UnityEngine;
using System.Collections.Generic;

public static partial class Geometry
{
    public static float SignedAngle(float2 v1, float2 v2)
    {
        float v1SqrMagnitude = v1.x * v1.x + v1.y * v1.y;
        float v2SqrMagnitude = v2.x * v2.x + v2.y * v2.y;
        
        float num = Mathf.Sqrt( v1SqrMagnitude * v2SqrMagnitude );
        if (num < Mathf.Epsilon)
        {
            return 0.0f;
        }

        float dotv1v2 = v1.x * v2.x + v1.y * v2.y;
        float crossv1v2 = v1.x * v2.y - v1.y * v2.x; 
        float angle = Mathf.Acos( Mathf.Clamp( dotv1v2 / num, -1f, 1f ) ) * 57.29578f;
        
        return angle * Mathf.Sign( crossv1v2 );
    }
    
    private static bool SegmentIntersection(float2 s1, float2 e1, float2 s2, float2 e2, out float2 point)
    {
#if OPTIMIZED
        float a1 = e1.y - s1.y;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.y;
 
        float a2 = e2.y - s2.y;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.y;
 
        float delta = a1 * b2 - a2 * b1;
        float signDelta = delta < 0 ? -1 : ( delta > 0 ? 1 : 0 );
        float absDelta = delta * signDelta;

        if (absDelta < Mathf.Epsilon)
        {
            point = new float2(0,0);
            return false;
        }
        else
        {
            point = new float2((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);

            float s1e1XMin, s1e1XMax;
            float s1e1YMin, s1e1YMax;
            float s2e2XMin, s2e2XMax;
            float s2e2YMin, s2e2YMax;

            if (s1.x < e1.x) { s1e1XMin = s1.x; s1e1XMax = e1.x; } else { s1e1XMin = e1.x; s1e1XMax = s1.x; }
            if (s1.y < e1.y) { s1e1YMin = s1.y; s1e1YMax = e1.y; } else { s1e1YMin = e1.y; s1e1YMax = s1.y; }
            if (s2.x < e2.x) { s2e2XMin = s2.x; s2e2XMax = e2.x; } else { s2e2XMin = e2.x; s2e2XMax = s2.x; }
            if (s2.y < e2.y) { s2e2YMin = s2.y; s2e2YMax = e2.y; } else { s2e2YMin = e2.y; s2e2YMax = s2.y; }

            float infx = s1e1XMin > s2e2XMin ? s1e1XMin : s2e2XMin;
            float infy = s1e1YMin > s2e2YMin ? s1e1YMin : s2e2YMin;
            float supx = s1e1XMax < s2e2XMax ? s1e1XMax : s2e2XMax;
            float supy = s1e1YMax < s2e2YMax ? s1e1YMax : s2e2YMax;
            
            if (infx == supx && infy == supy)
            {
                float2 d = point - new float2(infx, infy);
                float dmag = Mathf.Sqrt(d.x * d.x + d.y * d.y);
                return dmag <= Mathf.Epsilon;
            }
            else if (infx == supx)
            {
                return point.y >= infy && point.y <= supy;
            }
            else if (infy == supy)
            {
                return point.x >= infx && point.x <= supx;
            }
            else
            {
                return point.x >= infx && point.x <= supx && point.y >= infy && point.y <= supy;
            }
        }
#else
        float a1 = e1.y - s1.y;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.y;
 
        float a2 = e2.y - s2.y;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.y;
 
        float delta = a1 * b2 - a2 * b1;

        if (Mathf.Abs(delta) < Mathf.Epsilon)
        {
            point = new float2(0,0);
            return false;
        }
        else
        {
            point = new float2((b2 * c1 - b1 * c2) / delta, (a1 * c2 - a2 * c1) / delta);

            float infx = Mathf.Max(Mathf.Min(s1.x, e1.x), Mathf.Min(s2.x, e2.x));
            float infy = Mathf.Max(Mathf.Min(s1.y, e1.y), Mathf.Min(s2.y, e2.y));
            float supx = Mathf.Min(Mathf.Max(s1.x, e1.x), Mathf.Max(s2.x, e2.x));
            float supy = Mathf.Min(Mathf.Max(s1.y, e1.y), Mathf.Max(s2.y, e2.y));
            
            if (infx == supx && infy == supy)
            {
                float2 d = point - new float2(infx, infy);
                float dmag = Mathf.Sqrt(d.x * d.x + d.y * d.y);
                return dmag <= Mathf.Epsilon;
            }
            else if (infx == supx)
            {
                return point.y >= infy && point.y <= supy;
            }
            else if (infy == supy)
            {
                return point.x >= infx && point.x <= supx;
            }
            else
            {
                return point.x >= infx && point.x <= supx && point.y >= infy && point.y <= supy;
            }
        }
#endif
    }
    
    public static uint CrossingNumber(float2 point, List<float2> hull)
    {
        int hullPointCount = hull.Count;
        
        if (hullPointCount < 2)
        {
            return 0;
        }

        uint crossingNumber = 0;
        
        for( int i=0; i<hullPointCount; i++ )
        {
            int iPlusOne = i+1;
            if( i == hullPointCount-1 ) iPlusOne = 0;

            float2 pi = hull[i];
            float2 piPlusOne = hull[iPlusOne];

            float edgeDistanceY = ( piPlusOne.y - pi.y );
            float edgeDistanceX = ( piPlusOne.x - pi.x );

            if( ( ( pi.y <= point.y ) && ( piPlusOne.y > point.y) ) || 
                ( ( pi.y > point.y ) && ( piPlusOne.y <= point.y) ) ) 
            {
                float vt = ( point.y - pi.y ) / edgeDistanceY;

                if( point.x < pi.x + vt * edgeDistanceX )
                {
                    crossingNumber++;
                }
            }
        }

        return crossingNumber;
    }
    
    public static bool CCW(float2 a, float2 b, float2 c) 
    {
        return ((b.x - a.x) * (c.y - a.y)) > ((b.y - a.y) * (c.x - a.x));
    }
    
    public static float TriangleArea(float2 v0, float2 v1, float2 v2)
    {
        return Mathf.Abs( 0.5f * ( v0.x*v1.y - v1.x*v0.y + v1.x*v2.y - v2.x*v1.y + v2.x*v0.y - v0.x*v2.y ) );
    }    
}
