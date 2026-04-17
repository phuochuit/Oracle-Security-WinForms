namespace ConnectOracle
{
    partial class PhanQuyen
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
            this.cbo_mode = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_grantrole = new System.Windows.Forms.Button();
            this.btn_revokerole = new System.Windows.Forms.Button();
            this.btn_adduser = new System.Windows.Forms.Button();
            this.btn_removeuser = new System.Windows.Forms.Button();
            this.btn_droprole = new System.Windows.Forms.Button();
            this.btn_createrole = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_grant = new System.Windows.Forms.Button();
            this.btn_revokeall = new System.Windows.Forms.Button();
            this.btnGrant_Click = new System.Windows.Forms.Button();
            this.btn_revoke = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboRoleGroup = new System.Windows.Forms.ComboBox();
            this.cboUser = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTable = new System.Windows.Forms.ComboBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.chkUpdate = new System.Windows.Forms.CheckBox();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.chkInsert = new System.Windows.Forms.CheckBox();
            this.btn_GrantDBA = new System.Windows.Forms.Button();
            this.btn_RevokeDBA = new System.Windows.Forms.Button();
            this.btn_GrantOption = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbo_mode
            // 
            this.cbo_mode.FormattingEnabled = true;
            this.cbo_mode.Location = new System.Drawing.Point(345, 236);
            this.cbo_mode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbo_mode.Name = "cbo_mode";
            this.cbo_mode.Size = new System.Drawing.Size(108, 24);
            this.cbo_mode.TabIndex = 21;
            this.cbo_mode.SelectedIndexChanged += new System.EventHandler(this.cbo_mode_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_grantrole);
            this.groupBox2.Controls.Add(this.btn_revokerole);
            this.groupBox2.Controls.Add(this.btn_adduser);
            this.groupBox2.Controls.Add(this.btn_removeuser);
            this.groupBox2.Controls.Add(this.btn_droprole);
            this.groupBox2.Controls.Add(this.btn_createrole);
            this.groupBox2.Location = new System.Drawing.Point(500, 287);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(275, 220);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RoleGroup";
            // 
            // btn_grantrole
            // 
            this.btn_grantrole.Location = new System.Drawing.Point(151, 33);
            this.btn_grantrole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_grantrole.Name = "btn_grantrole";
            this.btn_grantrole.Size = new System.Drawing.Size(105, 26);
            this.btn_grantrole.TabIndex = 3;
            this.btn_grantrole.Text = "GrantRole";
            this.btn_grantrole.UseVisualStyleBackColor = true;
            this.btn_grantrole.Click += new System.EventHandler(this.btn_grantrole_Click);
            // 
            // btn_revokerole
            // 
            this.btn_revokerole.Location = new System.Drawing.Point(151, 86);
            this.btn_revokerole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_revokerole.Name = "btn_revokerole";
            this.btn_revokerole.Size = new System.Drawing.Size(105, 26);
            this.btn_revokerole.TabIndex = 3;
            this.btn_revokerole.Text = "RevokeRole";
            this.btn_revokerole.UseVisualStyleBackColor = true;
            this.btn_revokerole.Click += new System.EventHandler(this.btn_revokerole_Click);
            // 
            // btn_adduser
            // 
            this.btn_adduser.Location = new System.Drawing.Point(151, 138);
            this.btn_adduser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_adduser.Name = "btn_adduser";
            this.btn_adduser.Size = new System.Drawing.Size(105, 26);
            this.btn_adduser.TabIndex = 3;
            this.btn_adduser.Text = "Add User";
            this.btn_adduser.UseVisualStyleBackColor = true;
            this.btn_adduser.Click += new System.EventHandler(this.btn_adduser_Click);
            // 
            // btn_removeuser
            // 
            this.btn_removeuser.Location = new System.Drawing.Point(15, 138);
            this.btn_removeuser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_removeuser.Name = "btn_removeuser";
            this.btn_removeuser.Size = new System.Drawing.Size(105, 26);
            this.btn_removeuser.TabIndex = 3;
            this.btn_removeuser.Text = "Remove User";
            this.btn_removeuser.UseVisualStyleBackColor = true;
            this.btn_removeuser.Click += new System.EventHandler(this.btn_removeuser_Click);
            // 
            // btn_droprole
            // 
            this.btn_droprole.Location = new System.Drawing.Point(15, 86);
            this.btn_droprole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_droprole.Name = "btn_droprole";
            this.btn_droprole.Size = new System.Drawing.Size(105, 26);
            this.btn_droprole.TabIndex = 3;
            this.btn_droprole.Text = "Drop Role";
            this.btn_droprole.UseVisualStyleBackColor = true;
            this.btn_droprole.Click += new System.EventHandler(this.btn_droprole_Click);
            // 
            // btn_createrole
            // 
            this.btn_createrole.Location = new System.Drawing.Point(15, 33);
            this.btn_createrole.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_createrole.Name = "btn_createrole";
            this.btn_createrole.Size = new System.Drawing.Size(105, 26);
            this.btn_createrole.TabIndex = 3;
            this.btn_createrole.Text = "Create Role";
            this.btn_createrole.UseVisualStyleBackColor = true;
            this.btn_createrole.Click += new System.EventHandler(this.btn_createrole_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_grant);
            this.groupBox1.Controls.Add(this.btn_revokeall);
            this.groupBox1.Controls.Add(this.btnGrant_Click);
            this.groupBox1.Controls.Add(this.btn_revoke);
            this.groupBox1.Location = new System.Drawing.Point(287, 287);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(160, 220);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "User";
            // 
            // btn_grant
            // 
            this.btn_grant.Location = new System.Drawing.Point(20, 48);
            this.btn_grant.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_grant.Name = "btn_grant";
            this.btn_grant.Size = new System.Drawing.Size(105, 26);
            this.btn_grant.TabIndex = 3;
            this.btn_grant.Text = "Grant";
            this.btn_grant.UseVisualStyleBackColor = true;
            this.btn_grant.Click += new System.EventHandler(this.btn_grant_Click);
            // 
            // btn_revokeall
            // 
            this.btn_revokeall.Location = new System.Drawing.Point(20, 180);
            this.btn_revokeall.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_revokeall.Name = "btn_revokeall";
            this.btn_revokeall.Size = new System.Drawing.Size(105, 26);
            this.btn_revokeall.TabIndex = 3;
            this.btn_revokeall.Text = "Revoke All";
            this.btn_revokeall.UseVisualStyleBackColor = true;
            this.btn_revokeall.Click += new System.EventHandler(this.btn_revokeall_Click);
            // 
            // btnGrant_Click
            // 
            this.btnGrant_Click.Location = new System.Drawing.Point(20, 135);
            this.btnGrant_Click.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGrant_Click.Name = "btnGrant_Click";
            this.btnGrant_Click.Size = new System.Drawing.Size(105, 26);
            this.btnGrant_Click.TabIndex = 3;
            this.btnGrant_Click.Text = "Grant All";
            this.btnGrant_Click.UseVisualStyleBackColor = true;
            this.btnGrant_Click.Click += new System.EventHandler(this.btn_grantall_Click);
            // 
            // btn_revoke
            // 
            this.btn_revoke.Location = new System.Drawing.Point(20, 96);
            this.btn_revoke.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_revoke.Name = "btn_revoke";
            this.btn_revoke.Size = new System.Drawing.Size(105, 26);
            this.btn_revoke.TabIndex = 3;
            this.btn_revoke.Text = "Revoke";
            this.btn_revoke.UseVisualStyleBackColor = true;
            this.btn_revoke.Click += new System.EventHandler(this.btn_revoke_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(283, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "RoleGroup";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 16);
            this.label4.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "User";
            // 
            // cboRoleGroup
            // 
            this.cboRoleGroup.FormattingEnabled = true;
            this.cboRoleGroup.Location = new System.Drawing.Point(375, 155);
            this.cboRoleGroup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboRoleGroup.Name = "cboRoleGroup";
            this.cboRoleGroup.Size = new System.Drawing.Size(431, 24);
            this.cboRoleGroup.TabIndex = 11;
            this.cboRoleGroup.SelectedIndexChanged += new System.EventHandler(this.LoadRoles_SelectedIndexChanged);
            // 
            // cboUser
            // 
            this.cboUser.FormattingEnabled = true;
            this.cboUser.Location = new System.Drawing.Point(345, 105);
            this.cboUser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboUser.Name = "cboUser";
            this.cboUser.Size = new System.Drawing.Size(460, 24);
            this.cboUser.TabIndex = 12;
            this.cboUser.SelectedIndexChanged += new System.EventHandler(this.cboUser_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(281, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(283, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 16);
            this.label1.TabIndex = 18;
            this.label1.Text = "Table";
            // 
            // cboTable
            // 
            this.cboTable.FormattingEnabled = true;
            this.cboTable.Location = new System.Drawing.Point(345, 48);
            this.cboTable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTable.Name = "cboTable";
            this.cboTable.Size = new System.Drawing.Size(460, 24);
            this.cboTable.TabIndex = 13;
            this.cboTable.SelectedIndexChanged += new System.EventHandler(this.cboTable_SelectedIndexChanged);
            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.Location = new System.Drawing.Point(732, 206);
            this.chkDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.Size = new System.Drawing.Size(69, 20);
            this.chkDelete.TabIndex = 7;
            this.chkDelete.Text = "Delete";
            this.chkDelete.UseVisualStyleBackColor = true;
            // 
            // chkUpdate
            // 
            this.chkUpdate.AutoSize = true;
            this.chkUpdate.Location = new System.Drawing.Point(585, 206);
            this.chkUpdate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkUpdate.Name = "chkUpdate";
            this.chkUpdate.Size = new System.Drawing.Size(74, 20);
            this.chkUpdate.TabIndex = 8;
            this.chkUpdate.Text = "Update";
            this.chkUpdate.UseVisualStyleBackColor = true;
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Location = new System.Drawing.Point(456, 206);
            this.chkSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(67, 20);
            this.chkSelect.TabIndex = 9;
            this.chkSelect.Text = "Select";
            this.chkSelect.UseVisualStyleBackColor = true;
            // 
            // chkInsert
            // 
            this.chkInsert.AutoSize = true;
            this.chkInsert.Location = new System.Drawing.Point(327, 206);
            this.chkInsert.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkInsert.Name = "chkInsert";
            this.chkInsert.Size = new System.Drawing.Size(61, 20);
            this.chkInsert.TabIndex = 10;
            this.chkInsert.Text = "Insert";
            this.chkInsert.UseVisualStyleBackColor = true;
            this.chkInsert.CheckedChanged += new System.EventHandler(this.chkInsert_CheckedChanged);
            // 
            // btn_GrantDBA
            // 
            this.btn_GrantDBA.Location = new System.Drawing.Point(908, 46);
            this.btn_GrantDBA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_GrantDBA.Name = "btn_GrantDBA";
            this.btn_GrantDBA.Size = new System.Drawing.Size(169, 28);
            this.btn_GrantDBA.TabIndex = 9;
            this.btn_GrantDBA.Text = "Cấp quyền admin";
            this.btn_GrantDBA.UseVisualStyleBackColor = true;
            this.btn_GrantDBA.Click += new System.EventHandler(this.btn_GrantDBA_Click);
            // 
            // btn_RevokeDBA
            // 
            this.btn_RevokeDBA.Location = new System.Drawing.Point(908, 101);
            this.btn_RevokeDBA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_RevokeDBA.Name = "btn_RevokeDBA";
            this.btn_RevokeDBA.Size = new System.Drawing.Size(169, 28);
            this.btn_RevokeDBA.TabIndex = 22;
            this.btn_RevokeDBA.Text = "Hủy quyền Admin";
            this.btn_RevokeDBA.UseVisualStyleBackColor = true;
            this.btn_RevokeDBA.Click += new System.EventHandler(this.btn_RevokeDBA_Click);
            // 
            // btn_GrantOption
            // 
            this.btn_GrantOption.Location = new System.Drawing.Point(837, 362);
            this.btn_GrantOption.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_GrantOption.Name = "btn_GrantOption";
            this.btn_GrantOption.Size = new System.Drawing.Size(204, 26);
            this.btn_GrantOption.TabIndex = 4;
            this.btn_GrantOption.Text = "Grant (Cho phép cấp lại)";
            this.btn_GrantOption.UseVisualStyleBackColor = true;
            this.btn_GrantOption.Click += new System.EventHandler(this.btn_GrantOption_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(908, 158);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 28);
            this.button1.TabIndex = 23;
            this.button1.Text = "Profile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PhanQuyen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 554);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_GrantOption);
            this.Controls.Add(this.btn_RevokeDBA);
            this.Controls.Add(this.btn_GrantDBA);
            this.Controls.Add(this.cbo_mode);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboRoleGroup);
            this.Controls.Add(this.cboUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboTable);
            this.Controls.Add(this.chkDelete);
            this.Controls.Add(this.chkUpdate);
            this.Controls.Add(this.chkSelect);
            this.Controls.Add(this.chkInsert);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PhanQuyen";
            this.Text = "PhanQuyen";
            this.Load += new System.EventHandler(this.PhanQuyen_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbo_mode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_grantrole;
        private System.Windows.Forms.Button btn_revokerole;
        private System.Windows.Forms.Button btn_adduser;
        private System.Windows.Forms.Button btn_removeuser;
        private System.Windows.Forms.Button btn_droprole;
        private System.Windows.Forms.Button btn_createrole;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_grant;
        private System.Windows.Forms.Button btn_revokeall;
        private System.Windows.Forms.Button btnGrant_Click;
        private System.Windows.Forms.Button btn_revoke;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboRoleGroup;
        private System.Windows.Forms.ComboBox cboUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTable;
        private System.Windows.Forms.CheckBox chkDelete;
        private System.Windows.Forms.CheckBox chkUpdate;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.CheckBox chkInsert;
        private System.Windows.Forms.Button btn_GrantDBA;
        private System.Windows.Forms.Button btn_RevokeDBA;
        private System.Windows.Forms.Button btn_GrantOption;
        private System.Windows.Forms.Button button1;
    }
}