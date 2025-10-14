using DeepSpace.Domain.Core;

namespace DeepSpace.Application.Systems
{
    public interface ISystem
    {
        void Update(World world, float deltaTime);
    }
}