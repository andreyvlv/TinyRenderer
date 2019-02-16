using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using TinyRenderer.Helpers;
using TinyRenderer.Shaders;
using TinyRenderer.Geometry;

namespace TinyRenderer.Renderer
{
    class Rasterizer
    {        
        public void Triangle(Vector4[] pts, IShader shader, DirectBitmap bitmap, PictureBox box, float[] z_buffer, bool isShadowBuffer = false)
        {                       
            Vector2 b_box_min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 b_box_max = new Vector2(float.MinValue, float.MinValue);

            // i = 0; j = 0;
            b_box_min.X = Math.Min(b_box_min.X, pts[0].X / pts[0].W);
            b_box_max.X = Math.Max(b_box_max.X, pts[0].X / pts[0].W);

            // i = 0; j = 1;
            b_box_min.Y = Math.Min(b_box_min.Y, pts[0].Y / pts[0].W);
            b_box_max.Y = Math.Max(b_box_max.Y, pts[0].Y / pts[0].W);

            // i = 1; j = 0;
            b_box_min.X = Math.Min(b_box_min.X, pts[1].X / pts[1].W);
            b_box_max.X = Math.Max(b_box_max.X, pts[1].X / pts[1].W);

            // i = 1; j = 1;
            b_box_min.Y = Math.Min(b_box_min.Y, pts[1].Y / pts[1].W);
            b_box_max.Y = Math.Max(b_box_max.Y, pts[1].Y / pts[1].W);

            // i = 2; j = 0;
            b_box_min.X = Math.Min(b_box_min.X, pts[2].X / pts[2].W);
            b_box_max.X = Math.Max(b_box_max.X, pts[2].X / pts[2].W);

            // i = 1; j = 1;
            b_box_min.Y = Math.Min(b_box_min.Y, pts[2].Y / pts[2].W);
            b_box_max.Y = Math.Max(b_box_max.Y, pts[2].Y / pts[2].W);           

            Vec2i P;
            Color color;

            Vector3 z_vec = new Vector3(pts[0].Z, pts[1].Z, pts[2].Z);
            Vector3 w_vec = new Vector3(pts[0].W, pts[1].W, pts[2].W);

            for (P.X = (int)b_box_min.X; P.X <= (int)b_box_max.X; P.X++)
            {
                for (P.Y = (int)b_box_min.Y; P.Y <= (int)b_box_max.Y; P.Y++)
                {
                    Vector3 c = Barycentric
                    (
                        new Vector2(pts[0].X / pts[0].W, pts[0].Y / pts[0].W),
                        new Vector2(pts[1].X / pts[1].W, pts[1].Y / pts[1].W),
                        new Vector2(pts[2].X / pts[2].W, pts[2].Y / pts[2].W),
                        new Vector2(P.X, P.Y)
                    );

                    float z = Vector3.Dot(z_vec, c);
                    float w = Vector3.Dot(w_vec, c);                    

                    int frag_depth = (int)(z / w);

                    if (c.X < 0 || c.Y < 0 || c.Z < 0 || z_buffer[P.X + P.Y * box.Width] > frag_depth) continue;
                   
                    color = shader.Fragment(c);
                    
                    if (isShadowBuffer)
                        z_buffer[P.X + P.Y * box.Width] = frag_depth;
                    else
                    {
                        z_buffer[P.X + P.Y * box.Width] = frag_depth;
                        bitmap.SetPixel(P.X, P.Y, color);
                    }                   
                }
            }
        }       

        Vector3 Barycentric(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {            
            var s1 = new Vector3(C.Y - A.Y, B.Y - A.Y, A.Y - P.Y);
            var s2 = new Vector3(C.X - A.X, B.X - A.X, A.X - P.X);
           
            var u = Vector3.Cross(s1, s2);

            if (Math.Abs(u.Z) < 1e-2f)
                return new Vector3(-1, 1, 1);

            return new Vector3(1.0f - (u.X + u.Y) / u.Z, u.Y / u.Z, u.X / u.Z);
        }
    }
}
