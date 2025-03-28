using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Sub_EditWord : Form
    {
        WordEntry result;
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        bool isAnhViet = true;
        private FrmMain frmMain;

        public Sub_EditWord(FrmMain frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        private void btSwitch_Click(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh-Việt" : "Việt-Anh";
            SetupAutoComplete();
            
        }

        private void Sub_EditWord_Load(object sender, EventArgs e)
        {
            SetupAutoComplete();
            frmMain.ImportExcel(excelpath);
        }
        public void SetupAutoComplete()
        {

            txtNhap.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtNhap.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            txtNhap.AutoCompleteCustomSource.Clear();
            foreach (var word in frmMain.diction)
            {
                if (isAnhViet)
                {
                    autoCompleteCollection.Add(word.English);
                }
                else
                {
                    autoCompleteCollection.Add(word.Meaning);
                }
            }
            txtNhap.AutoCompleteCustomSource = autoCompleteCollection;
        }



        private void btSearch_Click_1(object sender, EventArgs e)
        {
            string searchText = txtNhap.Text;

            bool found = frmMain.Search(searchText, (word1, pronunciation, wordType, word2, example1, example2) =>
            {
                result = new WordEntry()
                {
                    English = word1,
                    Pronunciation = pronunciation,
                    WordType = wordType,
                    Meaning = word2,
                    Example1 = example1,
                    Example2 = example2
                };
                txtTuVung.Text = word1;
                txtPhienAm.Text = pronunciation;
                txtTuloai.Text = wordType;
                txtNghia.Text = word2;
                txtVidu1.Text = example1;
                txtVidu2.Text = example2;
            });
            hienThi();
            
           
        }
        private void hienThi()
        {
            panel2.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            txtNghia.Visible = true;
            txtTuloai.Visible = true;
            txtPhienAm.Visible = true;
            txtTuVung.Visible = true;
            txtVidu1.Visible = true;
            txtVidu2.Visible = true;
            btSave.Visible = true;
            
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (result != null)
                {
                    if (string.IsNullOrWhiteSpace(txtTuVung.Text) || string.IsNullOrWhiteSpace(txtNghia.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ từ vựng và nghĩa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (isAnhViet)
                    {
                        result.English = txtTuVung.Text.Trim();
                        result.Pronunciation = txtPhienAm.Text.Trim();
                        result.WordType = txtTuloai.Text.Trim();
                        result.Meaning = txtNghia.Text.Trim();
                        result.Example1 = txtVidu1.Text.Trim();
                        result.Example2 = txtVidu2.Text.Trim();
                    }
                    else {
                        result.Meaning = txtTuVung.Text.Trim();
                        result.Pronunciation = txtPhienAm.Text.Trim();
                        result.WordType = txtTuloai.Text.Trim();
                        result.English = txtNghia.Text.Trim();
                        result.Example1 = txtVidu1.Text.Trim();
                        result.Example2 = txtVidu2.Text.Trim();
                    }

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
    }
}
