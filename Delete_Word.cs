using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Delete_Word : Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        bool isAnhViet = true;
        public Delete_Word()
        {
            InitializeComponent();
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu nhập
                if (string.IsNullOrWhiteSpace(txtNhap.Text))
                {
                    MessageBox.Show("Vui lòng nhập từ cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(excelpath))
                {
                    MessageBox.Show("Không tìm thấy file Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FileInfo file = new FileInfo(excelpath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                bool found = false;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy Sheet đầu tiên
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    int colCount = worksheet.Dimension?.Columns ?? 0;

                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (worksheet.Cells[row, col].Value?.ToString() == txtNhap.Text)
                            {
                                worksheet.Cells[row, col].Value = ""; // Xóa từ
                                found = true;
                            }
                        }
                    }

                    if (found)
                    {
                        package.Save(); // Lưu thay đổi
                        MessageBox.Show("Xóa từ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy từ cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                txtNhap.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa từ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btNhaplai_Click(object sender, EventArgs e)
        {
            txtNhap.Text = "";
        }

        

        
        private void btChange_Click(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btChange.Text = isAnhViet ? "Anh - Việt" : "Việt - Anh";
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                Close(); // Đóng form nếu người dùng chọn OK
            }
        }

    }
}
