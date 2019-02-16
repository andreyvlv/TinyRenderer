using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TinyRenderer.Geometry
{
    class VMatrix3x3
    {
        Vector3[] body;

        public VMatrix3x3(float[][] body)
        {
            this.body = new Vector3[3];
            this.body[0] = new Vector3(body[0][0], body[0][1], body[0][2]);
            this.body[1] = new Vector3(body[1][0], body[1][1], body[1][2]);
            this.body[2] = new Vector3(body[2][0], body[2][1], body[2][2]);
        }

        public VMatrix3x3()
        {
            this.body = new Vector3[3]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)
            };
        }

        public VMatrix3x3(Vector3[] body)
        {
            this.body = body;
        }

        public VMatrix3x3(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            this.body = new Vector3[3]
            {
                v1, v2, v3
            };
        }

        public float this[int index1, int index2]
        {
            set
            {
                switch (index2)
                {
                    case 0:
                        body[index1].X = value;
                        break;
                    case 1:
                        body[index1].Y = value;
                        break;
                    case 2:
                        body[index1].Z = value;
                        break;                   
                }
            }

            get
            {
                switch (index2)
                {
                    case 0:
                        return body[index1].X;
                    case 1:
                        return body[index1].Y;
                    case 2:
                        return body[index1].Z;                   
                    default:
                        return 0;
                }
            }
        }

        public static VMatrix3x3 Identity(int dimensions)
        {
            float[][] body = new float[dimensions][];

            for (int i = 0; i < dimensions; i++)
            {
                body[i] = new float[dimensions];

                for (int j = 0; j < dimensions; j++)
                {
                    body[i][j] = (i == j ? 1.0f : 0.0f);
                }
            }
            return new VMatrix3x3(body);
        }

        public void Set_Col(Vector3 v, int col_num)
        {           
            this[0, col_num] = v.X;
            this[1, col_num] = v.Y;
            this[2, col_num] = v.Z;
        }

        public Vector3 Get_Col(int col_num)
        {
            return new Vector3(this[0, col_num], this[1, col_num], this[2, col_num]);
        }

        public static Vector3 MultiplyMatrix3x3AndVec3f(VMatrix3x3 var_t, Vector3 bar)
        {
            Vector3 m = new Vector3
                (
                    Vector3.Dot(var_t.body[0], bar),
                    Vector3.Dot(var_t.body[1], bar),
                    Vector3.Dot(var_t.body[2], bar)
                );
            return m;
        }
    }
}
