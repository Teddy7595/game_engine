using Core;
using Silk.NET.OpenGL;
using System.Numerics;
using System;

namespace Application.OpenGL
{
    public class RenderOpenGLAdapter : IRender
    {
        private GL _gl;
        private Silk.NET.Windowing.IWindow _window;
        public bool IsRunning => _window != null && !_window.IsClosing;

        public void Initialize(int width, int height, string title)
        {
            var options = Silk.NET.Windowing.WindowOptions.Default;
            options.Size = new Silk.NET.Maths.Vector2D<int>(width, height);
            options.Title = title;
            options.API = new Silk.NET.Windowing.GraphicsAPI(
                Silk.NET.Windowing.ContextAPI.OpenGL,
                Silk.NET.Windowing.ContextProfile.Core,
                Silk.NET.Windowing.ContextFlags.Default,
                new Silk.NET.Windowing.APIVersion(4, 1)
            );
            _window = Silk.NET.Windowing.Window.Create(options);
            _window.Load += OnLoad;
            _window.Render += OnRender;
            _window.Closing += OnClose;
            _window.Run(() => { /* Optionally handle per-frame logic here */ });
        }

        private void OnLoad()
        {
            this._gl = this._window.CreateOpenGL();
            this._gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            this._gl.Enable(EnableCap.DepthTest);

            // Initialize shaders, buffers, etc. here
            //SetupCubeAndShaders(this._gl);
        }

        private void OnClose()
        {
            // Cleanup resources here
            //CleanupCubeAndShaders(this._gl);
            // Example: if you have a VAO handle stored, delete it here
            // this._gl?.DeleteVertexArray(vaoHandle);
        }

        private void OnRender(double deltaTime)
        {
            BeginDraw();
            EndDraw();
        }

        private void BeginDraw()
        {
            throw new NotImplementedException();
        }

        private void EndDraw()
        {
            throw new NotImplementedException();
        }

        private void Clear(Vector4 color)
        {
            throw new NotImplementedException();
        }

        private void DrawMesh(Guid meshId, Guid materialId, Matrix4x4 transform)
        {
            throw new NotImplementedException();
        }
    }
}