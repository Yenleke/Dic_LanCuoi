using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace Dic_AppTest
{
    public partial class FrmMain: Form
    {
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        List<WordEntry> diction = new List<WordEntry>();
        bool isAnhViet = true;
        public FrmMain()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show($"Đường dẫn file: {Path.GetFullPath(excelpath)}");
            try
            {
               
                using (var package = new ExcelPackage(new FileInfo(Path.GetFullPath(excelpath))))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu trong file Excel!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        try
                        {
                            int j = 1;
                            string English = worksheet.Cells[i, j++].Value?.ToString() ?? "";
                            string Pronunciation = worksheet.Cells[i, j++].Value?.ToString() ?? "";
                            string WordType = worksheet.Cells[i, j++].Value?.ToString() ?? "";
                            string Meaning = worksheet.Cells[i, j++].Value?.ToString() ?? "";
                            string Example1 = worksheet.Cells[i, j++].Value?.ToString() ?? "";
                            string Example2 = worksheet.Cells[i, j++].Value?.ToString() ?? "";

                            WordEntry dic = new WordEntry()
                            {
                                English = English,
                                Pronunciation = Pronunciation,
                                WordType = WordType,
                                Meaning = Meaning,
                                Example1 = Example1,
                                Example2 = Example2
                            };

                            diction.Add(dic);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Lỗi dòng {i}: {ex.Message}");
                        }
                    }

                    MessageBox.Show($"Số lượng từ trong danh sách: {diction.Count}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void hienThi()
        {
            LbTiengAnh.Visible = true;
            lbPhienAm.Visible = true;
            lbTuLoai.Visible = true;
            lbNghia.Visible = true;
            lbViDu1.Visible = true;
            lbViDu2.Visible = true;
        }

        private void btSwitch_Click_1(object sender, EventArgs e)
        {
            isAnhViet= !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh - Việt" : "Việt - Anh";
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            
           


        }

        private void thêmTừToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                    LbTiengAnh.Text = result.Meaning;  // Hiển thị nghĩa (từ tiếng Việt)
                    lbPhienAm.Text = result.Pronunciation;
                    lbTuLoai.Text = result.WordType;
                    lbNghia.Text = result.English;  // Đảo nghĩa với từ tiếng Anh
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
    }
}
