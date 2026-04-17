using Oracle.ManagedDataAccess.Client;
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

namespace ConnectOracle
{
    public partial class Muc5 : Form
    {
        public Muc5()
        {
            InitializeComponent();
        }

        private void Muc5_Load(object sender, EventArgs e)
        {
            LoadComboBoxData();
            SuggestFilePath();
        }

        private void LoadComboBoxData()
        {
            try
            {
                OracleConnection conn = Database.Get_Connect();
                if (conn.State != ConnectionState.Open) Database.Connect();

                // Load danh sách User (loại bỏ user hệ thống để dễ nhìn)
                string sqlUser = "SELECT USERNAME FROM ALL_USERS WHERE USERNAME NOT IN ('SYS','SYSTEM') ORDER BY USERNAME";
                OracleDataAdapter daUser = new OracleDataAdapter(sqlUser, conn);
                DataTable dtUser = new DataTable();
                daUser.Fill(dtUser);

                cboUsers.DataSource = dtUser;
                cboUsers.DisplayMember = "USERNAME";
                cboUsers.ValueMember = "USERNAME";

                // Load danh sách Tablespace
                string sqlTBS = "SELECT TABLESPACE_NAME FROM USER_TABLESPACES";
                OracleDataAdapter daTBS = new OracleDataAdapter(sqlTBS, conn);
                DataTable dtTBS = new DataTable();
                daTBS.Fill(dtTBS);

                cboTablespaces.DataSource = dtTBS;
                cboTablespaces.DisplayMember = "TABLESPACE_NAME";
                cboTablespaces.ValueMember = "TABLESPACE_NAME";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void SuggestFilePath()
        {
            try
            {
                OracleConnection conn = Database.Get_Connect();
                string sql = "SELECT FILE_NAME FROM DBA_DATA_FILES WHERE ROWNUM = 1";
                OracleCommand cmd = new OracleCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    // Lấy thư mục chứa file đó
                    string fullPath = result.ToString();
                    string directory = Path.GetDirectoryName(fullPath);
                    txtPath.Text = directory + "\\";
                }
            }
            catch
            {
                // Nếu không lấy được thì để trống hoặc để mặc định ổ C
                txtPath.Text = @"C:\app\oracle\oradata\ORCL\";
            }
        }

        private void btnCreateTBS_Click(object sender, EventArgs e)
        {
            string tbsName = txtTablespaceName.Text.Trim();
            string path = txtPath.Text.Trim(); // Đường dẫn thư mục
            int size = (int)numSize.Value;

            if (string.IsNullOrEmpty(tbsName) || string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Vui lòng nhập tên Tablespace và đường dẫn lưu file.");
                return;
            }

            // Tên file .dbf sẽ tự đặt theo tên tablespace
            string fullFilePath = Path.Combine(path, tbsName + ".dbf");

            // SQL Tạo Tablespace
            // Cú pháp: CREATE TABLESPACE ten DATAFILE 'duong_dan' SIZE 100M AUTOEXTEND ON;
            string sql = $"CREATE TABLESPACE {tbsName} " +
                         $"DATAFILE '{fullFilePath}' " +
                         $"SIZE {size}M " +
                         $"AUTOEXTEND ON NEXT 10M MAXSIZE UNLIMITED";

            try
            {
                OracleConnection conn = Database.Get_Connect();
                if (conn.State != ConnectionState.Open) Database.Connect();

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Tạo Tablespace [{tbsName}] thành công!\nFile lưu tại: {fullFilePath}");

                // Refresh lại combobox Tablespace
                LoadComboBoxData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo Tablespace: " + ex.Message + "\n\nLưu ý: Đường dẫn file phải tồn tại trên máy SERVER cài Oracle.");
            }
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            if (cboUsers.SelectedValue == null || cboTablespaces.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn User và Tablespace.");
                return;
            }

            string user = cboUsers.SelectedValue.ToString();
            string tbs = cboTablespaces.SelectedValue.ToString();
            string quota = "";

            if (chkUnlimited.Checked)
            {
                quota = "UNLIMITED";
            }
            else
            {
                quota = numQuota.Value.ToString() + "M";
            }

            // SQL Cấp Quota
            // Cú pháp: ALTER USER ten_user QUOTA 50M ON ten_tablespace;
            string sql = $"ALTER USER \"{user}\" QUOTA {quota} ON {tbs}";

            try
            {
                OracleConnection conn = Database.Get_Connect();
                if (conn.State != ConnectionState.Open) Database.Connect();

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Đã cấp quyền lưu trữ ({quota}) cho user [{user}] trên tablespace [{tbs}].");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cấp Quota: " + ex.Message);
            }
        }

        private void chkUnlimited_CheckedChanged(object sender, EventArgs e)
        {
            numQuota.Enabled = !chkUnlimited.Checked;
        }
    }
}
