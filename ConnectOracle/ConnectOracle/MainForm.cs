using System;
using System.Data;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client; // Đảm bảo đã cài NuGet này

namespace ConnectOracle
{
    public partial class MainForm : Form
    {
        // Biến kết nối toàn cục
        private OracleConnection conn = null;

        public MainForm()
        {
            InitializeComponent();
        }

        // --- 1. LOAD FORM & KẾT NỐI ---
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Lấy kết nối từ class Database (giả sử bạn đã login thành công ở Form Login)
            // Nếu code bạn chưa có class Database tĩnh, hãy thay bằng logic kết nối của bạn
            try
            {
                conn = Database.Get_Connect(); // <-- Đảm bảo bạn có hàm này
            }
            catch { }

            // Kiểm tra kết nối
            if (conn == null || conn.State != ConnectionState.Open)
            {
                // Nếu chưa có kết nối (chạy thẳng Form này để test), ta thử kết nối thủ công hoặc báo lỗi
                // Ở đây mình ví dụ báo lỗi và đóng nếu không có kết nối
                MessageBox.Show("Chưa có kết nối Database! Vui lòng chạy từ Form Đăng nhập.", "Cảnh báo");
                // this.Close(); // Bỏ comment dòng này nếu muốn bắt buộc đăng nhập
                return;
            }

            LoadSchemas();

            // Mặc định chọn tab đầu
            cboSchema.SelectedIndex = -1;
        }

        // --- 2. CÁC HÀM LOAD DỮ LIỆU VÀO COMBOBOX ---
        private void LoadSchemas()
        {
            try
            {
                cboSchema.Items.Clear();
                // Lấy danh sách tất cả User (trừ hệ thống)
                string sql = "SELECT username FROM all_users WHERE username NOT IN ('SYS','SYSTEM','XDB') ORDER BY username";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cboSchema.Items.Add(dr.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load Schema: " + ex.Message); }
        }

        private void cboSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSchema.SelectedIndex == -1) return;
            try
            {
                cboTable.Items.Clear();
                string owner = cboSchema.SelectedItem.ToString();
                string sql = $"SELECT table_name FROM all_tables WHERE owner = '{owner}' ORDER BY table_name";

                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cboTable.Items.Add(dr.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load Table: " + ex.Message); }
        }

        // --- 3. HÀM CHẠY THỦ TỤC CHUNG (Helper) ---
        private void RunProcedure(string procName, OracleParameter[] parameters, string successMsg)
        {
            if (string.IsNullOrEmpty(cboSchema.Text) || string.IsNullOrEmpty(cboTable.Text))
            {
                MessageBox.Show("Vui lòng chọn Schema và Table trước!", "Cảnh báo");
                return;
            }

            try
            {
                using (OracleCommand cmd = new OracleCommand(procName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show(successMsg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi: " + ex.Message, "Lỗi DB");
            }
        }

        // --- 4. CÁC SỰ KIỆN NÚT BẤM (EXECUTE) ---

        // Tab FGA: Kích hoạt
        private void btnExecFGA_Click(object sender, EventArgs e)
        {
            if (txtFGA_Policy.Text == "" || txtFGA_Col.Text == "") return;

            OracleParameter[] p = {
                new OracleParameter("p_schema", OracleDbType.Varchar2) { Value = cboSchema.Text },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = cboTable.Text },
                new OracleParameter("p_policy", OracleDbType.Varchar2) { Value = txtFGA_Policy.Text },
                new OracleParameter("p_col", OracleDbType.Varchar2) { Value = txtFGA_Col.Text }
            };
            RunProcedure("sp_Add_FGA_Dynamic", p, "Đã kích hoạt Giám sát FGA thành công!");
        }

        // Tab VPD: Áp dụng
        private void btnExecVPD_Click(object sender, EventArgs e)
        {
            if (txtVPD_Policy.Text == "" || txtVPD_Condition.Text == "") return;

            OracleParameter[] p = {
                new OracleParameter("p_schema", OracleDbType.Varchar2) { Value = cboSchema.Text },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = cboTable.Text },
                new OracleParameter("p_policy", OracleDbType.Varchar2) { Value = txtVPD_Policy.Text },
                new OracleParameter("p_condition", OracleDbType.Varchar2) { Value = txtVPD_Condition.Text }
            };
            RunProcedure("sp_Add_VPD_Dynamic", p, "Đã áp dụng VPD (Ẩn dòng) thành công!");
        }

        // Tab Recovery: Phục hồi
        private void btnExecRecover_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn khôi phục bảng {cboTable.Text} về {numRecoverTime.Value} phút trước?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No) return;

            OracleParameter[] p = {
                new OracleParameter("p_schema", OracleDbType.Varchar2) { Value = cboSchema.Text },
                new OracleParameter("p_table", OracleDbType.Varchar2) { Value = cboTable.Text },
                new OracleParameter("p_minutes", OracleDbType.Int32) { Value = (int)numRecoverTime.Value }
            };
            RunProcedure("sp_Flashback_Dynamic", p, "Dữ liệu đã được phục hồi!");
        }

        // --- 5. CÁC SỰ KIỆN XEM DỮ LIỆU (VIEWER) - ĐÂY LÀ PHẦN BẠN ĐANG THIẾU ---

        // Nút: Xem Nhật ký (Tab FGA)
        private void btnViewLog_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboTable.Text))
            {
                MessageBox.Show("Vui lòng chọn Bảng cần xem log!");
                return;
            }
            try
            {
                // Xem log audit của bảng đang chọn
                string sql = $"SELECT timestamp, db_user, sql_text, policy_name, statement_type " +
                             $"FROM dba_fga_audit_trail " +
                             $"WHERE object_name = '{cboTable.Text}' " +
                             $"ORDER BY timestamp DESC";

                DataTable dt = new DataTable();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                dgvData.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xem log: " + ex.Message); }
        }

        // Nút: Xem Dữ liệu Bảng (Tab Recovery)
        private void btnViewData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cboSchema.Text) || string.IsNullOrEmpty(cboTable.Text))
            {
                MessageBox.Show("Vui lòng chọn Schema và Table!");
                return;
            }
            try
            {
                // Select dữ liệu bảng để kiểm tra
                string fullTableName = $"{cboSchema.Text}.{cboTable.Text}";
                string sql = $"SELECT * FROM {fullTableName}";

                DataTable dt = new DataTable();
                using (OracleCommand cmd = new OracleCommand(sql, conn))
                {
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
                dgvData.DataSource = dt;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xem dữ liệu: " + ex.Message); }
        }

        private void btnDropVPD_Click(object sender, EventArgs e)
        {
            if (txtVPD_Policy.Text == "")
            {
                MessageBox.Show("Vui lòng nhập Tên Chính sách cần xóa!", "Thiếu thông tin");
                return;
            }

            DialogResult dr = MessageBox.Show($"Bạn có chắc muốn xóa chính sách VPD: {txtVPD_Policy.Text}?",
                                              "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.No) return;

            OracleParameter[] p = {
        new OracleParameter("p_schema", OracleDbType.Varchar2) { Value = cboSchema.Text },
        new OracleParameter("p_table", OracleDbType.Varchar2) { Value = cboTable.Text },
        new OracleParameter("p_policy", OracleDbType.Varchar2) { Value = txtVPD_Policy.Text }
    };

            // Gọi thủ tục sp_Drop_VPD_Dynamic vừa tạo ở Bước 1
            RunProcedure("sp_Drop_VPD_Dynamic", p, "Đã xóa chính sách VPD thành công! Dữ liệu đã hiện lại bình thường.");
        }
    }
}