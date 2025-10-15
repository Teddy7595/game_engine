using DeepSpace.Domain.Core;
using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Systems
{
    public class RotationSystem : ISystem
    {
        public void Update(World world, float deltaTime)
        {
            //Buscamos todas las entidades con un componente de TransformComponent
            var entities = world.View<TransformComponent>();

            foreach(var entity in entities)
            {
                var transform = world.GetComponent<TransformComponent>(entity);
                if(transform != null)
                {
                    // 1. Obtenemos una copia local de la rotación.
                    var currentRotation = transform.Rotation;

                    // 2. Modificamos la copia.
                    currentRotation.X += deltaTime * 0.5f;
                    currentRotation.Y += deltaTime * 1.0f;

                    // 3. Reasignamos la copia modificada de vuelta a la propiedad.
                    //    Esto ejecuta el 'set' de la propiedad con el nuevo valor.
                    transform.Rotation = currentRotation;
        
                }
            }
        }
    }
}