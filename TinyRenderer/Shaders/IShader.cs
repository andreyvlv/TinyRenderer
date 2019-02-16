using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TinyRenderer.Shaders
{
    interface IShader
    {
        Vector4 Vertex(int iface, int nthvert);
        Color Fragment(Vector3 bar);
    }
}
