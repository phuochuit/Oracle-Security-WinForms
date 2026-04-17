namespace ConnectOracle
{
    partial class Muc5
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
            this.txtTablespaceName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.numSize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCreateTBS = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGrant = new System.Windows.Forms.Button();
            this.numQuota = new System.Windows.Forms.NumericUpDown();
            this.chkUnlimited = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboUsers = new System.Windows.Forms.ComboBox();
            this.cboTablespaces = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuota)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTablespaceName
            // 
            this.txtTablespaceName.Location = new System.Drawing.Point(166, 46);
            this.txtTablespaceName.Name = "txtTablespaceName";
            this.txtTablespaceName.Size = new System.Drawing.Size(260, 20);
            this.txtTablespaceName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(76, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tablespace:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(45, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Đường dẫn lưu file:";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(166, 72);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(260, 20);
            this.txtPath.TabIndex = 3;
            // 
            // numSize
            // 
            this.numSize.Location = new System.Drawing.Point(166, 98);
            this.numSize.Name = "numSize";
            this.numSize.Size = new System.Drawing.Size(120, 20);
            this.numSize.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(70, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Kích thước file:";
            // 
            // btnCreateTBS
            // 
            this.btnCreateTBS.Location = new System.Drawing.Point(166, 124);
            this.btnCreateTBS.Name = "btnCreateTBS";
            this.btnCreateTBS.Size = new System.Drawing.Size(113, 23);
            this.btnCreateTBS.TabIndex = 6;
            this.btnCreateTBS.Text = "Tạo Tablespace";
            this.btnCreateTBS.UseVisualStyleBackColor = true;
            this.btnCreateTBS.Click += new System.EventHandler(this.btnCreateTBS_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Chọn user cấp quyền";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(51, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 16);
            this.label5.TabIndex = 8;
            this.label5.Text = "Chọn tablespace";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(59, 262);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 16);
            this.label6.TabIndex = 9;
            this.label6.Text = "Dung lượng cấp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(27, 295);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(291, 16);
            this.label7.TabIndex = 10;
            this.label7.Text = "Check vào nếu muốn không giới hạn dung lượng";
            // 
            // btnGrant
            // 
            this.btnGrant.Location = new System.Drawing.Point(166, 314);
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.Size = new System.Drawing.Size(113, 23);
            this.btnGrant.TabIndex = 17;
            this.btnGrant.Text = "Cấp Quota";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
            // 
            // numQuota
            // 
            this.numQuota.Location = new System.Drawing.Point(166, 262);
            this.numQuota.Name = "numQuota";
            this.numQuota.Size = new System.Drawing.Size(120, 20);
            this.numQuota.TabIndex = 18;
            // 
            // chkUnlimited
            // 
            this.chkUnlimited.AutoSize = true;
            this.chkUnlimited.Location = new System.Drawing.Point(324, 297);
            this.chkUnlimited.Name = "chkUnlimited";
            this.chkUnlimited.Size = new System.Drawing.Size(15, 14);
            this.chkUnlimited.TabIndex = 19;
            this.chkUnlimited.UseVisualStyleBackColor = true;
            this.chkUnlimited.CheckedChanged += new System.EventHandler(this.chkUnlimited_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(12, 177);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 16);
            this.label8.TabIndex = 20;
            this.label8.Text = "Cấp Quota";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "Tạo Tablespace";
            // 
            // cboUsers
            // 
            this.cboUsers.FormattingEnabled = true;
            this.cboUsers.Location = new System.Drawing.Point(166, 210);
            this.cboUsers.Name = "cboUsers";
            this.cboUsers.Size = new System.Drawing.Size(260, 21);
            this.cboUsers.TabIndex = 22;
            // 
            // cboTablespaces
            // 
            this.cboTablespaces.FormattingEnabled = true;
            this.cboTablespaces.Location = new System.Drawing.Point(166, 235);
            this.cboTablespaces.Name = "cboTablespaces";
            this.cboTablespaces.Size = new System.Drawing.Size(260, 21);
            this.cboTablespaces.TabIndex = 23;
            // 
            // Muc5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 450);
            this.Controls.Add(this.cboTablespaces);
            this.Controls.Add(this.cboUsers);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.chkUnlimited);
            this.Controls.Add(this.numQuota);
            this.Controls.Add(this.btnGrant);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCreateTBS);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numSize);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTablespaceName);
            this.Name = "Muc5";
            this.Text = "Muc5";
            this.Load += new System.EventHandler(this.Muc5_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numQuota)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTablespaceName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.NumericUpDown numSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCreateTBS;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGrant;
        private System.Windows.Forms.NumericUpDown numQuota;
        private System.Windows.Forms.CheckBox chkUnlimited;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboUsers;
        private System.Windows.Forms.ComboBox cboTablespaces;
    }
}