using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using TinyRenderer.Geometry;

namespace TinyRenderer.Helpers.ModelReader
{      
    class Model
    {
        public List<Vector3> Verteces { get; set; }
        public List<Vector2> TextureVerteces { get; set; }
        public List<Vector3> Normals { get; set; }

        public List<int[]> FacesV { get; set; }
        public List<int[]> FacesT { get; set; }
        public List<int[]> FacesN { get; set; }

        public DirectBitmap Texture {get; set;}
        public DirectBitmap NormalMap { get; set; }
        public DirectBitmap SpecularMap { get; set; }

        public Model()
        {
            Verteces = new List<Vector3>();
            TextureVerteces = new List<Vector2>();
            Normals = new List<Vector3>();
            
            FacesV = new List<int[]>();
            FacesT = new List<int[]>();
            FacesN = new List<int[]>();
        }

        public Vector3 GetVertex(int iface, int nvert)
        {
            var v = Verteces[FacesV[iface][nvert] - 1];
            return v;
        }       

        public Vector2 GetUV(int iface, int nvert)
        {
            var vt = TextureVerteces[FacesT[iface][nvert] - 1];
            return vt;
        }

        public Color Diffuse(Vector2 vec)
        {           
            return Texture.GetPixel((int)(vec.X * Texture.Width), (int)(vec.Y * Texture.Height));
        }

        public float Specular(Vector2 vec)
        {            
            var color = SpecularMap.GetPixel((int)(vec.X * SpecularMap.Width), (int)(vec.Y * SpecularMap.Height));
            return color.B;
        }

        public Vector3 Normal(Vector2 vec)
        {            
            var color = NormalMap.GetPixel((int)(vec.X * NormalMap.Width), (int)(vec.Y * NormalMap.Height));
            return new Vector3(color.R, color.G, color.B);
        }

        public Vector3 GetNormal(int iface, int nvert)
        {
            return Normals[FacesN[iface][nvert] - 1];
        }
    }
}
