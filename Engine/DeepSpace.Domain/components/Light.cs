using System.Drawing;

namespace DeepSpace.Domain.Components
{
    public class LightComponent : IComponent
    {
        public Color Color { get; set; }
        public float Intensity { get; set; }
    }
}