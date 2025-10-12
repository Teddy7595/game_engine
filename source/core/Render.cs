using System.Numerics;

namespace Core
{
    public interface IRender
    {
        void Initialize(int width, int height, string title);
        void BeginDraw();
        void EndDraw();
        void Clear(Vector4 color);
        void DrawMesh(Guid meshId, Guid materialId, System.Numerics.Matrix4x4 transform);
        bool IsRunning { get; }
    }
}