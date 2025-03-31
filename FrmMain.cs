using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using System.Collections;
using System.Drawing.Text;
using System.Drawing;



namespace Dic_AppTest
{
    public partial class FrmMain : Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        public List<WordEntry> diction = new List<WordEntry>();
        bool isAnhViet = true;
        public static bool isLoggin = false;
        public static string ten = "";
        public FrmMain()
        {
           
       InitializeComponent();
            if (!isLoggin)
            {
                LogIn();
                
                isLoggin = true;
            }
                
            
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            new SetupTextup(txtNhap, "Type here to search...");
        }


        private void LogIn()
        {
            Log_in dangNhap = new Log_in();
            dangNhap.ShowDialog();    
            if (dangNhap.ShowDialog() != DialogResult.OK)
                {
                System.Windows.Forms.Application.Exit();
                
                    return;
                }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {

            lbuserName.Text = ten;       
            ImportExcel(excelpath);
            SetupAutoComplete();
        }

        public bool Search(string searchText, Action<string, string, string, string, string, string> updateUI)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Vui lòng nhập từ cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            searchText = searchText.Trim().ToLower();

            WordEntry result = isAnhViet
                ? diction.FirstOrDefault(d => d.English.ToLower() == searchText)
                : diction.FirstOrDefault(d => d.Meaning.ToLower().Contains(searchText));

            if (result != null)
            {
                if (isAnhViet)
                {
                    updateUI(result.English, result.Pronunciation, result.WordType, result.Meaning, result.Example1, result.Example2);
                }
                else
                {
                    updateUI(result.Meaning, result.Pronunciation, result.WordType, result.English, result.Example1, result.Example2);
                }
                return true;
            }
            else
            {
                MessageBox.Show("Không tìm thấy từ này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        private void SetupAutoComplete()
        {
            txtNhap.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtNhap.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            txtNhap.AutoCompleteCustomSource.Clear();
            foreach (var word in diction)
            {

                autoCompleteCollection.Add(isAnhViet ? word.English : word.Meaning);
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

        public void ImportExcel(string filePath)
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

                    for (int i = 2; i <= rowCount; i++)
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

        public void btSwitch_Click_1(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh - Việt" : "Việt - Anh";
            SetupAutoComplete();
        }


        private void btSearch_Click_1(object sender, EventArgs e)
        {

            Search(txtNhap.Text, (word1, pronunciation, wordType, word2, example1, example2) =>
            {
                LbTiengAnh.Text = word1;
                lbPhienAm.Text = pronunciation;
                lbTuLoai.Text = wordType;
                lbNghia.Text = word2;
                lbViDu1.Text = example1;
                lbViDu2.Text = example2;
            });
            hienThi();
            string searchWord = txtNhap.Text.Trim();
            if (!string.IsNullOrEmpty(searchWord))
            {
                // Ghi từ vào file Excel
                SaveSearchToHistory(searchWord);

                // Nếu form lịch sử đang mở, cập nhật luôn
                if (history != null && !history.IsDisposed)
                {
                    history.LoadHistoryFromExcel();
                }
            }
        }

        private void Search()
        {
            Search(txtNhap.Text, (word1, pronunciation, wordType, word2, example1, example2) =>
            {
                LbTiengAnh.Text = word1;
                lbPhienAm.Text = pronunciation;
                lbTuLoai.Text = wordType;
                lbNghia.Text = word2;
                lbViDu1.Text = example1;
                lbViDu2.Text = example2;
            });
            string searchWord = txtNhap.Text.Trim();
            if (!string.IsNullOrEmpty(searchWord))
            {
                // Ghi từ vào file Excel
                SaveSearchToHistory(searchWord);

                // Nếu form lịch sử đang mở, cập nhật luôn
                if (history != null && !history.IsDisposed)
                {
                    history.LoadHistoryFromExcel();
                }
            }
        }


        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter: Search(); break;
                default: break;
            }
            return base.ProcessDialogKey(keyData);
        }

        private History history;


        private void ThêmTừToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sub_AddWord add = new Sub_AddWord();
            add.ShowDialog();
        }

        private void SửaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sub_EditWord edit = new Sub_EditWord(this);
            edit.ShowDialog();

        }


        private void LoadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại thay vì đóng
            FrmMain newForm = new FrmMain();
            newForm.ShowDialog(); // Hiển thị form mới ở chế độ modal
            this.Close(); // Đóng form cũ sau khi form mới đóng lại
        }

        private void XóaTừToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete_Word delete = new Delete_Word();
            delete.ShowDialog();
        }
       

        private void SaveSearchToHistory(string word)
        {
            try
            {
                string filePath = "search_history.xlsx";

                // Đảm bảo thư viện EPPlus có thể sử dụng
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = File.Exists(filePath) ? new ExcelPackage(new FileInfo(filePath)) : new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? package.Workbook.Worksheets.Add("History");

                    // Tìm dòng trống tiếp theo
                    int nextRow = worksheet.Dimension?.Rows + 1 ?? 2;

                    // Nếu là lần đầu tiên, thêm tiêu đề
                    if (nextRow == 2 && worksheet.Cells["A1"].Value == null)
                    {
                        worksheet.Cells[1, 1].Value = "";
                    }

                    // Ghi từ vào dòng tiếp theo
                    worksheet.Cells[nextRow, 1].Value = word;

                    // Lưu lại file
                    package.SaveAs(new FileInfo(filePath));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu lịch sử: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void txtNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
                e.Handled = true;
            }
        }

        private void hIstoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History historyForm = new History();
            historyForm.Show();

        }

        private void flashCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_FlashCard fl = new Frm_FlashCard();
            fl.Show();
        }
    }
}
