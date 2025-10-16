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

// --- Creamos la entidad del "Planeta" ---
var planetEntity = world.CreateEntity();
world.AddComponent(planetEntity, new TagComponent("Planeta"));
world.AddComponent(planetEntity, new TransformComponent { Position = Vector3.Zero }); // En el centro
world.AddComponent(planetEntity, new AutoRotateComponent());
world.AddComponent(planetEntity, new RenderableComponent("Cube"));

// --- ¡NUEVO! Creamos la entidad de la "Luna" ---
var moonEntity = world.CreateEntity();
world.AddComponent(moonEntity, new TagComponent("Luna"));
// La posicionamos a la derecha y la hacemos más pequeña
world.AddComponent(moonEntity, new TransformComponent 
{ 
    Position = new Vector3(2, 0, 0), 
    Scale = new Vector3(0.3f) 
});
world.AddComponent(moonEntity, new AutoRotateComponent()); // También la hacemos girar
world.AddComponent(moonEntity, new RenderableComponent("Cube")); // ¡Reutiliza la misma malla!

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





    