using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Text;
using System.Drawing;
using static Dic_AppTest.History;
using Dic_AppTest;


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
            new Set(txtNhap, "Type here to search...");
            
        }
       
        private void LogIn()
        {
            Log_in dangNhap = new Log_in();
            dangNhap.ShowDialog();    
            if (dangNhap.ShowDialog() != DialogResult.OK)
                {
                
                dangNhap.Close();
                return;
                }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            lbuserName.Text = ten;       
            ImportExcel(excelpath);
            SetupAutoComplete();
           
            RoundedControl.SetRoundedRegion(this, 10);
            LbTiengAnh.Text=lbNghia.Text = lbPhienAm.Text = lbTuLoai.Text = lbViDu1.Text = lbViDu2.Text = "";
            label4.BackColor = panel2.BackColor;
        }

        public bool Search(string searchText, bool i, Action<WordEntry> updateUI)
        {
            
            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Vui lòng nhập từ cần tìm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            searchText = searchText.Trim().ToLower();

            WordEntry result = i
                ? diction.FirstOrDefault(d => d.English.ToLower() == searchText)
                : diction.FirstOrDefault(d => d.Meaning.ToLower().Contains(searchText));

            if (result != null)
            {
                updateUI(result);
                return true;
            }
            else
            {
                MessageBox.Show("Không tìm thấy từ này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        public void SetupAutoComplete()
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
            label4.BackColor = Color.SteelBlue;
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

       
        private void btSwitch_Click_1(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh - Việt" : "Việt - Anh";
            SetupAutoComplete();
        }


        private void btSearch_Click_1(object sender, EventArgs e)
        {
            Found();
        }

        private void Found()
        {
            Search(txtNhap.Text, isAnhViet, (result) =>
            {
                if (isAnhViet)
                {
                    LbTiengAnh.Text = result.English;
                    lbPhienAm.Text = result.Pronunciation;
                    lbTuLoai.Text = result.WordType;
                    lbNghia.Text = result.Meaning;
                    lbViDu1.Text = result.Example1;
                    lbViDu2.Text = result.Example2;
                }
                else
                {
                    LbTiengAnh.Text = result.Meaning;
                    lbPhienAm.Text = result.Pronunciation;
                    lbTuLoai.Text = result.WordType;
                    lbNghia.Text = result.English;
                    lbViDu1.Text = result.Example1;
                    lbViDu2.Text = result.Example2;
                }
                   
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


        //Xóa từ
        private void XóaTừToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete_Word delete = new Delete_Word();
            delete.ShowDialog();
        }



        // Tạo file excel để lưu lại lịch sử tìm kiếm
        private History history;
        private void SaveSearchToHistory(string word)
        {
            try
            {
                string filePath = "search_history.xlsx";

                // Đảm bảo thư viện EPPlus có thể sử dụng
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = File.Exists(filePath) ? new ExcelPackage(new FileInfo(filePath)) : new ExcelPackage()) // kiểm tra nếu file đã tt thì mở chưa thì tạo mới
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault() ?? package.Workbook.Worksheets.Add("History"); //Lấy worksheet đầu tiên trong file nếu có. khco thì tạo History

                    // Tìm dòng trống tiếp theo, nếu file trống thì mặc định nr = 2, do dòng 1 dùng ghi header
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

        private void hIstoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History historyForm = new History();
            historyForm.Show();

        }


        private void txtNhap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Found();
                e.Handled = true;
            }
        }

        

        private void flashCardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_FlashCard fl = new Frm_FlashCard();
            fl.Show();
        }

        private void BTChange_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show("Bạn có chắc muốn đổi User name?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                this.Hide();
                isLoggin = false;
                LogIn();
                isLoggin = true;
                lbuserName.Text = ten;
                this.Show();
            }
        }

     
    }
}
