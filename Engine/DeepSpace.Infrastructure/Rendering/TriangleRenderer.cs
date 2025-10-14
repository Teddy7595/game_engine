using DeepSpace.Domain.Components;
using DeepSpace.Application.Interfaces;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DeepSpace.Infrastructure.Rendering
{
    public class TriangleRenderer : IRenderer
    {
        private readonly GL _gl;
        private uint _vertexBufferObject;
        private uint _vertexArrayObject;
        private uint _shaderProgram;
        private int _modelMatrixLocation;
        private int _viewMatrixLocation;
        private int _projectionMatrixLocation;

        // Vértices de nuestro triángulo (en coordenadas normalizadas de -1 a 1)
        private readonly float[] _vertices =
        {
            -0.5f, -0.5f, 0.0f, // Abajo izquierda
             0.5f, -0.5f, 0.0f, // Abajo derecha
             0.0f,  0.5f, 0.0f  // Arriba centro
        };

        public TriangleRenderer(GL gl)
        {
            _gl = gl;
        }

        public void Load()
        {
            // Crear el vertex array object y el vertex buffer object
            _vertexArrayObject = _gl.GenVertexArray();
            _gl.BindVertexArray(_vertexArrayObject);

            _vertexBufferObject = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vertexBufferObject);

            // cargar los datos de los vértices en el buffer

            unsafe
            {
                fixed (float* v = _vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(float) * _vertices.Length), v, BufferUsageARB.StaticDraw);
                }
            }

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
            _modelMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "model");
            _viewMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "view");
            _projectionMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "projection");
            
            // Limpiar shaders ya que están enlazados al programa
            _gl.DeleteShader(vertexShader);
            _gl.DeleteShader(fragmentShader);

            //-- configurar los punteros de atributos de vértices
            //le decimos a OpenGL como interpretar los datos del buffer
            _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), IntPtr.Zero);
            _gl.EnableVertexAttribArray(0);
        }

        public void DrawMesh(TransformComponent transform, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
        {
            _gl.UseProgram(_shaderProgram);
            // Crear la matriz de modelo basada en la posición, rotación y escala del TransformComponent
            Matrix4x4 modelMatrix = Matrix4x4.CreateTranslation(transform.Position);
            // Enviar la matriz de modelo al shader
            unsafe
            {
                _gl.UniformMatrix4(_modelMatrixLocation, 1, false, (float*)&modelMatrix);
                _gl.UniformMatrix4(_viewMatrixLocation, 1, false, (float*)&viewMatrix);
                _gl.UniformMatrix4(_projectionMatrixLocation, 1, false, (float*)&projectionMatrix);
            }
            _gl.BindVertexArray(_vertexArrayObject);
            _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
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
    }
}