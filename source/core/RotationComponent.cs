namespace Core
{
    public struct RotationComponent : IComponent
    {
        public float Angle;
        public System.Numerics.Vector3 Axis;
        public System.Numerics.Quaternion Rotation;
    }
}