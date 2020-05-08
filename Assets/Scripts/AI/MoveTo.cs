﻿using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MoveTo : BehaviourTreeNode
{
    public float Radius = 25.0f;
    
    #region MonoBehaviour
    void OnDrawGizmos()
    {
#if UNITY_EDITOR        
        BehaviourTree behaviourTree = GetComponentInParent<BehaviourTree>();
        if (!behaviourTree)
        {
            return;
        }
        if (!behaviourTree.EntityProxy)
        {
            return;
        }
        if (behaviourTree.EntityProxy.entityId == 0)
        {
            return;
        }
        
        Gizmos.color = behaviourTree.EntityProxy.GetTeamColor();
        Gizmos.matrix = Matrix4x4.Translate( transform.position );
            
        const int NumPoints = 32;
        for (int i = 0; i < NumPoints; i++)
        {
            float j = (i > 0) ? (i - 1) : (NumPoints - 1);

            float angleI = i * 360.0f / NumPoints * Mathf.Deg2Rad;
            float angleJ = j * 360.0f / NumPoints * Mathf.Deg2Rad;
                
            Gizmos.DrawLine
            (
                new Vector3( Mathf.Cos(angleI), 0, Mathf.Sin(angleI) ) * Radius,
                new Vector3( Mathf.Cos(angleJ), 0, Mathf.Sin(angleJ) ) * Radius
            );
        }

        if (EditorApplication.isPlaying)
        {
        }
        else
        {
        }
#endif
    }
    #endregion
    
    #region BehaviourTreeNode
    public override void Initiate(BehaviourTreeNode parentNode)
    {
        entityId = parentNode.entityId;
        status = Status.Running;
    }

    public override void Run()
    {
    }
    #endregion
}
