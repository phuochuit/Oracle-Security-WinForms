using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace ConnectOracle
{
    public partial class FormChinh : Form
    {
        // Biến lưu thông tin hiển thị (VD: aaa)
        private string _appUserDisplay;

        // Biến lưu thông tin thực tế dưới DB (VD: ttt)
        // Gán giá trị rỗng để không bao giờ bị Null
        private string _realDbUser = "";
        private int _mySid;
        private int _mySerial;

        // Timer duy nhất để kiểm tra kết nối
        private System.Windows.Forms.Timer _timerSecurity;

        // Các biến kết nối và mã hóa
        private OracleConnection _conn;
        private DesOracle _des;

        public FormChinh(string user)
        {
            InitializeComponent();

            // Lưu tên user nhập từ form đăng nhập (aaa)
            _appUserDisplay = user;

            CenterToScreen();

            // Lấy kết nối đã mở sẵn từ class Database
            _conn = Database.Get_Connect();
            _des = new DesOracle(_conn);
        }

        private void FormChinh_Load(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra kết nối sơ bộ
                if (Database.Conn == null || Database.Conn.State != ConnectionState.Open)
                {
                    MessageBox.Show("Không có kết nối đến Oracle!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // 2. Lấy thông tin Session thực tế (SID, SERIAL, User đã mã hóa)
                GetMySessionInfo();

                // 3. Hiển thị thông tin lên tiêu đề Form để bạn dễ theo dõi
                // Dòng này chứng minh Form đã nhận diện đúng User mã hóa dưới DB
                this.Text = $"App: {_appUserDisplay} | DB User: {_realDbUser} | SID: {_mySid} | Serial#: {_mySerial}";

                // 4. Cấu hình Timer kiểm tra trạng thái (3 giây check 1 lần)
                _timerSecurity = new System.Windows.Forms.Timer();
                _timerSecurity.Interval = 3000;
                _timerSecurity.Tick += TimerSecurity_Tick;
                _timerSecurity.Start();

                // 5. Load danh sách bảng (Logic cũ của bạn)
                LoadTableList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động Form: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm này hỏi Oracle: "Tôi là ai? SID của tôi là bao nhiêu?"
        /// Giúp giải quyết vấn đề User App (aaa) khác User DB (ttt).
        /// </summary>
        private void GetMySessionInfo()
        {
            try
            {
                // SYS_CONTEXT('USERENV', 'SID') trả về SID của chính session đang kết nối
                string sql = @"SELECT SID, SERIAL#, USER 
                               FROM V$SESSION 
                               WHERE SID = SYS_CONTEXT('USERENV', 'SID')";

                using (OracleCommand cmd = new OracleCommand(sql, Database.Conn))
                {
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            _mySid = Convert.ToInt32(dr["SID"]);
                            _mySerial = Convert.ToInt32(dr["SERIAL#"]);
                            _realDbUser = dr["USER"].ToString(); // Đây sẽ là 'TTT' (đã mã hóa)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu lỗi ngay lúc lấy thông tin thì có thể kết nối đã hỏng
                MessageBox.Show("Không lấy được thông tin Session: " + ex.Message);
            }
        }

        /// <summary>
        /// Sự kiện Timer chạy mỗi 3 giây
        /// </summary>
        private void TimerSecurity_Tick(object sender, EventArgs e)
        {
            CheckIfAlive();
        }

        /// <summary>
        /// Thực hiện kiểm tra xem mình còn "sống" không
        /// </summary>
        private void CheckIfAlive()
        {
            try
            {
                if (Database.Conn == null || Database.Conn.State != ConnectionState.Open)
                {
                    _timerSecurity.Stop();
                    ForceLogout("Mất kết nối mạng hoặc Database đã đóng.");
                    return;
                }

                // Cố gắng chạy một lệnh siêu nhẹ (SELECT 1).
                // QUAN TRỌNG: Nếu Session đã bị Kill, lệnh này sẽ QUĂNG EXCEPTION ORA-00028 ngay lập tức.
                // Chúng ta không cần query xem mình còn tồn tại không, vì nếu không tồn tại, ta không thể query!
                using (OracleCommand cmd = new OracleCommand("SELECT 1 FROM DUAL", Database.Conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (OracleException ex)
            {
                // Bắt các mã lỗi "chết người" để Log out
                // 28:   Session has been killed
                // 3113: End-of-file on communication channel (Rớt mạng/DB sập)
                // 3114: Not connected to Oracle
                // 3135: Connection lost contact
                if (ex.Number == 28 || ex.Number == 3113 || ex.Number == 3114 || ex.Number == 3135)
                {
                    _timerSecurity.Stop(); // Dừng timer để không spam lỗi
                    ForceLogout($"Tài khoản DB ({_realDbUser}) tại SID {_mySid} đã bị ngắt kết nối.\nLý do: ORA-{ex.Number}");
                }
            }
            catch (Exception ex)
            {
                // Các lỗi khác (ví dụ code C# lỗi) thì chỉ in ra debug, không logout vội
                Console.WriteLine("Lỗi check status: " + ex.Message);
            }
        }

        /// <summary>
        /// Hàm xử lý đăng xuất bắt buộc
        /// </summary>
        private void ForceLogout(string reason)
        {
            // 1. Dừng Timer ngay lập tức
            _timerSecurity.Stop();

            // 2. Hiện thông báo cho người dùng
            MessageBox.Show(reason, "Cảnh báo phiên làm việc", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // =======================================================================
            // 🔴 BƯỚC MỚI: ĐÓNG TẤT CẢ CÁC FORM CON (Mục 5, RSA, Des...)
            // =======================================================================
            try
            {
                // Tạo một danh sách copy các form đang mở (để tránh lỗi khi vừa duyệt vừa đóng)
                System.Collections.Generic.List<Form> openForms = new System.Collections.Generic.List<Form>();
                foreach (Form f in Application.OpenForms)
                {
                    openForms.Add(f);
                }

                foreach (Form f in openForms)
                {
                    // Nếu form đó KHÔNG PHẢI là FormChinh (chính mình) 
                    // và KHÔNG PHẢI là form Đăng Nhập (sắp mở) thì đóng nó lại.
                    if (f != this && f.Name != "DangNhap")
                    {
                        f.Close(); // Đóng form con (Muc5, RSA, Des...)
                        f.Dispose();
                    }
                }
            }
            catch { /* Bỏ qua lỗi nếu có form nào cứng đầu */ }
            // =======================================================================

            // 3. Ngắt kết nối DB an toàn
            try { Database.Disconnect(); } catch { }

            // 4. Xử lý logic kill process đồ án
            KillDoAnProcess();

            // 5. Ẩn form này và hiện form đăng nhập
            this.Hide();
            DangNhap login = new DangNhap();
            login.Show();

            // 6. Đóng form hiện tại hoàn toàn
            this.Close();
        }

        private void KillDoAnProcess()
        {
            if (!string.IsNullOrEmpty(_appUserDisplay) && _appUserDisplay.Equals("DoAn_BMCSDL", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    foreach (var process in System.Diagnostics.Process.GetProcessesByName("DoAn_BMCSDL"))
                        process.Kill();
                }
                catch { }
            }
        }

        // ============================================================
        // CÁC HÀM LOGIC GIAO DIỆN CŨ (Load bảng, Nút bấm...)
        // ============================================================

        private void LoadTableList()
        {
            // ================================================================
            // 1. CẤU HÌNH CHUNG: LUÔN HIỆN NÚT PHÂN QUYỀN VỚI MỌI USER
            // ================================================================
            btnPhanquyen.Visible = true;

            // ================================================================
            // 2. KIỂM TRA QUYỀN SYS
            // ================================================================
            // Kiểm tra xem User thực tế dưới DB có phải là SYS không
            if (_realDbUser.Equals("SYS", StringComparison.OrdinalIgnoreCase))
            {
                // --- NẾU LÀ SYS (ADMIN) ---

                // Ẩn các chức năng xem dữ liệu thường (SYS không cần xem data user)
                comboBox1.Visible = false;
                btn_load.Visible = false;
                dataGridView1.Visible = false;

                // Tạo thông báo cảnh báo giao diện Admin
                // Kiểm tra xem label đã add chưa để tránh add nhiều lần nếu gọi hàm lại
                if (this.Controls.Find("lblSysWarning", true).Length == 0)
                {
                    Label lblSys = new Label()
                    {
                        Name = "lblSysWarning", // Đặt tên để quản lý
                        Text = "Tài khoản SYS (DB Admin) - Chế độ Quản trị.",
                        AutoSize = true,
                        ForeColor = System.Drawing.Color.Red,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        Location = new Point(20, 50) // Bạn chỉnh tọa độ này cho đẹp với form
                    };
                    this.Controls.Add(lblSys);
                }

                // Kết thúc hàm ở đây, không tải bảng dữ liệu
                return;
            }

            // ================================================================
            // 3. NẾU KHÔNG PHẢI SYS (LÀ USER THƯỜNG)
            // ================================================================

            // Hiện lại các control xem dữ liệu (để user thường dùng)
            comboBox1.Visible = true;
            btn_load.Visible = true;
            dataGridView1.Visible = true;

            // (Lưu ý: Tôi đã XÓA dòng 'btnPhanquyen.Visible = false' ở đây 
            // để user thường VẪN THẤY nút phân quyền)

            // Logic tải danh sách bảng từ Database
            try
            {
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                OracleCommand cmd = new OracleCommand("BEGIN :cursor := DoAn_BMCSDL.GET_USER_TABLES; END;", Database.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // --- SỬA ĐỔI TỪ ĐÂY ---
                // Gán cả DataTable vào ComboBox để giữ được cột OWNER và TABLE_NAME
                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "TABLE_NAME"; // Chỉ hiện tên bảng cho đẹp
                comboBox1.ValueMember = "OWNER";        // Giá trị ẩn bên dưới là tên Owner
                                                        // ----------------------
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng: " + ex.Message);
            }
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy hàng dữ liệu đang chọn trong ComboBox
                DataRowView selectedRow = comboBox1.SelectedItem as DataRowView;

                if (selectedRow == null)
                {
                    MessageBox.Show("Vui lòng chọn bảng!", "Thông báo");
                    return;
                }

                // Lấy thông tin từ DataRowView
                string tableName = selectedRow["TABLE_NAME"].ToString();
                string ownerName = selectedRow["OWNER"].ToString(); // Lấy Owner động (ví dụ: DOAN_BMCSDL, HR, SCOTT...)

                // Gọi hàm mới với 2 tham số
                OracleCommand cmd = new OracleCommand("BEGIN :cursor := DoAn_BMCSDL.GET_TABLE_DATA(:p_owner, :p_table_name); END;", Database.Conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("cursor", OracleDbType.RefCursor, ParameterDirection.ReturnValue);

                // Truyền tham số Owner và TableName
                cmd.Parameters.Add("p_owner", OracleDbType.Varchar2).Value = ownerName;
                cmd.Parameters.Add("p_table_name", OracleDbType.Varchar2).Value = tableName;

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            // Nút đăng xuất chủ động
            _timerSecurity.Stop();

            try { Database.Disconnect(); } catch { }

            MessageBox.Show("Đăng xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            KillDoAnProcess();

            this.Hide();
            DangNhap loginForm = new DangNhap();
            loginForm.Show();
            this.Close();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTH_Click(object sender, EventArgs e)
        {
            try
            {
                string text = txtPlainText.Text;
                string key = txtKey.Text;

                // Lưu ý: Class DesOracle phải hoạt động đúng
                byte[] b = _des.EncryptDES(text, key);
                txtMa.Text = Convert.ToBase64String(b);
                txtVBG.Text = _des.DecryptDES(b, key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mã hóa: " + ex.Message);
            }
        }

        private void btnMuc6_Click(object sender, EventArgs e)
        {
            Muc6 frmM6 = new Muc6();
            this.Hide();
            frmM6.ShowDialog();
            this.Show();
        }

        private void btnMuc5_Click(object sender, EventArgs e)
        {
            Muc5 frmM5 = new Muc5();
            this.Hide();
            frmM5.ShowDialog();
            this.Show();
        }

        // Các event rỗng
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

        private void btnDesFile_Click(object sender, EventArgs e)
        {
            FileEncryptionUI des = new FileEncryptionUI();
            this.Hide();
            des.ShowDialog();
            this.Show();
        }

        private void btnRsafile_Click(object sender, EventArgs e)
        {
            FileEncryptionRSA rsa = new FileEncryptionRSA();
            this.Hide();
            rsa.ShowDialog();
            this.Show();
        }

        private void btnKySo_Click(object sender, EventArgs e)
        {
            SignedNumber frm = new SignedNumber();
            frm.ShowDialog();
        }

        private void testSign_Click(object sender, EventArgs e)
        {
            FormVerify test = new FormVerify();
            test.ShowDialog();
        }

        private void btnPhanquyen_Click(object sender, EventArgs e)
        {
            PhanQuyen pq = new PhanQuyen();
            pq.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportEncryptedColumn ec = new ExportEncryptedColumn();
            ec.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DesEncryptedColumn dc = new DesEncryptedColumn();
            dc.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainForm mf = new MainForm();
            mf.ShowDialog();
        }
    }
}