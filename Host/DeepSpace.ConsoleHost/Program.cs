using DeepSpace.Application.Systems;
using DeepSpace.Domain.Core;
using DeepSpace.Domain.Components;
using DeepSpace.Infrastructure.Windowing;
using System.Numerics;
using System.Drawing;

// --- CONFIGURACIÓN ---
var world = new World();
var systemManager = new SystemManager();
systemManager.AddSystem(new DebugLogSystem());
systemManager.AddSystem(new RotationSystem());

// Creamos la entidad del cubo de pruebas en el centro del mundo
var triangleEntity = world.CreateEntity();
world.AddComponent(triangleEntity, new TagComponent("Mi cubo de pruebas"));
world.AddComponent(triangleEntity, new TransformComponent { Position = Vector3.Zero }); // En (0,0,0)
world.AddComponent(triangleEntity, new RenderableComponent("Cube")); // Usando la malla "Cubo"
world.AddComponent(triangleEntity, new AutoRotateComponent()); // Le añadimos auto-rotación

// Creamos la entidad de la CÁMARA
var cameraEntity = world.CreateEntity();
world.AddComponent(cameraEntity, new TagComponent("Cámara Principal"));
// La posicionamos un poco hacia atrás en el eje Z para que pueda "ver" el origen
world.AddComponent(cameraEntity, new TransformComponent { Position = new Vector3(0, 0, 3) });
world.AddComponent(cameraEntity, new CameraComponent());
world.AddComponent(cameraEntity, new MainCameraComponent()); // La marcamos como principal

// Creamos una entidad de luz
var lightEntity = world.CreateEntity();
world.AddComponent(lightEntity, new TagComponent("Luz Principal"));
world.AddComponent(lightEntity, new TransformComponent { Position = new Vector3(0, 5, 0) });
world.AddComponent(lightEntity, new LightComponent { Color = Color.Red, Intensity = 0.85f });

// --- EJECUCIÓN ---
var gameWindow = new GameWindow(systemManager, world);
gameWindow.Run();





    