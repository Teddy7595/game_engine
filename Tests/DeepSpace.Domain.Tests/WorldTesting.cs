using NUnit.Framework;
using DSEngine.Domain.Core;
using DSEngine.Domain.Components;

namespace DSEngine.Domain.Tests
{
    [TestFixture] // Marca esta clase como un contenedor de pruebas para NUnit
    public class WorldTests
    {
        [Test] // Marca este método como una prueba individual
        public void AddComponent_And_GetComponent_ShouldStoreAndRetrieveComponent()
        {
            // 1. Arrange (Organizar)
            var world = new World();
            var entity = world.CreateEntity();
            var originalComponent = new TagComponent("TestTag");

            // 2. Act (Actuar)
            world.AddComponent(entity, originalComponent);
            var retrievedComponent = world.GetComponent<TagComponent>(entity);

            // 3. Assert (Afirmar)
            Assert.That(retrievedComponent, Is.Not.Null); // Afirmamos que el componente no es nulo
            Assert.That(retrievedComponent.Tag, Is.EqualTo("TestTag")); // Afirmamos que tiene los datos correctos
            Assert.That(retrievedComponent, Is.SameAs(originalComponent)); // Afirmamos que es la misma instancia
        }

        [Test]
        public void RemoveComponent_ShouldMakeComponentUnavailable()
        {
            // 1. Arrange
            var world = new World();
            var entity = world.CreateEntity();
            var component = new TransformComponent();
            world.AddComponent(entity, component);

            // 2. Act
            world.RemoveComponent<TransformComponent>(entity);
            var retrievedComponent = world.GetComponent<TransformComponent>(entity);

            // 3. Assert
            Assert.That(retrievedComponent, Is.Null); // Afirmamos que el componente ahora es nulo
        }
    }
}