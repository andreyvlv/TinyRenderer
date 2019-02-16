using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TinyRenderer.Geometry
{
    static class Vector3Extensions
    {
        public static Vector3 Normalize(this Vector3 vector3)
        {
            float norm = (float)Math.Sqrt(vector3.X * vector3.X + vector3.Y * vector3.Y + vector3.Z * vector3.Z);
            vector3.X /= norm;
            vector3.Y /= norm;
            vector3.Z /= norm;
            return vector3;
        }

        public static float Norm(this Vector3 vector3)
        {
            return (float)Math.Sqrt(vector3.X * vector3.X + vector3.Y * vector3.Y + vector3.Z * vector3.Z);
        }

        public static Vector3 ToVector3(this Vector4 vector)
        {
            return new Vector3(vector.X / vector.W, vector.Y / vector.W, vector.Z / vector.W);
        }
    }
}
