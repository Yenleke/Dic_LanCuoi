namespace Dic_AppTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new Dic_AppTest.Panel();
            this.txtNhap = new System.Windows.Forms.TextBox();
            this.btSwitch = new System.Windows.Forms.Button();
            this.btSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Goudy Stout", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(228, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 48);
            this.label1.TabIndex = 1;
            this.label1.Text = "DICTIONARY";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.CornerRadius = 20;
            this.panel1.Location = new System.Drawing.Point(104, 107);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(685, 58);
            this.panel1.TabIndex = 2;
            // 
            // txtNhap
            // 
            this.txtNhap.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNhap.Location = new System.Drawing.Point(122, 122);
            this.txtNhap.Name = "txtNhap";
            this.txtNhap.Size = new System.Drawing.Size(456, 24);
            this.txtNhap.TabIndex = 3;
            // 
            // btSwitch
            // 
            this.btSwitch.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSwitch.Location = new System.Drawing.Point(597, 115);
            this.btSwitch.Name = "btSwitch";
            this.btSwitch.Size = new System.Drawing.Size(91, 43);
            this.btSwitch.TabIndex = 4;
            this.btSwitch.Text = "Anh - Việt";
            this.btSwitch.UseVisualStyleBackColor = true;
            this.btSwitch.Click += new System.EventHandler(this.btSwitch_Click);
            // 
            // btSearch
            // 
            this.btSearch.BackColor = System.Drawing.SystemColors.Window;
            this.btSearch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btSearch.BackgroundImage")));
            this.btSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btSearch.FlatAppearance.BorderSize = 0;
            this.btSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSearch.Location = new System.Drawing.Point(694, 113);
            this.btSearch.Name = "btSearch";
            this.btSearch.Size = new System.Drawing.Size(82, 46);
            this.btSearch.TabIndex = 4;
            this.btSearch.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.Location = new System.Drawing.Point(-16, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(911, 80);
            this.label2.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 616);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.btSwitch);
            this.Controls.Add(this.txtNhap);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Panel panel1;
        private System.Windows.Forms.TextBox txtNhap;
        private System.Windows.Forms.Button btSwitch;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Label label2;
    }
}

