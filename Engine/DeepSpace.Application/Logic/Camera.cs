using System.Numerics;
using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Logic
{
    public class Camera
    {
        public Vector3 Front { get; private set; } = -Vector3.UnitZ;
        public Vector3 Right { get; private set; } = Vector3.UnitX;
        public Vector3 Up { get; private set; } = Vector3.UnitY;
        public Matrix4x4 GetViewMatrix(TransformComponent transform)
        {
            var position = transform.Position;
        
            // Calculamos el vector "frontal" de la cámara a partir de los ángulos de Euler
            Vector3 front;
            front.X = MathF.Cos(transform.Rotation.X) * MathF.Cos(transform.Rotation.Y);
            front.Y = MathF.Sin(transform.Rotation.X);
            front.Z = MathF.Cos(transform.Rotation.X) * MathF.Sin(transform.Rotation.Y);
            Front = Vector3.Normalize(front);
            
            // El vector "derecha" y "arriba" se calculan a partir del vector frontal
            Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));

            return Matrix4x4.CreateLookAt(position, position + Front, Up);
        }

        public Matrix4x4 GetProjectionMatrix(CameraComponent camera, float aspectRatio)
        {
            // Crear la matriz de proyección perspectiva
            return Matrix4x4.CreatePerspectiveFieldOfView(
                camera.FieldOfViewRadians,
                aspectRatio,
                camera.NearPlane,
                camera.FarPlane
            );
        }
    }
}