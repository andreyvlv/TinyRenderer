using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TinyRenderer.Geometry;
using TinyRenderer.Helpers.ModelReader;

namespace TinyRenderer.Shaders
{
    class NoToneShader : IShader
    {
        public VMatrix3x2 varying_uv;
        public VMatrix3x3 varying_tri;

        Model Model;
        Matrix4x4 ViewPort;
        Matrix4x4 Projection;
        Matrix4x4 ModelView;

        Matrix4x4 VPM;

        Vector3 light_dir;

        public NoToneShader(Model Model, Matrix4x4 ViewPort, Matrix4x4 Projection, Matrix4x4 ModelView, Vector3 light_dir)
        {
            this.Model = Model;
            this.ViewPort = ViewPort;
            this.Projection = Projection;
            this.ModelView = ModelView;
            this.light_dir = light_dir;

            VPM = ViewPort * Projection * ModelView;         

            varying_uv = new VMatrix3x2();
            varying_tri = new VMatrix3x3();
        }

        public Vector4 Vertex(int iface, int nthvert)
        {
            Vector3 gl_Vertex = Model.GetVertex(iface, nthvert);
            varying_uv.Set_Col(Model.GetUV(iface, nthvert), nthvert);
            varying_tri.Set_Col(gl_Vertex, nthvert);
            var m = VPM.MultiplyByVector3_V4(gl_Vertex);
            return m;
        }

        public Color Fragment(Vector3 bar)
        {
            Vector2 uv = VMatrix3x2.MultiplyMatrix3x2AndVec3(varying_uv, bar); // interpolate texture coordinates           
            var color = Model.Diffuse(uv);           

            var v1 = varying_tri.Get_Col(0);
            var v2 = varying_tri.Get_Col(1);
            var v3 = varying_tri.Get_Col(2);

            Vector3 v31 = v3 - v1;
            Vector3 v21 = v2 - v1;

            Vector3 n = Vector3.Cross(v31, v21);
            n = n.Normalize();

            float intensity = Vector3.Dot(n, light_dir);

            //color = Color.White; // for no texture rendering

            int r = ToByteRange(color.R * intensity);
            int g = ToByteRange(color.G * intensity);
            int b = ToByteRange(color.B * intensity);

            color = Color.FromArgb(r, g, b);
            return color;
        }

        int ToByteRange(double value)
        {
            return (int)Math.Max(0, Math.Min(value, 255));
        }

    }
}
