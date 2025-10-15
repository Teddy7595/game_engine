using DeepSpace.Domain.Core;
using DeepSpace.Domain.Components;
using DeepSpace.Application.Interfaces;
using DeepSpace.Application.Inputs;


namespace DeepSpace.Application.Systems
{
    public class InputSystem : ISystem
    {
        private float _cameraSpeed = 2.5f; // Unidades por segundo
        private readonly IInput _inputManager;

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

            // 2. Comprobar las teclas y modificar la posición.
            var currentPosition = transform.Position;


            // Ahora usamos nuestro propio enum, que es agnóstico a la librería.
            if (_inputManager.IsKeyPressed(EngineKey.W))
                currentPosition.Z -= _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyPressed(EngineKey.S))
                currentPosition.Z += _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyPressed(EngineKey.A))
                currentPosition.X -= _cameraSpeed * deltaTime;
            if (_inputManager.IsKeyPressed(EngineKey.D))
                currentPosition.X += _cameraSpeed * deltaTime;

            // 3. Reasignar la posición modificada.
            transform.Position = currentPosition;
        }
    }
}