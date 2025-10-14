namespace DeepSpace.Domain.Exceptions
{
    public class ComponentNotFoundException : Exception
    {
        public ComponentNotFoundException(Type componentType, Guid entityId)
            : base($"Component of type {componentType.Name} not found for entity {entityId}.") { }
    }
}