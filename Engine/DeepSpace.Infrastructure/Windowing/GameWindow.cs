using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using DeepSpace.Domain.Core;
using DeepSpace.Application.Systems;
using DeepSpace.Infrastructure.Rendering;

namespace DeepSpace.Infrastructure.Windowing
{
    public class GameWindow
    {
        private readonly IWindow _window;
        private readonly SystemManager _systemManager;
        private readonly World _world;
        private GL? _gl;

        public GameWindow(SystemManager systemManager, World world)
        {
            _systemManager = systemManager;
            _world = world;

            // Configurar la ventana con Silk.NET
            var options = WindowOptions.Default;
            options.Title = "DeepSpace Engine - ECS Example";
            options.Size = new(1280, 720); // Ancho y alto en píxeles

            _window = Window.Create(options);

            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            //_window.Resize += OnResize;
        }

        private void OnLoad()
        {
            _gl = _window.CreateOpenGL();

            // Inicializar el renderizador de triángulos
            var _renderer = new TriangleRenderer(_gl);
            _renderer.Load();

            // Crear el sistema de renderizado y agregarlo al SystemManager
            var renderSystem = new RenderSystem(_renderer);
            _systemManager.AddSystem(renderSystem);
        }

        private void OnUpdate(double deltaTime)
        {
            
        }

        public void Run() => _window.Run();

        private void OnRender(double deltaTime)
        {
            _gl.ClearColor(0.1f, 0.1f, 0.2f, 1.0f); // Un azul oscuro
            _gl.Clear(ClearBufferMask.ColorBufferBit);

            _systemManager.UpdateAll(_world, (float)deltaTime);
        }

    }
}