﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRoot : BehaviourTreeNode
{
    private BehaviourTreeNode _childBehaviourTreeNode = null;
    
    #region MonoBehaviour
    void Start()
    {
        Initiate();
        switch (status)
        {
            case Status.Failure:
                Debug.Log( "[BehaviourTree] " + name + " failed while initiating." );
                break;
            case Status.Success:
                Debug.Log( "[BehaviourTree] " + name + " succeeded while initiating." );
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (status == Status.Running)
        {
            Run();
            switch (status)
            {
                case Status.Failure:
                    Debug.Log( "[BehaviourTree] " + name + " failed." );
                    break;
                case Status.Success:
                    Debug.Log( "[BehaviourTree] " + name + " succeeded." );
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
    
    #region BehaviourTreeNode
    public override void Initiate()
    {
        Transform thisTransform = this.transform;
        int thisChildCount = thisTransform.childCount;
        for (int i = 0; i < thisChildCount; i++)
        {
            Transform childTransform = thisTransform.GetChild(i);
            BehaviourTreeNode childBehaviourTreeNode = childTransform.GetComponent<BehaviourTreeNode>();
            if (childBehaviourTreeNode)
            {
                _childBehaviourTreeNode = childBehaviourTreeNode;
                break;
            }
        }

        if (!_childBehaviourTreeNode)
        {
            Debug.LogError( "[BehaviourTreeRoot] \"" + name + "\" failed to locate child BehaviourTreeNode!" );
            status = Status.Failure;
        }
        else
        {
            _childBehaviourTreeNode.Initiate();
            status = _childBehaviourTreeNode.status;
        }
    }

    public override void Run()
    {
        if (status == Status.Running)
        {
            _childBehaviourTreeNode.Run();
            status = _childBehaviourTreeNode.status;
        }
    }
    #endregion
}