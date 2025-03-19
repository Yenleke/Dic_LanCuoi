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
    public partial class History: Form
    {
        public History()
        {
            InitializeComponent();
            LoadHistory();
            // Bo góc cho Button
            RoundedControl.SetRoundedRegion(btXoa1tu, 25);
            RoundedControl.SetRoundedRegion(btXoaall, 25);

            // Bo góc cho ListBox (nếu cần)
            RoundedControl.SetRoundedRegion(listBox1, 10);

            RoundedControl.SetRoundedRegion(this, 10); // Bo góc form
        }
        SearchHistory history = new SearchHistory();
        private BindingSource bindingSource = new BindingSource();


        // Tải lịch sử vào ListBox
        private void LoadHistory()
        {
            // Xóa danh sách cũ trước khi thêm mới
            history.ClearHistory();  // Đảm bảo không bị trùng lặp dữ liệu

            // Thêm một số từ mẫu để kiểm tra
            history.AddToHistory("Apple");
            history.AddToHistory("Orange");
            history.AddToHistory("Banana");
            history.AddToHistory("Grapes");
            history.AddToHistory("Mango");

            // Cập nhật DataSource
            bindingSource.DataSource = history.GetHistoryDisplay();
            bindingSource.ResetBindings(false); // Cập nhật ListBox
            listBox1.DataSource = bindingSource;
        }


        private void btXoa1tu_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedEntry = listBox1.SelectedItem.ToString();
                string selectedWord = selectedEntry.Split('-')[0].Trim();
                history.RemoveFromHistory(selectedWord);

                //LoadHistory();

                // Cập nhật danh sách hiển thị mà không load lại danh sách mẫu
                bindingSource.DataSource = history.GetHistoryDisplay();
                bindingSource.ResetBindings(false);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một từ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btXoaall_Click(object sender, EventArgs e)
        {
            history.ClearHistory();
            //LoadHistory();

            // Cập nhật danh sách hiển thị mà không load lại danh sách mẫu
            bindingSource.DataSource = history.GetHistoryDisplay();
            bindingSource.ResetBindings(false);
        }

        public class RoundedControl
        {
            public static void SetRoundedRegion(Control control, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
                path.CloseFigure();

                control.Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RoundedControl.SetRoundedRegion(this, 20); // Bo góc form
        }

       
        //private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    if (listBox1.SelectedItem != null)
        //    {
        //        string selectedEntry = listBox1.SelectedItem.ToString();
        //        string selectedWord = selectedEntry.Split('-')[0].Trim();

        //        History dictionaryForm = new History();
        //        dictionaryForm.SearchWord(selectedWord);
        //        dictionaryForm.Show();
        //    }
        //}
    }
}
