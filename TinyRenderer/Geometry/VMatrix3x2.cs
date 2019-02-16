using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TinyRenderer.Geometry
{
    class VMatrix3x2
    {
        Vector3[] body;

        public VMatrix3x2(float[][] body)
        {
            this.body = new Vector3[2];
            this.body[0] = new Vector3(body[0][0], body[0][1], body[0][2]);
            this.body[1] = new Vector3(body[1][0], body[1][1], body[1][2]);            
        }

        public VMatrix3x2()
        {
            this.body = new Vector3[2]
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)               
            };
        }

        public VMatrix3x2(Vector3[] body)
        {
            this.body = body;
        }

        public VMatrix3x2(Vector3 v1, Vector3 v2)
        {
            this.body = new Vector3[2]
            {
                v1, v2
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

        public void Set_Col(Vector2 v, int col_num)
        {            
            this[0, col_num] = v.X;
            this[1, col_num] = v.Y;
        }       

        public static Vector2 MultiplyMatrix3x2AndVec3(VMatrix3x2 var_t, Vector3 bar)
        {
            Vector2 m = new Vector2
                (
                    Vector3.Dot(var_t.body[0], bar),
                    Vector3.Dot(var_t.body[1], bar)                    
                );
            return m;
        }
    }
}
