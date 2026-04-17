using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilsLib;
using Oracle.ManagedDataAccess.Client;

namespace SignUpForm
{
    public partial class SignUp : Form
    {
        string sysConnection = "User Id=sys;Password=sys;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.241.234.95)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));DBA Privilege=SYSDBA;";

        public SignUp()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. LẤY DỮ LIỆU GỐC
            string originalUser = txtUser.Text.Trim(); // Ví dụ: "kkk"
            string originalPass = txtPass.Text.Trim();
            string userEmail = txtEmail.Text.Trim();

            // 2. VALIDATE (GIỮ NGUYÊN)
            if (string.IsNullOrEmpty(originalUser) || string.IsNullOrEmpty(originalPass) || string.IsNullOrEmpty(userEmail))
            {
                MessageBox.Show("⚠️ Vui lòng nhập đầy đủ thông tin!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!userEmail.Contains("@") || !userEmail.Contains("."))
            {
                MessageBox.Show("❌ Email không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (OracleEncryptionUtils.IsAdminUser(originalUser))
            {
                MessageBox.Show("❌ Không được tạo tài khoản quản trị!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // =========================================================
                // 2. TÍNH TOÁN USER VÀ PASS (MÃ HÓA CẢ HAI)
                // =========================================================

                string userToCreate = originalUser;
                string passToCreate = originalPass; // Mặc định giữ nguyên

                // Nếu không phải user đặc biệt thì MÃ HÓA CẢ USER VÀ PASS
                if (!originalUser.Equals("DoAn_BMCSDL", StringComparison.OrdinalIgnoreCase))
                {
                    userToCreate = OracleEncryptionUtils.EncryptWithOracle(originalUser, 19);
                    passToCreate = OracleEncryptionUtils.EncryptWithOracle(originalPass, 19); // 🔥 SỬA: Mã hóa password luôn
                }

                // Kiểm tra an toàn
                if (string.IsNullOrEmpty(userToCreate) || string.IsNullOrEmpty(passToCreate))
                {
                    MessageBox.Show($"❌ Lỗi mã hóa: Tên hoặc mật khẩu bị rỗng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // =========================================================
                // 3. GỬI OTP
                // =========================================================
                string otpCode = EmailHelper.GenerateOTP();

                this.Enabled = false; this.Cursor = Cursors.WaitCursor;
                bool isSent = EmailHelper.SendOTP(userEmail, otpCode);
                this.Cursor = Cursors.Default; this.Enabled = true;

                if (!isSent)
                {
                    MessageBox.Show("❌ Không thể gửi Email OTP.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                FormNhapOTP frmOTP = new FormNhapOTP(otpCode);
                frmOTP.ShowDialog();
                if (!frmOTP.IsVerified) return;

                // =========================================================
                // 4. TẠO USER VÀ LƯU DB
                // =========================================================

                using (OracleConnection conn = new OracleConnection(sysConnection))
                {
                    conn.Open();

                    // A. TẠO USER ORACLE
                    // 🔥 SỬA: Dùng passToCreate (đã mã hóa) thay vì originalPass
                    string sqlCreate = $"CREATE USER \"{userToCreate.ToUpper()}\" IDENTIFIED BY \"{passToCreate}\"";

                    using (OracleCommand cmd = new OracleCommand(sqlCreate, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // B. CẤP QUYỀN
                    string sqlGrant = $"GRANT CONNECT, RESOURCE, DBA TO \"{userToCreate.ToUpper()}\"";
                    using (OracleCommand cmd = new OracleCommand(sqlGrant, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // C. LƯU EMAIL
                    string sqlInsert = "INSERT INTO SYS.TBL_USER_EMAIL (USERNAME, EMAIL) VALUES (:u, :e)";
                    using (OracleCommand cmd = new OracleCommand(sqlInsert, conn))
                    {
                        cmd.Parameters.Add("u", OracleDbType.Varchar2).Value = userToCreate.ToUpper();
                        cmd.Parameters.Add("e", OracleDbType.Varchar2).Value = userEmail;
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show($"✅ Đăng ký thành công!\nTài khoản: {originalUser.ToUpper()}", "Thành công");

                txtUser.Clear(); txtPass.Clear(); txtEmail.Clear(); txtUser.Focus();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-01920"))
                    MessageBox.Show("❌ Tài khoản này đã tồn tại!", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show($"❌ Lỗi: {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
