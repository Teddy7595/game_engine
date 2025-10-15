using DeepSpace.Application.Inputs;
using DeepSpace.Application.Interfaces;
using Silk.NET.Input;
using System.Numerics;

namespace DeepSpace.Infrastructure.Input
{
    public class InputManager : IInput
    {
        private IKeyboard? _keyboard;
        private IMouse? _mouse;
        private Vector2 _lastMousePosition;


        public void Initialize(IKeyboard keyboard, IMouse mouse)
        {
            _keyboard = keyboard;
            _mouse = mouse;
        }

        public bool IsKeyDown(EngineKey key)
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
                _ => Key.Unknown
            };
        }

        // Métodos adicionales para manejar el ratón y otros inputs pueden añadirse aquí.
        public void CaptureLastMousePosition()
        {
            _lastMousePosition = _mouse?.Position ?? Vector2.Zero;
        }

        public Vector2 GetMouseDelta()
        {
            if (_mouse == null) return Vector2.Zero;
            var currentMousePosition = _mouse.Position;
            var delta = currentMousePosition - _lastMousePosition;
            _lastMousePosition = currentMousePosition; // Actualizar la última posición
            return delta;
        }
    }
}