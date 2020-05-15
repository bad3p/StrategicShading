
using Types;

namespace Structs
{
    public struct Transform
    {
        public double3 position;
        public float4 rotation;
        public float3 scale;
    };
    
    public struct Hierarchy
    {
        public uint parentEntityId;
        public uint firstChildEntityId;
        public uint nextSiblingEntityId;
    };

    public struct Personnel
    {
        public float morale;
        public float fitness;
        public uint count;
        public uint wounded;
        public uint killed;
    };
    
    public struct Firearms
    {
        public uint ammo;
        public float4 distance;
        public float4 firepower;
        public uint stateId;
        public float stateTimeout;
    };
    
    public struct Movement
    {
        public double3 targetPosition;
        public float4 targetRotation;
        public float targetVelocity;
        public float targetAngularVelocity;
    };
    
    public struct Firepower
    {
        public uint targetEntityId;
        public uint ammunitionBudget;
    };
}
