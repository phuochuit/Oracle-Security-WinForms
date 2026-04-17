using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;

namespace ConnectOracle
{
    public partial class FormVerify : Form
    {
        // --- 1. KHAI BÁO BIẾN TOÀN CỤC (Để các hàm đều nhìn thấy) ---
        private TextBox txtPdfPath;
        private TextBox txtPubKeyPath;
        private Label lblStatus;
        private Label lblChiTiet;
        private Button btnPdf;      // Đưa button ra ngoài
        private Button btnKey;      // Đưa button ra ngoài
        private Button btnVerify;   // Đưa button ra ngoài

        public FormVerify()
        {
            // 1. Vẽ giao diện trước
            InitializeComponent();

            // 2. Gán sự kiện SAU khi giao diện đã có (An toàn cho Designer)
            // Sự kiện chọn file PDF
            btnPdf.Click += (s, e) =>
            {
                OpenFileDialog op = new OpenFileDialog { Filter = "PDF Files|*.pdf" };
                if (op.ShowDialog() == DialogResult.OK)
                    txtPdfPath.Text = op.FileName;
            };

            // Sự kiện chọn file Key XML
            btnKey.Click += (s, e) =>
            {
                OpenFileDialog op = new OpenFileDialog { Filter = "XML Key|*.xml" };
                if (op.ShowDialog() == DialogResult.OK)
                    txtPubKeyPath.Text = op.FileName;
            };

            // Sự kiện nút Verify
            btnVerify.Click += BtnVerify_Click;
        }

        // --- 2. THIẾT KẾ GIAO DIỆN (Code tay - Chỉ chứa thuộc tính hiển thị) ---
        private void InitializeComponent()
        {
            this.txtPdfPath = new TextBox();
            this.txtPubKeyPath = new TextBox();
            this.lblStatus = new Label();
            this.lblChiTiet = new Label();

            // Khởi tạo các nút (đã khai báo ở trên đầu)
            this.btnPdf = new Button();
            this.btnKey = new Button();
            this.btnVerify = new Button();

            Label lbl1 = new Label();
            Label lbl2 = new Label();

            this.SuspendLayout();

            // Setup Labels & Textbox
            lbl1.Text = "File PDF:"; lbl1.Location = new System.Drawing.Point(20, 30);
            txtPdfPath.Location = new System.Drawing.Point(100, 27); txtPdfPath.Size = new System.Drawing.Size(300, 20); txtPdfPath.ReadOnly = true;

            lbl2.Text = "Public Key:"; lbl2.Location = new System.Drawing.Point(20, 70);
            txtPubKeyPath.Location = new System.Drawing.Point(100, 67); txtPubKeyPath.Size = new System.Drawing.Size(300, 20); txtPubKeyPath.ReadOnly = true;

            // Setup Buttons (Chỉ setup vị trí, text, KHÔNG gán sự kiện ở đây)
            btnPdf.Text = "..."; btnPdf.Location = new System.Drawing.Point(410, 25); btnPdf.Size = new System.Drawing.Size(40, 23);

            btnKey.Text = "..."; btnKey.Location = new System.Drawing.Point(410, 65); btnKey.Size = new System.Drawing.Size(40, 23);

            btnVerify.Text = "KIỂM TRA CHỮ KÝ"; btnVerify.Location = new System.Drawing.Point(150, 110); btnVerify.Size = new System.Drawing.Size(150, 40);

            // Setup Result Labels
            lblStatus.Location = new System.Drawing.Point(20, 170); lblStatus.AutoSize = true;
            lblStatus.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            lblStatus.Text = "Trạng thái: ...";

            lblChiTiet.Location = new System.Drawing.Point(20, 200); lblChiTiet.AutoSize = true; lblChiTiet.Size = new System.Drawing.Size(400, 50);

            // Add Controls to Form
            this.Controls.Add(lbl1); this.Controls.Add(txtPdfPath); this.Controls.Add(btnPdf);
            this.Controls.Add(lbl2); this.Controls.Add(txtPubKeyPath); this.Controls.Add(btnKey);
            this.Controls.Add(btnVerify); this.Controls.Add(lblStatus); this.Controls.Add(lblChiTiet);

            this.ClientSize = new System.Drawing.Size(480, 260);
            this.Text = "Check Signature (iTextSharp 5)";
            this.ResumeLayout(false);
        }

