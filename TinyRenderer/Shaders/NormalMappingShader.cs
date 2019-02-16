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
    class NormalMappingShader : IShader
    {
        public VMatrix3x2 varying_uv;

        public Matrix4x4 uniform_m;
        public Matrix4x4 uniform_mit;

        Model Model;

        Matrix4x4 ViewPort;
        Matrix4x4 Projection;
        Matrix4x4 ModelView;

        Matrix4x4 VPM;

        Vector3 light_dir;

        public NormalMappingShader(Model Model, Matrix4x4 ViewPort, Matrix4x4 Projection, Matrix4x4 ModelView, Vector3 light_dir)
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
            return m;
        }

        public Color Fragment(Vector3 bar)
        {
            Vector2 uv = VMatrix3x2.MultiplyMatrix3x2AndVec3(varying_uv, bar);
            var color = Model.Diffuse(uv);

            var norm = Model.Normal(uv);
            var n = uniform_mit.MultiplyByVector3_V3(norm);
            n = n.Normalize();

            var l = uniform_m.MultiplyByVector3_V3(light_dir);
            l = l.Normalize();

            Vector3 ref_l = Vector3.Normalize((Vector3.Multiply(n, Vector3.Dot(n, l) * 2) - l));  // reflected light
            float spec = (float)Math.Pow(Math.Max(ref_l.Z, 0.0f), Model.Specular(uv));
            float diff = Math.Max(0, Vector3.Dot(n, l));

            int r = (int)Math.Min(2 + color.R * (diff + .49 * spec), 255); //5, 0.6
            int g = (int)Math.Min(2 + color.G * (diff + .49 * spec), 255);
            int b = (int)Math.Min(2 + color.B * (diff + .49 * spec), 255);

            color = Color.FromArgb(r, g, b);
            return color;
        }
    }
}
