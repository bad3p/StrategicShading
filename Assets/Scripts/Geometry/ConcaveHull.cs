#define OPTIMIZED

using System.Collections.Generic;
using Types;
using UnityEngine;

public static partial class Geometry
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
            // debugging purpose : break at the last step
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
