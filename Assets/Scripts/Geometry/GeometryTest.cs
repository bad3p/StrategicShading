using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEditor;

public class GeometryTest : MonoBehaviour
{
    public int K = 3;
    public int N = 3;

    List<float2> MakePointCloud()
    {
        Transform[] pointTransforms = GetComponentsInChildren<Transform>();

        List<float2> result = new List<float2>();

        float2 point = new float2();
        foreach (var pointTransform in pointTransforms)
        {
            if (pointTransform != this.transform)
            {
                point.x = pointTransform.position.x;
                point.y = pointTransform.position.z;
                result.Add( point );
            }
        }
        return result;
    }
    
    void OnDrawGizmos()
    {
        List<float2> pointCloud = MakePointCloud();
        List<float2> convexHull = Geometry.ConvexHull(pointCloud);

        Gizmos.color = Color.green;
        
        for (int i = 0; i < convexHull.Count; i++)
        {
            int iPrev = (i == 0) ? (convexHull.Count - 1) : (i - 1);

            Gizmos.DrawLine
            (
                new Vector3( convexHull[iPrev].x, 0, convexHull[iPrev].y ),
                new Vector3( convexHull[i].x, 0, convexHull[i].y )
            );
        }
        
        pointCloud = MakePointCloud(); 

        bool result = false;
        List<float2> concaveHull = Geometry.KNearestHull(pointCloud, K, ref result, N);
        
        Gizmos.color = Color.white;

        for (int i = 0; i < concaveHull.Count; i++)
        {
            int iPrev = (i == 0) ? (concaveHull.Count - 1) : (i - 1);

            Gizmos.DrawLine
            (
                new Vector3( concaveHull[iPrev].x, 0, concaveHull[iPrev].y ),
                new Vector3( concaveHull[i].x, 0, concaveHull[i].y )
            );
        }

        /* 
        for (int j = 0; j < pointCloud.Count; j++)
        {
            uint crossingNumber = ConcaveHull.CrossingNumber(pointCloud[j], concaveHull);

            Handles.Label( new Vector3(pointCloud[j].x,0,pointCloud[j].y), crossingNumber.ToString() );
        }
        */
    }
}
