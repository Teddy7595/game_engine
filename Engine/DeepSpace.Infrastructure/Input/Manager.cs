using DeepSpace.Application.Inputs;
using DeepSpace.Application.Interfaces;
using Silk.NET.Input;

namespace DeepSpace.Infrastructure.Input
{
    public class InputManager : IInput
    {
        private IKeyboard? _keyboard;
        private IMouse? _mouse;

        public void Initialize(IKeyboard keyboard)
        {
            _keyboard = keyboard;
        }

        public bool IsKeyPressed(EngineKey key)
        {
            if (_keyboard == null) throw new InvalidOperationException("Keyboard not initialized.");
            return _keyboard.IsKeyPressed(ToSilkKey(key));
        }

        private Key ToSilkKey(EngineKey engineKey)
        {
            return engineKey switch
            {
                EngineKey.W => Key.W,
                EngineKey.A => Key.A,
                EngineKey.S => Key.S,
                EngineKey.D => Key.D,
                EngineKey.Space => Key.Space,
                _ => throw new ArgumentOutOfRangeException(nameof(engineKey), $"No mapping for {engineKey}")
            };
        }
        
    }
}