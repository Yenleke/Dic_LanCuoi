using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class History : Form
    {
        private string historyFilePath = "search_history.xlsx";
        private BindingSource bindingSource = new BindingSource();

        public History()
        {
            InitializeComponent();
            LoadHistory();
            listBox1.DataSource = bindingSource;

            // Bo góc cho Button
            RoundedControl.SetRoundedRegion(btXoa1tu, 25);
            RoundedControl.SetRoundedRegion(btXoaall, 25);
            RoundedControl.SetRoundedRegion(listBox1, 10);
            RoundedControl.SetRoundedRegion(this, 10);
        }

        private void LoadHistory()
        {
            try
            {
                if (!File.Exists(historyFilePath))
                    return;

                FileInfo file = new FileInfo(historyFilePath);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension?.Rows ?? 0;

                    List<string> words = new List<string>();
                    for (int row = 1; row <= rowCount; row++)
                    {
                        string word = worksheet.Cells[row, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(word))
                        {
                            words.Add(word);
                        }
                    }

                    bindingSource.DataSource = words;
                    bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btXoa1tu_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedWord = listBox1.SelectedItem.ToString().Trim();
                RemoveFromHistory(selectedWord);
                LoadHistory();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một từ để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btXoaall_Click(object sender, EventArgs e)
        {
            ClearHistory();
            LoadHistory();
        }

        private void RemoveFromHistory(string wordToRemove)
        {
            try
            {
                if (!File.Exists(historyFilePath))
                    return;

                FileInfo file = new FileInfo(historyFilePath);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    List<string> words = new List<string>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        string word = worksheet.Cells[row, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(word) && word != wordToRemove)
                        {
                            words.Add(word);
                        }
                    }

                    worksheet.Cells.Clear(); // Xóa nội dung, không làm mất cấu trúc bảng
                    for (int i = 0; i < words.Count; i++)
                    {
                        worksheet.Cells[i + 1, 1].Value = words[i];
                    }

                    package.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa từ khỏi lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearHistory()
        {
            try
            {
                if (File.Exists(historyFilePath))
                {
                    FileInfo file = new FileInfo(historyFilePath);
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(file))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        worksheet.Cells.Clear();
                        package.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        public void LoadHistoryFromExcel()
        {
            try
            {
                if (!File.Exists(historyFilePath))
                    return;

                FileInfo file = new FileInfo(historyFilePath);
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension?.Rows ?? 0;

                    List<string> words = new List<string>();

                    for (int row = 1; row <= rowCount; row++)
                    {
                        string word = worksheet.Cells[row, 1].Value?.ToString();
                        if (!string.IsNullOrEmpty(word))
                        {
                            words.Add(word);
                        }
                    }

                    bindingSource.DataSource = words;
                    bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}