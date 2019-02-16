using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Geometry
{
    struct Vec2i
    {
        public int X;
        public int Y;

        public Vec2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vec2i(float x, float y)
        {
            X = (int)(x + 0.5f);
            Y = (int)(y + 0.5f);
        }              
    }
}
