using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : BehaviourTreeNode
{
    private int _childNodeIndex = -1;
    private int _childNodeCount = 0;
    private BehaviourTreeNode[] _childNodes = new BehaviourTreeNode[0]; 
    
    #region BehaviourTreeNode
    public override void Initiate()
    {
        Transform thisTransform = this.transform;
        int thisChildCount = thisTransform.childCount;
        
        _childNodes = new BehaviourTreeNode[thisChildCount];
        _childNodeCount = 0;
        _childNodeIndex = -1;
        
        for (int i = 0; i < thisChildCount; i++)
        {
            Transform childTransform = thisTransform.GetChild(i);
            BehaviourTreeNode childBehaviourTreeNode = childTransform.GetComponent<BehaviourTreeNode>();
            if (childBehaviourTreeNode)
            {
                _childNodes[_childNodeCount] = childBehaviourTreeNode;
                _childNodeCount++;
            }
        }

        if (_childNodeCount == 0)
        {
            Debug.LogError( "[Selector] \"" + name + "\" failed to locate child BehaviourTreeNode(s)!" );
            status = Status.Failure;
        }
        else
        {
            _childNodeIndex = 0;

            while (_childNodeIndex < _childNodeCount)
            {
                _childNodes[_childNodeIndex].Initiate();
                status = _childNodes[_childNodeIndex].status;
                if( status == Status.Failure )
                {
                    _childNodeIndex++;
                }
                else
                {
                    break;
                }
            }

            if (status == Status.Failure && _childNodeIndex == _childNodeCount)
            {
                Debug.Log( "[Selector] \"" + name + "\" failed to initiate any child BehaviourTreeNode(s)." );
            }
        }
    }

    public override void Run()
    {
        if (status == Status.Running)
        {
            if (_childNodeIndex < _childNodeCount)
            {
                _childNodes[_childNodeIndex].Run();
                status = _childNodes[_childNodeIndex].status;
                switch (status)
                {
                    case Status.Success:
                        Debug.Log( "[Selector] \"" + name + "\" succeeded." );
                        break;
                    case Status.Failure:
                        _childNodeIndex++;
                        while (_childNodeIndex < _childNodeCount)
                        {
                            _childNodes[_childNodeIndex].Initiate();
                            status = _childNodes[_childNodeIndex].status;
                            if( status == Status.Failure )
                            {
                                _childNodeIndex++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (status == Status.Failure && _childNodeIndex == _childNodeCount)
                        {
                            Debug.Log( "[Selector] \"" + name + "\" failed." );
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Debug.LogError( "[Selector] \"" + name + "\" encountered inconsistent _childNodeIndex!" );
            }
        }
    }
    #endregion
}