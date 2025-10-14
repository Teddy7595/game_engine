using DeepSpaceEngine.Domain.Core;
using DeepSpaceEngine.Domain.Components;

namespace DeepSpaceEngine.Application.Systems
{
    public class DebugLogSystem : ISystem
    {
        public void Update(World world, float deltaTime)
        {
            //le pedimos al world que nos devuelva todos los entities que tengan un componente de TagComponent
            var taggedEntities = world.View<TagComponent>();

            //iteramos sobre los entities que tienen un TagComponent
            foreach (var entity in taggedEntities)
            {
                //obtenemos el TransformComponent del entity y su TagComponent
                var transform = world.GetComponent<TransformComponent>(entity);
                //como sabemos que el entity tiene un TagComponent, podemos obtenerlo directamente
                var tag = world.GetComponent<TagComponent>(entity);

                 // 4. Si tiene ambos componentes, imprimimos su información.
                if (transform != null && tag != null)
                    System.Diagnostics.Debug.WriteLine($"Entity '{tag.Tag}' is at Position(X:{transform.Position.X}, Y:{transform.Position.Y}, Z:{transform.Position.Z})");

            }
        }
    }
}