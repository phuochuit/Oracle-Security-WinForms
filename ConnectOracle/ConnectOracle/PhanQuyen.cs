using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectOracle
{
    public partial class PhanQuyen : Form
    {
        private string _connectionString = "";
        public PhanQuyen()
        {
            InitializeComponent();
            CenterToScreen();

            // Lấy chuỗi kết nối từ class Database tĩnh của bạn
            if (Database.Conn != null)
            {
                _connectionString = Database.Conn.ConnectionString;
            }
        }

        private void PhanQuyen_Load(object sender, EventArgs e)
        {
            try
            {
                LoadDataToComboBoxes();
                CheckAdminButton();

                // Gán sự kiện cho các control nếu chưa gán trong Design
                cboTable.SelectedIndexChanged += (s, ev) => LoadPermissions();
                cboUser.SelectedIndexChanged += (s, ev) => { if (cbo_mode.SelectedIndex == 0) LoadPermissions(); };
                cboRoleGroup.SelectedIndexChanged += (s, ev) => { if (cbo_mode.SelectedIndex == 1) LoadPermissions(); };

                // Mode: 0 = User, 1 = Role
                cbo_mode.Items.Clear();
                cbo_mode.Items.Add("User");
                cbo_mode.Items.Add("Role");
                cbo_mode.SelectedIndex = 0;
                cbo_mode.SelectedIndexChanged += (s, ev) => LoadPermissions();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void LoadDataToComboBoxes()
        {
            try
            {
                string sqlTable = @"SELECT owner, table_name 
                            FROM all_tables 
                            WHERE owner = 'DOAN_BMCSDL' OR owner = USER 
                            ORDER BY owner, table_name";

                DataTable dtTable = GetData(sqlTable);

                // Thêm cột hiển thị dạng "OWNER.TABLE" cho dễ nhìn (Optional)
                dtTable.Columns.Add("DisplayColumn", typeof(string), "owner + '.' + table_name");

                cboTable.DataSource = dtTable;
                cboTable.DisplayMember = "DisplayColumn"; // Hiển thị kiểu: TTT.SINHVIEN
                cboTable.ValueMember = "owner";

                // 2. Load Users (Giữ nguyên hoặc lọc bớt user hệ thống)
                DataTable dtUser = GetData("SELECT username FROM all_users WHERE created > TO_DATE('01-01-2015','dd-mm-yyyy') ORDER BY username");
                cboUser.DataSource = dtUser;
                cboUser.DisplayMember = "username";
                cboUser.ValueMember = "username";

                // 3. Load Roles
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }

        private void LoadRoles()
        {
            DataTable dtRole = new DataTable();
            try
            {
                // CÁCH 1: Dành cho SYS hoặc User có quyền cao (Xem toàn bộ Role trong hệ thống)
                dtRole = GetData("SELECT role FROM dba_roles ORDER BY role");
            }
            catch
            {
                // CÁCH 2: Dành cho User thường (Nếu Cách 1 lỗi quyền, chạy cách này)
                // Chỉ hiện các Role mà user hiện tại đã được cấp hoặc có quyền nhìn thấy
                try
                {
                    dtRole = GetData("SELECT role FROM user_role_privs ORDER BY role");
                }
                catch
                {
                    // Nếu vẫn lỗi thì tạo bảng rỗng để không crash chương trình
                    dtRole = new DataTable();
                    dtRole.Columns.Add("role");
                }
            }

            // Gán dữ liệu vào ComboBox
            cboRoleGroup.DataSource = dtRole;
            cboRoleGroup.DisplayMember = "role";
            cboRoleGroup.ValueMember = "role";
        }

        // =======================================================
        // 2. KIỂM TRA QUYỀN HIỆN CÓ (Check Permissions)
        // =======================================================
        private void LoadPermissions()
        {
            if (cboTable.SelectedValue == null) return;
            DataRowView selectedRow = cboTable.SelectedItem as DataRowView;
            string tableName = selectedRow["table_name"].ToString();
            string tableOwner = selectedRow["owner"].ToString();
            string granteeName = (cbo_mode.SelectedIndex == 0) ? cboUser.Text : cboRoleGroup.Text;

            if (string.IsNullOrEmpty(granteeName)) return;

            // Reset Checkbox
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = false;

            // SỬA: Dùng all_tab_privs thay vì dba_tab_privs
            string sql = @"SELECT privilege 
                   FROM all_tab_privs 
                   WHERE table_name = :tableName 
                   AND grantee = :granteeName
                   AND table_schema = :tableOwner"; // Quan trọng: Chỉ định rõ schema

            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                {
                    cmd.Parameters.Add(new OracleParameter("tableName", tableName.ToUpper()));
                    cmd.Parameters.Add(new OracleParameter("granteeName", granteeName.ToUpper()));
                    cmd.Parameters.Add(new OracleParameter("tableOwner", tableOwner.ToUpper()));

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string priv = reader["privilege"].ToString().ToUpper();
                            switch (priv)
                            {
                                case "SELECT": chkSelect.Checked = true; break;
                                case "INSERT": chkInsert.Checked = true; break;
                                case "UPDATE": chkUpdate.Checked = true; break;
                                case "DELETE": chkDelete.Checked = true; break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi load quyền: " + ex.Message);
            }
        }
        private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadRoles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbo_mode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_grant_Click(object sender, EventArgs e)
        {
            ProcessPermission("GRANT", false);
        }

        private void btn_revoke_Click(object sender, EventArgs e)
        {
            ProcessPermission("REVOKE");
        }

        // Trong PhanQuyen.cs

        // Thêm tham số withGrantOption (mặc định là false)
        private void ProcessPermission(string action, bool withGrantOption = false)
        {
            // 1. Lấy thông tin từ ComboBox Table
            if (cboTable.SelectedItem == null) return;


            DataRowView selectedRow = cboTable.SelectedItem as DataRowView;
            string tableName = selectedRow["table_name"].ToString();
            string tableOwner = selectedRow["owner"].ToString();

            string targetName = (cbo_mode.SelectedIndex == 0) ? cboUser.Text : cboRoleGroup.Text;

            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(targetName))
            {
                MessageBox.Show("Vui lòng chọn Bảng và User/Role.");
                return;
            }

            // 2. Gom các quyền được chọn
            string perms = "";
            if (chkSelect.Checked) perms += "SELECT, ";
            if (chkInsert.Checked) perms += "INSERT, ";
            if (chkUpdate.Checked) perms += "UPDATE, ";
            if (chkDelete.Checked) perms += "DELETE, ";

            if (perms == "")
            {
                MessageBox.Show("Vui lòng chọn ít nhất một quyền!");
                return;
            }
            perms = perms.TrimEnd(',', ' ');

            // 3. Tạo câu lệnh SQL
            string fullTableName = $"{tableOwner}.{tableName}";
            string preposition = (action == "GRANT") ? "TO" : "FROM";

            string sqlTable = $"{action} {perms} ON {fullTableName} {preposition} {targetName}";

            // 🟢 ĐOẠN MỚI: Thêm WITH GRANT OPTION nếu được yêu cầu
            if (action == "GRANT" && withGrantOption)
            {
                sqlTable += " WITH GRANT OPTION";
            }

            // 4. Thực thi
            if (ExecuteOracleCommand(sqlTable))
            {
                string msg = $"{action} thành công trên bảng {fullTableName}!";
                if (withGrantOption) msg += "\n(Đã cho phép User này cấp quyền lại cho người khác)";

                MessageBox.Show(msg);
                LoadPermissions();
            }
        }
        private void btn_grantall_Click(object sender, EventArgs e)
        {
            // Cấp tất cả 4 quyền
            chkSelect.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = true;
            ProcessPermission("GRANT");
        }

        private void btn_revokeall_Click(object sender, EventArgs e)
        {
            chkInsert.Checked = chkInsert.Checked = chkUpdate.Checked = chkDelete.Checked = true;
            btn_revoke_Click(sender, e);
        }

        private void btn_createrole_Click(object sender, EventArgs e)
        {
            // Lấy text nhập tay
            string roleName = cboRoleGroup.Text.Trim();
            if (string.IsNullOrEmpty(roleName)) return;

            if (!IsValidName(roleName))
            {
                MessageBox.Show("Tên Role không được chứa ký tự đặc biệt hoặc khoảng trắng!");
                return;
            }

            string sql = $"CREATE ROLE {roleName}";
            if (ExecuteOracleCommand(sql))
            {
                MessageBox.Show("Tạo Role thành công!");
                LoadRoles();
            }
        }

        private void btn_droprole_Click(object sender, EventArgs e)
        {
            string roleName = cboRoleGroup.Text.Trim();
            if (string.IsNullOrEmpty(roleName)) return;

            string sql = $"DROP ROLE {roleName}";
            if (ExecuteOracleCommand(sql))
            {
                MessageBox.Show("Xóa Role thành công!");
                LoadRoles();
            }
        }

        private void btn_adduser_Click(object sender, EventArgs e)
        {
            string roleName = cboRoleGroup.Text;
            string userName = cboUser.Text;

            // Trong Oracle: Gán role cho user = GRANT Role TO User
            string sql = $"GRANT {roleName} TO {userName}";

            if (ExecuteOracleCommand(sql))
                MessageBox.Show($"Đã thêm User {userName} vào Role {roleName}");
        }

        private void btn_removeuser_Click(object sender, EventArgs e)
        {
            string roleName = cboRoleGroup.Text;
            string userName = cboUser.Text;

            // Trong Oracle: Xóa user khỏi role = REVOKE Role FROM User
            string sql = $"REVOKE {roleName} FROM {userName}";

            if (ExecuteOracleCommand(sql))
                MessageBox.Show($"Đã gỡ User {userName} khỏi Role {roleName}");
        }

        private void btn_grantrole_Click(object sender, EventArgs e)
        {
            // Đảm bảo đang chọn Mode là Role để hàm lấy đúng tên
            cbo_mode.SelectedIndex = 1;
            ProcessPermission("GRANT");
        }

        private void btn_revokerole_Click(object sender, EventArgs e)
        {
            cbo_mode.SelectedIndex = 1;
            ProcessPermission("REVOKE");
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Oracle không hỗ trợ lệnh DENY trực tiếp. Hệ thống sẽ thực hiện REVOKE thay thế.");
            ProcessPermission("REVOKE");
        }

        private void btnRevokeDeny_Click(object sender, EventArgs e)
        {

        }

        private bool IsValidName(string input)
        {
            // Chỉ cho phép chữ (a-z, A-Z), số (0-9), gạch dưới (_)
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[a-zA-Z0-9_]+$");
        }

        private bool ExecuteOracleCommand(string sql)
        {
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                {
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (OracleException ex)
            {
                MessageBox.Show("Lỗi Oracle: " + ex.Message);
                return false;
            }
        }

        // Hàm lấy dữ liệu vào DataTable (SELECT)
        private DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
            using (OracleDataAdapter da = new OracleDataAdapter(sql, Database.Conn))
            {
                da.Fill(dt);
            }
            return dt;
        }

        private void btn_GrantDBA_Click(object sender, EventArgs e)
        {
            // 1. Lấy tên User từ ComboBox
            string userName = cboUser.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng chọn User cần cấp quyền!");
                return;
            }

            // 2. Viết câu lệnh SQL (Grant Role DBA)
            string sql = $"GRANT DBA TO {userName}";

            // 3. Thực thi
            if (ExecuteOracleCommand(sql))
            {
                MessageBox.Show($"Đã cấp quyền Admin (DBA) thành công cho {userName}!");
            }
        }

        private void CheckAdminButton()
        {
            // 1. Lấy tên user hiện tại từ Database
            string currentUser = GetCurrentOracleUser();

            // 2. Kiểm tra xem có phải là SYS không
            bool isSys = currentUser.Equals("SYS", StringComparison.OrdinalIgnoreCase);

            // --- Cấu hình nút Cấp quyền (Grant) ---
            btn_GrantDBA.Enabled = isSys;
            btn_GrantDBA.Text = isSys ? "Cấp quyền DBA" : "Chỉ SYS mới được dùng";
            btn_GrantDBA.BackColor = isSys ? Color.Red : Color.Gray;

            // --- Cấu hình nút Hủy quyền (Revoke) - MỚI THÊM ---
            // Đảm bảo bạn đã tạo nút btn_RevokeDBA bên giao diện Design
            if (btn_RevokeDBA != null)
            {
                btn_RevokeDBA.Enabled = isSys;
                btn_RevokeDBA.BackColor = isSys ? Color.OrangeRed : Color.Gray;
            }
        }

        // Hàm lấy user từ Oracle (Copy vào class PhanQuyen nếu chưa có)
        private string GetCurrentOracleUser()
        {
            string currentUser = "";
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
                using (OracleCommand cmd = new OracleCommand("SELECT USER FROM DUAL", Database.Conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null) currentUser = result.ToString();
                }
            }
            catch { }
            return currentUser;
        }

        private void btn_RevokeDBA_Click(object sender, EventArgs e)
        {
            // 1. Lấy tên User từ ComboBox
            string userName = cboUser.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                MessageBox.Show("Vui lòng chọn User cần hủy quyền!");
                return;
            }

            // 2. Viết câu lệnh SQL (REVOKE Role DBA)
            // Cú pháp: REVOKE DBA FROM <TênUser>
            string sql = $"REVOKE DBA FROM {userName}";

            // 3. Thực thi
            // Sử dụng lại hàm ExecuteOracleCommand có sẵn của bạn
            if (ExecuteOracleCommand(sql))
            {
                MessageBox.Show($"Đã HỦY quyền Admin (DBA) thành công khỏi user {userName}!");
            }
        }

        private void btn_GrantOption_Click(object sender, EventArgs e)
        {
            ProcessPermission("GRANT", true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QuanLyProfile ql = new QuanLyProfile();
            ql.ShowDialog();
        }

        private void chkInsert_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
