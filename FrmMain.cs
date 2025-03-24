using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;

namespace Dic_AppTest
{
    public partial class FrmMain : Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        List<WordEntry> diction = new List<WordEntry>();
        bool isAnhViet = true;

        public FrmMain()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            ImportExcel(excelpath);
            SetupAutoComplete();
            
        }
        private void btSearch_Click_1(object sender, EventArgs e)
        {
            string searchText = txtNhap.Text.Trim().ToLower();
            if (isAnhViet)
            {
                var result = diction.FirstOrDefault(d => d.English.ToLower() == searchText);
                if (result != null)
                {
                    LbTiengAnh.Text = result.English;
                    lbPhienAm.Text = result.Pronunciation;
                    lbTuLoai.Text = result.WordType;
                    lbNghia.Text = result.Meaning;
                    lbViDu1.Text = result.Example1;
                    lbViDu2.Text = result.Example2;
                    hienThi();
                    txtNhap.Clear();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy từ này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                var result = diction.FirstOrDefault(d => d.Meaning.ToLower().Contains(searchText));
                if (result != null)
                {
                    LbTiengAnh.Text = result.Meaning;
                    lbPhienAm.Text = result.Pronunciation;
                    lbTuLoai.Text = result.WordType;
                    lbNghia.Text = result.English;
                    lbViDu1.Text = result.Example1;
                    lbViDu2.Text = result.Example2;
                    hienThi();
                    txtNhap.Clear();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy từ này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }

        private void SetupAutoComplete()
        {
            txtNhap.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtNhap.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            foreach (var word in diction)
            {
                autoCompleteCollection.Add(word.English);
            }

            txtNhap.AutoCompleteCustomSource = autoCompleteCollection;
        }

        private void hienThi()
        {
            LbTiengAnh.Visible = true;
            lbPhienAm.Visible = true;
            lbTuLoai.Visible = true;
            lbNghia.Visible = true;
            lbViDu1.Visible = true;
            lbViDu2.Visible = true;
            label4.Visible = true;
        }

    

        private void thêmTừToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sub_AddWord addWord = new Sub_AddWord();
            addWord.Show();
        }



        private void loaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại thay vì đóng
            FrmMain newForm = new FrmMain();
            newForm.ShowDialog(); // Hiển thị form mới ở chế độ modal
            this.Close(); // Đóng form cũ sau khi form mới đóng lại
        }

        private void importFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Excel file|*.xlsx|Word file|*.docx";
            if (f.ShowDialog() == DialogResult.OK)
            {
                string epath = f.FileName;
                string extension = Path.GetExtension(epath);

                if (extension == ".xlsx")
                {
                    ImportExcel(epath);
                }
                else if (extension == ".docx")
                {
                    ImportWord(epath);
                }
            }
        }

        private void ImportExcel(string filePath)
        {
            try
            {
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu trong file Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    int rowCount = worksheet.Dimension.End.Row;
                    int colCount = worksheet.Dimension.End.Column;


                

                    for (int i = 2; i <= rowCount; i++)  // Bỏ qua dòng tiêu đề
                    {
                        string English = worksheet.Cells[i, 1].Value?.ToString()?.Trim() ?? "";
                        string Pronunciation = worksheet.Cells[i, 2].Value?.ToString()?.Trim() ?? "";
                        string WordType = worksheet.Cells[i, 3].Value?.ToString()?.Trim() ?? "";
                        string Meaning = worksheet.Cells[i, 4].Value?.ToString()?.Trim() ?? "";
                        string Example1 = worksheet.Cells[i, 5].Value?.ToString()?.Trim() ?? "";
                        string Example2 = worksheet.Cells[i, 6].Value?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(English))  // Chỉ thêm nếu có dữ liệu
                        {
                            diction.Add(new WordEntry()
                            {
                                English = English,
                                Pronunciation = Pronunciation,
                                WordType = WordType,
                                Meaning = Meaning,
                                Example1 = Example1,
                                Example2 = Example2
                            });
                        }
                    }
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi import Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         
         
        }

        private void ImportWord(string filePath)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = null;
            try
            {
                doc = wordApp.Documents.Open(filePath, ReadOnly: true);
                string text = doc.Content.Text;

              
                string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(';');
                    if (parts.Length >= 4)
                    {
                        diction.Add(new WordEntry()
                        {
                            English = parts[0].Trim(),
                            Pronunciation = parts[1].Trim(),
                            WordType = parts[2].Trim(),
                            Meaning = parts[3].Trim(),
                            Example1 = parts.Length > 4 ? parts[4].Trim() : "",
                            Example2 = parts.Length > 5 ? parts[5].Trim() : ""
                        });
                    }
                }

                MessageBox.Show("Import thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Cập nhật giao diện ngay lập tức
                SetupAutoComplete();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi import Word: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                doc?.Close();
                wordApp.Quit();
            }
        }

        private void btSwitch_Click_1(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh - Việt" : "Việt - Anh";
        }

       
    }
}
