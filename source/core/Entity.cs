namespace Core
{
    public struct Entity(uint id)
    {
        public uint Id = id;
    }

    public interface IEntityFactory
    {
        Entity CreateEntity(params IComponent[] initalComponent);
        Entity CreateRenderizableCube(System.Numerics.Vector3 position, Guid materialId, Guid meshId);
    }
}