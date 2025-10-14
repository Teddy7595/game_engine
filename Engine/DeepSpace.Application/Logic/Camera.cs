using System.Numerics;
using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Logic
{
    public class Camera
    {
        public static Matrix4x4 GetViewMatrix(TransformComponent transform)
        {
            // Crear la matriz de vista basada en la posición y rotación del TransformComponent
            // El vector "up" le dice a la camara cual es la direccion de arriba
            var cameraPosition = transform.Position;
            var cameraTarget = Vector3.Zero; // Mirando hacia el origen
            var cameraUp = Vector3.UnitY; // Arriba en la dirección Y positiva

            return Matrix4x4.CreateLookAt(cameraPosition, cameraTarget, cameraUp);
        }

        public static Matrix4x4 GetProjectionMatrix(CameraComponent camera, float aspectRatio)
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