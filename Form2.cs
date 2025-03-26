using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dic_AppTest
{
    public partial class Sub_EditWord : Form
    {
        List<WordEntry> diction = new List<WordEntry>();
        WordEntry selectedWord;
        bool isAnhViet = true;
        
        public Sub_EditWord(List<WordEntry> dictionary)
        {
            InitializeComponent();
            this.diction = dictionary;
        }

        private void btSwitch_Click(object sender, EventArgs e)
        {
            isAnhViet = !isAnhViet;
            btSwitch.Text = isAnhViet ? "Anh-Việt" : "Việt-Anh";

        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            string searchWord = txtNhap.Text.Trim().ToLower();
            selectedWord = diction.FirstOrDefault(d => d.English.ToLower() == searchWord);
            if (selectedWord != null)
            {
                if(isAnhViet)
                {
                    txtTuVung.Text = selectedWord.English;
                    txtPhienAm.Text = selectedWord.Pronunciation;
                    txtTuloai.Text = selectedWord.WordType;
                    txtNghia.Text = selectedWord.Meaning;
                    txtVidu1.Text = selectedWord.Example1;
                    txtVidu2.Text = selectedWord.Example2;
                }
                else
                {
                    txtTuVung.Text = selectedWord.Meaning;
                    txtPhienAm.Text = selectedWord.Pronunciation;
                    txtTuloai.Text = selectedWord.WordType;
                    txtNghia.Text = selectedWord.English;
                    txtVidu1.Text = selectedWord.Example1;
                    txtVidu2.Text = selectedWord.Example2;
                }
                               
            }
            else
            {
                MessageBox.Show("\"Không tìm thấy từ này!\", \"Thông báo\", MessageBoxButtons.OK, MessageBoxIcon.Information");
            }
        }
    }
}
