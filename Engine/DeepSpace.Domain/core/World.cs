using DeepSpaceEngine.Domain.Entities;
using DeepSpaceEngine.Domain.Components;

namespace DeepSpaceEngine.Domain.Core
{
    public class World
    {
        // Un diccionario para guardar todos los componentes.
        // La clave es el TIPO de componente (ej: typeof(TransformComponent)).
        // El valor es otro diccionario que mapea el ID de la Entidad a la instancia del componente.
        private readonly Dictionary<Type, Dictionary<Guid, IComponent>> _componentStores = new();

        // construimos la entidad con un ID unico global
        public Entity CreateEntity(string name = null)
        {
            var entity = new Entity(Guid.Parse(name ?? Guid.NewGuid().ToString()));
            return entity;
        }

        // Añadir un componente a una entidad
        public void AddComponent<T>(Entity entity, T component) where T : class, IComponent
        {
            var type = typeof(T);
            // Si no existe un diccionario para este tipo de componente, lo creamos
            if (!_componentStores.ContainsKey(type))
                _componentStores[type] = new Dictionary<Guid, IComponent>();

            // Añadimos o actualizamos el componente para la entidad
            _componentStores[type][entity.Id] = component;
        }
        // Obtener un componente de una entidad
        public T? GetComponent<T>(Entity entity) where T : class, IComponent
        {
            // Obtener el diccionario de componentes para el tipo T
            var componentType = typeof(T);

            //Buscamos si tenemos el almacen para este tipo de componente
            if (_componentStores.TryGetValue(componentType, out var store))
            {
                // Si existe, intentamos obtener el componente para la entidad dada
                if (store.TryGetValue(entity.Id, out var component))
                    return component as T;
                
            }
            return null;
        }
        // Eliminar un componente de una entidad
        public void RemoveComponent<T>(Entity entity) where T : class, IComponent
        {
            var type = typeof(T);
            // Si existe un diccionario para este tipo de componente, intentamos eliminar el componente para la entidad dada
            if (_componentStores.TryGetValue(type, out var store))
                // Si existe, intentamos eliminar el componente para la entidad dada
                store.Remove(entity.Id);
        }
        public IEnumerable<Entity> View<T>() where T : class, IComponent
        {
            //obtenemos el tipo de componente
            var componentType = typeof(T);

            //buscamos en el almacen si existe este tipo de componente
            if (_componentStores.TryGetValue(componentType, out var store))
                //Si existe, todas las claves (Keys) en su diccionario
                //son los IDs de las entidades que tienen ese componente.
                //Las convertimos de Guid de vuelta a nuestro tipo Entity.
                return store.Keys.Select(id => new Entity(id));
            //Si no existe, devolvemos una lista vacía.
            return Enumerable.Empty<Entity>();
        }

        
    }
}