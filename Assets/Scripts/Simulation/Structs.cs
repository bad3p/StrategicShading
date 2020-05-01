
using Types;

namespace Structs
{
    public struct Entity
    {
        public uint teamId;
        public uint transformId;
        public uint hierarchyId;
        public uint personnelId;
        public uint firearmsId;
        public uint actionId;
    };

    public struct Transform
    {
        public uint entityId;
        public double3 position;
        public float4 rotation;
        public float3 scale;
    };
    
    public struct Hierarchy
    {
        public uint entityId;
        public uint parentEntityId;
        public uint firstChildEntityId;
        public uint nextSiblingEntityId;
    };

    public struct Personnel
    {
        public uint entityId;
        public float morale;
        public float fitness;
        public uint count;
        public uint wounded;
        public uint killed;
    };
    
    public struct Firearms
    {
        public uint entityId;
        public uint ammo;
        public float4 distance;
        public float4 firepower;
        public uint stateId;
        public float stateTimeout;
    };
    
    public struct Action
    {
        public uint entityId;
        public uint actionId;
        public uint targetEntityId;
        public double3 targetPosition;
        public float actionTimeout;
    };
}
