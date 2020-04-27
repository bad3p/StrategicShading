using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Types;
using UnityEditor;

public class Angle2D : MonoBehaviour
{
    public Transform vPrev;
    public Transform vCurr;
    public Transform vNext;

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
    
    void OnDrawGizmos()
    {
        if (vPrev && vCurr && vNext)
        {
            Gizmos.DrawLine( vPrev.position, vCurr.position );
            Gizmos.DrawLine( vCurr.position, vNext.position );

            float2 vPrev2D = new float2(vPrev.position.x, vPrev.position.z);
            float2 vCurr2D = new float2(vCurr.position.x, vCurr.position.z);
            float2 vNext2D = new float2(vNext.position.x, vNext.position.z);

            float2 vPrevCurr2D = vPrev2D - vCurr2D;
            float2 vNextCurr2D = vNext2D - vCurr2D;

            float mag = Mathf.Sqrt( vPrevCurr2D.x*vPrevCurr2D.x + vPrevCurr2D.y*vPrevCurr2D.y );
            vPrevCurr2D *= 1.0f / mag; 
            
            mag = Mathf.Sqrt( vNextCurr2D.x*vNextCurr2D.x + vNextCurr2D.y*vNextCurr2D.y );
            vNextCurr2D *= 1.0f / mag;

            float angle = GetAngle(vNextCurr2D,vPrevCurr2D);
            angle = angle < 0 ? 360 + angle : angle;
            
            Handles.Label( vCurr.position, angle.ToString("F5") );
        }
    }
}
