
using Types;
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
        if ((descBuffer[entityId] & ComputeShaderEmulator.HIERARCHY) != ComputeShaderEmulator.HIERARCHY)
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
    
    protected static double3 GetCenterOfHierarchy(uint entityId)
    {
        double3 result = new double3(0, 0, 0);
        
        if ((descBuffer[entityId] & ComputeShaderEmulator.HIERARCHY) != ComputeShaderEmulator.HIERARCHY)
        {
            return result;
        }
        
        uint childEntityId = hierarchyBuffer[entityId].firstChildEntityId;
        if (childEntityId == 0)
        {
            return result;
        }
        
        uint childCount = 0;
        while (childEntityId > 0)
        {
            if( (descBuffer[childEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM )
            {
                result += transformBuffer[childEntityId].position;
                childCount++;
            }
            else if( hierarchyBuffer[childEntityId].firstChildEntityId > 0 )
            {
                result += GetCenterOfHierarchy(hierarchyBuffer[childEntityId].firstChildEntityId);
                childCount++;
            }

            childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
        }

        if (childCount > 0)
        {
            result = result / childCount;
        }
        
        return result;
    }
    
    protected static uint GetNearestEntityInHierarchy(uint entityId, double3 position)
    {
        uint nearestEntityId = 0;
        float nearestDistance = float.MaxValue;
        
        if ((descBuffer[entityId] & ComputeShaderEmulator.HIERARCHY) != ComputeShaderEmulator.HIERARCHY)
        {
            return nearestEntityId;
        }
        
        uint childEntityId = hierarchyBuffer[entityId].firstChildEntityId;
        if (childEntityId == 0)
        {
            return nearestEntityId;
        }
        
        while (childEntityId > 0)
        {
            if( (descBuffer[childEntityId] & ComputeShaderEmulator.TRANSFORM) == ComputeShaderEmulator.TRANSFORM )
            {
                double3 direction = position - transformBuffer[childEntityId].position;
                float distance = ComputeShaderEmulator.length( direction );
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEntityId = childEntityId;
                }
                
            }
            else if( hierarchyBuffer[childEntityId].firstChildEntityId > 0 )
            {
                uint depthSearchResult = GetNearestEntityInHierarchy( childEntityId, position );
                if (depthSearchResult > 0)
                {
                    double3 direction = position - transformBuffer[depthSearchResult].position;
                    float distance = ComputeShaderEmulator.length( direction );
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestEntityId = depthSearchResult;    
                    }
                }
            }

            childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
        }
        
        return nearestEntityId;
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