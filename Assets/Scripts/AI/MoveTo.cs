using Types;
using UnityEngine;

public class MoveTo : BehaviourTreeNode
{
    public double3 Position = new double3(0,0,0);

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
