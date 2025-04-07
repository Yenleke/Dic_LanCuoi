using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Delete_Word : Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx"; //lấy dữ liệu từ file để xóa
        bool isAnhViet = true; 
        public Delete_Word()
        {
            InitializeComponent();
            new SetupTextup(txtNhap, "Type here...");
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNhap.Text)) //txt trống thì hiện messagebox
                {
                    MessageBox.Show("Vui lòng nhập từ cần xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!File.Exists(excelpath)) // không tìm thấy từ trong file thì hiện thông báo
                {
                    MessageBox.Show("Không tìm thấy file Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FileInfo file = new FileInfo(excelpath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // cấu hình EPLUS dùng chỉnh sửa file miễn phí

                bool found = false;
                int rowToDelete = -1;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Lấy Sheet đầu tiên
                    int rowCount = worksheet.Dimension?.Rows ?? 0; // Số dòng có dữ liệu
                    int colCount = worksheet.Dimension?.Columns ?? 0; // Số cột có dữ liệu

                    for (int row = 1; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (worksheet.Cells[row, col].Value?.ToString().Trim() == txtNhap.Text.Trim())
                            {
                                rowToDelete = row; // Lưu lại hàng cần xóa
                                found = true;
                                break;
                            }
                        }
                        if (found) break; // Thoát khỏi vòng lặp nếu tìm thấy
                    }

                    if (found && rowToDelete > 0)
                    {
                        worksheet.DeleteRow(rowToDelete); // Xóa toàn bộ hàng chứa từ cần xóa
                        package.Save(); // Lưu file Excel
                        MessageBox.Show("Xóa từ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy từ cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                txtNhap.Text = ""; // Reset ô nhập sau khi xóa
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
            Close();
        }

        private void Delete_Word_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (MessageBox.Show("Bạn muốn đóng form?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                e.Cancel = true;
        }
    }
}
