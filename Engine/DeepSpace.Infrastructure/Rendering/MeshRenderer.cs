using DeepSpace.Domain.Components;
using DeepSpace.Application.Interfaces;
using Silk.NET.OpenGL;
using System.Numerics;
using System.Drawing;

namespace DeepSpace.Infrastructure.Rendering
{
    public class MeshRenderer : IRenderer
    {
        private readonly GL _gl;
        private uint _shaderProgram;
        private int _modelMatrixLocation;
        private int _viewMatrixLocation;
        private int _projectionMatrixLocation;
        private int _lightPosLocation;
        private int _lightColorLocation;
        private int _viewPosLocation;
        private int _materialDiffuseLocation;
        private int _materialShininessLocation;
        private int _textureSamplerLocation;

        public MeshRenderer(GL gl)
        {
            _gl = gl;
        }

        public void Load()
        {

            // --- cargar y compilar shaders ---
            // 1. Obtenemos la ruta del directorio donde se está ejecutando el programa.
            string baseDirectory = AppContext.BaseDirectory;
            // 2. Combinamos esa ruta con la ruta relativa de nuestros shaders de forma segura.
            string vertexShaderPath = Path.Combine(baseDirectory, "Shaders/shader.vert");
            string fragmentShaderPath = Path.Combine(baseDirectory, "Shaders/shader.frag");

            // 3. Leemos los archivos desde la ruta absoluta y correcta.
            var vertexShaderSource = File.ReadAllText(vertexShaderPath);
            var fragmentShaderSource = File.ReadAllText(fragmentShaderPath);

            uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
            _gl.ShaderSource(vertexShader, vertexShaderSource);
            _gl.CompileShader(vertexShader);
            CheckCompileErrors(vertexShader, "VERTEX");

            uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
            _gl.ShaderSource(fragmentShader, fragmentShaderSource);
            _gl.CompileShader(fragmentShader);
            CheckCompileErrors(fragmentShader, "FRAGMENT");

            _shaderProgram = _gl.CreateProgram();
            _gl.AttachShader(_shaderProgram, vertexShader);
            _gl.AttachShader(_shaderProgram, fragmentShader);
            _gl.LinkProgram(_shaderProgram);
            CheckLinkErrors(_shaderProgram);

            // Obtener la ubicación de la variable uniforme "model" en el shader
            // --- OBTENER LAS NUEVAS UBICACIONES DE UNIFORMS ---
            _modelMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "model");
            _viewMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "view");
            _projectionMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "projection");
            // UBICACIONES NUEVAS
            _lightPosLocation = _gl.GetUniformLocation(_shaderProgram, "lightPos");
            _lightColorLocation = _gl.GetUniformLocation(_shaderProgram, "lightColor");
            _viewPosLocation = _gl.GetUniformLocation(_shaderProgram, "viewPos");
            // REFERENCIAS A MATERIAL
            _materialDiffuseLocation = _gl.GetUniformLocation(_shaderProgram, "material.diffuse");
            _materialShininessLocation = _gl.GetUniformLocation(_shaderProgram, "material.shininess");

            //Textura location
            _textureSamplerLocation = _gl.GetUniformLocation(_shaderProgram, "textureSampler");


            // Limpiar shaders ya que están enlazados al programa
            _gl.DeleteShader(vertexShader);
            _gl.DeleteShader(fragmentShader);
        }

        public void DrawMesh(
            IMesh mesh,
            MaterialComponent material,
            TransformComponent transform,
            ITexture texture,
            Matrix4x4 viewMatrix,
            Matrix4x4 projectionMatrix,
            Vector3 lightPosition,
            Vector3 viewPosition,
            Color lightColor
        )
        {
            CheckGLErrors("Before DrawMesh");
            var concreteMesh = mesh as Mesh;
            var concreteTexture = texture as DSTexture;
            if (concreteMesh == null && concreteMesh == null) return;

            // 1. Usar el programa de shaders y bindear la malla y textura
            _gl.UseProgram(_shaderProgram);
            concreteMesh.Bind();
            concreteTexture.Bind(TextureUnit.Texture0);

            // 2. Calcular y enviar los uniforms (lógica de matrices y luz)
            Matrix4x4 model = Matrix4x4.CreateScale(transform.Scale) *
                Matrix4x4.CreateFromYawPitchRoll(transform.Rotation.Y, transform.Rotation.X, transform.Rotation.Z) *
                Matrix4x4.CreateTranslation(transform.Position);

            // Enviar las matrices al shader
            unsafe
            {
                _gl.UniformMatrix4(_modelMatrixLocation, 1, false, (float*)&model);
                _gl.UniformMatrix4(_viewMatrixLocation, 1, false, (float*)&viewMatrix);
                _gl.UniformMatrix4(_projectionMatrixLocation, 1, false, (float*)&projectionMatrix);

                _gl.Uniform3(_lightPosLocation, lightPosition);
                _gl.Uniform3(_lightColorLocation, lightColor.R / 255.0f, lightColor.G / 255.0f, lightColor.B / 255.0f);
                _gl.Uniform3(_viewPosLocation, viewPosition);

                var color = material.DiffuseColor;
                _gl.Uniform3(_materialDiffuseLocation, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
                CheckGLErrors("Set material diffuse");
                _gl.Uniform1(_materialShininessLocation, material.Shininess);
                CheckGLErrors("Set material shininess");
                // Setear el sampler de textura
                _gl.Uniform1(_textureSamplerLocation, 0); // Texture unit 0
                CheckGLErrors("Set texture sampler");

                _gl.DrawElements(PrimitiveType.Triangles, concreteMesh._indexCount, DrawElementsType.UnsignedInt, null);
            }
            CheckGLErrors("Después de DrawElements");

            concreteMesh.Unbind();
            concreteTexture.Unbind();
            CheckGLErrors("Al final de DrawMesh");
        }

        private void CheckGLErrors(string stage)
        {
            GLEnum error;
            while ((error = _gl.GetError()) != GLEnum.NoError)
            {
                Console.WriteLine($"ERROR DE OPENGL en la etapa '{stage}': {error}");
            }
        }

        private void CheckCompileErrors(uint shader, string type)
        {
            _gl.GetShader(shader, ShaderParameterName.CompileStatus, out var status);
            if (status != (int)GLEnum.True)
            {
                string infoLog = _gl.GetShaderInfoLog(shader);
                Console.WriteLine($"ERROR::SHADER_COMPILATION_ERROR of type: {type}\n{infoLog}");
            }
        }

        private void CheckLinkErrors(uint program)
        {
            _gl.GetProgram(program, ProgramPropertyARB.LinkStatus, out var status);
            if (status != (int)GLEnum.True)
            {
                string infoLog = _gl.GetProgramInfoLog(program);
                Console.WriteLine($"ERROR::PROGRAM_LINKING_ERROR of type: PROGRAM\n{infoLog}");
            }
        }

        public void Unload()
        {
            _gl.DeleteProgram(_shaderProgram);
        }

        public void Clear(Color color)
        {
            _gl.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _gl.Enable(EnableCap.DepthTest);
        }
    }
}