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
        private uint _vertexBufferObject;
        private uint _vertexArrayObject;
        private uint _shaderProgram;
        private int _modelMatrixLocation;
        private int _viewMatrixLocation;
        private int _projectionMatrixLocation;
        private uint _elementBufferObject;
        private int _lightPosLocation;   
        private int _lightColorLocation; 
        private int _viewPosLocation;    
        private int _objectColorLocation;

        // Los 8 vértices únicos de un cubo
        private readonly float[] _vertices =
        {
            // Cara de atrás (-Z)
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,

            // Cara de adelante (+Z)
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,

            // Cara de la izquierda (-X)
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,

            // Cara de la derecha (+X)
            0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,
            0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
            0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,
            0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,

            // Cara de abajo (-Y)
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
            0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,
            0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,

            // Cara de arriba (+Y)
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
            0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,
            0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f
        };

        // Los índices que forman los 12 triángulos
        private readonly uint[] _indices =
        {
            0, 1, 2,  0, 2, 3,    // Cara de atrás
            4, 5, 6,  4, 6, 7,    // Cara de adelante
            8, 9, 10, 8, 10, 11,  // Cara de la izquierda
            12, 13, 14, 12, 14, 15, // Cara de la derecha
            16, 17, 18, 16, 18, 19, // Cara de abajo
            20, 21, 22, 20, 22, 23  // Cara de arriba
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
            // --- OBTENER LAS NUEVAS UBICACIONES DE UNIFORMS ---
            _modelMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "model");
            _viewMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "view");
            _projectionMatrixLocation = _gl.GetUniformLocation(_shaderProgram, "projection");
            
            // UBICACIONES NUEVAS
            _lightPosLocation = _gl.GetUniformLocation(_shaderProgram, "lightPos");
            _lightColorLocation = _gl.GetUniformLocation(_shaderProgram, "lightColor");
            _viewPosLocation = _gl.GetUniformLocation(_shaderProgram, "viewPos");
            _objectColorLocation = _gl.GetUniformLocation(_shaderProgram, "objectColor");
            // --- FIN DE OBTENER UBICACIONES ---
            
            // Limpiar shaders ya que están enlazados al programa
            _gl.DeleteShader(vertexShader);
            _gl.DeleteShader(fragmentShader);

            //-- configurar los punteros de atributos de vértices
            // El stride (paso) ahora es de 6 floats (3 de posición + 3 de normal)
            int stride = 6 * sizeof(float);

            // Atributo de Posición (location = 0)
            unsafe
            {
                _gl.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, (uint)stride, null);
            }
            _gl.EnableVertexAttribArray(0);

            // Atributo de Normal (location = 1)
            // El offset es de 3 floats, que es donde empiezan los datos de la normal
            unsafe
            {
                _gl.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, (uint)stride, (void*)(3 * sizeof(float)));
            }
            _gl.EnableVertexAttribArray(1);
        }

        public void DrawMesh(TransformComponent transform, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix, Vector3 lightPosition, Vector3 viewPosition, Color lightColor, float lightIntensity)
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

                 // --- ENVIAR LOS NUEVOS UNIFORMS ---
                _gl.Uniform3(_lightPosLocation, lightPosition.X, lightPosition.Y, lightPosition.Z);
                _gl.Uniform3(_lightColorLocation, lightColor.R / 255.0f, lightColor.G / 255.0f, lightColor.B / 255.0f);
                _gl.Uniform3(_viewPosLocation, viewPosition.X, viewPosition.Y, viewPosition.Z);
                // Para el color del objeto (cubo), lo dejamos en un naranja fijo por ahora
                _gl.Uniform3(_objectColorLocation, 1.0f, 0.5f, 0.2f);
                //Si queremos usar la intensidad de la luz en el shader, podemos enviarla también
                //_gl.Uniform1(_lightIntensityLocation, lightIntensity);
                // --- FIN DE ENVIAR UNIFORMS ---
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

        public void Unload()
        {
            _gl.DeleteProgram(_shaderProgram);
            _gl.DeleteBuffer(_vertexBufferObject);
            _gl.DeleteVertexArray(_vertexArrayObject);
            _gl.DeleteBuffer(_elementBufferObject);
        }

        public void Clear(Color color)
        {
            _gl.ClearColor(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
            _gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _gl.Enable(EnableCap.DepthTest);
        }
    }
}