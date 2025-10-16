using System.Drawing;

namespace DeepSpace.Domain.Components
{
    public class MaterialComponent : IComponent
    {
        public Color DiffuseColor { get; set; } = Color.Orange;
        public float Shininess { get; set; } = 32.0f; // Valor por defecto
    }
}