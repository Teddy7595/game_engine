namespace DeepSpace.Domain.Components
{
    public class RenderableComponent: IComponent
    {
        public string MeshName { get; set; }

        public RenderableComponent(string meshName)
        {
            MeshName = meshName;
        }
    }
}