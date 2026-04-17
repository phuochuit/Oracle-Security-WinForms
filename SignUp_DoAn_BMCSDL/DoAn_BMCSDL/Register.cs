using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.IO;

namespace DoAn_BMCSDL
{
    public partial class Register : Form
    {
        string connectionString = "User Id=DoAn_BMCSDL;Password=12345;Data Source=//10.241.234.95:1521/orcl;";

        private Random random = new Random();

        //private static readonly string secretKey = "20";

        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567";
        public Register()
        {
            InitializeComponent();
            CenterToScreen();

            txt_password.UseSystemPasswordChar = true;

            // Cấu hình hiển thị lỗi
            txt_errors.Multiline = true;
            txt_errors.ReadOnly = true;
            txt_errors.ScrollBars = ScrollBars.Vertical;
            txt_errors.ForeColor = System.Drawing.Color.Red;
            txt_errors.Text = "";
        }

        private void btn_signup_Click(object sender, EventArgs e)
        {
            txt_errors.Text = ""; // Clear lỗi cũ
            txt_errors.ForeColor = System.Drawing.Color.Red;

            string username = txt_username.Text.Trim();
            string password = txt_password.Text;
            string fullname = txt_fullname.Text.Trim();
            string email = txt_email.Text.Trim();
            string phone = txt_phonenumber.Text.Trim();
            DateTime dob = dtp_dateofbirth.Value;

            // 🔒 Mã hóa mật khẩu (nếu bạn có hàm này)
            //string encryptedPassword = EncryptPasswordRandom(password, 19);

            string encryptedPassword = EncryptPasswordInOracle(password, 19);

            try
            {
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    using (OracleTransaction tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // ✅ 1. Kiểm tra username trùng
                            OracleCommand checkCmd = new OracleCommand(
                                "SELECT COUNT(*) FROM ACCOUNT WHERE USERNAME = :u", conn);
                            checkCmd.Parameters.Add(":u", username);
                            checkCmd.Transaction = tran;
                            int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                            if (exists > 0)
                            {
                                txt_errors.Text = "⚠️ Username đã tồn tại";
                                tran.Rollback();
                                return;
                            }

                            // ✅ 2. Thêm vào ACCOUNT
                            OracleCommand cmd1 = new OracleCommand(
                                "INSERT INTO ACCOUNT (USERNAME, PASSWORD, ROLE_NAME) VALUES (:u, :p, 'USER')", conn);
                            cmd1.Parameters.Add(":u", username);
                            cmd1.Parameters.Add(":p", encryptedPassword);
                            cmd1.Transaction = tran;
                            cmd1.ExecuteNonQuery();

                            // ✅ 3. Thêm vào USERINFO
                            OracleCommand cmd2 = new OracleCommand(
                                "INSERT INTO USERINFO (USERNAME, FULLNAME, DATEOFBIRTH, EMAIL, PHONENUMBER) " +
                                "VALUES (:u, :f, :d, :e, :ph)", conn);
                            cmd2.Parameters.Add(":u", username);
                            cmd2.Parameters.Add(":f", fullname);
                            cmd2.Parameters.Add(":d", dob);
                            cmd2.Parameters.Add(":e", email);
                            cmd2.Parameters.Add(":ph", phone);
                            cmd2.Transaction = tran;
                            cmd2.ExecuteNonQuery();

                            // ✅ 4. Ghi log
                            OracleCommand cmd3 = new OracleCommand(
                                "INSERT INTO AUDIT_LOG (USERNAME, ACTION) VALUES (:u, 'REGISTER')", conn);
                            cmd3.Parameters.Add(":u", username);
                            cmd3.Transaction = tran;
                            cmd3.ExecuteNonQuery();

                            tran.Commit();

                            txt_errors.ForeColor = System.Drawing.Color.Green;
                            txt_errors.Text = "✅ Đăng ký thành công!";
                        }
                        catch (OracleException ex)
                        {
                            tran.Rollback();
                            txt_errors.ForeColor = System.Drawing.Color.Red;
                            txt_errors.Text = "❌ Lỗi Oracle #" + ex.Number + ": " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txt_errors.ForeColor = System.Drawing.Color.Red;
                txt_errors.Text = "❌ Lỗi kết nối DB: " + ex.Message;
            }
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //private string EncryptPassword(string plainText)
        //{
        //    using (Aes aes = Aes.Create())
        //    {
        //        aes.Key = Encoding.UTF8.GetBytes(secretKey);
        //        aes.IV = new byte[16]; // IV 16 bytes toàn 0 (đơn giản hóa)

        //        using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
        //        using (var ms = new MemoryStream())
        //        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //        using (var sw = new StreamWriter(cs))
        //        {
        //            sw.Write(plainText);
        //            sw.Close();
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}


        //private string EncryptPasswordRandom(string plain, int key)
        //{
        //    bool useAdd = random.Next(0, 2) == 0; // 50% chọn cộng, 50% chọn nhân
        //    return useAdd ? EncryptPasswordAdd(plain, key) : EncryptPasswordMul(plain, key);
        //}

        //mã hóa mat khẩu + key
        //private string EncryptPasswordAdd(string plain, int key)
        //{
        //    string result = "";
        //    foreach (char c in plain)
        //    {
        //        int index = Alphabet.IndexOf(c);
        //        if (index >= 0) // ký tự hợp lệ
        //        {
        //            int newIndex = (index + key) % Alphabet.Length;
        //            result += Alphabet[newIndex];
        //        }
        //        else
        //        {
        //            result += c; // nếu không có trong bảng thì giữ nguyên
        //        }
        //    }
        //    return result;
        //}

        ////mã hóa mat khẩu x key
        //private string EncryptPasswordMul(string plain, int key)
        //{
        //    string result = "";
        //    foreach (char c in plain)
        //    {
        //        int index = Alphabet.IndexOf(c);
        //        if (index >= 0) // ký tự hợp lệ
        //        {
        //            int newIndex = (index * key) % Alphabet.Length;
        //            result += Alphabet[newIndex];
        //        }
        //        else
        //        {
        //            result += c; // nếu không có trong bảng thì giữ nguyên
        //        }
        //    }
        //    return result;
        //}

        private string EncryptPasswordInOracle(string plainText, int key)
        {
            string encrypted = "";

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("encrypt_random", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Tham số trả về
                    OracleParameter ret = new OracleParameter();
                    ret.ParameterName = "RETURN_VALUE";
                    ret.OracleDbType = OracleDbType.Varchar2;
                    ret.Direction = ParameterDirection.ReturnValue;
                    ret.Size = 4000;
                    cmd.Parameters.Add(ret);

                    // Tham số đầu vào
                    cmd.Parameters.Add("p_plain", OracleDbType.Varchar2, plainText, ParameterDirection.Input);
                    cmd.Parameters.Add("p_key", OracleDbType.Int32, key, ParameterDirection.Input);

                    cmd.ExecuteNonQuery();
                    encrypted = ret.Value.ToString();
                }
            }

            return encrypted;
        }

    }
}
