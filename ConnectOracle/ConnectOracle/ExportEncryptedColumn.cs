using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ConnectOracle
{
    public partial class ExportEncryptedColumn : Form
    {
        // --- CÁC CONTROL GIAO DIỆN ---
        private ComboBox cboTables;
        private ComboBox cboColumns;
        private Label lbl1, lbl2, lblTitle;
        private Button btnExport;
        private RsaEncrypter rsaEngine; // Sử dụng class bạn đã cung cấp

        public ExportEncryptedColumn()
        {
            InitializeComponent_CodeTay(); // Vẽ giao diện
            rsaEngine = new RsaEncrypter();
            LoadTables(); // Load danh sách bảng khi mở Form
        }

        // 1. Load danh sách bảng của user DOAN_BMCSDL
        private void LoadTables()
        {
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                // Lấy tất cả bảng thuộc schema DOAN_BMCSDL (hoặc USER_TABLES nếu đang login bằng user đó)
                // Ở đây mình filter cứng theo owner 'DOAN_BMCSDL' cho chắc ăn
                string sql = "SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = 'DOAN_BMCSDL' ORDER BY TABLE_NAME";

                // Nếu login đúng user rồi thì dùng câu này gọn hơn:
                // string sql = "SELECT TABLE_NAME FROM USER_TABLES ORDER BY TABLE_NAME";

                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    cboTables.Items.Clear();
                    while (dr.Read())
                    {
                        cboTables.Items.Add(dr["TABLE_NAME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load bảng: " + ex.Message + "\n(Hãy chắc chắn bạn đã kết nối DB)");
            }
        }

        // 2. Load danh sách cột khi chọn Bảng
        private void cboTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTables.SelectedItem == null) return;
            string tableName = cboTables.SelectedItem.ToString();

            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                string sql = "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE OWNER = 'DOAN_BMCSDL' AND TABLE_NAME = :tName";
                // Hoặc: string sql = "SELECT COLUMN_NAME FROM USER_TAB_COLUMNS WHERE TABLE_NAME = :tName";

                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                {
                    cmd.Parameters.Add(new OracleParameter("tName", tableName));
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        cboColumns.Items.Clear();
                        while (dr.Read())
                        {
                            cboColumns.Items.Add(dr["COLUMN_NAME"].ToString());
                        }
                    }
                }
                if (cboColumns.Items.Count > 0) cboColumns.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load cột: " + ex.Message);
            }
        }

        // 3. XỬ LÝ CHÍNH: Lấy dữ liệu -> Ghi File -> Mã hóa
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (cboTables.SelectedItem == null || cboColumns.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Bảng và Cột cần xuất!");
                return;
            }

            string tableName = cboTables.SelectedItem.ToString();
            string columnName = cboColumns.SelectedItem.ToString();

            // Tạo thư mục lưu kết quả
            string folder = Path.Combine(Application.StartupPath, "KetQua_MaHoa_RSA");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            // Đặt tên file
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string rawFilePath = Path.Combine(folder, $"{tableName}_{columnName}_{timestamp}.txt"); // File tạm
            string encFilePath = Path.Combine(folder, $"{tableName}_{columnName}.enc"); // File kết quả

            // File Key (Sinh mới mỗi lần xuất để bảo mật cao nhất - giống chữ ký số)
            string pubKeyPath = Path.Combine(folder, "Key_Public.xml");
            string priKeyPath = Path.Combine(folder, "Key_Private.xml");

            try
            {
                // BƯỚC A: LẤY DỮ LIỆU TỪ ORACLE
                StringBuilder sb = new StringBuilder();
                // Lưu ý: Tên bảng/cột phải nối chuỗi, không bind parameter được
                string queryData = $"SELECT \"{columnName}\" FROM \"DOAN_BMCSDL\".\"{tableName}\"";

                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                using (OracleCommand cmd = new OracleCommand(queryData, Database.Conn))
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    sb.AppendLine($"--- DỮ LIỆU BẢNG {tableName} - CỘT {columnName} ---");
                    sb.AppendLine($"Thời gian xuất: {DateTime.Now}");
                    sb.AppendLine("--------------------------------------------------");

                    while (dr.Read())
                    {
                        string val = dr[0] == DBNull.Value ? "(NULL)" : dr[0].ToString();
                        sb.AppendLine(val);
                    }
                }

                if (sb.Length == 0)
                {
                    MessageBox.Show("Không lấy được dữ liệu nào!");
                    return;
                }

                // BƯỚC B: GHI RA FILE TẠM (PLAINTEXT)
                File.WriteAllText(rawFilePath, sb.ToString(), Encoding.UTF8);

                // BƯỚC C: SINH KHÓA RSA (Public + Private)
                // Gọi hàm GenerateRsaKeyPair từ class RsaEncrypter của bạn
                rsaEngine.GenerateRsaKeyPair(pubKeyPath, priKeyPath);

                // BƯỚC D: MÃ HÓA FILE TẠM -> FILE .ENC
                // Sử dụng hàm EncryptFileTarget mà bạn đã cung cấp (Mã hóa file đích dùng Public Key)
                bool isSuccess = rsaEngine.EncryptFileTarget(rawFilePath, pubKeyPath, encFilePath);

                // BƯỚC E: DỌN DẸP
                // Xóa file dữ liệu gốc (raw) để bảo mật, chỉ giữ lại file mã hóa và key
                if (File.Exists(rawFilePath)) File.Delete(rawFilePath);

                if (isSuccess)
                {
                    MessageBox.Show("XUẤT VÀ MÃ HÓA THÀNH CÔNG!\n\n" +
                                    "1. File mã hóa: " + Path.GetFileName(encFilePath) + "\n" +
                                    "2. Private Key: Key_Private.xml (Dùng để giải mã)\n" +
                                    "3. Public Key: Key_Public.xml\n\n" +
                                    "Đường dẫn: " + folder);

                    // Mở thư mục chứa file
                    System.Diagnostics.Process.Start(folder);
                }
                else
                {
                    MessageBox.Show("Có lỗi trong quá trình mã hóa RSA!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        // --- CODE GIAO DIỆN (Thay cho Designer) ---
        private void InitializeComponent_CodeTay()
        {
            this.cboTables = new ComboBox();
            this.cboColumns = new ComboBox();
            this.btnExport = new Button();
            this.lbl1 = new Label();
            this.lbl2 = new Label();
            this.lblTitle = new Label();

            this.SuspendLayout();

            // Title
            lblTitle.Text = "XUẤT DỮ LIỆU DATABASE & MÃ HÓA RSA";
            lblTitle.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            lblTitle.ForeColor = System.Drawing.Color.Blue;
            lblTitle.AutoSize = true;
            lblTitle.Location = new System.Drawing.Point(30, 20);

            // Table Selection
            lbl1.Text = "Chọn Bảng (Schema: DOAN_BMCSDL):";
            lbl1.Location = new System.Drawing.Point(30, 60);
            lbl1.AutoSize = true;
            lbl1.Size = new System.Drawing.Size(250, 20);

            cboTables.Location = new System.Drawing.Point(30, 85);
            cboTables.Size = new System.Drawing.Size(300, 25);
            cboTables.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTables.SelectedIndexChanged += cboTables_SelectedIndexChanged;

            // Column Selection
            lbl2.Text = "Chọn Cột dữ liệu cần mã hóa:";
            lbl2.Location = new System.Drawing.Point(30, 125);
            lbl2.AutoSize = true;

            cboColumns.Location = new System.Drawing.Point(30, 150);
            cboColumns.Size = new System.Drawing.Size(300, 25);
            cboColumns.DropDownStyle = ComboBoxStyle.DropDownList;

            // Button
            btnExport.Text = "THỰC HIỆN MÃ HÓA";
            btnExport.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            btnExport.Location = new System.Drawing.Point(30, 200);
            btnExport.Size = new System.Drawing.Size(300, 50);
            btnExport.Click += btnExport_Click;
            btnExport.BackColor = System.Drawing.Color.LightGreen;
            btnExport.Cursor = Cursors.Hand;

            // Form Settings
            this.ClientSize = new System.Drawing.Size(380, 280);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lbl1);
            this.Controls.Add(cboTables);
            this.Controls.Add(lbl2);
            this.Controls.Add(cboColumns);
            this.Controls.Add(btnExport);
            this.Text = "Export & Encrypt (RSA)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
        }
    }
}