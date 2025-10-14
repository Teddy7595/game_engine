namespace DeepSpace.Domain.Components
{
    public class TransformComponent: IComponent
    {
        public System.Numerics.Vector3 Position { get; set; } = new System.Numerics.Vector3(1, 2, 3);
        public System.Numerics.Vector3 Rotation { get; set; }
        public System.Numerics.Vector3 Scale { get; set; } = new System.Numerics.Vector3(1, 1, 1);
    }
}