#define OPTIMIZED

using System.Collections.Generic;
using Types;
using UnityEngine;

public static partial class Geometry
{
    public static List<float2> ConvexHull(List<float2> pointCloud)
    {
        CleanupDuplicatePoints(pointCloud);
        
        pointCloud.Sort( (p1, p2) => ( p1.x < p2.x ) ? (-1) : ( (p1.x > p2.x) ? (1) : (0) ) );
        
        List<float2> hull = new List<float2>();
        
        for(int i=0; i<pointCloud.Count; i++)
        {
            float2 p = pointCloud[i]; 
            while (hull.Count >= 2 && !CCW( hull[hull.Count - 2], hull[hull.Count - 1], p)) 
            {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(p);
        }
        
        int t = hull.Count + 1;
        for (int i = pointCloud.Count - 1; i >= 0; i--) 
        {
            float2 p = pointCloud[i];
            while (hull.Count >= t && !CCW(hull[hull.Count - 2], hull[hull.Count - 1], p)) 
            {
                hull.RemoveAt(hull.Count - 1);
            }
            hull.Add(p);
        }
 
        hull.RemoveAt(hull.Count - 1);
        return hull;
    }
}
