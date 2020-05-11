
using UnityEngine;

public class BehaviourTree : BehaviourTreeNode
{
    public uint EntityID;

    private BehaviourTreeNode _childNode = null;
    
    #region MonoBehaviour
    void Start()
    {
        Initiate( null as BehaviourTreeNode );
        switch (status)
        {
            case Status.Failure:
                Debug.Log( "[BehaviourTree] \"" + name + "\" failed while initiating." );
                break;
            case Status.Success:
                Debug.Log( "[BehaviourTree] \"" + name + "\" succeeded while initiating." );
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
                    Debug.Log( "[BehaviourTree] \"" + name + "\" failed." );
                    break;
                case Status.Success:
                    Debug.Log( "[BehaviourTree] \"" + name + "\" succeeded." );
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
    
    #region BehaviourTreeNode
    public override void Initiate(BehaviourTreeNode parentNode)
    {
        if (parentNode != null)
        {
            Debug.LogError( "[BehaviourTree] \"" + name + "\" inconsistent Initiate() argument!" );
            status = Status.Failure;
            return;
        }
        
        entityId = EntityID;
        if (entityId == 0)
        {
            Debug.LogError( "[BehaviourTree] \"" + name + "\" failed, entityId is 0!" );
            status = Status.Failure;
            return;
        }

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
            Debug.LogError( "[BehaviourTree] \"" + name + "\" failed to locate child BehaviourTreeNode!" );
            status = Status.Failure;
        }
        else
        {
            _childNode.Initiate( this );
            status = _childNode.status;
        }
    }

    public override void Run()
    {
        if (status == Status.Running)
        {
            _childNode.Run();
            status = _childNode.status;
        }
    }
    #endregion
}