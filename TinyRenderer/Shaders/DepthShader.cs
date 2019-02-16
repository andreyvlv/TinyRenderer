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
    class DepthShader : IShader
    {
        public VMatrix3x3 varying_tri;
        
        Model Model;
        
        Matrix4x4 ViewPort;        
        Matrix4x4 Projection;        
        Matrix4x4 ModelView;

        Matrix4x4 VPM;

        public DepthShader(Model Model, Matrix4x4 ViewPort, Matrix4x4 Projection, Matrix4x4 ModelView)
        {
            this.Model = Model;
            this.ViewPort = ViewPort;
            this.Projection = Projection;
            this.ModelView = ModelView;           

            VPM = ViewPort * Projection * ModelView;           
            varying_tri = new VMatrix3x3();
        }

        public Vector4 Vertex(int iface, int nthvert)
        {
            Vector3 gl_Vertex = Model.GetVertex(iface, nthvert);            
            var m = VPM.MultiplyByVector3_V4(gl_Vertex);
            varying_tri.Set_Col(m.ToVector3(), nthvert);
            return m;           
        }

        public Color Fragment(Vector3 bar)
        {           
            var m = VMatrix3x3.MultiplyMatrix3x3AndVec3f(varying_tri, bar);

            int r = (int)Math.Min(255 * (m.Z / 2000), 255);
            int g = (int)Math.Min(255 * (m.Z / 2000), 255);
            int b = (int)Math.Min(255 * (m.Z / 2000), 255);

            var color = Color.FromArgb(r, g, b);
            return color;
        }
        
    }
}