        // --- 3. XỬ LÝ SỰ KIỆN CHÍNH ---
        private void BtnVerify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPdfPath.Text) || string.IsNullOrEmpty(txtPubKeyPath.Text))
            {
                MessageBox.Show("Vui lòng chọn đủ file PDF và Key XML.");
                return;
            }

            try
            {
                VerifySignature(txtPdfPath.Text, txtPubKeyPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        // --- 4. LOGIC XÁC MINH (Giữ nguyên code của bạn vì Logic RSA đã đúng) ---
        private void VerifySignature(string pdfPath, string xmlKeyPath)
        {
            PdfReader reader = null;
            try
            {
                string xmlContent = File.ReadAllText(xmlKeyPath);

                // Check Private Key
                if (xmlContent.Contains("<D>") || xmlContent.Contains("<P>") || xmlContent.Contains("<Q>"))
                {
                    var traloi = MessageBox.Show(
                        "CẢNH BÁO: File XML này chứa KHÓA RIÊNG (Private Key).\nBạn có muốn tiếp tục không?",
                        "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (traloi == DialogResult.No) return;
                }

                RSACryptoServiceProvider rsaFromXml = new RSACryptoServiceProvider();
                rsaFromXml.FromXmlString(xmlContent);

                byte[] pdfBytes = File.ReadAllBytes(pdfPath);
                reader = new PdfReader(pdfBytes);
                AcroFields fields = reader.AcroFields;
                var names = fields.GetSignatureNames();

                if (names.Count == 0)
                {
                    UpdateUI(false, "File này chưa được ký!");
                    return;
                }

                bool isFileModified = false;
                bool isKeyMatch = false;
                bool isCoverWholeDocument = true;

                foreach (string name in names)
                {
                    PdfPKCS7 pkcs7 = fields.VerifySignature(name);

                    if (!pkcs7.Verify()) isFileModified = true;
                    if (!fields.SignatureCoversWholeDocument(name)) isCoverWholeDocument = false;

                    var bcCert = pkcs7.SigningCertificate;
                    X509Certificate2 dotnetCert = new X509Certificate2(bcCert.GetEncoded());
                    RSACryptoServiceProvider rsaInPdf = (RSACryptoServiceProvider)dotnetCert.PublicKey.Key;

                    if (CompareKeys(rsaFromXml, rsaInPdf)) isKeyMatch = true;
                }

                if (isFileModified) UpdateUI(false, "NGUY HIỂM: File đã bị chỉnh sửa nội dung!");
                else if (!isCoverWholeDocument) UpdateUI(false, "CẢNH BÁO: File đã bị sửa đổi (thêm trang) sau khi ký.");
                else if (!isKeyMatch) UpdateUI(false, "SAI NGƯỜI KÝ: Key XML không khớp với chữ ký trong file.");
                else UpdateUI(true, "HỢP LỆ: Văn bản toàn vẹn và đúng người ký.");
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        private bool CompareKeys(RSACryptoServiceProvider key1, RSACryptoServiceProvider key2)
        {
            var p1 = key1.ExportParameters(false);
            var p2 = key2.ExportParameters(false);
            if (p1.Modulus.Length != p2.Modulus.Length) return false;
            for (int i = 0; i < p1.Modulus.Length; i++)
                if (p1.Modulus[i] != p2.Modulus[i]) return false;
            return true;
        }

        private void UpdateUI(bool isValid, string msg)
        {
            lblStatus.Text = isValid ? "KẾT QUẢ: OK" : "KẾT QUẢ: FAILED";
            lblStatus.ForeColor = isValid ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            lblChiTiet.Text = msg;
        }
    }
}