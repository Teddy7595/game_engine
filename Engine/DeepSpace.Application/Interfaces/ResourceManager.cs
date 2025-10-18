namespace DeepSpace.Application.Interfaces
{
    public interface IResourceManager
    {
        IMesh? GetMesh(string name);
        ITexture? GetTexture(string name);
    }
}