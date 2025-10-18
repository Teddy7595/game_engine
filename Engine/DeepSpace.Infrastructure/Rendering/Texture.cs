using DeepSpace.Application.Interfaces;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace DeepSpace.Infrastructure.Rendering
{
    public class DSTexture : ITexture
    {
        public uint Handle { get; private set; }
        private readonly GL _gl;

        public DSTexture(GL gl, string path)
        {
            _gl = gl;

            //1) Generar el manejador de textura
            Handle = _gl.GenTexture();
            Bind();
            //2) Leer la imagen desde el disco en bytes
            byte[] fileData = File.ReadAllBytes(path);

            ImageResult image = ImageResult.FromMemory(fileData, ColorComponents.RedGreenBlueAlpha);
            if (image == null || image.Data == null)
                throw new Exception($"Failed to load texture from path: {path}");

            //3) Mandar la imagen a la tarjeta gráfica
            unsafe
            {
                fixed (byte* dataPtr = image.Data)
                {
                    _gl.TexImage2D(TextureTarget.Texture2D,
                                   0,
                                   InternalFormat.Rgba,
                                   (uint)image.Width,
                                   (uint)image.Height,
                                   0,
                                   PixelFormat.Rgba,
                                   PixelType.UnsignedByte,
                                   dataPtr);
                }
            }
            //4) Configurar parámetros de la textura (muestreo, wrap, etc)
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.Repeat);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.Repeat);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.LinearMipmapLinear);
            _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
            _gl.GenerateMipmap(TextureTarget.Texture2D);
            //5) Desvincular la textura
            Unbind();
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            _gl.ActiveTexture(unit);
            _gl.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public void Unbind()
        {
            _gl.BindTexture(TextureTarget.Texture2D, 0);
        }
    }

}