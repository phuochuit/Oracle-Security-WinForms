using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using GSF;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Oracle.ManagedDataAccess.Client;
using Word = Microsoft.Office.Interop.Word;

namespace ConnectOracle
{
    public partial class SignedNumber : Form
    {
        private TextBox txtNguoiKy;
        private Button btnThucHien;
        private Label label1;
        private Label label2;

        private void InitializeComponent()
        {
            this.txtNguoiKy = new System.Windows.Forms.TextBox();
            this.btnThucHien = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNguoiKy
            // 
            this.txtNguoiKy.Location = new System.Drawing.Point(289, 147);
            this.txtNguoiKy.Name = "txtNguoiKy";
            this.txtNguoiKy.Size = new System.Drawing.Size(288, 22);
            this.txtNguoiKy.TabIndex = 0;
            this.txtNguoiKy.TextChanged += new System.EventHandler(this.txtNguoiKy_TextChanged);
            // 
            // btnThucHien
            // 
            this.btnThucHien.Location = new System.Drawing.Point(261, 296);
            this.btnThucHien.Name = "btnThucHien";
            this.btnThucHien.Size = new System.Drawing.Size(134, 46);
            this.btnThucHien.TabIndex = 2;
            this.btnThucHien.Text = "Thực hiện";
            this.btnThucHien.UseVisualStyleBackColor = true;
            this.btnThucHien.Click += new System.EventHandler(this.btnThucHien_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(153, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nhập thông tin ký số:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(301, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "CHỮ KÝ SỐ";
            // 
            // SignedNumber
            // 
            this.ClientSize = new System.Drawing.Size(988, 468);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnThucHien);
            this.Controls.Add(this.txtNguoiKy);
            this.Name = "SignedNumber";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public SignedNumber()
        {
            InitializeComponent();
        }

        private void btnThucHien_Click(object sender, EventArgs e)
        {
            string nguoiKy = txtNguoiKy.Text.Trim();
            if (string.IsNullOrEmpty(nguoiKy)) { MessageBox.Show("Nhập tên người ký!"); return; }

            try
            {
                string folder = Path.Combine(Application.StartupPath, "KetQua_KySo");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                // 1. Sinh khóa cho Người A
                string pubKeyPath = Path.Combine(folder, "Key_A_Public.xml");
                string priKeyPath = Path.Combine(folder, "Key_A_Private.xml"); // File này giữ bí mật, xóa sau khi ký xong nếu muốn

                PdfSignerHelper signer = new PdfSignerHelper();
                signer.GenerateKeys(pubKeyPath, priKeyPath);

                // 2. Tạo File PDF Gốc (tạm thời)
                string rawPdf = Path.Combine(folder, "Draft.pdf");
                // Giả sử hàm ExportToPdf của bạn xuất ra file rawPdf này (chưa có chữ ký)
                // Lưu ý: Sửa hàm ExportToPdf cũ của bạn: KHÔNG ADD visual signature text thủ công nữa, để PdfSigner lo.

                // 3. LẤY DỮ LIỆU TỪ ORACLE
                DataTable dt = new DataTable();
                string sql = "SELECT USERNAME, FULLNAME, DATEOFBIRTH, EMAIL, PHONENUMBER FROM USERINFO";

                // Mở kết nối nếu đang đóng (dùng class Database của bạn)
                if (Database.Conn.State == ConnectionState.Closed) Database.Conn.Open();

                using (OracleDataAdapter adp = new OracleDataAdapter(sql, Database.Conn))
                {
                    adp.Fill(dt);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu trong bảng USERINFO!");
                    return;
                }

                // 4. GỌI HÀM XUẤT PDF (ĐÃ ĐƯỢC SỬA BÊN DƯỚI)
                ExportToPdf_Raw(dt, rawPdf);

                // 3. Ký số vào file PDF
                string signedPdf = Path.Combine(folder, "VanBan_DaKy.pdf");
                string xmlPrivateKey = File.ReadAllText(priKeyPath);

                signer.SignPdf(rawPdf, signedPdf, xmlPrivateKey, nguoiKy, "DaKy");

                // 4. Dọn dẹp (Xóa file draft và private key nếu cần bảo mật cao)
                if (File.Exists(rawPdf)) File.Delete(rawPdf);

                MessageBox.Show($"Đã ký thành công!\nGửi cho người B các file sau trong thư mục {folder}:\n1. VanBan_DaKy.pdf\n2. Key_A_Public.xml");
                System.Diagnostics.Process.Start(folder);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ExportToPdf_Raw(DataTable dt, string path)
        {
            // Tạo file PDF
            Document doc = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();

            // --- SỬA LỖI FONT Ở ĐÂY ---
            // 1. Cấu hình Font chữ Tiếng Việt
            // Load font Arial từ Windows
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            // QUAN TRỌNG: Phải ghi rõ iTextSharp.text.Font để tránh nhầm với Font của Window Form
            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            iTextSharp.text.Font fontHeader = new iTextSharp.text.Font(bf, 11, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font fontData = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // 2. Thêm Tiêu đề
            Paragraph title = new Paragraph("DANH SÁCH NHÂN VIÊN (USERINFO)", fontTitle);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            doc.Add(title);

            // 3. Tạo Bảng (5 cột)
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 15f, 25f, 15f, 25f, 20f });

            // --- Vẽ Tiêu đề cột (Header) ---
            string[] headers = { "Username", "Họ và Tên", "Ngày sinh", "Email", "SĐT" };
            foreach (string h in headers)
            {
                PdfPCell cell = new PdfPCell(new Phrase(h, fontHeader));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.BackgroundColor = BaseColor.DARK_GRAY;
                cell.Padding = 7;
                table.AddCell(cell);
            }

            // --- VÒNG LẶP ĐỌC DỮ LIỆU TỪ DATATABLE ---
            foreach (DataRow row in dt.Rows)
            {
                // Username
                table.AddCell(new PdfPCell(new Phrase(row["USERNAME"].ToString(), fontData)));

                // Fullname (Tiếng Việt)
                table.AddCell(new PdfPCell(new Phrase(row["FULLNAME"].ToString(), fontData)));

                // Ngày sinh
                string dob = "";
                if (row["DATEOFBIRTH"] != DBNull.Value)
                {
                    // Kiểm tra kiểu dữ liệu để convert cho đúng
                    DateTime d;
                    if (DateTime.TryParse(row["DATEOFBIRTH"].ToString(), out d))
                    {
                        dob = d.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        dob = row["DATEOFBIRTH"].ToString();
                    }
                }
                PdfPCell cellDob = new PdfPCell(new Phrase(dob, fontData));
                cellDob.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cellDob);

                // Email
                table.AddCell(new PdfPCell(new Phrase(row["EMAIL"].ToString(), fontData)));

                // Phone
                PdfPCell cellPhone = new PdfPCell(new Phrase(row["PHONENUMBER"].ToString(), fontData));
                cellPhone.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cellPhone);
            }

            // Thêm bảng vào file PDF
            doc.Add(table);

            // Thêm ngày xuất
            Paragraph footer = new Paragraph($"\nNgày xuất: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", fontData);
            footer.Alignment = Element.ALIGN_RIGHT;
            doc.Add(footer);

            doc.Close();
        }

        private void radDOCX_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radPDF_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtNguoiKy_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
