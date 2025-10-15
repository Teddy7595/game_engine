using System.Numerics;
using DeepSpace.Application.Inputs;

namespace DeepSpace.Application.Interfaces
{
    public interface IInput
    {
        bool IsKeyDown(EngineKey key);
        void CaptureLastMousePosition(); 
        Vector2 GetMouseDelta();
    }
}