using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Interfaces
{
    public interface IRenderer
    {
        void DrawTriangles(TransformComponent transform);
    }
}