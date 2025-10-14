using DeepSpaceEngine.Domain.Core;

namespace DeepSpaceEngine.Application.Systems
{
    public interface ISystem
    {
        void Update(World world, float deltaTime);
    }
}