using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;

public class SegmentIntersection : MonoBehaviour
{
    public Transform A;
    public Transform B;
    public Transform C;
    public Transform D;
    
    private static bool SegmentIntersectionEx(float2 s1, float2 e1, float2 s2, float2 e2, out float2 point)
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

    void OnDrawGizmos()
    {
        if (A && B && C && D)
        {
            Gizmos.DrawLine( A.transform.position, B.transform.position );
            Gizmos.DrawLine( C.transform.position, D.transform.position );
            
            float2 a = new float2( A.transform.position.x, A.transform.position.z );
            float2 b = new float2( B.transform.position.x, B.transform.position.z );
            float2 c = new float2( C.transform.position.x, C.transform.position.z );
            float2 d = new float2( D.transform.position.x, D.transform.position.z );
            
            float2 ip = new float2( 0, 0 );
            
            if( SegmentIntersectionEx(a, b, c, d, out ip) )
            {
                Vector3 ipos = new Vector3( ip.x, 0, ip.y );
                Gizmos.color = Color.red;
                Gizmos.DrawSphere( ipos, 0.025f );
            }
        }
    }
}
