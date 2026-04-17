using System;
using System.IO;
using System.Windows.Forms;

namespace ConnectOracle
{
    public partial class FileEncryptionUI : Form
    {
        // Sử dụng Class DesEncrypter đã viết ở bài trước
        DesEncrypter DesEngine;

        public FileEncryptionUI()
        {
            InitializeComponent();
            CenterToScreen();
            DesEngine = new DesEncrypter();
        }

        // --- CÁC HÀM HỖ TRỢ CHỌN FILE ---
        string GetFileDialogOpen(string filter)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = filter;
            if (file.ShowDialog() == DialogResult.OK)
            {
                if (file.CheckFileExists) return file.FileName;
                else MessageBox.Show("File không tồn tại!");
            }
            return "";
        }

        // --- NÚT BROWSE ---
        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            // Cho phép chọn mọi loại file để mã hóa
            txtEncryptFile.Text = GetFileDialogOpen("All Files (*.*)|*.*");
        }

        private void btnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            txtDecryptFile.Text = GetFileDialogOpen("Encrypted Files (*.des;*.enc)|*.des;*.enc|All Files (*.*)|*.*");
        }

        private void btnBrowseKey_Click(object sender, EventArgs e)
        {
            // Key bây giờ là file text chứa Base64 (sinh ra từ Export form)
            txtKeyFile.Text = GetFileDialogOpen("Key Files (*.txt)|*.txt|All Files (*.*)|*.*");
        }

        // --- XỬ LÝ MÃ HÓA (ENCRYPT) ---
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            string inputFile = txtEncryptFile.Text;

            if (string.IsNullOrEmpty(inputFile) || !File.Exists(inputFile))
            {
                MessageBox.Show("Vui lòng chọn file tồn tại để mã hóa.");
                return;
            }

            // Tự động đặt tên file output và file key
            string outputFile = inputFile + ".des";
            string keyFile = Path.Combine(Path.GetDirectoryName(inputFile), "Key_Secret.txt");

            // 1. Sinh khóa DES ra file (Để dùng lại sau này)
            DesEngine.GenerateKeyToFile(keyFile);

            // 2. Mã hóa file theo luồng (Stream)
            bool ok = DesEngine.EncryptFile(inputFile, keyFile, outputFile);

            if (ok)
            {
                MessageBox.Show("Mã hóa thành công!\n\n" +
                                "File gốc: " + inputFile + "\n" +
                                "File mã hóa: " + outputFile + "\n" +
                                "File Key: " + keyFile + "\n\n" +
                                "(Hãy giữ file Key cẩn thận để giải mã)");
            }
        }

        // --- XỬ LÝ GIẢI MÃ (DECRYPT) ---
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            string encFile = txtDecryptFile.Text;
            string keyFile = txtKeyFile.Text;

            if (string.IsNullOrEmpty(encFile) || !File.Exists(encFile))
            {
                MessageBox.Show("Vui lòng chọn file mã hóa (.des / .enc).");
                return;
            }
            if (string.IsNullOrEmpty(keyFile) || !File.Exists(keyFile))
            {
                MessageBox.Show("Vui lòng chọn file Key (.txt).");
                return;
            }

            // Tự động đặt tên file giải mã (bỏ đuôi .des hoặc thêm .dec)
            string outFile;
            if (encFile.EndsWith(".des"))
                outFile = encFile.Substring(0, encFile.Length - 4); // Bỏ đuôi .des
            else if (encFile.EndsWith(".enc"))
                outFile = encFile.Substring(0, encFile.Length - 4); // Bỏ đuôi .enc
            else
                outFile = encFile + ".dec"; // Thêm đuôi .dec

            // Gọi hàm DecryptFile của class DesEncrypter
            bool ok = DesEngine.DecryptFile(encFile, keyFile, outFile);

            if (ok)
            {
                MessageBox.Show("Giải mã thành công!\nFile kết quả: " + outFile);

                // Mở thư mục chứa file
                System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + outFile + "\"");
            }
        }
    }
}