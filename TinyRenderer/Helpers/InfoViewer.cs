using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TinyRenderer.Helpers
{
    static class InfoViewer
    {
        public static void ShowDiagnosticInfo(string info, PointF location, Graphics g)
        {
            using (g)            
                g.DrawString(info, new Font("Segoe UI", 12), new SolidBrush(Color.White), location);           
        }
    }
}
