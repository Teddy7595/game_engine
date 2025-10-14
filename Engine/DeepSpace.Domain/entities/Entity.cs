namespace DeepSpaceEngine.Domain.Entities
{
    public readonly struct Entity
    {
        public Guid Id { get; init; }

        public Entity(Guid id)
        {
            Id = id;
        }
    }

}