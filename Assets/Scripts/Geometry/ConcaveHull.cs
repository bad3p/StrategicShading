#define OPTIMIZED

using System.Collections.Generic;
using Types;
using UnityEngine;

public static class ConcaveHull
{
    private static void CleanupDuplicatePoints(List<float2> pointCloud)
    {
        for (int i = pointCloud.Count - 1; i >= 0; i--)
        {
            bool isDuplicate = false;
            for (int j = i - 1; j >= 0; j--)
            {
                float2 delta = pointCloud[i] - pointCloud[j];
                float distance = Mathf.Sqrt(delta.x * delta.x + delta.y * delta.y);
                if (distance <= Mathf.Epsilon)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (isDuplicate)
            {
                pointCloud.RemoveAt(i);
            }
        }
    }
    
    private static int GetStartPointIndex(List<float2> pointCloud)
    {
        int result = 0;
        for (int i = 1; i < pointCloud.Count; i++)
        {
            if (pointCloud[i].y < pointCloud[result].y)
            {
                result = i;
            }
        }
        return result;
    }
    
    private static void GetNearestPointIndices(float2 p, List<float2> pointCloud, int k, ref int[] nearestPointIndices, ref float[] nearestPointDistances)
    {
        int nearestPointCount = 0;
        for (int i = 0; i < pointCloud.Count; i++)
        {
            float2 p1 = pointCloud[i];
            float2 p0p1 = p1 - p;
            float distance = Mathf.Sqrt(p0p1.x * p0p1.x + p0p1.y * p0p1.y);

            bool found = false;
            for (int j = 0; j < nearestPointCount; j++)
            {
                if (nearestPointDistances[j] > distance)
                {
                    if (j < k - 1)
                    {
                        System.Array.Copy(nearestPointIndices, j, nearestPointIndices, j + 1, k - j - 1);
                        System.Array.Copy(nearestPointDistances, j, nearestPointDistances, j + 1, k - j - 1);
                    }

                    nearestPointIndices[j] = i;
                    nearestPointDistances[j] = distance;

                    if (nearestPointCount < k )
                    {
                        nearestPointCount++;
                    }

                    found = true;
                    break;
                }
            }

            if (!found && nearestPointCount < k)
            {
                nearestPointIndices[nearestPointCount] = i;
                nearestPointDistances[nearestPointCount] = distance;
                nearestPointCount++;
            }
        }
    }
    
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

    private static void SortPointsByAngle(float2 pPrev, float2 p, List<float2> pointCloud, int k, ref int[] nearestPointIndices, ref float[] nearestPointAngles)
    {
        float2 v0 = pPrev - p;
        float v0Mag = Mathf.Sqrt(v0.x * v0.x + v0.y * v0.y);
        if (Mathf.Abs(v0Mag) > Mathf.Epsilon) 
        {
            v0 *= 1.0f / v0Mag;
        }

        for (int i = 0; i < k; i++)
        {
            float2 pNext = pointCloud[nearestPointIndices[i]];
            float2 v1 = pNext - p;
            float v1Mag = Mathf.Sqrt(v1.x * v1.x + v1.y * v1.y);
            if (Mathf.Abs(v1Mag) > Mathf.Epsilon)
            {
                v1 *= 1.0f / v1Mag;
            }

            nearestPointAngles[i] = SignedAngle(v1, v0);
            nearestPointAngles[i] = (nearestPointAngles[i] < 0) ? (360 + nearestPointAngles[i]) : nearestPointAngles[i];
        }

        for (int i = 0; i < k - 1; i++)
        {
            int maxIndex = i;
            for (int j = i + 1; j < k; j++)
            {
                if (nearestPointAngles[maxIndex] < nearestPointAngles[j])
                {
                    maxIndex = j;
                }
            }

            if (maxIndex != i)
            {
                int tempIndex = nearestPointIndices[maxIndex];
                float tempAngle = nearestPointAngles[maxIndex];

                nearestPointIndices[maxIndex] = nearestPointIndices[i];
                nearestPointAngles[maxIndex] = nearestPointAngles[i];

                nearestPointIndices[i] = tempIndex;
                nearestPointAngles[i] = tempAngle;
            }
        }
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

    private static int FindNextHullPointIndex(float2 currentPoint, float2 previousPoint, List<float2> pointCloud, List<float2> hull, int k, ref int[] nearestPointIndices, ref float[] nearestPointDistances)
    {
        GetNearestPointIndices( currentPoint, pointCloud, k, ref nearestPointIndices, ref nearestPointDistances );
        SortPointsByAngle( previousPoint, currentPoint, pointCloud, k, ref nearestPointIndices, ref nearestPointDistances );

        float2 ip = new float2(0,0);
        for (int i = 0; i < k; i++)
        {
            bool isIntersected = false;
            if (hull.Count > 1)
            {
                for (int j = 1; j < hull.Count-1; j++)
                {
                    int jPrev = (j - 1);
                    isIntersected = SegmentIntersection(currentPoint, pointCloud[nearestPointIndices[i]], hull[jPrev], hull[j], out ip);
                    if (isIntersected)
                    {
                        break;
                    }
                }
            }

            if (!isIntersected)
            {
                return nearestPointIndices[i];
            }
        }
        return -1;
    }
    
    public static List<float2> KNearestHull(List<float2> pointCloud, int k, ref bool result, int n = int.MaxValue)
    {
        k = Mathf.Max(k, 3);
        
        List<float2> hull = new List<float2>();

        CleanupDuplicatePoints( pointCloud );

        if (pointCloud.Count < 3)
        {
            result = false;
            return hull;
        }
        
        if (pointCloud.Count == 3)
        {
            result = true;
            hull.AddRange(pointCloud);
            return hull;
        }

        k = Mathf.Min(k, pointCloud.Count - 1);

        int firstPointIndex = GetStartPointIndex(pointCloud);
        float2 firstPoint = pointCloud[firstPointIndex];
        hull.Add(firstPoint);

        float2 currentPoint = pointCloud[firstPointIndex];
        float2 previousPoint = firstPoint - new float2(1.0f, 0.0f);
        pointCloud.RemoveAt( firstPointIndex );

        int[] nearestPointIndices = new int[pointCloud.Count];
        float[] nearestPointDistances = new float[pointCloud.Count];

        result = false;
        while ( pointCloud.Count > k && n > 0 )
        {
            if (n == 1)
            {
                n = n * 2 - 1;
            }
            
            int kk = k;
            while (kk < pointCloud.Count)
            {
                int nextHullPointIndex = FindNextHullPointIndex(currentPoint, previousPoint, pointCloud, hull, kk, ref nearestPointIndices, ref nearestPointDistances);
                if (nextHullPointIndex != -1)
                {
                    previousPoint = currentPoint;
                    currentPoint = pointCloud[nextHullPointIndex];
                    hull.Add(currentPoint);
                    pointCloud.RemoveAt(nextHullPointIndex);
                    break;
                }
                kk++;
            }

            if( kk == pointCloud.Count )
            {
                result = true;
                break;
            }

            bool allPointsInsideHull = true;
            for (int i = 0; i < pointCloud.Count; i++)
            {
                uint crossingNumber = CrossingNumber(pointCloud[i], hull);
                if ( (crossingNumber & 1) == 0 )
                {
                    allPointsInsideHull = false;
                    break;
                }
            }

            if (allPointsInsideHull)
            {
                if (pointCloud.Count > k && hull.Count > 1 )
                {
                    GetNearestPointIndices( hull[0], pointCloud, k, ref nearestPointIndices, ref nearestPointDistances );

                    int nearestPointIndex = 0; 
                    float2 nearestPoint = pointCloud[nearestPointIndices[nearestPointIndex]];
                    float2 nearestVector = nearestPoint - hull[0];
                    float nearestDistance = Mathf.Sqrt( nearestVector.x * nearestVector.x + nearestVector.y * nearestVector.y );

                    for (int i = 1; i < k; i++)
                    {
                        float2 neighbourPoint = pointCloud[nearestPointIndices[i]];
                        float2 neighbourVector = neighbourPoint - hull[0];
                        float neighbourDistance = Mathf.Sqrt( neighbourVector.x * neighbourVector.x + neighbourVector.y * neighbourVector.y );
                        if (neighbourDistance < nearestDistance)
                        {
                            nearestPointIndex = i; 
                            nearestPoint = neighbourPoint;
                            nearestVector = neighbourVector;
                            nearestDistance = neighbourDistance;        
                        }
                    }

                    float2 closingVector = hull[0] - hull[hull.Count - 1];
                    float closingDistance = Mathf.Sqrt( closingVector.x * closingVector.x + closingVector.y * closingVector.y );
                    if (closingDistance <= nearestDistance * 2)
                    {
                        result = true;
                        break;    
                    }
                }
                else
                {
                    result = true;
                    break;
                }
            }
            
            n--;
        }

        if (n == 0)
        {
            result = true;
        }
        
        return hull;
    }
}
