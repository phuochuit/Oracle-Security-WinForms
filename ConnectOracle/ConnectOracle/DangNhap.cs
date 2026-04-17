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
using UtilsLib;

namespace ConnectOracle
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
            CenterToScreen();
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_button_Click(object sender, EventArgs e)
        {
            string host = txt_host.Text.Trim();
            string port = txt_port.Text.Trim();
            string sid = txt_sid.Text.Trim();
            string user = txt_user.Text.Trim();
            string pass = txt_pass.Text.Trim();

            // ✅ Kiểm tra dữ liệu nhập
            if (!Check_TextBox(host, port, sid, user, pass))
                return;

            try
            {
                string connectUser = user;
                string connectPass = pass;
                string upperUser = user.ToUpper();

                // 🔒 Mã hóa nếu không phải SYS/SYSTEM/DoAn_BMCSDL
                if (!user.Equals("DoAn_BMCSDL", StringComparison.OrdinalIgnoreCase) &&
                    !upperUser.Equals("SYS") &&
                    !upperUser.Equals("SYSTEM"))
                {
                    connectUser = OracleEncryptionUtils.EncryptWithOracle(user, 19);
                    connectPass = OracleEncryptionUtils.EncryptWithOracle(pass, 19);
                }

                // 🔗 Thiết lập kết nối DB
                Database.Set_Database("10.241.234.95", "1521", sid, connectUser, connectPass);

                if (!Database.Connect())
                {
                    MessageBox.Show("❌ User hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string userEmail = "";
                    // Lưu ý: User hiện tại phải có quyền SELECT trên bảng này
                    string sqlGetMail = "SELECT EMAIL FROM SYS.TBL_USER_EMAIL WHERE UPPER(USERNAME) = :u";

                    using (OracleCommand cmd = new OracleCommand(sqlGetMail, Database.Conn))
                    {
                        cmd.Parameters.Add("u", OracleDbType.Varchar2).Value = connectUser.ToUpper();
                        var result = cmd.ExecuteScalar();
                        if (result != null) userEmail = result.ToString();
                    }

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        string myOTP = EmailHelper.GenerateOTP();

                        this.Hide(); // Ẩn form đăng nhập

                        // Gửi mail
                        bool sendOK = EmailHelper.SendOTP(userEmail, myOTP);
                        if (!sendOK)
                        {
                            MessageBox.Show("Không thể gửi Email OTP. Vui lòng thử lại sau.");
                            Database.Disconnect(); // Ngắt kết nối để bảo mật
                            this.Show();
                            return;
                        }

                        MessageBox.Show($"Đã gửi OTP đến: {userEmail}", "Xác thực");

                        // Mở Form OTP
                        FormNhapOTP frmOTP = new FormNhapOTP(myOTP);
                        frmOTP.ShowDialog();

                        // 4. KIỂM TRA KẾT QUẢ OTP (Quan trọng)
                        // Nếu User đóng form (IsVerified = false) thì cũng coi như thất bại
                        if (!frmOTP.IsVerified)
                        {
                            MessageBox.Show("Đăng nhập bị hủy hoặc OTP sai!", "Thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Database.Disconnect(); // ĐÁ VĂNG KẾT NỐI NGAY
                            this.Show(); // Hiện lại form đăng nhập
                            return; // DỪNG LẠI, KHÔNG CHO CHẠY XUỐNG DƯỚI
                        }
                    }
                    else
                    {
                        // Tùy chọn: Nếu không có email thì cho qua luôn
                        MessageBox.Show("User này không có Email bảo mật. Đang vào thẳng...");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi trong quá trình xác thực OTP: " + ex.Message);
                    Database.Disconnect();
                    this.Show();
                    return;
                }

                try
                {
                    using (OracleCommand cmdKill = new OracleCommand("SYS.SP_KICK_OLD_SESSIONS", Database.Conn))
                    {
                        cmdKill.CommandType = CommandType.StoredProcedure;
                        cmdKill.Parameters.Add("p_username", OracleDbType.Varchar2).Value = connectUser;

                        cmdKill.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Lưu ý: Nếu user đăng nhập không có quyền execute thủ tục này sẽ văng lỗi
                    // Cần đảm bảo đã GRANT EXECUTE ở Bước 1
                    MessageBox.Show("Không thể thực hiện Single Sign-On: " + ex.Message);
                }

                MessageBox.Show("✅ Kết nối thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 🚀 Logic mở project riêng cho user đồ án
                if (user.Equals("DoAn_BMCSDL", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string exePath = @"E:\CSDL\BMCSDL\28_TruongToDinhPhuoc\SignUp_DoAn_BMCSDL\DoAn_BMCSDL\bin\Debug\DoAn_BMCSDL.exe";
                        System.Diagnostics.Process.Start(exePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể mở project DoAn_BMCSDL:\n" + ex.Message);
                    }
                }

                // 🧩 Mở form chính
                FormChinh form2 = new FormChinh(user);
                this.Hide();
                form2.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool Check_TextBox(string host, string port, string sid, string user, string pass)
        {
            if (host == "")
            {
                MessageBox.Show("Chưa điền thông tin Host");
                txt_host.Focus();
                return false;
            }
            else if (port == "")
            {
                MessageBox.Show("Chưa điền thông tin Port");
                txt_port.Focus();
                return false;
            }
            else if (sid == "")
            {
                MessageBox.Show("Chưa điền thông tin Sid");
                txt_sid.Focus();
                return false;
            }
            else if (user == "")
            {
                MessageBox.Show("Chưa điền thông tin User");
                txt_user.Focus();
                return false;
            }
            else if (pass == "")
            {
                MessageBox.Show("Chưa điền thông tin Pass");
                txt_pass.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lnk_register_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            {
                try
                {
                    // thay đường dẫn bằng đường dẫn thật trên máy bạn
                    System.Diagnostics.Process.Start(
                        @"D:\Code\BMCSDL\28_TruongToDinhPhuoc\28_TruongToDinhPhuoc\SignUpForm\SignUpForm\bin\Debug\SignUpForm.exe"
                    );
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không mở được chương trình đăng ký: " + ex.Message);
                }
            }
        }
    }
}
