namespace Runtime
{
    using Core;

    public class EntityManager
    {
        private uint _nextEntityId = 1;
        private Dictionary<Type, Dictionary<uint, IComponent>> _componentStore;

        public Entity CreateEntity()
        {
            var newId = this._nextEntityId++;
            return new Entity(newId);
        }

        public void DestroyEntity(Entity entity) { }

        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            var componentType = typeof(T);
            if (!this._componentStore.ContainsKey(componentType))
                this._componentStore[componentType] = new Dictionary<uint, IComponent>();

            this._componentStore[componentType][entity.Id] = component;
        }

        public T GetComponent<T>(Entity entity) where T : IComponent
        {
            var componentType = typeof(T);
            if (this._componentStore.ContainsKey(componentType) && this._componentStore[componentType].ContainsKey(entity.Id))
                return (T)this._componentStore[componentType][entity.Id];

            throw new KeyNotFoundException($"Component of type {componentType} not found for entity {entity.Id}");
        }

        public void RemoveComponent<T>(Entity entity) where T : IComponent
        {
            var componentType = typeof(T);
            if (this._componentStore.ContainsKey(componentType))
                this._componentStore[componentType].Remove(entity.Id);
        }

        public void MutateComponent<T>(Entity entity, Action<T> mutateAction) where T : IComponent
        {
            var component = this.GetComponent<T>(entity);
            mutateAction.Invoke(component);
            this.AddComponent(entity, component);
        }

        public IEnumerable<(Entity entity, T1 comp1, T2 comp2)> GetEntitiesWithComponents<T1, T2>()
            where T1 : IComponent
            where T2 : IComponent
        {
            var type1 = typeof(T1);
            var type2 = typeof(T2);

            if (!this._componentStore.ContainsKey(type1) || !this._componentStore.ContainsKey(type2))
                yield break;

            var store1 = this._componentStore[type1];
            var store2 = this._componentStore[type2];

            foreach (var kvp in store1)
            {
                var entityId = kvp.Key;
                if (store2.ContainsKey(entityId))
                {
                    yield return (new Entity(entityId), (T1)kvp.Value, (T2)store2[entityId]);
                }
            }
        }

    }
}