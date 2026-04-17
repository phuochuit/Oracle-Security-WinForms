using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms; // Để dùng MessageBox

namespace ConnectOracle
{
    public class EmailHelper
    {
        // ⚠️ THAY BẰNG EMAIL THẬT CỦA BẠN
        private static string SENDER_EMAIL = "riderkamen0909@gmail.com";

        // ⚠️ THAY BẰNG MẬT KHẨU ỨNG DỤNG 16 KÝ TỰ (MỚI TẠO, KHÔNG KHOẢNG TRẮNG)
        private static string SENDER_PASS = "silrjahdeqjpmjpz";

        public static string GenerateOTP()
        {
            Random r = new Random();
            return r.Next(100000, 999999).ToString(); // Tạo mã 6 số ngẫu nhiên
        }

        public static bool SendOTP(string toEmail, string otpCode)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(SENDER_EMAIL);
                mail.To.Add(toEmail);
                mail.Subject = "Mã xác thực đăng nhập (OTP)";
                mail.Body = $"<div style='font-family:Arial; font-size:14px;'>" +
                            $"<h3>Xin chào,</h3>" +
                            $"<p>Mã xác thực (OTP) của bạn là: <b style='color:red; font-size:20px;'>{otpCode}</b></p>" +
                            $"<p>Vui lòng không chia sẻ mã này cho bất kỳ ai.</p>" +
                            $"</div>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(SENDER_EMAIL, SENDER_PASS);

                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi mail: " + ex.ToString()); // Hiện chi tiết lỗi
                return false;
            }
        }
    }
}