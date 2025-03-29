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
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        bool isAnhViet = true;
        private FrmMain frmMain;

        public Sub_EditWord(FrmMain frmMain)
        {
            InitializeComponent();
            this.frmMain = frmMain;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            new SetupTextup(txtNhap, "Type here...");
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

private void SaveToExcel()
        {
            FileInfo file = new FileInfo(excelpath);
            using (ExcelPackage package = file.Exists ? new ExcelPackage(file) : new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0 ?
                    package.Workbook.Worksheets[0] : package.Workbook.Worksheets.Add("TuDien");

                // Kiểm tra tiêu đề
                if (worksheet.Dimension == null || worksheet.Dimension.Rows == 0)
                {
                    worksheet.Cells[1, 1].Value = "Từ vựng";
                    worksheet.Cells[1, 2].Value = "Phiên âm";
                    worksheet.Cells[1, 3].Value = "Loại từ";
                    worksheet.Cells[1, 4].Value = "Nghĩa";
                    worksheet.Cells[1, 5].Value = "Ví dụ 1";
                    worksheet.Cells[1, 6].Value = "Ví dụ 2";
                }

                // Lấy số dòng hiện có trong sheet
                int rowCount = worksheet.Dimension?.Rows ?? 0;
                bool found = false;

                // Tìm dòng chứa từ cần sửa
                for (int row = 2; row <= rowCount; row++) // Bỏ qua dòng tiêu đề
                {
                    string wordInCell = worksheet.Cells[row, 1].Value?.ToString();
                    string meaningInCell = worksheet.Cells[row, 4].Value?.ToString();

                    if ((isAnhViet && wordInCell == txtTuVung.Text) || (!isAnhViet && meaningInCell == txtNghia.Text))
                    {
                        // Cập nhật dữ liệu tại dòng tìm được
                        worksheet.Cells[row, 1].Value = txtTuVung.Text;
                        worksheet.Cells[row, 2].Value = txtPhienAm.Text;
                        worksheet.Cells[row, 3].Value = txtTuloai.Text;
                        worksheet.Cells[row, 4].Value = txtNghia.Text;
                        worksheet.Cells[row, 5].Value = txtVidu1.Text;
                        worksheet.Cells[row, 6].Value = txtVidu2.Text;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Không tìm thấy từ để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                package.Save();
                MessageBox.Show("Cập nhật từ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(txtTuVung.Text) || string.IsNullOrWhiteSpace(txtNghia.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ từ vựng và nghĩa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string tuVung = txtTuVung.Text.Trim();
                string nghia = txtNghia.Text.Trim();
                string phienAm = txtPhienAm.Text.Trim();
                string loaiTu = txtTuloai.Text.Trim();
                string viDu1 = txtVidu1.Text.Trim();
                string viDu2 = txtVidu2.Text.Trim();
                var existingWord = frmMain.diction.FirstOrDefault(d => isAnhViet ? d.English == tuVung : d.Meaning == nghia);
                if (existingWord != null)
                {

                    existingWord.English = isAnhViet ? tuVung : nghia;
                    existingWord.Meaning = isAnhViet ? nghia : tuVung;
                    existingWord.Pronunciation = phienAm;
                    existingWord.WordType = loaiTu;
                    existingWord.Example1 = viDu1;
                    existingWord.Example2 = viDu2;

                }

                SaveToExcel();


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
