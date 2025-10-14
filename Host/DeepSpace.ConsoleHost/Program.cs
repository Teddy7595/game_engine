using DeepSpace.Application.Systems;
using DeepSpace.Domain.Core;
using DeepSpace.Domain.Components;
using DeepSpace.Infrastructure.Windowing;

//-- CONFIGURATION
var World = new World(); //-- Initialize World
var SystemManager = new SystemManager(); //-- Initialize System Manager

SystemManager.AddSystem(new DebugLogSystem()); //--- Register Systems Here


var playerEntity = World.CreateEntity(); //-- Create an Entity
World.AddComponent(playerEntity, new TagComponent("Mi Triangulo")); //-- Add Tag Component
World.AddComponent(playerEntity, new TransformComponent
{
    Position = new System.Numerics.Vector3(0.5f, 0.25f, 0.0f)
}); //-- Add Transform Component
World.AddComponent(playerEntity, new RenderableComponent()); //-- Add Renderable Component

var GameWindow = new GameWindow(SystemManager, World); //-- Initialize Game Window

GameWindow.Run(); //-- Start Game Loop





    