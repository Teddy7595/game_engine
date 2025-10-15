using DeepSpace.Application.Inputs;

namespace DeepSpace.Application.Interfaces
{
    public interface IInput
    {
        bool IsKeyPressed(EngineKey key);
    }
}