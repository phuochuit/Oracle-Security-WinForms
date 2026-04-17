namespace ConnectOracle
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.grpTarget = new System.Windows.Forms.GroupBox();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboSchema = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabFGA = new System.Windows.Forms.TabPage();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.btnExecFGA = new System.Windows.Forms.Button();
            this.txtFGA_Col = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFGA_Policy = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabVPD = new System.Windows.Forms.TabPage();
            this.btnDropVPD = new System.Windows.Forms.Button(); // Nút mới
            this.btnExecVPD = new System.Windows.Forms.Button();
            this.txtVPD_Condition = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtVPD_Policy = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabRecovery = new System.Windows.Forms.TabPage();
            this.btnViewData = new System.Windows.Forms.Button();
            this.btnExecRecover = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.numRecoverTime = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.grpTarget.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabFGA.SuspendLayout();
            this.tabVPD.SuspendLayout();
            this.tabRecovery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecoverTime)).BeginInit();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.SuspendLayout();
            // 
            // grpTarget
            // 
            this.grpTarget.Controls.Add(this.cboTable);
            this.grpTarget.Controls.Add(this.label7);
            this.grpTarget.Controls.Add(this.cboSchema);
            this.grpTarget.Controls.Add(this.label6);
            this.grpTarget.Location = new System.Drawing.Point(12, 12);
            this.grpTarget.Name = "grpTarget";
            this.grpTarget.Size = new System.Drawing.Size(776, 75);
            this.grpTarget.TabIndex = 0;
            this.grpTarget.TabStop = false;
            this.grpTarget.Text = "Chọn Đối tượng tác động (Động)";
            // 
            // cboTable
            // 
            this.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(481, 26);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(200, 21);
            this.cboTable.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(344, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Chọn Bảng (Table):";
            // 
            // cboSchema
            // 
            this.cboSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSchema.FormattingEnabled = true;
            this.cboSchema.Location = new System.Drawing.Point(130, 24);
            this.cboSchema.Name = "cboSchema";
            this.cboSchema.Size = new System.Drawing.Size(185, 21);
            this.cboSchema.TabIndex = 1;
            this.cboSchema.SelectedIndexChanged += new System.EventHandler(this.cboSchema_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Chọn Schema/User:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabFGA);
            this.tabControl1.Controls.Add(this.tabVPD);
            this.tabControl1.Controls.Add(this.tabRecovery);
            this.tabControl1.Location = new System.Drawing.Point(12, 93);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 200);
            this.tabControl1.TabIndex = 1;
            // 
            // tabFGA
            // 
            this.tabFGA.Controls.Add(this.btnViewLog);
            this.tabFGA.Controls.Add(this.btnExecFGA);
            this.tabFGA.Controls.Add(this.txtFGA_Col);
            this.tabFGA.Controls.Add(this.label9);
            this.tabFGA.Controls.Add(this.txtFGA_Policy);
            this.tabFGA.Controls.Add(this.label8);
            this.tabFGA.Location = new System.Drawing.Point(4, 22);
            this.tabFGA.Name = "tabFGA";
            this.tabFGA.Padding = new System.Windows.Forms.Padding(3);
            this.tabFGA.Size = new System.Drawing.Size(768, 174);
            this.tabFGA.TabIndex = 0;
            this.tabFGA.Text = "Giám sát (FGA)";
            this.tabFGA.UseVisualStyleBackColor = true;
            // 
            // btnViewLog
            // 
            this.btnViewLog.BackColor = System.Drawing.Color.LightCyan;
            this.btnViewLog.Location = new System.Drawing.Point(450, 116);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(155, 39);
            this.btnViewLog.TabIndex = 5;
            this.btnViewLog.Text = "Xem Nhật ký (Log)";
            this.btnViewLog.UseVisualStyleBackColor = false;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // btnExecFGA
            // 
            this.btnExecFGA.Location = new System.Drawing.Point(283, 116);
            this.btnExecFGA.Name = "btnExecFGA";
            this.btnExecFGA.Size = new System.Drawing.Size(155, 39);
            this.btnExecFGA.TabIndex = 4;
            this.btnExecFGA.Text = "Kích hoạt FGA";
            this.btnExecFGA.UseVisualStyleBackColor = true;
            this.btnExecFGA.Click += new System.EventHandler(this.btnExecFGA_Click);
            // 
            // txtFGA_Col
            // 
            this.txtFGA_Col.Location = new System.Drawing.Point(283, 73);
            this.txtFGA_Col.Name = "txtFGA_Col";
            this.txtFGA_Col.Size = new System.Drawing.Size(252, 20);
            this.txtFGA_Col.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(180, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Cột cần Audit:";
            // 
            // txtFGA_Policy
            // 
            this.txtFGA_Policy.Location = new System.Drawing.Point(283, 36);
            this.txtFGA_Policy.Name = "txtFGA_Policy";
            this.txtFGA_Policy.Size = new System.Drawing.Size(252, 20);
            this.txtFGA_Policy.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(180, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Tên Chính sách:";
            // 
            // tabVPD
            // 
            this.tabVPD.Controls.Add(this.btnDropVPD); // Thêm nút mới vào tab
            this.tabVPD.Controls.Add(this.btnExecVPD);
            this.tabVPD.Controls.Add(this.txtVPD_Condition);
            this.tabVPD.Controls.Add(this.label11);
            this.tabVPD.Controls.Add(this.txtVPD_Policy);
            this.tabVPD.Controls.Add(this.label10);
            this.tabVPD.Location = new System.Drawing.Point(4, 22);
            this.tabVPD.Name = "tabVPD";
            this.tabVPD.Padding = new System.Windows.Forms.Padding(3);
            this.tabVPD.Size = new System.Drawing.Size(768, 174);
            this.tabVPD.TabIndex = 1;
            this.tabVPD.Text = "Bảo mật dòng (VPD)";
            this.tabVPD.UseVisualStyleBackColor = true;
            // 
            // btnDropVPD (Nút Xóa mới)
            // 
            this.btnDropVPD.BackColor = System.Drawing.Color.MistyRose;
            this.btnDropVPD.Location = new System.Drawing.Point(450, 116);
            this.btnDropVPD.Name = "btnDropVPD";
            this.btnDropVPD.Size = new System.Drawing.Size(155, 39);
            this.btnDropVPD.TabIndex = 6;
            this.btnDropVPD.Text = "Xóa VPD";
            this.btnDropVPD.UseVisualStyleBackColor = false;
            this.btnDropVPD.Click += new System.EventHandler(this.btnDropVPD_Click);
            // 
            // btnExecVPD
            // 
            this.btnExecVPD.Location = new System.Drawing.Point(283, 116);
            this.btnExecVPD.Name = "btnExecVPD";
            this.btnExecVPD.Size = new System.Drawing.Size(155, 39);
            this.btnExecVPD.TabIndex = 5;
            this.btnExecVPD.Text = "Áp dụng VPD";
            this.btnExecVPD.UseVisualStyleBackColor = true;
            this.btnExecVPD.Click += new System.EventHandler(this.btnExecVPD_Click);
            // 
            // txtVPD_Condition
            // 
            this.txtVPD_Condition.Location = new System.Drawing.Point(283, 73);
            this.txtVPD_Condition.Name = "txtVPD_Condition";
            this.txtVPD_Condition.Size = new System.Drawing.Size(252, 20);
            this.txtVPD_Condition.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(164, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Điều kiện (Where):";
            // 
            // txtVPD_Policy
            // 
            this.txtVPD_Policy.Location = new System.Drawing.Point(283, 36);
            this.txtVPD_Policy.Name = "txtVPD_Policy";
            this.txtVPD_Policy.Size = new System.Drawing.Size(252, 20);
            this.txtVPD_Policy.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(164, 39);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Tên Chính sách:";
            // 
            // tabRecovery
            // 
            this.tabRecovery.Controls.Add(this.btnViewData);
            this.tabRecovery.Controls.Add(this.btnExecRecover);
            this.tabRecovery.Controls.Add(this.label13);
            this.tabRecovery.Controls.Add(this.numRecoverTime);
            this.tabRecovery.Controls.Add(this.label12);
            this.tabRecovery.Location = new System.Drawing.Point(4, 22);
            this.tabRecovery.Name = "tabRecovery";
            this.tabRecovery.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecovery.Size = new System.Drawing.Size(768, 174);
            this.tabRecovery.TabIndex = 2;
            this.tabRecovery.Text = "Phục hồi Dữ liệu (Flashback)";
            this.tabRecovery.UseVisualStyleBackColor = true;
            // 
            // btnViewData
            // 
            this.btnViewData.BackColor = System.Drawing.Color.LightCyan;
            this.btnViewData.Location = new System.Drawing.Point(450, 91);
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.Size = new System.Drawing.Size(155, 39);
            this.btnViewData.TabIndex = 6;
            this.btnViewData.Text = "Xem Dữ liệu Bảng";
            this.btnViewData.UseVisualStyleBackColor = false;
            this.btnViewData.Click += new System.EventHandler(this.btnViewData_Click);
            // 
            // btnExecRecover
            // 
            this.btnExecRecover.ForeColor = System.Drawing.Color.Red;
            this.btnExecRecover.Location = new System.Drawing.Point(283, 91);
            this.btnExecRecover.Name = "btnExecRecover";
            this.btnExecRecover.Size = new System.Drawing.Size(155, 39);
            this.btnExecRecover.TabIndex = 5;
            this.btnExecRecover.Text = "Phục hồi Dữ liệu";
            this.btnExecRecover.UseVisualStyleBackColor = true;
            this.btnExecRecover.Click += new System.EventHandler(this.btnExecRecover_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(385, 41);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(28, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "phút";
            // 
            // numRecoverTime
            // 
            this.numRecoverTime.Location = new System.Drawing.Point(283, 39);
            this.numRecoverTime.Name = "numRecoverTime";
            this.numRecoverTime.Size = new System.Drawing.Size(96, 20);
            this.numRecoverTime.TabIndex = 1;
            this.numRecoverTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(164, 41);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Lùi về quá khứ:";
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.dgvData);
            this.grpData.Location = new System.Drawing.Point(12, 309);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(776, 230);
            this.grpData.TabIndex = 2;
            this.grpData.TabStop = false;
            this.grpData.Text = "Kết quả / Dữ liệu (Viewer)";
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvData.Location = new System.Drawing.Point(3, 16);
            this.dgvData.Name = "dgvData";
            this.dgvData.Size = new System.Drawing.Size(770, 211);
            this.dgvData.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 561);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.grpTarget);
            this.Name = "MainForm";
            this.Text = "Đồ án Security - Oracle Dynamic Tools";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpTarget.ResumeLayout(false);
            this.grpTarget.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabFGA.ResumeLayout(false);
            this.tabFGA.PerformLayout();
            this.tabVPD.ResumeLayout(false);
            this.tabVPD.PerformLayout();
            this.tabRecovery.ResumeLayout(false);
            this.tabRecovery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecoverTime)).EndInit();
            this.grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpTarget;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboSchema;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabFGA;
        private System.Windows.Forms.TabPage tabVPD;
        private System.Windows.Forms.TabPage tabRecovery;
        private System.Windows.Forms.Button btnExecFGA;
        private System.Windows.Forms.TextBox txtFGA_Col;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFGA_Policy;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnExecVPD;
        private System.Windows.Forms.TextBox txtVPD_Condition;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtVPD_Policy;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnExecRecover;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numRecoverTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Button btnViewData;
        private System.Windows.Forms.Button btnDropVPD; // Khai báo nút mới
    }
}