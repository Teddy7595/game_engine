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
            IMesh mesh,
            MaterialComponent material,
            TransformComponent transform,
            Matrix4x4 view,
            Matrix4x4 projection,
            Vector3 lightPosition,
            Vector3 viewPosition,
            Color lightColor
            );
    }
}