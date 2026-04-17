namespace ConnectOracle
{
    partial class FormChinh
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
            this.btn_logout = new System.Windows.Forms.Button();
            this.btn_exit = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btn_load = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPlainText = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.txtVBG = new System.Windows.Forms.TextBox();
            this.btnTH = new System.Windows.Forms.Button();
            this.btnMuc6 = new System.Windows.Forms.Button();
            this.btnMuc5 = new System.Windows.Forms.Button();
            this.btnDesFile = new System.Windows.Forms.Button();
            this.btnRsafile = new System.Windows.Forms.Button();
            this.btnKySo = new System.Windows.Forms.Button();
            this.testSign = new System.Windows.Forms.Button();
            this.btnPhanquyen = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_logout
            // 
            this.btn_logout.Location = new System.Drawing.Point(1024, 730);
            this.btn_logout.Margin = new System.Windows.Forms.Padding(4);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(100, 28);
            this.btn_logout.TabIndex = 0;
            this.btn_logout.Text = "Logout";
            this.btn_logout.UseVisualStyleBackColor = true;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(1132, 730);
            this.btn_exit.Margin = new System.Windows.Forms.Padding(4);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(100, 28);
            this.btn_exit.TabIndex = 1;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(-3, 74);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(704, 185);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(295, 266);
            this.btn_load.Margin = new System.Windows.Forms.Padding(4);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(100, 28);
            this.btn_load.TabIndex = 3;
            this.btn_load.Text = "Load";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(-3, 41);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(475, 24);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 369);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Mã hóa DES ở đây";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 401);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Key";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 433);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mã hóa";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(16, 465);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Văn bản gốc";
            // 
            // txtPlainText
            // 
            this.txtPlainText.Location = new System.Drawing.Point(184, 364);
            this.txtPlainText.Margin = new System.Windows.Forms.Padding(4);
            this.txtPlainText.Name = "txtPlainText";
            this.txtPlainText.Size = new System.Drawing.Size(949, 22);
            this.txtPlainText.TabIndex = 9;
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(184, 396);
            this.txtKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(949, 22);
            this.txtKey.TabIndex = 10;
            // 
            // txtMa
            // 
            this.txtMa.Location = new System.Drawing.Point(184, 428);
            this.txtMa.Margin = new System.Windows.Forms.Padding(4);
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(949, 22);
            this.txtMa.TabIndex = 11;
            // 
            // txtVBG
            // 
            this.txtVBG.Location = new System.Drawing.Point(184, 460);
            this.txtVBG.Margin = new System.Windows.Forms.Padding(4);
            this.txtVBG.Name = "txtVBG";
            this.txtVBG.Size = new System.Drawing.Size(949, 22);
            this.txtVBG.TabIndex = 12;
            // 
            // btnTH
            // 
            this.btnTH.Location = new System.Drawing.Point(601, 492);
            this.btnTH.Margin = new System.Windows.Forms.Padding(4);
            this.btnTH.Name = "btnTH";
            this.btnTH.Size = new System.Drawing.Size(100, 28);
            this.btnTH.TabIndex = 13;
            this.btnTH.Text = "Thực hiện";
            this.btnTH.UseVisualStyleBackColor = true;
            this.btnTH.Click += new System.EventHandler(this.btnTH_Click);
            // 
            // btnMuc6
            // 
            this.btnMuc6.Location = new System.Drawing.Point(1132, 295);
            this.btnMuc6.Margin = new System.Windows.Forms.Padding(4);
            this.btnMuc6.Name = "btnMuc6";
            this.btnMuc6.Size = new System.Drawing.Size(100, 28);
            this.btnMuc6.TabIndex = 14;
            this.btnMuc6.Text = "Quản lý User";
            this.btnMuc6.UseVisualStyleBackColor = true;
            this.btnMuc6.Click += new System.EventHandler(this.btnMuc6_Click);
            // 
            // btnMuc5
            // 
            this.btnMuc5.Location = new System.Drawing.Point(1132, 260);
            this.btnMuc5.Margin = new System.Windows.Forms.Padding(4);
            this.btnMuc5.Name = "btnMuc5";
            this.btnMuc5.Size = new System.Drawing.Size(100, 28);
            this.btnMuc5.TabIndex = 15;
            this.btnMuc5.Text = "TableSpace";
            this.btnMuc5.UseVisualStyleBackColor = true;
            this.btnMuc5.Click += new System.EventHandler(this.btnMuc5_Click);
            // 
            // btnDesFile
            // 
            this.btnDesFile.Location = new System.Drawing.Point(1135, 41);
            this.btnDesFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnDesFile.Name = "btnDesFile";
            this.btnDesFile.Size = new System.Drawing.Size(100, 28);
            this.btnDesFile.TabIndex = 1;
            this.btnDesFile.Text = "DES_file";
            this.btnDesFile.Click += new System.EventHandler(this.btnDesFile_Click);
            // 
            // btnRsafile
            // 
            this.btnRsafile.Location = new System.Drawing.Point(1135, 76);
            this.btnRsafile.Margin = new System.Windows.Forms.Padding(4);
            this.btnRsafile.Name = "btnRsafile";
            this.btnRsafile.Size = new System.Drawing.Size(100, 28);
            this.btnRsafile.TabIndex = 0;
            this.btnRsafile.Text = "RSA_file";
            this.btnRsafile.Click += new System.EventHandler(this.btnRsafile_Click);
            // 
            // btnKySo
            // 
            this.btnKySo.Location = new System.Drawing.Point(1053, 112);
            this.btnKySo.Margin = new System.Windows.Forms.Padding(4);
            this.btnKySo.Name = "btnKySo";
            this.btnKySo.Size = new System.Drawing.Size(179, 28);
            this.btnKySo.TabIndex = 16;
            this.btnKySo.Text = "SignedNumber";
            this.btnKySo.Click += new System.EventHandler(this.btnKySo_Click);
            // 
            // testSign
            // 
            this.testSign.Location = new System.Drawing.Point(1053, 148);
            this.testSign.Margin = new System.Windows.Forms.Padding(4);
            this.testSign.Name = "testSign";
            this.testSign.Size = new System.Drawing.Size(179, 28);
            this.testSign.TabIndex = 17;
            this.testSign.Text = "Kiểm tra ký số";
            this.testSign.Click += new System.EventHandler(this.testSign_Click);
            // 
            // btnPhanquyen
            // 
            this.btnPhanquyen.Location = new System.Drawing.Point(1056, 183);
            this.btnPhanquyen.Margin = new System.Windows.Forms.Padding(4);
            this.btnPhanquyen.Name = "btnPhanquyen";
            this.btnPhanquyen.Size = new System.Drawing.Size(179, 28);
            this.btnPhanquyen.TabIndex = 18;
            this.btnPhanquyen.Text = "Phân quyền";
            this.btnPhanquyen.Click += new System.EventHandler(this.btnPhanquyen_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1024, 76);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 19;
            this.button1.Text = "RSA_file_1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1024, 40);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 20;
            this.button2.Text = "DES_file_1";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1053, 219);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(179, 28);
            this.button3.TabIndex = 21;
            this.button3.Text = "Chính Sách";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormChinh
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1248, 768);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPhanquyen);
            this.Controls.Add(this.testSign);
            this.Controls.Add(this.btnKySo);
            this.Controls.Add(this.btnRsafile);
            this.Controls.Add(this.btnDesFile);
            this.Controls.Add(this.btnMuc5);
            this.Controls.Add(this.btnMuc6);
            this.Controls.Add(this.btnTH);
            this.Controls.Add(this.txtVBG);
            this.Controls.Add(this.txtMa);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.txtPlainText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_logout);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormChinh";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.FormChinh_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_logout;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPlainText;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.TextBox txtMa;
        private System.Windows.Forms.TextBox txtVBG;
        private System.Windows.Forms.Button btnTH;
        private System.Windows.Forms.Button btnMuc6;
        private System.Windows.Forms.Button btnMuc5;
        private System.Windows.Forms.Button btnDesFile;
        private System.Windows.Forms.Button btnRsafile;
        private System.Windows.Forms.Button btnKySo;
        private System.Windows.Forms.Button testSign;
        private System.Windows.Forms.Button btnPhanquyen;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}