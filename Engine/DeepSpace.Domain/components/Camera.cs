namespace DeepSpace.Domain.Components
{

    public class MainCameraComponent : IComponent{}

    public class CameraComponent : IComponent
    {
        public float FieldOfViewRadians { get; set; } = MathF.PI / 4; // 45 grados
        public float AspectRatio { get; set; } = 16.0f / 9.0f;
        public float NearPlane { get; set; } = 0.1f;
        public float FarPlane { get; set; } = 100.0f;
    }
}