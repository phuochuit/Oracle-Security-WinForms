using Oracle.ManagedDataAccess.Client;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing; // Thư viện để vẽ giao diện
using System.Windows.Forms;
using UtilsLib;

namespace ConnectOracle
{
    public class QuanLyProfile : Form
    {
        // --- CÁC BIẾN GIAO DIỆN ---
        private DataGridView dgvUsers;
        private TextBox txtProfileName;
        private NumericUpDown numLimitSession, numPassLife, numFailedLogin;
        private Button btnCreateProfile, btnAssign, btnUnlock, btnResetPass;
        private ComboBox cboUsers, cboProfiles;
        private TextBox txtNewPass;
        private IContainer components = null;

        public QuanLyProfile()
        {
            // QUAN TRỌNG: Hàm này sẽ vẽ các nút bấm lên màn hình
            InitializeComponent_CodeTay();
            CenterToScreen();
        }

        // --- 1. HÀM TỰ VẼ GIAO DIỆN (Thay cho Designer) ---
        private void InitializeComponent_CodeTay()
        {
            this.Size = new Size(1100, 650);
            this.Text = "QUẢN LÝ PROFILE & TÀI KHOẢN (SECURITY ADMIN)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);

            // --- PHẦN TRÁI: DANH SÁCH USER ---
            dgvUsers = new DataGridView();
            dgvUsers.Location = new Point(20, 50);
            dgvUsers.Size = new Size(600, 530);
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.CellClick += DgvUsers_CellClick;

            Label lblList = new Label { Text = "DANH SÁCH USER HỆ THỐNG", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = Color.Blue };
            this.Controls.Add(lblList);
            this.Controls.Add(dgvUsers);

            // --- PHẦN PHẢI: CÔNG CỤ ---
            int x = 650;
            int y = 50;
            int w = 400;

            // GROUP 1: TẠO PROFILE
            GroupBox grpProfile = new GroupBox { Text = "1. TẠO PROFILE MỚI", Location = new Point(x, y), Size = new Size(w, 240), ForeColor = Color.DarkBlue };

            grpProfile.Controls.Add(new Label { Text = "Tên Profile:", Location = new Point(20, 30), AutoSize = true });
            txtProfileName = new TextBox { Location = new Point(160, 27), Width = 200 };
            grpProfile.Controls.Add(txtProfileName);

            grpProfile.Controls.Add(new Label { Text = "Số Session/User:", Location = new Point(20, 70), AutoSize = true });
            numLimitSession = new NumericUpDown { Location = new Point(160, 67), Width = 200, Value = 2 };
            grpProfile.Controls.Add(numLimitSession);

            grpProfile.Controls.Add(new Label { Text = "Hạn Pass (ngày):", Location = new Point(20, 110), AutoSize = true });
            numPassLife = new NumericUpDown { Location = new Point(160, 107), Width = 200, Value = 30, Maximum = 365 };
            grpProfile.Controls.Add(numPassLife);

            grpProfile.Controls.Add(new Label { Text = "Sai Pass bị khóa:", Location = new Point(20, 150), AutoSize = true });
            numFailedLogin = new NumericUpDown { Location = new Point(160, 147), Width = 200, Value = 3 };
            grpProfile.Controls.Add(numFailedLogin);

            btnCreateProfile = new Button { Text = "TẠO PROFILE", Location = new Point(160, 190), Width = 200, Height = 35, BackColor = Color.LightSkyBlue };
            btnCreateProfile.Click += BtnCreateProfile_Click;
            grpProfile.Controls.Add(btnCreateProfile);

            this.Controls.Add(grpProfile);

            // GROUP 2: QUẢN LÝ USER
            y += 260;
            GroupBox grpUser = new GroupBox { Text = "2. QUẢN LÝ TÀI KHOẢN", Location = new Point(x, y), Size = new Size(w, 270), ForeColor = Color.DarkRed };

            grpUser.Controls.Add(new Label { Text = "User đang chọn:", Location = new Point(20, 30), AutoSize = true });
            cboUsers = new ComboBox { Location = new Point(160, 27), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            grpUser.Controls.Add(cboUsers);

            grpUser.Controls.Add(new Label { Text = "Gán Profile:", Location = new Point(20, 70), AutoSize = true });
            cboProfiles = new ComboBox { Location = new Point(160, 67), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            grpUser.Controls.Add(cboProfiles);

            btnAssign = new Button { Text = "GÁN PROFILE", Location = new Point(160, 100), Width = 200, Height = 30, BackColor = Color.LightGreen };
            btnAssign.Click += BtnAssign_Click;
            grpUser.Controls.Add(btnAssign);

            // Dòng kẻ
            Label line = new Label { Text = "____________________________________________", Location = new Point(20, 130), AutoSize = true, ForeColor = Color.Gray };
            grpUser.Controls.Add(line);

            btnUnlock = new Button { Text = "MỞ KHÓA USER (UNLOCK)", Location = new Point(160, 160), Width = 200, Height = 35, BackColor = Color.Orange, Font = new Font(this.Font, FontStyle.Bold) };
            btnUnlock.Click += BtnUnlock_Click;
            grpUser.Controls.Add(btnUnlock);

            grpUser.Controls.Add(new Label { Text = "Pass mới:", Location = new Point(20, 210), AutoSize = true });
            txtNewPass = new TextBox { Location = new Point(100, 207), Width = 150 };
            grpUser.Controls.Add(txtNewPass);

            btnResetPass = new Button { Text = "ĐỔI PASS", Location = new Point(260, 205), Width = 100, Height = 30, BackColor = Color.LightCoral };
            btnResetPass.Click += BtnResetPass_Click;
            grpUser.Controls.Add(btnResetPass);

            this.Controls.Add(grpUser);
        }

        // --- 2. LOGIC XỬ LÝ (BACKEND) ---

        private void QuanLyProfile_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load User
                string sqlUser = "SELECT USERNAME, ACCOUNT_STATUS, PROFILE, CREATED FROM DBA_USERS WHERE USERNAME NOT IN ('SYS','SYSTEM') ORDER BY CREATED DESC";
                DataTable dtUser = GetData(sqlUser);
                dgvUsers.DataSource = dtUser;
                cboUsers.DataSource = dtUser;
                cboUsers.DisplayMember = "USERNAME";
                cboUsers.ValueMember = "USERNAME";

                // Load Profile
                string sqlProfile = "SELECT DISTINCT PROFILE FROM DBA_PROFILES ORDER BY PROFILE";
                DataTable dtProfile = GetData(sqlProfile);
                cboProfiles.DataSource = dtProfile;
                cboProfiles.DisplayMember = "PROFILE";
                cboProfiles.ValueMember = "PROFILE";
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void BtnCreateProfile_Click(object sender, EventArgs e)
        {
            string sql = $@"CREATE PROFILE {txtProfileName.Text} LIMIT 
                            SESSIONS_PER_USER {numLimitSession.Value} 
                            PASSWORD_LIFE_TIME {numPassLife.Value} 
                            FAILED_LOGIN_ATTEMPTS {numFailedLogin.Value}";
            if (ExecuteCmd(sql)) { MessageBox.Show("Tạo thành công!"); LoadData(); }
        }

        private void BtnAssign_Click(object sender, EventArgs e)
        {
            if (ExecuteCmd($"ALTER USER \"{cboUsers.Text}\" PROFILE \"{cboProfiles.Text}\""))
            { MessageBox.Show("Gán thành công!"); LoadData(); }
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (ExecuteCmd($"ALTER USER \"{cboUsers.Text}\" ACCOUNT UNLOCK"))
            { MessageBox.Show("Đã MỞ KHÓA!"); LoadData(); }
        }

        private void BtnResetPass_Click(object sender, EventArgs e)
        {
            // 1. Lấy thông tin đầu vào
            string user = cboUsers.Text.Trim();
            string rawPass = txtNewPass.Text.Trim(); // Ví dụ: Admin nhập "aaa"

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(rawPass))
            {
                MessageBox.Show("Vui lòng nhập User và Mật khẩu mới!");
                return;
            }

            string passwordToSave = rawPass; // Mặc định là pass thô

            // 2. LOGIC MÃ HÓA (Phải khớp 100% với Form Đăng Nhập)
            // Kiểm tra các user đặc biệt không cần mã hóa (giống bên Login)
            string upperUser = user.ToUpper();
            if (!user.Equals("DoAn_BMCSDL", StringComparison.OrdinalIgnoreCase) &&
                !upperUser.Equals("SYS") &&
                !upperUser.Equals("SYSTEM"))
            {
                try
                {
                    // QUAN TRỌNG: Biến "aaa" thành "ttt" trước khi lưu xuống DB
                    passwordToSave = OracleEncryptionUtils.EncryptWithOracle(rawPass, 19);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi mã hóa: " + ex.Message);
                    return;
                }
            }

            // 3. Gửi lệnh xuống Oracle
            // Lúc này lệnh sẽ là: ALTER USER ... IDENTIFIED BY "ttt"
            // Oracle sẽ lưu "ttt" làm mật khẩu.
            string sql = $"ALTER USER \"{user}\" IDENTIFIED BY \"{passwordToSave}\"";

            if (ExecuteAdminCommand(sql))
            {
                MessageBox.Show($"Đã đổi mật khẩu thành công!\n(User nhập '{rawPass}' -> Hệ thống lưu '{passwordToSave}')");
            }
        }

        // Hàm hỗ trợ thực thi lệnh SQL (INSERT, UPDATE, DELETE, CREATE, ALTER...)
        private bool ExecuteAdminCommand(string sql)
        {
            try
            {
                // Lấy kết nối từ class Database chung của bạn
                OracleConnection conn = Database.Get_Connect();

                // Kiểm tra nếu đóng thì mở lại
                if (conn.State != ConnectionState.Open) Database.Connect();

                // Thực thi lệnh
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();

                return true; // Thành công
            }
            catch (Exception ex)
            {
                // Nếu lỗi (ví dụ: User không đủ quyền, sai cú pháp...) thì báo lỗi
                MessageBox.Show("Lỗi Oracle: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Thất bại
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) { cboUsers.SelectedValue = dgvUsers.Rows[e.RowIndex].Cells["USERNAME"].Value.ToString(); }
        }

        // --- HÀM HỖ TRỢ ---
        private DataTable GetData(string sql)
        {
            OracleConnection conn = Database.Get_Connect();
            if (conn.State != ConnectionState.Open) Database.Connect();
            OracleDataAdapter da = new OracleDataAdapter(sql, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        private bool ExecuteCmd(string sql)
        {
            try
            {
                OracleConnection conn = Database.Get_Connect();
                if (conn.State != ConnectionState.Open) Database.Connect();
                new OracleCommand(sql, conn).ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }
    }
}