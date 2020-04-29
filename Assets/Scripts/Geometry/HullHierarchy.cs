using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEditor;

public class HullHierarchy : MonoBehaviour
{
    public int K = 3;
    public Color Color = Color.white;

    private int _hullFrame = 0;
    private List<float2> _cloud = new List<float2>();
    private List<float2> _hull = new List<float2>();

    public int rank
    {
        get
        {
            int result = 0;
            
            int childCount = transform.childCount;
            bool hasChildHulls = false;
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                HullHierarchy hullHierarchy = childTransform.GetComponent<HullHierarchy>();
                if (hullHierarchy)
                {
                    result = Mathf.Max(result, hullHierarchy.rank);
                    hasChildHulls = true;
                }
            }

            return result + (hasChildHulls ? 1 : 0);
        }
    }
    
    public List<float2> cloud
    {
        get { return _cloud; }
    }
    
    public List<float2> hull
    {
        get { return _hull; }
    }

    List<float2> MakePointCloud()
    {
        if (rank == 0)
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
                    result.Add(point);
                }
            }

            return result;
        }
        else
        {
            List<float2> result = new List<float2>();

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                HullHierarchy hullHierarchy = childTransform.GetComponent<HullHierarchy>();
                if (hullHierarchy)
                {
                    hullHierarchy.UpdateHull();
                    result.AddRange( hullHierarchy.hull );
                }
            }

            return result;
        }
    }

    void UpdateHull()
    {
        if (_hullFrame != Time.frameCount)
        {
            _hullFrame = Time.frameCount;
            _cloud = MakePointCloud();
            if (_cloud.Count > 0)
            {
                List<float2> tempCloud = new List<float2>();
                tempCloud.AddRange( _cloud );
                
                if (rank > 1)
                {
                    //bool result = false;
                    //_hull = Geometry.KNearestHull(tempCloud, K, ref result);
                    _hull = Geometry.ConvexHull(tempCloud);
                }
                else
                {
                    _hull = Geometry.ConvexHull(tempCloud);
                }
            }
            else
            {
                _hull.Clear();
            }
        }
    }
    
    void OnDrawGizmos()
    {
        UpdateHull();

        Gizmos.color = this.Color;
        
        for (int i = 0; i < _hull.Count; i++)
        {
            int iPrev = (i == 0) ? (_hull.Count - 1) : (i - 1);

            Gizmos.DrawLine
            (
                new Vector3( _hull[iPrev].x, 0, _hull[iPrev].y ),
                new Vector3( _hull[i].x, 0, _hull[i].y )
            );
        }
    }
}
