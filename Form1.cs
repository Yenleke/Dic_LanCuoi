using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btSwitch_Click(object sender, EventArgs e)
        {
            btSwitch.Text = (btSwitch.Text == "Anh - Việt") ? "Việt - Anh" : "Anh - Việt";
        }
    }
}
