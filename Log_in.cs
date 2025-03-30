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
    public partial class Log_in : Form
    {
        public Log_in()
        {
            InitializeComponent();
        }
        private List<User> users = new List<User>();
        public static User loginUser { get; private set; } 
        
        private void btLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Vui lòng nhập user name", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string userName = txtUser.Text;
            User user = new User(userName);
            users.Add(user);
            FrmMain.ten = userName;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
