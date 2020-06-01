
using System;
using Types;
using UnityEngine;
using ECS = ComputeShaderEmulator;

public abstract class BehaviourTreeNode : MonoBehaviour
{
    public enum Status
    {
        Running,
        Success,
        Failure
    }
    
    #region ECSBinding
    private uint _entityId = 0;

    public uint entityId
    {
        get { return _entityId; }
        protected set { _entityId = value; }
    }
    
    protected static uint entityCount { get => ComputeShaderEmulator._entityCount; }
    protected static uint[] descBuffer { get => ComputeShaderEmulator._descBuffer; }
    protected static Structs.Transform[] transformBuffer { get => ComputeShaderEmulator._transformBuffer; }
    protected static Structs.Hierarchy[] hierarchyBuffer { get => ComputeShaderEmulator._hierarchyBuffer; }
    protected static Structs.Personnel[] personnelBuffer { get => ComputeShaderEmulator._personnelBuffer; }
    protected static Structs.Firearm[] FirearmBuffer { get => ComputeShaderEmulator.FirearmBuffer; }
    protected static Structs.Movement[] movementBuffer { get => ComputeShaderEmulator._movementBuffer; }
    protected static Structs.Targeting[] TargetingBuffer { get => ComputeShaderEmulator.TargetingBuffer; }
    
    protected static void ForEveryChildEntity(uint entityId, Action<uint> callback)
    {
        if (ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))
        {
            uint childEntityId = hierarchyBuffer[entityId].firstChildEntityId;
            while (childEntityId > 0)
            {
                callback(childEntityId);
                childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
            }
        }
    } 
    
    protected static uint GetEntityChildCount(uint entityId)
    {
        if (!ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))
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

    protected static uint GetEntityFilteredChildCount(uint entityId, Func<uint,bool> filterCallback)
    {
        if (!ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))
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
            if (filterCallback(childEntityId))
            {
                childCount++;
            }
            childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
        }

        return childCount;
    }
    
    protected static double3 GetCenterOfHierarchy(uint entityId)
    {
        double3 result = new double3(0, 0, 0);
        
        if (!ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))
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
            if( ComputeShaderEmulator.HasComponents(childEntityId,ComputeShaderEmulator.TRANSFORM) )
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
        
        if (!ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))            
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
            if (ComputeShaderEmulator.HasComponents(childEntityId,ComputeShaderEmulator.TRANSFORM))
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

    protected static double GetDistanceFromEntityToLine(uint entityId, double2 p0, double2 p1)
    {
        double ldx = p1.x - p0.x;
        double ldy = p1.y - p0.y;
        double llen = Math.Sqrt( ldx * ldx + ldy * ldy );
        double2 p = transformBuffer[entityId].position.xz;
        double rx = p0.x - p.x;
        double ry = p0.y - p.y;
        return (llen > Mathf.Epsilon) ? (Math.Abs(ldx * ry - ldy * rx) / llen) : (Math.Sqrt(rx*rx + ry*ry));
    }
    
    protected static double GetNearestDistanceFromChildEntityToLine(uint entityId, double2 p0, double2 p1)
    {
        if (!ComputeShaderEmulator.HasComponents(entityId,ComputeShaderEmulator.HIERARCHY))            
        {
            return 0;
        }
        
        uint childEntityId = hierarchyBuffer[entityId].firstChildEntityId;
        if (childEntityId == 0)
        {
            return 0;
        }

        double nearestDistance = float.MaxValue;

        double ldx = p1.x - p0.x;
        double ldy = p1.y - p0.y;
        double llen = Math.Sqrt( ldx * ldx + ldy * ldy );

        while (childEntityId > 0)
        {
            double2 p = transformBuffer[childEntityId].position.xz;
            double rx = p0.x - p.x;
            double ry = p0.y - p.y;
            double distance = (llen > Mathf.Epsilon) ? (Math.Abs(ldx * ry - ldy * rx) / llen) : (Math.Sqrt(rx*rx + ry*ry));
            nearestDistance = Math.Min(nearestDistance, distance);
            childEntityId = hierarchyBuffer[childEntityId].nextSiblingEntityId;
        }

        return nearestDistance;
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