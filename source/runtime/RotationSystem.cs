using Core;
using System;
using System.Numerics;

namespace Runtime
{
    public class RotationSystem : ISystem
    {
        private readonly EntityManager _entityManager;

        //establish a constant rotation speed (radians per second)
        private const float RotationSpeed = (float)(Math.PI / 2); // 90 degrees per second

        public RotationSystem(EntityManager entityManager)
        {
            this._entityManager = entityManager;
        }

        public void Update(float deltaTime)
        {
            var entitiesToRotate = this._entityManager.GetEntitiesWithComponents<RotationComponent, PositionComponent>();
            foreach (var (entity, rotationComp, positionComp) in entitiesToRotate)
            {
                var rotationDelta = Quaternion.CreateFromAxisAngle(Vector3.UnitY, RotationSpeed * deltaTime);

                this._entityManager.MutateComponent<RotationComponent>(entity, rotComponent =>
                {
                    rotComponent.Rotation *= rotationDelta;
                });
            }
        }
    }
}