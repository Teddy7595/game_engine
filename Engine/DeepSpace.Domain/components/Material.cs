using System.Drawing;

namespace DeepSpace.Domain.Components
{
    public class MaterialComponent : IComponent
    {
        public Color DiffuseColor { get; set; } = Color.White; // Valor por defecto
        public float Shininess { get; set; } = 32.0f; // Valor por defecto
        public string? TextureName { get; set; } = null; // Nombre de la textura asociada, si la hay
    }
}