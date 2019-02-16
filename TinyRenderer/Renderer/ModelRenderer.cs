using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TinyRenderer.Renderer.RendererTypes;

namespace TinyRenderer.Renderer
{
    class ModelRenderer
    {
        IRenderer renderer;

        public ModelRenderer(IRenderer renderer)
        {
            this.renderer = renderer;
        }

        public void Render()
        {
            renderer.Render();
        }               
    }
}
