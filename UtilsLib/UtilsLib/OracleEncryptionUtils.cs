using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace UtilsLib
{
    public static class OracleEncryptionUtils
    {
        // Connection string mặc định (có thể override)
        private static string _sysConnectionString =
            "User Id=sys;Password=sys;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.241.234.95)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));DBA Privilege=SYSDBA;";

        /// <summary>
        /// Thiết lập connection string tùy chỉnh cho SYS
        /// </summary>
        public static void SetSysConnectionString(string connectionString)
        {
            _sysConnectionString = connectionString;
        }

        /// <summary>
        /// Mã hóa văn bản bằng hàm ENCRYPT_ADD trong Oracle
        /// </summary>
        /// <param name="plainText">Văn bản gốc cần mã hóa</param>
        /// <param name="key">Khóa mã hóa (Caesar cipher shift)</param>
        /// <returns>Văn bản đã mã hóa</returns>
        public static string EncryptWithOracle(string plainText, int key = 19)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            string encrypted = "";

            try
            {
                using (OracleConnection conn = new OracleConnection(_sysConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand("SELECT encrypt_add(:p_text, :p_key) FROM dual", conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("p_text", plainText));
                        cmd.Parameters.Add(new OracleParameter("p_key", key));

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                            encrypted = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mã hóa: {ex.Message}", ex);
            }

            return encrypted;
        }

        /// <summary>
        /// Mã hóa username và password cho Oracle
        /// </summary>
        /// <param name="username">Tên người dùng gốc</param>
        /// <param name="password">Mật khẩu gốc</param>
        /// <param name="key">Khóa mã hóa</param>
        /// <returns>Tuple chứa (encryptedUsername, encryptedPassword)</returns>
        public static (string encryptedUser, string encryptedPass) EncryptCredentials(
            string username, string password, int key = 19)
        {
            string encryptedUser = EncryptWithOracle(username.ToUpper(), key);
            string encryptedPass = EncryptWithOracle(password, key);
            return (encryptedUser, encryptedPass);
        }

        /// <summary>
        /// Kiểm tra user đã tồn tại trong Oracle chưa
        /// </summary>
        /// <param name="encryptedUsername">Tên user đã mã hóa</param>
        /// <returns>True nếu user đã tồn tại</returns>
        public static bool UserExists(string encryptedUsername)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(_sysConnectionString))
                {
                    conn.Open();
                    string query = $"SELECT COUNT(*) FROM dba_users WHERE username = '{encryptedUsername}'";
                    using (OracleCommand cmd = new OracleCommand(query, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo user mới trong Oracle với username/password đã mã hóa
        /// </summary>
        /// <param name="encryptedUsername">Tên user đã mã hóa</param>
        /// <param name="encryptedPassword">Mật khẩu đã mã hóa</param>
        public static void CreateOracleUser(string encryptedUsername, string encryptedPassword)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(_sysConnectionString))
                {
                    conn.Open();

                    // Tạo user
                    string sqlCreate = $"CREATE USER \"{encryptedUsername}\" IDENTIFIED BY \"{encryptedPassword}\" ACCOUNT UNLOCK";
                    using (OracleCommand cmd = new OracleCommand(sqlCreate, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cấp quyền cơ bản cho user mới
        /// </summary>
        /// <param name="encryptedUsername">Tên user đã mã hóa</param>
        public static void GrantBasicPrivileges(string encryptedUsername)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(_sysConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conn;

                        // Cấp quyền CONNECT
                        cmd.CommandText = $"GRANT CONNECT TO \"{encryptedUsername}\"";
                        cmd.ExecuteNonQuery();

                        // Cấp CREATE SESSION
                        cmd.CommandText = $"GRANT CREATE SESSION TO \"{encryptedUsername}\"";
                        cmd.ExecuteNonQuery();

                        // Cấp quota
                        cmd.CommandText = $"ALTER USER \"{encryptedUsername}\" QUOTA UNLIMITED ON USERS";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cấp quyền: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cấp quyền đầy đủ cho user (CONNECT, RESOURCE, CREATE TABLE, VIEW, ...)
        /// </summary>
        /// <param name="encryptedUsername">Tên user đã mã hóa</param>
        public static void GrantFullPrivileges(string encryptedUsername)
        {
            try
            {
                using (OracleConnection conn = new OracleConnection(_sysConnectionString))
                {
                    conn.Open();
                    using (OracleCommand cmd = new OracleCommand())
                    {
                        cmd.Connection = conn;

                        // Cấp quyền đầy đủ
                        cmd.CommandText = $"GRANT CONNECT, RESOURCE, CREATE SESSION, CREATE TABLE, CREATE VIEW TO \"{encryptedUsername}\"";
                        cmd.ExecuteNonQuery();

                        // Cấp quota
                        cmd.CommandText = $"ALTER USER \"{encryptedUsername}\" QUOTA UNLIMITED ON USERS";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cấp quyền: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo user mới với đầy đủ quyền (All-in-one)
        /// </summary>
        /// <param name="originalUsername">Tên user gốc (chưa mã hóa)</param>
        /// <param name="originalPassword">Mật khẩu gốc (chưa mã hóa)</param>
        /// <param name="key">Khóa mã hóa</param>
        /// <param name="grantFullPrivileges">True = cấp quyền đầy đủ, False = cấp quyền cơ bản</param>
        public static void CreateEncryptedUser(string originalUsername, string originalPassword,
            int key = 19, bool grantFullPrivileges = true)
        {
            // Mã hóa username và password
            var (encryptedUser, encryptedPass) = EncryptCredentials(originalUsername, originalPassword, key);

            // Kiểm tra user đã tồn tại chưa
            if (UserExists(encryptedUser))
            {
                throw new Exception($"User '{originalUsername}' đã tồn tại trong hệ thống!");
            }

            // Tạo user
            CreateOracleUser(encryptedUser, encryptedPass);

            // Cấp quyền
            if (grantFullPrivileges)
                GrantFullPrivileges(encryptedUser);
            else
                GrantBasicPrivileges(encryptedUser);
        }

        /// <summary>
        /// Kiểm tra username có phải là admin không
        /// </summary>
        public static bool IsAdminUser(string username)
        {
            string upper = username.ToUpper();
            return upper == "SYS" || upper == "SYSTEM";
        }
    }
}