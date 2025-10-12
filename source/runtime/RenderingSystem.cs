using System.Numerics;
using Core;

namespace Runtime
{
    public class RenderingSystem : ISystem
    {
        private readonly EntityManager _entityManager;
        private readonly IRender _renderer;

        public RenderingSystem(EntityManager entityManager, IRender renderer)
        {
            _entityManager = entityManager;
            _renderer = renderer;
        }

        public void Update(float deltaTime)
        {
            this._renderer.Clear(new System.Numerics.Vector4(0.1f, 0.1f, 0.1f, 1.0f));

            foreach (var (entity, position, renderable) in this._entityManager.GetEntitiesWithComponents<PositionComponent, RenderComponent>())
            {
                Matrix4x4 model = Matrix4x4.CreateTranslation(position.Position);
                this._renderer.DrawMesh(renderable.MeshId, renderable.MaterialId, model);
            }
        }
    }
}