namespace Core
{
    public struct RigidBodyComponent : IComponent
    {
        public System.Numerics.Vector3 Velocity;
        public float Mass;
        public bool IsKinematic;
        public bool UseGravity;
        //public float Drag;
        //public float AngularDrag;
        //public bool FreezeRotation;
        public System.Numerics.Vector3 Acceleration;
        //public System.Numerics.Vector3 AngularVelocity;
    }
}