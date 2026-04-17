using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectOracle
{
    public partial class FileEncryptionRSA : Form
    {
        RsaEncrypter Rsa;
        public FileEncryptionRSA()
        {
            InitializeComponent();
            CenterToScreen();
            Rsa = new RsaEncrypter();
        }

        string GetFileDialogOpen(string filter)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = filter;
            if (file.ShowDialog() == DialogResult.OK)
            {
                if (file.CheckFileExists)
                {
                    return file.FileName;
                }
                else
                {
                    MessageBox.Show("File không tồn tại!");
                    return null;
                }
            }
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = txt_encryptfile.Text;
            if (string.IsNullOrWhiteSpace(file) || !File.Exists(file))
            {
                MessageBox.Show("Vui lòng chọn file để mã hóa.");
                return;
            }

            bool ok = Rsa.EncryptFile(file);
            if (!ok)
            {
                MessageBox.Show("Mã hóa thất bại.");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txt_encryptfile.Text = GetFileDialogOpen("(*.docx;*.pdf;*.*)|*.docx;*.pdf;*.*");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt_decryptfile.Text = GetFileDialogOpen("(*.enc)|*.enc|(*.*)|*.*");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt_keyfile.Text = GetFileDialogOpen("(*.privkey)|*.privkey|(*.*)|*.*");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string encFile = txt_decryptfile.Text;
            string privateKey = txt_keyfile.Text;

            if (string.IsNullOrWhiteSpace(encFile) || !File.Exists(encFile))
            {
                MessageBox.Show("Vui lòng chọn file .enc để giải mã.");
                return;
            }
            if (string.IsNullOrWhiteSpace(privateKey) || !File.Exists(privateKey))
            {
                MessageBox.Show("Vui lòng chọn private key (*.privkey) để giải mã.");
                return;
            }

            bool ok = Rsa.DecryptFile(encFile, privateKey);
            if (!ok)
            {
                MessageBox.Show("Giải mã thất bại.");
            }
        }
    }
}
