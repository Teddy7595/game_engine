using DeepSpace.Domain.Core;
using DeepSpace.Domain.Components;
using DeepSpace.Application.Interfaces;
using DeepSpace.Application.Inputs;
using DeepSpace.Application.Logic;


namespace DeepSpace.Application.Systems
{
    public class InputSystem : ISystem
    {
        private float _cameraSpeed = 2.5f; // Unidades por segundo
        private readonly IInput _inputManager;
        private readonly Camera _cameraLogic = new();

        public InputSystem(IInput input)
        {
            _inputManager = input;
        }

        public void Update(World world, float deltaTime)
        {
            // 1. Encontrar la entidad de la cámara principal.
            var cameraEntity = world.View<MainCameraComponent>().FirstOrDefault();
            if (cameraEntity.Id == Guid.Empty) return; // No hay cámara que mover

            var transform = world.GetComponent<TransformComponent>(cameraEntity);
            if (transform == null) return;

            //Obtenemos el delta del raton desde el input manager
            var mouseDelta = _inputManager.GetMouseDelta();
            //convertimo el movimiento del raton en rotacion de la camara
            var currentRotation = transform.Rotation;
            // Ajusta la sensibilidad del ratón según sea necesario
            float mouseSensitivity = 0.1f;
            //La rotación en Y (Yaw) viene del movimiento X del ratón
            currentRotation.Y += mouseDelta.X * mouseSensitivity * deltaTime;
            //La rotación en X (Pitch) viene del movimiento Y del ratón
            currentRotation.X -= mouseDelta.Y * mouseSensitivity * deltaTime;
            // Limitar la rotación en X para evitar que la cámara se voltee
            currentRotation.X = Math.Clamp(currentRotation.X, -MathF.PI / 2.0f + 0.01f, MathF.PI / 2.0f - 0.01f);
            transform.Rotation = currentRotation;

            // --- Lógica de Movimiento (Teclado) - Actualizada ---
            // Primero, llamamos a GetViewMatrix para que se calculen los vectores actualizados
            _cameraLogic.GetViewMatrix(transform);
            //Obtenemos la posicion del momento
            var currentPosition = transform.Position;

            // CORRECCIÓN: Usamos _cameraSpeed y deltaTime, no los enums de las teclas
            if (_inputManager.IsKeyDown(EngineKey.W))
                currentPosition += _cameraLogic.Front * _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyDown(EngineKey.S))
                currentPosition -= _cameraLogic.Front * _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyDown(EngineKey.A))
                currentPosition -= _cameraLogic.Right * _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyDown(EngineKey.D))
                currentPosition += _cameraLogic.Right * _cameraSpeed * deltaTime;

            // CORRECCIÓN: Descomentamos la línea para que la posición se actualice
            transform.Position = currentPosition;
        }
    }
}