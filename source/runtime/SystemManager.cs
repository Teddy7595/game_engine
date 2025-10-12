using Core;

namespace Runtime
{
    public class SystemManager
    {
        /// <summary>
        /// List of registered systems
        /// </summary>
        private List<ISystem> _systems = new List<ISystem>();

        public void AddSystem(ISystem system)
        {
            _systems.Add(system);
        }

        public void UpdateAll(float deltaTime)
        {
            foreach (var system in _systems)
            {
                system.Update(deltaTime);
            }
        }
    }
}