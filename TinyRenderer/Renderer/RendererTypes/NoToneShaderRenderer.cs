using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Geometry;
using TinyRenderer.Helpers;
using TinyRenderer.Helpers.ModelReader;
using TinyRenderer.Shaders;

namespace TinyRenderer.Renderer.RendererTypes
{
    class NoToneShaderRenderer : IRenderer
    {
        PictureBox pictureBox;

        Model model;

        Vector3 light_dir;
        Vector3 eye;
        Vector3 center;
        Vector3 up;

        Matrix4x4 ModelView;
        Matrix4x4 Projection;
        Matrix4x4 ViewPort;

        int width;
        int height;

        float[] z_buffer;

        public NoToneShaderRenderer(Model model, PictureBox box)
        {
            pictureBox = box;
            this.model = model;

            width = pictureBox.Width;
            height = pictureBox.Height;

            light_dir = new Vector3(0, 0, -1);
            eye = new Vector3(0, 0, 3);
            center = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);

            Matrix4x4 Rot_X = Matrix4x4Extensions.Rotation_X(20);
            Matrix4x4 Rot_Y = Matrix4x4Extensions.Rotation_Y(15);
            eye = (Rot_X * Rot_Y).MultiplyByVector3_V3(eye);

            ModelView = Matrix4x4Extensions.LookAt(eye, center, up);
            Projection = Matrix4x4.Identity;
            ViewPort = Matrix4x4Extensions.Viewport(width / 8, height / 8, width * 3 / 4, height * 3 / 4);
            Projection.M43 = -1.0f / (eye - center).Norm();

            z_buffer = new float[pictureBox.Width * pictureBox.Height];
            for (int i = 0; i < z_buffer.Length; i++)
                z_buffer[i] = -float.MaxValue;

            light_dir.Normalize();
        }

        public void Render()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DirectBitmap bitmap = new DirectBitmap(width, height);

            Rasterizer rasterizer = new Rasterizer();

            //// start of picture render by no tone shader ////                    
            var nt_shader = new NoToneShader(model, ViewPort, Projection, ModelView, light_dir);

            for (int i = 0; i < model.FacesV.Count; i++)
            {
                Vector4[] screen_coords = new Vector4[3];
                for (int j = 0; j < model.FacesV[i].Length; j++)
                    screen_coords[j] = nt_shader.Vertex(i, j);

                rasterizer.Triangle(screen_coords, nt_shader, bitmap, pictureBox, z_buffer);
            }
            //// end of picture render by no tone shader ////

            sw.Stop();

            model.Texture.Dispose();
            bitmap.Bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);

            InfoViewer.ShowDiagnosticInfo
                ($"Render Time: {sw.Elapsed.TotalMilliseconds:f2} ms", new PointF(10, 30), Graphics.FromImage(bitmap.Bitmap));

            // output final render
            pictureBox.Image = bitmap.Bitmap;
        }
    }
}
