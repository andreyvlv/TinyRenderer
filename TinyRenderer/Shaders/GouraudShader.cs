using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using TinyRenderer.Helpers.ModelReader;
using TinyRenderer.Geometry;

namespace TinyRenderer.Shaders
{
    class GouraudShader : IShader
    {
        public Vector3 varying_intensity; // written by vertex shader, read by fragment shader
        public VMatrix3x2 varying_uv;

        Model Model;
        Matrix4x4 ViewPort;
        Matrix4x4 Projection;
        Matrix4x4 ModelView;

        Matrix4x4 VPM;

        Vector3 light_dir;

        public GouraudShader(Model Model, Matrix4x4 ViewPort, Matrix4x4 Projection, Matrix4x4 ModelView, Vector3 light_dir)
        {
            this.Model = Model;
            this.ViewPort = ViewPort;
            this.Projection = Projection;
            this.ModelView = ModelView;
            this.light_dir = light_dir;

            VPM = ViewPort * Projection * ModelView;
            varying_uv = new VMatrix3x2();
        }

        public Vector4 Vertex(int iface, int nthvert)
        {
            Vector3 gl_Vertex = Model.GetVertex(iface, nthvert);
            varying_uv.Set_Col(Model.GetUV(iface, nthvert), nthvert);
            var m = VPM.MultiplyByVector3_V4(gl_Vertex);
            switch (nthvert)
            {
                case 0:
                    varying_intensity.X = Math.Max(0, Vector3.Dot(Model.GetNormal(iface, nthvert), light_dir));
                    break;
                case 1:
                    varying_intensity.Y = Math.Max(0, Vector3.Dot(Model.GetNormal(iface, nthvert), light_dir));
                    break;
                case 2:
                    varying_intensity.Z = Math.Max(0, Vector3.Dot(Model.GetNormal(iface, nthvert), light_dir));
                    break;
            }
            return m;
        }

        //public Color Fragment(Vec3F bar, out bool discard)
        //{
        //    discard = false;
        //    Vec2f uv = varying_uv * bar.VecToMat(); // interpolate texture coordinates           
        //    var color = Model.Diffuse(uv);
        //    float intensity = varying_intensity * bar;   // interpolate intensity for the current pixel            
        //    int r = (int)Math.Min(255, color.R * intensity);
        //    int g = (int)Math.Min(255, color.G * intensity);
        //    int b = (int)Math.Min(255, color.B * intensity);
        //    color = Color.FromArgb(r, g, b);
        //    return color;
        //}

        public Color Fragment(Vector3 bar)
        {
            Vector2 uv = VMatrix3x2.MultiplyMatrix3x2AndVec3(varying_uv, bar); // interpolate texture coordinates           
            var color = Model.Diffuse(uv);
            float intensity = Vector3.Dot(varying_intensity, bar);   // interpolate intensity for the current pixel    

            if (intensity > .85) intensity = 1;
            else if (intensity > .60) intensity = 0.80f;
            else if (intensity > .45) intensity = 0.60f;
            else if (intensity > .30) intensity = 0.45f;
            else if (intensity > .15) intensity = 0.30f;
            else intensity = 0;

            int r = (int)Math.Min(255, color.R * intensity);
            int g = (int)Math.Min(0, color.G * intensity);
            int b = (int)Math.Min(200, color.B * intensity);
            color = Color.FromArgb(r, g, b);
            return color;
        }
    }
}
