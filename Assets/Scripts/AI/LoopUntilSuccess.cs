using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopUntilSuccess : BehaviourTreeNode
{
    private BehaviourTreeNode _childNode = null; 
    
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
                _childNode = childBehaviourTreeNode;
                break;
            }
        }

        if (!_childNode)
        {
            Debug.LogError( "[LoopUntilSuccess] \"" + name + "\" failed to locate child BehaviourTreeNode!" );
            status = Status.Failure;
        }
        else
        {
            _childNode.Initiate();
            status = Status.Running;
        }
    }

    public override void Run()
    {
        if (status != Status.Success)
        {
            switch (_childNode.status)
            {
            case Status.Running:
                _childNode.Run();
                break;
            case Status.Failure:
                _childNode.Initiate();
                break;
            default:
                status = Status.Success;
                Debug.Log( "[LoopUntilSuccess] \"" + name + "\" succeeded." );
                break;
            }
        }
    }
    #endregion
}