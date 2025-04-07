using OfficeOpenXml;
using System;
using System.IO;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Sub_AddWord : Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx";

        public Sub_AddWord()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(txtTuVung.Text) || string.IsNullOrWhiteSpace(txtNghia.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ từ vựng và nghĩa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FileInfo file = new FileInfo(excelpath);
                using (ExcelPackage package = file.Exists ? new ExcelPackage(file) : new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0 ?
                        package.Workbook.Worksheets[0] : package.Workbook.Worksheets.Add("TuDien");

                    // Xác định số dòng hiện tại
                    int rowCount = worksheet.Dimension?.Rows ?? 0;

                    if (rowCount == 0)
                    {
                        // Nếu file trống, thêm tiêu đề
                        worksheet.Cells[1, 1].Value = "Từ vựng";
                        worksheet.Cells[1, 2].Value = "Phiên âm";
                        worksheet.Cells[1, 3].Value = "Loại từ";
                        worksheet.Cells[1, 4].Value = "Nghĩa";
                        worksheet.Cells[1, 5].Value = "Ví dụ 1";
                        worksheet.Cells[1, 6].Value = "Ví dụ 2";
                        rowCount = 1; // Bắt đầu từ dòng 2
                    }
                    else
                    {
                        rowCount++; // Xuống dòng tiếp theo
                    }

                    // Ghi dữ liệu vào hàng mới
                    worksheet.Cells[rowCount, 1].Value = txtTuVung.Text;
                    worksheet.Cells[rowCount, 2].Value = txtPhienAm.Text;
                    worksheet.Cells[rowCount, 3].Value = txtTuloai.Text;
                    worksheet.Cells[rowCount, 4].Value = txtNghia.Text;
                    worksheet.Cells[rowCount, 5].Value = txtVidu1.Text;
                    worksheet.Cells[rowCount, 6].Value = txtVidu2.Text;

                    package.Save(); // Lưu lại file
                }

                MessageBox.Show("Thêm từ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa dữ liệu nhập sau khi lưu
                txtTuVung.Clear();
                txtPhienAm.Clear();
                txtTuloai.Clear();
                txtNghia.Clear();
                txtVidu1.Clear();
                txtVidu2.Clear();

                // Cập nhật lại danh sách từ vựng trong form tra từ điển
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu từ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btNhapLai_Click(object sender, EventArgs e)
        {
            txtTuVung.Clear();
            txtPhienAm.Clear();
            txtTuloai.Clear();
            txtNghia.Clear();
            txtVidu1.Clear();
            txtVidu2.Clear();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Sub_AddWord_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đóng form?", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.Cancel)
                e.Cancel = true;
        }

       

        private void txtTuVung_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtPhienAm_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtTuloai_TextChanged(object sender, EventArgs e)
        {

        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtVidu2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtVidu1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNghia_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Load(object sender, EventArgs e)
        {

        }
    }
}
