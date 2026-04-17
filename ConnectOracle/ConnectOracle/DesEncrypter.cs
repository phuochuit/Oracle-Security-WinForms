using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ConnectOracle
{
    public class DesEncrypter
    {
        // IV mặc định (8 bytes cho DES). 
        private readonly byte[] IV = { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

        // 1. Hàm sinh khóa DES ngẫu nhiên và lưu ra file
        public void GenerateKeyToFile(string keyPath)
        {
            try
            {
                using (DES des = DES.Create())
                {
                    des.GenerateKey();
                    // Lưu Key dưới dạng Base64
                    string keyBase64 = Convert.ToBase64String(des.Key);
                    File.WriteAllText(keyPath, keyBase64);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo khóa DES: " + ex.Message);
            }
        }

        public byte[] Encrypt(string plainText, byte[] key)
        {
            try
            {
                // Tạo MemoryStream để lưu dữ liệu mã hóa
                using (MemoryStream mStream = new MemoryStream())
                {
                    // Tạo đối tượng DES
                    using (DES des = DES.Create())
                    // Tạo bộ mã hóa từ key và IV
                    using (ICryptoTransform encryptor = des.CreateEncryptor(key, IV))
                    // Tạo luồng mã hóa
                    using (CryptoStream cStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                    {
                        // Chuyển chuỗi sang mảng byte
                        byte[] toEncrypt = Encoding.UTF8.GetBytes(plainText);

                        // Ghi mảng byte vào luồng mã hóa
                        cStream.Write(toEncrypt, 0, toEncrypt.Length);
                        cStream.FlushFinalBlock();

                        // Lấy dữ liệu mã hóa từ MemoryStream
                        byte[] ret = mStream.ToArray();
                        return ret;
                    }
                }
            }
            catch (CryptographicException e)
            {
                MessageBox.Show("A Cryptographic error occurred: " + e.Message);
                throw;
            }
        }

        public string Decrypt(byte[] encrypted, byte[] key)
        {
            try
            {
                // Tạo buffer để chứa dữ liệu giải mã
                byte[] decrypted = new byte[encrypted.Length];
                int offset = 0;

                // Tạo MemoryStream từ dữ liệu mã hóa
                using (MemoryStream mStream = new MemoryStream(encrypted))
                {
                    // Tạo đối tượng DES
                    using (DES des = DES.Create())
                    // Tạo bộ giải mã từ key và IV
                    using (ICryptoTransform decryptor = des.CreateDecryptor(key, IV))
                    // Tạo luồng giải mã
                    using (CryptoStream cStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read))
                    {
                        int read = 1;
                        while (read > 0)
                        {
                            read = cStream.Read(decrypted, offset, decrypted.Length - offset);
                            offset += read;
                        }
                    }
                }

                // Chuyển dữ liệu byte thành chuỗi và trả về
                return Encoding.UTF8.GetString(decrypted, 0, offset);
            }
            catch (CryptographicException e)
            {
                MessageBox.Show("A Cryptographic error occurred: " + e.Message);
                throw;
            }
        }

        // 2. Hàm mã hóa File
        public bool EncryptFile(string inputFilePath, string keyFilePath, string outputFilePath)
        {
            try
            {
                if (!File.Exists(inputFilePath)) return false;
                if (!File.Exists(keyFilePath))
                {
                    MessageBox.Show("Không tìm thấy file khóa: " + keyFilePath);
                    return false;
                }

                // Đọc Key
                string keyBase64 = File.ReadAllText(keyFilePath);
                byte[] key = Convert.FromBase64String(keyBase64);

                // Đọc File gốc
                byte[] inputBytes = File.ReadAllBytes(inputFilePath);

                using (DES des = DES.Create())
                using (ICryptoTransform encryptor = des.CreateEncryptor(key, IV))
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(inputBytes, 0, inputBytes.Length);
                    cs.FlushFinalBlock();

                    byte[] encryptedBytes = ms.ToArray();
                    File.WriteAllBytes(outputFilePath, encryptedBytes);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mã hóa DES: " + ex.Message);
                return false;
            }
        }

        // 3. Hàm giải mã File
        public bool DecryptFile(string encFilePath, string keyFilePath, string outFilePath)
        {
            try
            {
                string keyBase64 = File.ReadAllText(keyFilePath);
                byte[] key = Convert.FromBase64String(keyBase64);
                byte[] encBytes = File.ReadAllBytes(encFilePath);

                using (DES des = DES.Create())
                using (ICryptoTransform decryptor = des.CreateDecryptor(key, IV))
                using (MemoryStream ms = new MemoryStream(encBytes))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decryptedData = new byte[encBytes.Length];
                    int bytesRead = cs.Read(decryptedData, 0, decryptedData.Length);

                    // Cắt bỏ phần dư thừa
                    byte[] finalData = new byte[bytesRead];
                    Array.Copy(decryptedData, finalData, bytesRead);

                    File.WriteAllBytes(outFilePath, finalData);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi giải mã DES: " + ex.Message);
                return false;
            }
        }
    }
}