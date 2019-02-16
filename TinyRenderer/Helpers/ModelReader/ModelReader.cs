using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Helpers.ModelReader
{
    class ModelReader
    {
        public static Model Read(string modelPath, string texturePath, string normalMapPath, string specularMapPath)
        {
            var lines = File.ReadLines(modelPath);

            var model = new Model();


            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = ",";

            foreach (var line in lines)
            {
                // vertices line have the following form:  v 0.23 0.45 0.67
                if (line.Length > 2 && line[0] == 'v' && line[1] != 't' && line[1] != 'n')
                {
                    var spl = line.Split(' ').Skip(1).ToArray();
                    var vec = new Vector3
                        (
                            Convert.ToSingle(spl[0], provider),
                            Convert.ToSingle(spl[1], provider),
                            Convert.ToSingle(spl[2], provider)
                        );                   
                    model.Verteces.Add(vec);
                }

                // texture vertices line have the following form:  vt 0.23 0.45 0.00 (the third coordinate is not needed)
                if (line.StartsWith("vt"))
                {
                    var spl = line.Split(' ').Skip(2).ToArray();
                    var arr = new List<float>();
                    foreach (var item in spl)
                    {
                        var val = Convert.ToSingle(item, provider);
                        arr.Add(val);
                    }
                    var vec = new Vector2(arr[0], arr[1]);
                    model.TextureVerteces.Add(vec);
                }

                // normal vertices line have the following form: vn 0.23 0.45 0.67

                if (line.StartsWith("vn"))
                {
                    var spl = line.Split(' ').Skip(2).ToArray();
                    var vec = new Vector3
                       (
                           Convert.ToSingle(spl[0], provider),
                           Convert.ToSingle(spl[1], provider),
                           Convert.ToSingle(spl[2], provider)
                       );
                    model.Normals.Add(vec);
                }

                // lines starting with f indicate which points make up a triangle in verteces, tex vert and normal vert
                // for example:
                // first line is "f 24/1/24 25/2/25 26/3/26"
                // therefore, the first triangle is described in "v" lines 24, 25 and 26 and
                // has coordinates:
                // 1 => X: 0,134781, Y: -0,14723, Z: 0,48805
                // 2 => X: 0,131261, Y: -0,132153, Z: 0,49872
                // 3 => X: 0,14749, Y: -0,135105, Z: 0,489565
                // the second indices are the triangles of the texture space
                // the third indeces are the normals
                if (line.StartsWith("f"))
                {
                    var faceV = new int[3];
                    var faceT = new int[3];
                    var faceN = new int[3];

                    var spl = line.Split(' ').Skip(1).ToArray();

                    for (int i = 0; i < spl.Length; i++)
                    {
                        var spl1 = spl[i].Split('/').ToArray();
                        var facev = Convert.ToInt32(spl1[0]);
                        var facet = Convert.ToInt32(spl1[1]);
                        var facen = Convert.ToInt32(spl1[2]);
                        faceV[i] = facev;
                        faceT[i] = facet;
                        faceN[i] = facen;
                    }

                    model.FacesV.Add(faceV);
                    model.FacesT.Add(faceT);
                    model.FacesN.Add(faceN);
                }
            }            

            DirectBitmap texture = new DirectBitmap(1, 1);
            DirectBitmap normalMap = new DirectBitmap(1, 1);
            DirectBitmap specularMap = new DirectBitmap(1, 1);            

            Parallel.Invoke
               (
                   () => LoadTexture(texturePath, out texture),
                   () => LoadTexture(normalMapPath, out normalMap),
                   () => LoadTexture(specularMapPath, out specularMap)
               );

            model.Texture = texture;
            model.NormalMap = normalMap;
            model.SpecularMap = specularMap;
            return model;
        }       

        static void LoadTexture(string path, out DirectBitmap bitmap)
        {            
            var texture = new Bitmap(System.Drawing.Image.FromFile(path));
            texture.RotateFlip(RotateFlipType.Rotate180FlipX);
            bitmap = new DirectBitmap(texture.Width, texture.Height, texture);
        }

    }
}
