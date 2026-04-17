using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ConnectOracle
{
    internal class Database
    {
        public static OracleConnection Conn;

        public static string Host;
        public static string Port;
        public static string Sid;
        public static string User;
        public static string Password;

        public static void Set_Database(string host, string port, string sid, string user, string pass)
        {
            Database.Host = host;
            Database.Port = port;
            Database.Sid = sid;
            Database.User = user;
            Database.Password = pass;
        }

        public static void Disconnect()
        {
            if (Conn != null && Conn.State == System.Data.ConnectionState.Open)
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        public static bool Connect()
        {
            string connsys = "";
            try
            {
                if (!string.IsNullOrEmpty(User) && User.ToUpper().Equals("SYS"))
                    connsys = ";DBA Privilege=SYSDBA";

                // ✅ Connection string chuẩn cho remote Oracle
                string connString =
                    $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={Host})(PORT={Port}))" +
                    $"(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={Sid})));" +
                    $"User Id={User};Password={Password};" +
                    $"Pooling=false;Validate Connection=true;{connsys}";
                Conn = new OracleConnection(connString);
                Conn.Open();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Oracle:\n" + ex.Message, "Kết nối thất bại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public static OracleConnection Get_Connect()
        {
            if (Conn == null)
            {
                Connect();
            }
            return Conn;
        }
    }
}

