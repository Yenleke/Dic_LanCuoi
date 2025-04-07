using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing; 
using System.Windows.Forms;
namespace Dic_AppTest
{
    public class Set
    {
        private TextBox txtNhap;
        private string placeholder;
        private Color placeholderColor = Color.Gray;
        private Color textColor = Color.Black;

        public Set(TextBox txtNhap, string placeholder)
        {
            this.txtNhap = txtNhap;
            this.placeholder = placeholder;

            // Gán sự kiện khi focus và mất focus
            txtNhap.Enter += RemovePlaceholder;
            txtNhap.Leave += SetPlaceholder;

            // Khởi tạo placeholder nếu TextBox đang trống
            SetPlaceholder(null, null);
        }

        private void SetPlaceholder(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNhap.Text))
            {
                txtNhap.Text = placeholder;
                txtNhap.ForeColor = placeholderColor;
            }
        }

        private void RemovePlaceholder(object sender, EventArgs e)
        {
            if (txtNhap.Text == placeholder)
            {
                txtNhap.Text = "";
                txtNhap.ForeColor = textColor;
            }
        }
    }
}
