using DeepSpace.Domain.Components;
using DeepSpace.Application.Interfaces;
using Silk.NET.OpenGL;
using System.Numerics;

namespace DeepSpace.Infrastructure.Rendering
{
    public class MeshRenderer : IRenderer
    {
        private readonly GL _gl;
        private uint _vertexBufferObject;
        private uint _vertexArrayObject;
        private uint _shaderProgram;
        private int _modelMatrixLocation;
        private int _viewMatrixLocation;
        private int _projectionMatrixLocation;
        private uint _elementBufferObject;

        // Los 8 vértices únicos de un cubo
        private readonly float[] _vertices =
        {
            // Posición
            0.5f,  0.5f,  0.5f, // Arriba-derecha-frontal
            0.5f, -0.5f,  0.5f, // Abajo-derecha-frontal
            -0.5f, -0.5f,  0.5f, // Abajo-izquierda-frontal
            -0.5f,  0.5f,  0.5f, // Arriba-izquierda-frontal
            0.5f,  0.5f, -0.5f, // Arriba-derecha-trasera
            0.5f, -0.5f, -0.5f, // Abajo-derecha-trasera
            -0.5f, -0.5f, -0.5f, // Abajo-izquierda-trasera
            -0.5f,  0.5f, -0.5f  // Arriba-izquierda-trasera
        };

        // Los índices que forman los 12 triángulos
        private readonly uint[] _indices =
        {
            0, 1, 3, 1, 2, 3, // Cara frontal
            4, 5, 0, 5, 1, 0, // Cara derecha
            7, 6, 4, 6, 5, 4, // Cara trasera
            3, 2, 7, 2, 6, 7, // Cara izquierda
            4, 0, 7, 0, 3, 7, // Cara superior
            5, 6, 1, 6, 2, 1  // Cara inferior
        };

        public MeshRenderer(GL gl)
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

            // Crear y bind del Element Buffer Object (EBO)
            _elementBufferObject = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _elementBufferObject);
            unsafe
            {
                fixed (uint* ptr = _indices)
                {
                    _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (uint)(_indices.Length * sizeof(uint)), ptr, BufferUsageARB.StaticDraw);
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
            //Creamos las matrices de rotación y escala si es necesario
            Matrix4x4 rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(transform.Rotation.Y, transform.Rotation.X, transform.Rotation.Z);
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(transform.Scale);
            // Combinamos las matrices: primero escala, luego rotación, luego traslación
            modelMatrix = scaleMatrix * rotationMatrix * modelMatrix;
            // Enviar la matriz de modelo al shader
            unsafe
            {
                _gl.UniformMatrix4(_modelMatrixLocation, 1, false, (float*)&modelMatrix);
                _gl.UniformMatrix4(_viewMatrixLocation, 1, false, (float*)&viewMatrix);
                _gl.UniformMatrix4(_projectionMatrixLocation, 1, false, (float*)&projectionMatrix);
            }
            _gl.BindVertexArray(_vertexArrayObject);
            unsafe
            {
                _gl.DrawElements(PrimitiveType.Triangles, (uint)_indices.Length, DrawElementsType.UnsignedInt, null);
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
    }
}