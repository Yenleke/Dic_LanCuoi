using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Panel : UserControl
    {
        public int CornerRadius { get; set; } = 20;
        public Panel()
        {
            this.BackColor = Color.White;
            this.Resize += (s, e) => this.Invalidate();

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (GraphicsPath path = new GraphicsPath())
            {
                int arcSize = CornerRadius * 2;
                Rectangle rect = this.ClientRectangle;

                path.StartFigure();
                path.AddArc(rect.X, rect.Y, arcSize, arcSize, 180, 90);
                path.AddArc(rect.Right - arcSize, rect.Y, arcSize, arcSize, 270, 90);
                path.AddArc(rect.Right - arcSize, rect.Bottom - arcSize, arcSize, arcSize, 0, 90);
                path.AddArc(rect.X, rect.Bottom - arcSize, arcSize, arcSize, 90, 90);
                path.CloseFigure();

                this.Region = new Region(path);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

    

}
}
