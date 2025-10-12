using Core;

namespace Runtime
{
    public class DefaultEntityManager : IEntityFactory
    {
        private readonly EntityManager _entityManager;

        public DefaultEntityManager(EntityManager entityManager)
        {
            this._entityManager = entityManager;
        }

        public Entity CreateEntity(params IComponent[] initialComponents)
        {
            var entity = this._entityManager.CreateEntity();
            foreach (var component in initialComponents)
            {
                this._entityManager.AddComponent((dynamic)entity, (dynamic)component);
            }

            return entity;
        }

        public Entity CreateRenderizableCube(System.Numerics.Vector3 position, Guid materialId, Guid meshId)
        {
            var entity = this._entityManager.CreateEntity();
            this._entityManager.AddComponent(entity, new PositionComponent { Position = position });
            this._entityManager.AddComponent(entity, new RotationComponent { Rotation = System.Numerics.Quaternion.Identity });
            this._entityManager.AddComponent(entity, new RenderComponent { MaterialId = materialId, MeshId = meshId });
            this._entityManager.AddComponent(entity, new CollisionComponent
            {
                Radius = 0.5f,
                Center = position,
                IsTrigger = false,
                IsStatic = false,
                CollisionMeshId = Guid.Empty
            });
            this._entityManager.AddComponent(entity, new RigidBodyComponent { Mass = 1.0f, IsKinematic = false, UseGravity = true });
            return entity;
        }

    }

}