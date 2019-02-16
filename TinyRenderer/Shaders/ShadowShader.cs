using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyRenderer.Geometry;
using System.Numerics;
using TinyRenderer.Helpers.ModelReader;

namespace TinyRenderer.Shaders
{
    class ShadowShader : IShader
    {
        VMatrix3x2 varying_uv;
        VMatrix3x3 varying_tri;

        public Matrix4x4 uniform_m;
        public Matrix4x4 uniform_mit;
        public Matrix4x4 uniform_m_shadow;

        float[] shadow_buffer;

        Model Model;

        Matrix4x4 ViewPort;
        Matrix4x4 Projection;
        Matrix4x4 ModelView;

        Matrix4x4 VPM;

        Vector3 light_dir;

        int width;

        public ShadowShader(Model Model, Matrix4x4 ViewPort, Matrix4x4 Projection,
            Matrix4x4 ModelView,
            Vector3 light_dir, float[] shadow_buffer, int width)
        {
            this.Model = Model;
            this.ViewPort = ViewPort;
            this.Projection = Projection;
            this.ModelView = ModelView;            
            this.light_dir = light_dir;
            this.shadow_buffer = shadow_buffer;
            this.width = width;          

            VPM = ViewPort * Projection * ModelView;           

            varying_uv = new VMatrix3x2();
            varying_tri = new VMatrix3x3();
        }

        public Vector4 Vertex(int iface, int nthvert)
        {
            Vector3 gl_Vertex = Model.GetVertex(iface, nthvert);
            varying_uv.Set_Col(Model.GetUV(iface, nthvert), nthvert);                       
            var m = VPM.MultiplyByVector3_V4(gl_Vertex);
            varying_tri.Set_Col(m.ToVector3(), nthvert);
            return m;
        }      

        public Color Fragment(Vector3 bar)
        {            
            Vector3 m = VMatrix3x3.MultiplyMatrix3x3AndVec3f(varying_tri, bar);                     
            Vector3 sb = uniform_m_shadow.MultiplyByVector3_V3(m);
                        
            int idx = (int)(sb.X) + (int)(sb.Y) * width;

            var closestDepth = shadow_buffer[idx];
            var currentDepth = sb.Z;

            var shadow = 0.3f + 0.7f * (closestDepth < currentDepth + 43.34f ? 1.2f : 0.45f);  //1.2, 0.45

            // interpolate texture coordinates            
                        
            Vector2 uv = VMatrix3x2.MultiplyMatrix3x2AndVec3(varying_uv, bar);           
            var color = Model.Diffuse(uv);
                                      
            var norm = Model.Normal(uv);            
            var n = uniform_mit.MultiplyByVector3_V3(norm);          
            n = n.Normalize();
                       
            var l = uniform_m.MultiplyByVector3_V3(light_dir);            
            l = l.Normalize();

            // reflected light value
            
            var ref_l = Vector3.Normalize((Vector3.Multiply(n, Vector3.Dot(n, l) * 2) - l));
            
            float spec = (float)Math.Pow(Math.Max(ref_l.Z, 0.0f), Model.Specular(uv));
            float diff = Math.Max(0, Vector3.Dot(n, l));

            //color = Color.White; // for no texture testing

            int r = ToByteRange(2 + color.R * shadow * (diff + .49 * spec));
            int g = ToByteRange(2 + color.G * shadow * (diff + .49 * spec));
            int b = ToByteRange(2 + color.B * shadow * (diff + .49 * spec));            

            color = Color.FromArgb(r, g, b);
            return color;
        }

        int ToByteRange(double value)
        {           
            return (int)Math.Max(0, Math.Min(value, 255));
        }
    }
}
