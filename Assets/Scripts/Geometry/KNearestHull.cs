using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEditor;

public class KNearestHull : MonoBehaviour
{
    public int K = 3;
    public int N = 3;
    
    void OnDrawGizmos()
    {
        Transform[] pointTransforms = GetComponentsInChildren<Transform>();

        List<float2> pointCloud = new List<float2>(); 
        
        bool result = false;
        List<float2> concaveHull = new List<float2>();

        int k = K;
        while (!result )
        {
            pointCloud.Clear();
            foreach (var pointTransform in pointTransforms)
            {
                if (pointTransform != this.transform)
                {
                    pointCloud.Add(new float2(pointTransform.position.x, pointTransform.position.z));
                }
            }

            if (k == pointCloud.Count)
            {
                break;
            }
            
            concaveHull = ConcaveHull.KNearestHull(pointCloud, k, ref result, N);
            if (!result)
            {
                k++;
            }
        }

        for (int i = 1; i < concaveHull.Count; i++)
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
