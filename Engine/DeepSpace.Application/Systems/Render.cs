using System.Drawing;
using DeepSpace.Application.Interfaces;
using DeepSpace.Application.Logic;
using DeepSpace.Domain.Components;
using DeepSpace.Domain.Core;

namespace DeepSpace.Application.Systems
{
    public class RenderSystem : ISystem
    {
        private readonly IRenderer _renderer;
        private readonly float _aspectRatio;
        private readonly Camera _cameraLogic = new();

        // El sistema RECIBE la implementación del renderizador. No sabe cuál es.
        public RenderSystem(IRenderer renderer, float aspectRatio)
        {
            _renderer = renderer;
            _aspectRatio = aspectRatio;
        }

        /* public void Update(World world, float deltaTime)
        {
            // Obtener todas las entidades con el componente Renderable
            var renderableEntities = world.View<RenderableComponent>();

            foreach (var entity in renderableEntities)
            {
                var transform = world.GetComponent<TransformComponent>(entity);
                if (transform != null)
                {
                    //El sistema solo habla con la interfaz del renderizador mas no con la clase en concreto
                    _renderer.DrawTriangles(transform);
                }
            }
        } */

        public void Update(World world, float deltaTime)
        {
            //Obtenemos la camara activa
            var cameraEntity = world.View<CameraComponent>().FirstOrDefault();
            if (cameraEntity.Id == Guid.Empty) return; // No hay cámara activa

            var cameraTransform = world.GetRequiredComponent<TransformComponent>(cameraEntity);
            var cameraComponent = world.GetRequiredComponent<CameraComponent>(cameraEntity);

            // Configurar la vista de la cámara
            var viewMatrix = _cameraLogic.GetViewMatrix(cameraTransform);
            // Configurar la proyección de la cámara
            var projectionMatrix = _cameraLogic.GetProjectionMatrix(cameraComponent, _aspectRatio);

            var lightEntity = world.View<LightComponent>().FirstOrDefault();
            if (lightEntity.Id == Guid.Empty) return; // No hay luz activa

            var lightTransform = world.GetRequiredComponent<TransformComponent>(lightEntity);
            var lightComponent = world.GetRequiredComponent<LightComponent>(lightEntity);
            //Limpiar la pantalla antes de dibujar
            _renderer.Clear(Color.CornflowerBlue);
            // Obtener todas las entidades con el componente Renderable
            var renderableEntities = world.View<RenderableComponent>();
            foreach (var entity in renderableEntities)
            {
                var transform = world.GetRequiredComponent<TransformComponent>(entity);
                if (transform != null)
                {
                    //El sistema solo habla con la interfaz del renderizador mas no con la clase en concreto
                    _renderer.DrawMesh(
                        transform,
                        viewMatrix,
                        projectionMatrix,
                        lightTransform.Position,
                        cameraTransform.Position,
                        lightComponent.Color,
                        lightComponent.Intensity
                    );
                }
            }
        }
    }
}