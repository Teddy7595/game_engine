using System.Drawing;
using System.Numerics;
using DeepSpace.Domain.Components;

namespace DeepSpace.Application.Interfaces
{
    public interface IRenderer
    {
        void Load();
        void Unload();
        void Clear(Color color);
        void DrawMesh(
            TransformComponent transform,
            Matrix4x4 view,
            Matrix4x4 projection,
            Vector3 lightPosition,
            Vector3 viewPosition,
            Color lightColor,
            float lightIntensity);
    }
}