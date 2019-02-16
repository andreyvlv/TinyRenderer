using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyRenderer.Helpers.ModelReader
{
    class ModelFiles
    {
        public static Model GetAfricanHeadModel()
        {
            var modelPath = Environment.CurrentDirectory + @"/Models/african_head/african_head.obj";
            var texturePath = Environment.CurrentDirectory + @"/Models/african_head/textures/african_head_diffuse.png";
            var normalMapPath = Environment.CurrentDirectory + @"/Models/african_head/textures/african_head_nm.png";
            var specularMapPath = Environment.CurrentDirectory + @"/Models/african_head/textures/african_head_spec.png";

            var m = ModelReader.Read(modelPath, texturePath, normalMapPath, specularMapPath);
           
            return m;
        }

        public static Model GetDiabloModel()
        {
            var modelPath = Environment.CurrentDirectory + @"/Models/diablo/diablo3_pose.obj";
            var texturePath = Environment.CurrentDirectory + @"/Models/diablo/textures/diablo3_pose_diffuse.png";
            var normalMapPath = Environment.CurrentDirectory + @"/Models/diablo/textures/diablo3_pose_nm.png";
            var specularMapPath = Environment.CurrentDirectory + @"/Models/diablo/textures/diablo3_pose_spec.png";

            return ModelReader.Read(modelPath, texturePath, normalMapPath, specularMapPath);
        }

    }
}
