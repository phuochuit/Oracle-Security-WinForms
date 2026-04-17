using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ConnectOracle
{
    
    public partial class Muc6 : Form
    {
        public Muc6()
        {
            InitializeComponent();
        }

        private void Muc6_Load(object sender, EventArgs e)
        {
            LoadUserList();
        }

        private void LoadUserList()
        {
            try
            {
                // Lấy kết nối từ class Database của bạn
                OracleConnection conn = Database.Get_Connect();

                // Kiểm tra nếu kết nối bị đóng thì mở lại (phòng hờ)
                if (conn.State != ConnectionState.Open)
                {
                    Database.Connect();
                }

                // Query lấy danh sách User. 
                // Lọc bớt các user hệ thống mặc định của Oracle để dễ nhìn hơn
                string sql = "SELECT USERNAME, ACCOUNT_STATUS, CREATED, PROFILE " +
                             "FROM DBA_USERS " +
                             "WHERE USERNAME NOT IN ('SYS','SYSTEM','ANONYMOUS','PUBLIC') " +
                             "ORDER BY CREATED DESC";

                OracleDataAdapter da = new OracleDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvUsers.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy danh sách user: " + ex.Message);
            }
        }

        private void ExecuteAdminCommand(string sql)
        {
            try
            {
                OracleConnection conn = Database.Get_Connect();

                // Đảm bảo kết nối đang mở
                if (conn.State != ConnectionState.Open) Database.Connect();

                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery(); // Thực thi lệnh

                MessageBox.Show("Thực hiện thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Load lại danh sách để thấy trạng thái mới
                LoadUserList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(user)) return;

            // Câu lệnh SQL Khóa
            string sql = $"ALTER USER \"{user}\" ACCOUNT LOCK";
            ExecuteAdminCommand(sql);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(user)) return;

            // Câu lệnh SQL Mở khóa
            string sql = $"ALTER USER \"{user}\" ACCOUNT UNLOCK";
            ExecuteAdminCommand(sql);
        }

        private void btnDrop_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            if (string.IsNullOrEmpty(user)) return;

            // Hỏi xác nhận trước khi xóa
            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa user [{user}] và toàn bộ dữ liệu của họ?",
                                              "Cảnh báo nguy hiểm",
                                              MessageBoxButtons.YesNo,
                                              MessageBoxIcon.Warning);

            if (dr == DialogResult.Yes)
            {
                // CASCADE để xóa user kể cả khi họ đang có bảng dữ liệu
                string sql = $"DROP USER \"{user}\" CASCADE";
                ExecuteAdminCommand(sql);

                // Xóa xong thì clear textbox
                txtUsername.Text = "";
            }
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                // Lấy tên user gán vào textbox để chuẩn bị thao tác
                txtUsername.Text = row.Cells["USERNAME"].Value.ToString();
            }
        }
    }
}
