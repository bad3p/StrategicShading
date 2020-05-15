
using UnityEngine;

public abstract class BehaviourTreeNode : MonoBehaviour
{
    public enum Status
    {
        Running,
        Success,
        Failure
    }
    
    #region ETCBinding
    private uint _entityId = 0;

    public uint entityId
    {
        get { return _entityId; }
        protected set { _entityId = value; }
    }
    
    protected static int entityCount { get => ComputeShaderEmulator._entityCount; }
    protected static uint[] descBuffer { get => ComputeShaderEmulator._descBuffer; }
    protected static Structs.Transform[] transformBuffer { get => ComputeShaderEmulator._transformBuffer; }
    protected static Structs.Hierarchy[] hierarchyBuffer { get => ComputeShaderEmulator._hierarchyBuffer; }
    protected static Structs.Personnel[] personnelBuffer { get => ComputeShaderEmulator._personnelBuffer; }
    protected static Structs.Firearms[] firearmsBuffer { get => ComputeShaderEmulator._firearmsBuffer; }
    protected static Structs.Movement[] movementBuffer { get => ComputeShaderEmulator._movementBuffer; }
    protected static Structs.Firepower[] firepowerBuffer { get => ComputeShaderEmulator._firepowerBuffer; }
    
    protected static uint GetEntityChildCount(uint entityId)
    {
        const uint COMPONENT_MASK = ComputeShaderEmulator.HIERARCHY;
        if ((descBuffer[entityId] & COMPONENT_MASK) != COMPONENT_MASK)
        {
            return 0;
        }
        
        uint childEntityId = hierarchyBuffer[entityId].firstChildEntityId;
        if (childEntityId == 0)
        {
            return 0;
        }

        uint childCount = 0;
        while (childEntityId > 0)
        {
            childCount++;
            childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
        }

        return childCount;
    }
    #endregion
    
    #region Status
    private Status _status = Status.Running;
    public Status status
    {
        get { return _status; }
        protected set { _status = value;  }
    }
    #endregion
    
    #region Abstraction
    public abstract void Initiate(BehaviourTreeNode parentNode);
    public abstract void Run();
    #endregion
}