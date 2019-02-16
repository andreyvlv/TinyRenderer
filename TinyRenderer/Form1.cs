using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using TinyRenderer.Helpers.ModelReader;
using TinyRenderer.Renderer;
using TinyRenderer.Renderer.RendererTypes;
using TinyRenderer.Helpers;

namespace TinyRenderer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var model = ModelFiles.GetAfricanHeadModel();
            //var model = ModelFiles.GetDiabloModel();

            sw.Stop();

            var renderer = new ModelRenderer(new ShadowShaderRenderer(model, pictureBox1));
            //var renderer = new ModelRenderer(new NoToneShaderRenderer(model, pictureBox1));
            //var renderer = new ModelRenderer(new ToonShaderRenderer(model, pictureBox1));
            //var renderer = new ModelRenderer(new NormalMapShaderRenderer(model, pictureBox1));
            renderer.Render();

            InfoViewer.ShowDiagnosticInfo
                ($"Loading Time: {sw.Elapsed.TotalMilliseconds:f2} ms", new PointF(10, 10), Graphics.FromImage(pictureBox1.Image));

            InfoViewer.ShowDiagnosticInfo
                ($"Triangles Count: {model.FacesV.Count}", new PointF(10, 50), Graphics.FromImage(pictureBox1.Image));

            InfoViewer.ShowDiagnosticInfo
                ($"Resolution: {pictureBox1.Width} X {pictureBox1.Height}", new PointF(10, 70), Graphics.FromImage(pictureBox1.Image));
        }
    }
}
