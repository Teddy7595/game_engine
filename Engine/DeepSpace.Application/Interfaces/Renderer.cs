using System.Numerics;
using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Interfaces
{
    public interface IRenderer
    {
        void DrawMesh(
            TransformComponent transform,
            Matrix4x4 view,
            Matrix4x4 projection);
    }
}