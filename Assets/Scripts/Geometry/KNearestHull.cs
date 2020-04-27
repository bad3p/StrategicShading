using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class KNearestHull : MonoBehaviour
{
    public int K = 3;
    public int N = 3;
    
    void OnDrawGizmos()
    {
        Transform[] pointTransforms = GetComponentsInChildren<Transform>();

        List<float2> pointCloud = new List<float2>(); 
        foreach (var pointTransform in pointTransforms)
        {
            if (pointTransform != this.transform)
            {
                pointCloud.Add(new float2(pointTransform.position.x, pointTransform.position.z));
            }
        }

        List<float2> concaveHull = ConcaveHull.KNearestHull(pointCloud, K, N);

        for (int i = 1; i < concaveHull.Count; i++)
        {
            int iPrev = (i == 0) ? (concaveHull.Count - 1) : (i - 1);

            Gizmos.DrawLine
            (
                new Vector3( concaveHull[iPrev].x, 0, concaveHull[iPrev].y ),
                new Vector3( concaveHull[i].x, 0, concaveHull[i].y )
            );
        }
    }
}
