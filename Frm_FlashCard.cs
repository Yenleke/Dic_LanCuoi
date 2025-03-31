using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Frm_FlashCard : Form
    {
        Random rnd = new Random();
        string excelpath = "dictionary_fully_unique_sentences.xlsx";
        public List<WordEntry> diction = new List<WordEntry>();
        private bool isFlipped = false;
        private Timer flipTimer = new Timer();
        private int flipStep = 10;

        public Frm_FlashCard()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            flipTimer.Interval = 1;
        
        flipTimer.Tick += FlipAnimation;
        }

        private void Frm_FlashCard_Load(object sender, EventArgs e)
        {
            ImportExcel(excelpath);
            //Thêm lại control vào panelBack(chỉ chạy nếu bị lỗi)
            panelBack.Controls.Clear();
            if (panelBack.Controls.Count == 0)
            {
                panelBack.Controls.Add(lbNghia);
                panelBack.Controls.Add(lbVidu1);
                panelBack.Controls.Add(lbVidu2);
                panelBack.Controls.Add(label3);
                panelBack.Controls.Add(label10);
            }
            int index = rnd.Next(diction.Count);
            WordEntry selectedWord = diction[index];

            lbTiengAnh.Text = selectedWord.English;
            lbPhienAm.Text = selectedWord.Pronunciation;
            lbTuLoai.Text = selectedWord.WordType;
            lbNghia.Text = selectedWord.Meaning;
            lbVidu1.Text = selectedWord.Example1;
            lbVidu2.Text = selectedWord.Example2;

            isFlipped = false;
            ResetCard();
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
                    for (int i = 2; i <= rowCount; i++)
                    {
                        string English = worksheet.Cells[i, 1].Value?.ToString()?.Trim() ?? "";
                        string Pronunciation = worksheet.Cells[i, 2].Value?.ToString()?.Trim() ?? "";
                        string WordType = worksheet.Cells[i, 3].Value?.ToString()?.Trim() ?? "";
                        string Meaning = worksheet.Cells[i, 4].Value?.ToString()?.Trim() ?? "";
                        string Example1 = worksheet.Cells[i, 5].Value?.ToString()?.Trim() ?? "";
                        string Example2 = worksheet.Cells[i, 6].Value?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(English))
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



        private void ResetCard()
        {
            panelFront.Visible = true;
            panelBack.Visible = false;

            // Đảm bảo chỉ ẩn các thành phần trong panelBack chứ không phải toàn bộ panel
            foreach (Control ctrl in panelBack.Controls)
            {
                ctrl.Visible = false;
            }
        }



        private void FlipAnimation(object sender, EventArgs e)
        {
            int flipSpeed = 7;

            if (flipStep < 5)
            {
                panelFront.Width -= flipSpeed;
                panelBack.Width -= flipSpeed;
            }
            else if (flipStep < 10)
            {
                if (!isFlipped)
                {
                    panelFront.Visible = false;
                    panelBack.Visible = true;

                    // Hiển thị tất cả các thành phần trong panelBack khi lật
                    foreach (Control ctrl in panelBack.Controls)
                    {
                        ctrl.Visible = true;
                    }
                }
                else
                {
                    panelFront.Visible = true;
                    panelBack.Visible = false;
                }

                panelFront.Width += flipSpeed;
                panelBack.Width += flipSpeed;
            }
            else
            {
                flipTimer.Stop();
                isFlipped = !isFlipped;

                // Cập nhật hiển thị của các thành phần trong panelBack
                foreach (Control ctrl in panelBack.Controls)
                {
                    ctrl.Visible = isFlipped;
                }
            }
            flipStep++;
        }


        private void btTuMoi_Click(object sender, EventArgs e)
        {
            if (diction.Count == 0)
            {
                MessageBox.Show("Không có từ nào trong danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int index = rnd.Next(diction.Count);
            WordEntry selectedWord = diction[index];

            lbTiengAnh.Text = selectedWord.English;
            lbPhienAm.Text = selectedWord.Pronunciation;
            lbTuLoai.Text = selectedWord.WordType;
            lbNghia.Text = selectedWord.Meaning;
            lbVidu1.Text = selectedWord.Example1;
            lbVidu2.Text = selectedWord.Example2;

            isFlipped = false;
            ResetCard();
        }

        private void flip_Click_1(object sender, EventArgs e)
        {
            flipStep = 0;

            // Nếu đang lật sang mặt sau, đảm bảo nội dung của panelBack hiển thị
            if (!isFlipped)
            {
                foreach (Control ctrl in panelBack.Controls)
                {
                    ctrl.Visible = true;
                }
            }

            flipTimer.Start();

        }
    }
}
