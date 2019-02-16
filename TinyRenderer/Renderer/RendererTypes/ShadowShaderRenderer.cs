using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Shaders;
using TinyRenderer.Geometry;
using System.Numerics;
using TinyRenderer.Helpers.ModelReader;
using TinyRenderer.Helpers;

namespace TinyRenderer.Renderer.RendererTypes
{
    class ShadowShaderRenderer : IRenderer
    {
        PictureBox pictureBox;

        Model model;
        
        Vector3 light_dir;        
        Vector3 eye;        
        Vector3 center;        
        Vector3 up;
       
        int width;
        int height;      

        float[] z_buffer;
        float[] shadow_buffer;

        public ShadowShaderRenderer(Model model, PictureBox box)
        {
            pictureBox = box;
            this.model = model;

            width = pictureBox.Width;
            height = pictureBox.Height;

            light_dir = new Vector3(1, 1, -0.4f);
            eye = new Vector3(0, 0, 3);
            center = new Vector3(0, 0, 0);
            up = new Vector3(0, 1, 0);

            Matrix4x4 Rot_X = Matrix4x4Extensions.Rotation_X(20);
            Matrix4x4 Rot_Y = Matrix4x4Extensions.Rotation_Y(15);
                        
            eye = (Rot_X * Rot_Y).MultiplyByVector3_V3(eye);

            z_buffer = new float[width * height];
            shadow_buffer = new float[width * height];           

            for (int i = 0; i < z_buffer.Length; i++)
                z_buffer[i] = shadow_buffer[i] = float.MinValue;

            light_dir = light_dir.Normalize();           
        }

        public void Render()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            DirectBitmap bitmap = new DirectBitmap(width, height);

            Rasterizer rasterizer = new Rasterizer();

            //// start of shadow buffer rendering
            Matrix4x4 SB_ModelView = Matrix4x4Extensions.LookAt(light_dir, center, up);
            Matrix4x4 SB_Projection = Matrix4x4.Identity;
            Matrix4x4 ViewPort = Matrix4x4Extensions.Viewport(width / 8, height / 8, width * 3 / 4, height * 3 / 4);
            SB_Projection.M43 = 0;            

            var shader = new DepthShader(model, ViewPort, SB_Projection, SB_ModelView);

            for (int i = 0; i < model.FacesV.Count; i++)
            {
                Vector4[] screen_coords = new Vector4[3];
                for (int j = 0; j < model.FacesV[i].Length; j++)
                    screen_coords[j] = shader.Vertex(i, j);

                rasterizer.Triangle(screen_coords, shader, bitmap, pictureBox, shadow_buffer, true);
            }
            //// end of shadow buffer rendering

            //// start of picture render by shadow shader ////
            Matrix4x4 M = ViewPort * SB_Projection * SB_ModelView;

            Matrix4x4 ModelView = Matrix4x4Extensions.LookAt(eye, center, up);
            Matrix4x4 Projection = Matrix4x4.Identity;
            Projection.M43 = -1.0f / (eye - center).Norm();

            var sh_shader = new ShadowShader(model, ViewPort, Projection, ModelView, light_dir, shadow_buffer, width);          

            sh_shader.uniform_m = Projection * ModelView;
            sh_shader.uniform_mit = sh_shader.uniform_m.Inverse().Transpose();
            sh_shader.uniform_m_shadow = M * (ViewPort * Projection * ModelView).Inverse();

            for (int i = 0; i < model.FacesV.Count; i++)
            {
                Vector4[] screen_coords = new Vector4[3];
                for (int j = 0; j < model.FacesV[i].Length; j++)
                    screen_coords[j] = sh_shader.Vertex(i, j);

                rasterizer.Triangle(screen_coords, sh_shader, bitmap, pictureBox, z_buffer);
            }
            //// end of picture render by shadow shader ////

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
