namespace Core
{
    public struct CollisionComponent : IComponent
    {
        public float Radius;
        public System.Numerics.Vector3 Center;
        public Guid CollisionMeshId;
        public bool IsTrigger;
        public bool IsStatic;
    }
}