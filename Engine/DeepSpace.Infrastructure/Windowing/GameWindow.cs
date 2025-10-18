using Silk.NET.Windowing;
using Silk.NET.OpenGL;
using Silk.NET.Input;
using DeepSpace.Domain.Core;
using DeepSpace.Application.Systems;
using DeepSpace.Infrastructure.Rendering;
using DeepSpace.Infrastructure.Input;
using DeepSpace.Infrastructure.Core;

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
            if (_gl == null) throw new InvalidOperationException("Failed to create OpenGL context.");

            // Inicializar el gestor de recursos
            var resourceManager = new ResourceManager(_gl);
            resourceManager.CreateCubeMesh("Cube");

            // Cargar texturas u otros recursos aquí si es necesario
            string texturePath = Path.Combine(AppContext.BaseDirectory, "Assets");
            Console.WriteLine($"Cargando texturas desde: {texturePath}");
            resourceManager.LoadTexture("default_white", Path.Combine(texturePath, "white.png"));
            resourceManager.LoadTexture("container", Path.Combine(texturePath, "container.png"));

            // Inicializar el gestor de entrada
            var inputContext = _window.CreateInput();
            var keyboard = inputContext.Keyboards.FirstOrDefault() ?? throw new InvalidOperationException("No keyboard found.");
            var mouse = inputContext.Mice.FirstOrDefault() ?? throw new InvalidOperationException("No mouse found.");

            // Crear e inicializar el InputManager
            var inputManager = new InputManager();
            inputManager.Initialize(keyboard, mouse);

            // Ocultar y capturar el cursor del ratón para un control FPS
            if (mouse != null)
            {
                mouse.Cursor.CursorMode = CursorMode.Raw;
                inputManager.CaptureLastMousePosition();
            }

            // Inicializar el renderizador de triángulos
            var _renderer = new MeshRenderer(_gl);
            _renderer.Load();

            // Calcular la relación de aspecto
            var aspectRatio = (float)_window.Size.X / _window.Size.Y;
            // Pasar el gestor de recursos al sistema de renderizado
            _systemManager.AddSystem(new RenderSystem(_renderer, aspectRatio, resourceManager));
            // Añadir el sistema de input al SystemManager
            _systemManager.AddSystem(new InputSystem(inputManager));
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