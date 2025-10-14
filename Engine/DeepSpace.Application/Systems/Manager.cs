
using DeepSpace.Domain.Core;

namespace DeepSpace.Application.Systems
{
    public class SystemManager
    {
        private readonly List<ISystem> _systems = new List<ISystem>();

        public void UpdateAll(World world, float deltaTime)
        {
            foreach (var system in _systems)
            {
                system.Update(world, deltaTime);
            }
        }

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }
    }
}