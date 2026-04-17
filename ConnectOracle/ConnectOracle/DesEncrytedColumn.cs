using System;
using System.ComponentModel; // Quan trọng cho IContainer
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ConnectOracle
{
    public partial class DesEncryptedColumn : Form
    {
        // --- 1. KHAI BÁO BIẾN ---
        private IContainer components = null; // Biến quản lý tài nguyên chuẩn
        private ComboBox cboTablesDes;
        private ComboBox cboColumnsDes;
        private Label lbl1Des, lbl2Des, lblTitleDes;
        private Button btnExportDes;

        private DesEncrypter desEngine; // Class xử lý DES

        public DesEncryptedColumn()
        {
            InitializeComponent_CodeTayDes(); // Vẽ giao diện
            desEngine = new DesEncrypter();   // Khởi tạo
            LoadTablesDes();                  // Load dữ liệu
        }

        // --- 2. HÀM DISPOSE (Sửa lỗi tại đây) ---
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // --- 3. LOGIC LOAD DATABASE ---
        private void LoadTablesDes()
        {
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
                string sql = "SELECT TABLE_NAME FROM ALL_TABLES WHERE OWNER = 'DOAN_BMCSDL' ORDER BY TABLE_NAME";

                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    cboTablesDes.Items.Clear();
                    while (dr.Read()) { cboTablesDes.Items.Add(dr["TABLE_NAME"].ToString()); }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load bảng: " + ex.Message); }
        }

        private void cboTablesDes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTablesDes.SelectedItem == null) return;
            string tableName = cboTablesDes.SelectedItem.ToString();
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();
                string sql = "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE OWNER = 'DOAN_BMCSDL' AND TABLE_NAME = :tName";

                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                {
                    cmd.Parameters.Add(new OracleParameter("tName", tableName));
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        cboColumnsDes.Items.Clear();
                        while (dr.Read()) { cboColumnsDes.Items.Add(dr["COLUMN_NAME"].ToString()); }
                    }
                }
                if (cboColumnsDes.Items.Count > 0) cboColumnsDes.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load cột: " + ex.Message); }
        }

        // --- 4. XỬ LÝ NÚT BẤM (MÃ HÓA DES) ---
        private void btnExportDes_Click(object sender, EventArgs e)
        {
            if (cboTablesDes.SelectedItem == null || cboColumnsDes.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Bảng và Cột!");
                return;
            }

            string tableName = cboTablesDes.SelectedItem.ToString();
            string columnName = cboColumnsDes.SelectedItem.ToString();

            // Tạo thư mục
            string folder = Path.Combine(Application.StartupPath, "KetQua_MaHoa_DES");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            // Tên file
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string rawFilePath = Path.Combine(folder, $"{tableName}_{columnName}_{timestamp}.txt");
            string encFilePath = Path.Combine(folder, $"{tableName}_{columnName}.des");
            string secretKeyPath = Path.Combine(folder, "Key_Secret.txt");

            try
            {
                // A. Lấy dữ liệu
                StringBuilder sb = new StringBuilder();
                string queryData = $"SELECT \"{columnName}\" FROM \"DOAN_BMCSDL\".\"{tableName}\"";

                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                using (OracleCommand cmd = new OracleCommand(queryData, Database.Conn))
                using (OracleDataReader dr = cmd.ExecuteReader())
                {
                    sb.AppendLine($"--- DỮ LIỆU DES: {tableName}.{columnName} ---");
                    sb.AppendLine($"Time: {DateTime.Now}");
                    sb.AppendLine("-----------------------------------");
                    while (dr.Read())
                    {
                        sb.AppendLine(dr[0] == DBNull.Value ? "(NULL)" : dr[0].ToString());
                    }
                }

                if (sb.Length == 0) { MessageBox.Show("Không có dữ liệu!"); return; }

                // B. Ghi file tạm
                File.WriteAllText(rawFilePath, sb.ToString(), Encoding.UTF8);

                // C. Sinh khóa & Mã hóa
                desEngine.GenerateKeyToFile(secretKeyPath);
                bool isSuccess = desEngine.EncryptFile(rawFilePath, secretKeyPath, encFilePath);

                // D. Xóa file tạm
                if (File.Exists(rawFilePath)) File.Delete(rawFilePath);

                if (isSuccess)
                {
                    MessageBox.Show("THÀNH CÔNG!\nFile: " + Path.GetFileName(encFilePath) + "\nKey: Key_Secret.txt");
                    System.Diagnostics.Process.Start(folder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- 5. VẼ GIAO DIỆN (Thay thế Designer) ---
        private void InitializeComponent_CodeTayDes()
        {
            this.cboTablesDes = new ComboBox();
            this.cboColumnsDes = new ComboBox();
            this.btnExportDes = new Button();
            this.lbl1Des = new Label();
            this.lbl2Des = new Label();
            this.lblTitleDes = new Label();

            this.SuspendLayout();

            // Cấu hình Form
            this.ClientSize = new System.Drawing.Size(380, 280);
            this.Text = "DES Encryption Export";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Title
            lblTitleDes.Text = "XUẤT DỮ LIỆU & MÃ HÓA DES";
            lblTitleDes.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
            lblTitleDes.ForeColor = System.Drawing.Color.Red;
            lblTitleDes.AutoSize = true;
            lblTitleDes.Location = new System.Drawing.Point(30, 20);

            // Table
            lbl1Des.Text = "Chọn Bảng:";
            lbl1Des.Location = new System.Drawing.Point(30, 60);
            lbl1Des.AutoSize = true;

            cboTablesDes.Location = new System.Drawing.Point(30, 85);
            cboTablesDes.Size = new System.Drawing.Size(300, 25);
            cboTablesDes.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTablesDes.SelectedIndexChanged += cboTablesDes_SelectedIndexChanged;

            // Column
            lbl2Des.Text = "Chọn Cột:";
            lbl2Des.Location = new System.Drawing.Point(30, 125);
            lbl2Des.AutoSize = true;

            cboColumnsDes.Location = new System.Drawing.Point(30, 150);
            cboColumnsDes.Size = new System.Drawing.Size(300, 25);
            cboColumnsDes.DropDownStyle = ComboBoxStyle.DropDownList;

            // Button
            btnExportDes.Text = "THỰC HIỆN MÃ HÓA DES";
            btnExportDes.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            btnExportDes.Location = new System.Drawing.Point(30, 200);
            btnExportDes.Size = new System.Drawing.Size(300, 50);
            btnExportDes.Click += btnExportDes_Click;
            btnExportDes.BackColor = System.Drawing.Color.LightSalmon;

            // Add Controls
            this.Controls.Add(lblTitleDes);
            this.Controls.Add(lbl1Des);
            this.Controls.Add(cboTablesDes);
            this.Controls.Add(lbl2Des);
            this.Controls.Add(cboColumnsDes);
            this.Controls.Add(btnExportDes);

            this.ResumeLayout(false);
        }
    }
}