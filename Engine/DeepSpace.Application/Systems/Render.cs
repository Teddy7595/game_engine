using DeepSpace.Application.Interfaces;
using DeepSpace.Domain.Components;
using DeepSpace.Domain.Core;

namespace DeepSpace.Application.Systems
{
    public class RenderSystem : ISystem
    {
        private readonly IRenderer _renderer;

        // El sistema RECIBE la implementación del renderizador. No sabe cuál es.
        public RenderSystem(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public void Update(World world, float deltaTime)
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
        }
    }
}