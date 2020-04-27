
using System.Collections.Generic;
using Types;
using UnityEngine;

public static class ConcaveHull
{
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

    private static bool AlmostEqual(float2 a, float2 b)
    {
        float2 ab = b - a;
        float abLength = Mathf.Sqrt(ab.x * ab.x + ab.y * ab.y);
        return abLength <= Mathf.Epsilon;
    }
    
    private static void GetNearestPointIndices(float2 p, List<float2> pointCloud, ref int[] nearestPointIndices, ref float[] nearestPointDistances)
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
                    if (j < nearestPointDistances.Length - 1)
                    {
                        System.Array.Copy(nearestPointIndices, j, nearestPointIndices, j + 1, nearestPointIndices.Length - j - 1);
                        System.Array.Copy(nearestPointDistances, j, nearestPointDistances, j + 1, nearestPointDistances.Length - j - 1);
                    }

                    nearestPointIndices[j] = i;
                    nearestPointDistances[j] = distance;

                    if (nearestPointCount < nearestPointIndices.Length)
                    {
                        nearestPointCount++;
                    }

                    found = true;
                    break;
                }
            }

            if (!found && nearestPointCount < nearestPointIndices.Length)
            {
                nearestPointIndices[nearestPointCount] = i;
                nearestPointDistances[nearestPointCount] = distance;
                nearestPointCount++;
            }
        }
    }
    
    public static float GetAngle(float2 v1, float2 v2)
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

    private static void SortPointsByAngle(float2 pPrev, float2 p, List<float2> pointCloud, ref int[] nearestPointIndices, ref float[] nearestPointAngles)
    {
        float2 v0 = pPrev - p;
        float v0Mag = Mathf.Sqrt(v0.x * v0.x + v0.y * v0.y);
        if (Mathf.Abs(v0Mag) > Mathf.Epsilon) 
        {
            v0 *= 1.0f / v0Mag;
        }

        for (int i = 0; i < nearestPointIndices.Length; i++)
        {
            float2 pNext = pointCloud[nearestPointIndices[i]];
            float2 v1 = pNext - p;
            float v1Mag = Mathf.Sqrt(v1.x * v1.x + v1.y * v1.y);
            if (Mathf.Abs(v1Mag) > Mathf.Epsilon)
            {
                v1 *= 1.0f / v1Mag;
            }

            nearestPointAngles[i] = GetAngle(v1, v0);
            nearestPointAngles[i] = (nearestPointAngles[i] < 0) ? (360 + nearestPointAngles[i]) : nearestPointAngles[i];
        }

        for (int i = 0; i < nearestPointIndices.Length - 1; i++)
        {
            int maxIndex = i;
            for (int j = i + 1; j < nearestPointIndices.Length; j++)
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
            
            if ( point.x >= infx && 
                 point.x <= supx && 
                 point.y >= infy && 
                 point.y <= supy )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public static List<float2> KNearestPoints(float2 pPrev, float2 p, List<float2> pointCloud, int k)
    {
        List<float2> result = new List<float2>();
        
        int[] nearestPointIndices = new int[k];
        float[] nearestPointDistances = new float[k];

        GetNearestPointIndices(p, pointCloud, ref nearestPointIndices, ref nearestPointDistances);

        SortPointsByAngle(pPrev, p, pointCloud, ref nearestPointIndices, ref nearestPointDistances);

        for (int i = 0; i < nearestPointIndices.Length; i++)
        {
            result.Add( pointCloud[nearestPointIndices[i]]);
        }

        return result;
    }
    
    public static List<float2> KNearestHull(List<float2> pointCloud, int k, int n = int.MaxValue)
    {
        k = Mathf.Max(k, 3);
        
        List<float2> hull = new List<float2>();

        if (pointCloud.Count < 3)
        {
            return hull;
        }
        if (pointCloud.Count == 3)
        {
            hull.AddRange(pointCloud);
            return hull;
        }

        k = Mathf.Min(k, pointCloud.Count - 1);

        int firstPointIndex = GetStartPointIndex(pointCloud);
        float2 firstPoint = pointCloud[firstPointIndex];
        hull.Add(firstPoint);

        float2 currentPoint = firstPoint;
        float2 previousPoint = firstPoint - new float2(1.0f, 0.0f);
        pointCloud.RemoveAt( firstPointIndex );

        int[] nearestPointIndices = new int[k];
        float[] nearestPointDistances = new float[k];
        float2 ip = new float2(0,0);
        bool isIdling = true;
        
        while ( ( !AlmostEqual(firstPoint,currentPoint) || isIdling ) && pointCloud.Count > k && n > 0)
        {
            isIdling = false;

            if (n == 1)
            {
                n = (n+1) / 2 ;
            }
            
            GetNearestPointIndices( currentPoint, pointCloud, ref nearestPointIndices, ref nearestPointDistances );
            SortPointsByAngle( previousPoint, currentPoint, pointCloud, ref nearestPointIndices, ref nearestPointDistances );

            bool found = false;
            for (int i = 0; i < nearestPointIndices.Length; i++)
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
                            isIntersected = isIntersected && !AlmostEqual(currentPoint, ip);
                        }
                        if (isIntersected)
                        {
                            break;
                        }
                    }
                }

                if (!isIntersected)
                {
                    previousPoint = currentPoint;
                    currentPoint = pointCloud[nearestPointIndices[i]];
                    hull.Add(currentPoint);
                    pointCloud.RemoveAt(nearestPointIndices[i]);
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                break;
            }

            n--;
        }
        
        return hull;
    }
}
