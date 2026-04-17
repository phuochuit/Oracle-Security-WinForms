using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectOracle
{
    public partial class FormNhapOTP : Form
    {
        private string _serverOTP; // Mã đúng do hệ thống sinh ra
        public bool IsVerified { get; private set; } = false; // Kết quả trả về

        public FormNhapOTP(string otp)
        {
            InitializeComponent();
            _serverOTP = otp;
            CenterToScreen();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text.Trim() == _serverOTP)
            {
                IsVerified = true;
                this.Close(); // Đóng form, trả về kết quả đúng
            }
            else
            {
                MessageBox.Show("Mã OTP sai! Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtOTP.Clear();
                txtOTP.Focus();
            }
        }
    }
}
