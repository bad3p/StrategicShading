﻿using Types;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Advance : BehaviourTreeNode
{
    public float FrontlineWidth = 25.0f;
    
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
            
        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2,
            transform.position + transform.right * FrontlineWidth / 2
        );
        
        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 8,
            transform.position + transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth / 8
        );
        
        Gizmos.DrawLine
        (
            transform.position - transform.right * FrontlineWidth / 2,
            transform.position - transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth
        );
        
        Gizmos.DrawLine
        (
            transform.position + transform.right * FrontlineWidth / 2,
            transform.position + transform.right * FrontlineWidth / 2 - transform.forward * FrontlineWidth
        );

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