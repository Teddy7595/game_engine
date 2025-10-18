using Silk.NET.OpenGL;
using DeepSpace.Infrastructure.Rendering;
using DeepSpace.Application.Interfaces;

namespace DeepSpace.Infrastructure.Core
{
    public class ResourceManager : IResourceManager
    {
        private readonly GL? _gl;
        private readonly Dictionary<string, Mesh> _meshes = new Dictionary<string, Mesh>();
        private readonly Dictionary<string, DSTexture> _textures = new Dictionary<string, DSTexture>();

        public ResourceManager(GL gl)
        {
            _gl = gl;
        }

        public IMesh? GetMesh(string name)
        {
            _meshes.TryGetValue(name, out var mesh);
            return mesh;
        }

        public ITexture? GetTexture(string name)
        {
            _textures.TryGetValue(name, out var texture);
            return texture;
        }

        public void LoadTexture(string name, string path)
        {
            var texture = new DSTexture(_gl, path);
            _textures.Add(name, texture);
        }

        public void CreateCubeMesh(string name)
        {
            // Movemos los datos del cubo desde el antiguo MeshRenderer aquí
            float[] vertices =
            {
                // Posición           // Normal            // Coordenadas de Textura (UV)
                // Cara de atrás (-Z)
                -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
                0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
                0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,

                // Cara de adelante (+Z)
                -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
                0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
                0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,

                // Cara de la izquierda (-X)
                -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

                // Cara de la derecha (+X)
                0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
                0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
                0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
                0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,

                // Cara de abajo (-Y)
                -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
                0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
                0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,

                // Cara de arriba (+Y)
                -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
                0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
                0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f
            };

            uint[] indices =
            {
                0, 1, 2,  0, 2, 3,
                4, 5, 6,  4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
                16, 17, 18, 16, 18, 19,
                20, 21, 22, 20, 22, 23
            };

            var cubeMesh = new Mesh(_gl, vertices, indices);
            _meshes.Add(name, cubeMesh);
        }
    }
}