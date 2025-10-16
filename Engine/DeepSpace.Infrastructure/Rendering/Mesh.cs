using DeepSpace.Application.Interfaces;
using Silk.NET.OpenGL;

namespace DeepSpace.Infrastructure.Rendering
{
    public class Mesh : IMesh
    {
        // Mesh data and methods go here
        public uint _vao { get; private set; }
        public uint _vbo { get; private set; }
        public uint _ebo { get; private set; }
        public uint _indexCount { get; private set; }
        private readonly GL _gl;

        public Mesh(GL gl, float[] vertices, uint[] indices)
        {
            _gl = gl;
            _indexCount = (uint)indices.Length;

            // Crear el vertex array object y el vertex buffer object
            _vao = _gl.GenVertexArray();
            _gl.BindVertexArray(_vao);

            _vbo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);

            // cargar los datos de los vértices en el buffer
            unsafe
            {
                fixed (float* v = vertices)
                {
                    _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(sizeof(float) * vertices.Length), v, BufferUsageARB.StaticDraw);
                }
            }

            // Crear y bind del Element Buffer Object (EBO)
            _ebo = _gl.GenBuffer();
            _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
            unsafe
            {
                fixed (uint* ptr = indices)
                {
                    _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (uint)(indices.Length * sizeof(uint)), ptr, BufferUsageARB.StaticDraw);
                }
            }

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

            // Desvinculamos el VAO para evitar modificaciones accidentales
            _gl.BindVertexArray(0);
        }

        public void Bind()
        {
            _gl.BindVertexArray(_vao);
        }

        public void Unbind()
        {
            _gl.BindVertexArray(0);

        }

    }
}