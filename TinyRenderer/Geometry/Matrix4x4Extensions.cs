using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Geometry
{
    static class Matrix4x4Extensions
    {
        public static Vector3 MultiplyByVector3_V3(this Matrix4x4 base_matrix, Vector3 v)
        {
            Vector4 vec1 = new Vector4(base_matrix.M11, base_matrix.M12, base_matrix.M13, base_matrix.M14);
            Vector4 vec2 = new Vector4(base_matrix.M21, base_matrix.M22, base_matrix.M23, base_matrix.M24);
            Vector4 vec3 = new Vector4(base_matrix.M31, base_matrix.M32, base_matrix.M33, base_matrix.M34);
            Vector4 vec4 = new Vector4(base_matrix.M41, base_matrix.M42, base_matrix.M43, base_matrix.M44);
            Vector4 v4 = new Vector4(v.X, v.Y, v.Z, 1);

            Vector4 res = new Vector4
                (
                    Vector4.Dot(vec1, v4),
                    Vector4.Dot(vec2, v4),
                    Vector4.Dot(vec3, v4),
                    Vector4.Dot(vec4, v4)
                );

            return new Vector3(res.X / res.W, res.Y / res.W, res.Z / res.W);
        }

        public static Vector4 MultiplyByVector3_V4(this Matrix4x4 base_matrix, Vector3 v)
        {
            Vector4 vec1 = new Vector4(base_matrix.M11, base_matrix.M12, base_matrix.M13, base_matrix.M14);
            Vector4 vec2 = new Vector4(base_matrix.M21, base_matrix.M22, base_matrix.M23, base_matrix.M24);
            Vector4 vec3 = new Vector4(base_matrix.M31, base_matrix.M32, base_matrix.M33, base_matrix.M34);
            Vector4 vec4 = new Vector4(base_matrix.M41, base_matrix.M42, base_matrix.M43, base_matrix.M44);
            Vector4 v4 = new Vector4(v.X, v.Y, v.Z, 1);

            Vector4 res = new Vector4
                (
                    Vector4.Dot(vec1, v4),
                    Vector4.Dot(vec2, v4),
                    Vector4.Dot(vec3, v4),
                    Vector4.Dot(vec4, v4)
                );

            return res;
        }

        public static Matrix4x4 Inverse(this Matrix4x4 base_matrix)
        {
            Matrix4x4 out_m;
            var base_m = Matrix4x4.Invert(base_matrix, out out_m);
            return out_m;
        }

        public static Matrix4x4 Transpose(this Matrix4x4 base_matrix)
        {
            var base_m = Matrix4x4.Transpose(base_matrix);
            return base_m;
        }

        public static Matrix4x4 Rotation_X(float angle)
        {
            float cosangle = (float)Math.Cos(angle * Math.PI / 180);
            float sinangle = (float)Math.Sin(angle * Math.PI / 180);
            Matrix4x4 R =Matrix4x4.Identity;            
            R.M22 = R.M33 = cosangle;
            R.M23 = -sinangle;
            R.M32 = sinangle;
            return R;
        }
    
        public static Matrix4x4 Rotation_Y(float angle)
        {
            float cosangle = (float)Math.Cos(angle * Math.PI / 180);
            float sinangle = (float)Math.Sin(angle * Math.PI / 180);
            Matrix4x4 R = Matrix4x4.Identity;
            R.M11 = R.M33 = cosangle;
            R.M13 = sinangle;
            R.M31 = -sinangle;
            return R;
        }

        public static Matrix4x4 LookAt(Vector3 eye, Vector3 center, Vector3 up)
        {
            Vector3 z = (eye - center);            
            z = z.Normalize();

            Vector3 x = Vector3.Cross(up, z);            
            x = x.Normalize();

            Vector3 y = Vector3.Cross(z, x);            
            y = y.Normalize();

            Matrix4x4 res = Matrix4x4.Identity;

            res.M11 = x.X;
            res.M21 = y.X;
            res.M31 = z.X;
            res.M14 = -center.X;

            res.M12 = x.Y;
            res.M22 = y.Y;
            res.M32 = z.Y;
            res.M24 = -center.Y;

            res.M13 = x.Z;
            res.M23 = y.Z;
            res.M33 = z.Z;
            res.M34 = -center.Z;

            return res;
        }

        public static Matrix4x4 Viewport(int x, int y, int w, int h)
        {
            Matrix4x4 m = Matrix4x4.Identity;
            m.M14 = x + w / 2.0f;
            m.M24 = y + h / 2.0f;
            m.M34 = 255 / 2.0f;

            m.M11 = w / 2.0f;
            m.M22 = h / 2.0f;
            m.M33 = 255 / 2.0f;
            return m;
        }
    }
}
