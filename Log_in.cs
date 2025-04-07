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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Dic_AppTest
{
    public partial class Log_in : Form
    {
        public Log_in()
        {
            InitializeComponent();
        }
        private bool close;
        private string welcomeText = "";
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Vui lòng nhập user name", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(txtUser.Text == "")
            {
                Application.Exit();
            }
            
            FrmMain.ten = txtUser.Text;
            welcomeText = "Welcome, " + txtUser.Text + "!";
            lbWelcome.Visible = true;
            lbWelcome.Text = welcomeText;
            close = false;
            this.Close();
        }
       
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                btLogin.PerformClick(); // Giả lập nhấn nút Login
                return true;
            }
            return base.ProcessDialogKey(keyData);
        


        }
        private void Log_in_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(this.ClientRectangle, Color.LightSteelBlue, Color.White, 90);
            e.Graphics.FillRectangle(linearGradientBrush, this.ClientRectangle);
        }
        private void Log_in_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(FrmMain.ten == "")
            {
                Application.Exit();
            }
            if (!close)
            {               
                e.Cancel = true;
                timer2.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.05;
            if (this.Opacity <= 0)
            {
                close = true;
                this.Close();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            timer1.Enabled = true;
        }
    }
}
