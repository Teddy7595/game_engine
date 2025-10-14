using DeepSpaceEngine.Application.Systems;
using DeepSpaceEngine.Domain.Components;
using DeepSpaceEngine.Domain.Core;

// --- 1. CONFIGURACIÓN (Arrange) ---

// Crear el universo de nuestro juego
var world = new World();

// Crear los sistemas que operarán en el mundo
var debugSystem = new DebugLogSystem();

// Crear una entidad "Player" para que nuestro sistema la encuentre
var playerEntity = world.CreateEntity();
world.AddComponent(playerEntity, new TagComponent("Player 1"));
world.AddComponent(playerEntity, new TransformComponent 
{ 
    Position = new() { X = 10, Y = 20, Z = 30 } 
});

Console.WriteLine("Motor inicializado. Presiona Ctrl+C para salir.");
Console.WriteLine("---------------------------------------------");

// --- 2. EL BUCLE DEL JUEGO (Act) ---

// Un bucle infinito simple para simular los frames de un juego
while (true)
{
    // Por ahora, nuestro deltaTime es fijo, pero más adelante lo calcularemos.
    float deltaTime = 0.016f; // Simulando ~60 FPS

    // Ejecutamos la lógica de nuestro sistema
    debugSystem.Update(world, deltaTime);

    // Esperamos un poco para no saturar la CPU
    Thread.Sleep(1000); // Esperamos 1 segundo entre cada "frame"
}