using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public class RoundedPanel : Panel
    {
        public int CornerRadius { get; set; } = 20; // Độ bo góc (mặc định 20)

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = new GraphicsPath())
            {
                int arcSize = CornerRadius * 2;
                path.AddArc(0, 0, arcSize, arcSize, 180, 90);
                path.AddArc(Width - arcSize, 0, arcSize, arcSize, 270, 90);
                path.AddArc(Width - arcSize, Height - arcSize, arcSize, arcSize, 0, 90);
                path.AddArc(0, Height - arcSize, arcSize, arcSize, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path); // Cắt panel theo đường bo góc
                using (Pen pen = new Pen(Color.Black, 2)) // Viền (tùy chọn)
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
