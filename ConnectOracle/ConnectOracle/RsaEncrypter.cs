// RsaEncrypter.cs (improved: include original filename in header + SHA256 checks)
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace ConnectOracle
{
    public class RsaEncrypter
    {
        private const string MAGIC = "ORCL"; // 4 bytes
        private const byte VERSION = 1;

        public void GenerateRsaKeyPair(string publicPath, string privatePath, int keySize = 2048)
        {
            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                try
                {
                    rsa.PersistKeyInCsp = false;
                    string publicXml = rsa.ToXmlString(false);
                    string privateXml = rsa.ToXmlString(true);

                    File.WriteAllText(publicPath, publicXml);
                    File.WriteAllText(privatePath, privateXml);
                }
                finally
                {
                }
            }
        }

        // Helper: compute SHA256 hex string of file bytes
        private string ComputeSHA256Hex(byte[] data)
        {
            using (var sha = SHA256.Create())
            {
                byte[] h = sha.ComputeHash(data);
                StringBuilder sb = new StringBuilder(h.Length * 2);
                foreach (var b in h) sb.AppendFormat("{0:x2}", b);
                return sb.ToString();
            }
        }

        // Encrypt file: write header that includes original filename to allow perfect restore
        // Format:
        // MAGIC(4) | VERSION(1) | Fnamelen(4) | Filename (UTF8) | Klen(4) | EncryptedKey | IV(16) | Clen(8) | Ciphertext | HMAC(32) | OrigSHA256Len(4) | OrigSHA256 (hex bytes)
        public bool EncryptFile(string inputFilePath)
        {
            try
            {
                if (!File.Exists(inputFilePath))
                {
                    MessageBox.Show("File không tồn tại: " + inputFilePath);
                    return false;
                }

                string encFile = inputFilePath + ".enc";
                string pubKeyFile = inputFilePath + ".pubkey";
                string privKeyFile = inputFilePath + ".privkey";

                byte[] plaintext = File.ReadAllBytes(inputFilePath);
                string origHash = ComputeSHA256Hex(plaintext);

                // AES key + IV + ciphertext
                byte[] aesKey, aesIV, ciphertext;
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.GenerateKey();
                    aes.GenerateIV();
                    aesKey = aes.Key;
                    aesIV = aes.IV;

                    using (var ms = new MemoryStream())
                    using (var crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(plaintext, 0, plaintext.Length);
                        crypto.FlushFinalBlock();
                        ciphertext = ms.ToArray();
                    }
                }

                // Ensure public key exists (per-file behavior kept). If not, auto-generate and inform.
                if (!File.Exists(pubKeyFile))
                {
                    GenerateRsaKeyPair(pubKeyFile, privKeyFile);
                    MessageBox.Show("Không tìm thấy public key. Đã tự động tạo cặp RSA:\nPublic: " + pubKeyFile + "\nPrivate: " + privKeyFile + "\n\nHãy lưu private key an toàn để giải mã sau này.");
                }

                // Load public key, encrypt AES key
                byte[] encryptedAesKey;
                string publicXml = File.ReadAllText(pubKeyFile);
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(publicXml);
                    encryptedAesKey = rsa.Encrypt(aesKey, true);
                }

                // HMAC-SHA256 over (IV || ciphertext)
                byte[] hmac;
                using (var h = new HMACSHA256(aesKey))
                using (var msForH = new MemoryStream())
                {
                    msForH.Write(aesIV, 0, aesIV.Length);
                    msForH.Write(ciphertext, 0, ciphertext.Length);
                    hmac = h.ComputeHash(msForH.ToArray());
                }

                // Original filename bytes
                string originalName = Path.GetFileName(inputFilePath);
                byte[] nameBytes = Encoding.UTF8.GetBytes(originalName);

                // Write structured .enc
                using (var fs = new FileStream(encFile, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(Encoding.ASCII.GetBytes(MAGIC)); // 4
                    bw.Write(VERSION);                        // 1
                    bw.Write((Int32)nameBytes.Length);        // 4
                    bw.Write(nameBytes);                      // filename
                    bw.Write((Int32)encryptedAesKey.Length); // 4
                    bw.Write(encryptedAesKey);
                    bw.Write(aesIV);                          // 16
                    bw.Write((Int64)ciphertext.Length);       // 8
                    bw.Write(ciphertext);
                    bw.Write(hmac);                           // 32
                    // write original hash for debugging/verification (length + bytes)
                    byte[] hashBytes = Encoding.ASCII.GetBytes(origHash);
                    bw.Write((Int32)hashBytes.Length);
                    bw.Write(hashBytes);
                }

                MessageBox.Show("Mã hóa xong.\nFile mã hóa: " + encFile + "\n(Private key lưu nếu tạo tự động: " + privKeyFile + ")\nSHA256 gốc: " + origHash);
                return true;
            }
            catch (CryptographicException ce)
            {
                MessageBox.Show("Lỗi mật mã: " + ce.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
        }

        // Decrypt and restore original filename; compare SHA256 and show both
        public bool DecryptFile(string encFilePath, string privateKeyPath)
        {
            try
            {
                if (!File.Exists(encFilePath))
                {
                    MessageBox.Show("File mã hóa không tồn tại: " + encFilePath);
                    return false;
                }
                if (!File.Exists(privateKeyPath))
                {
                    MessageBox.Show("Private key không tồn tại: " + privateKeyPath);
                    return false;
                }

                byte[] encryptedAesKey;
                byte[] iv;
                byte[] ciphertext;
                byte[] expectedHmac;
                string originalName = null;
                string origHashFromFile = null;

                using (var fs = new FileStream(encFilePath, FileMode.Open, FileAccess.Read))
                using (var br = new BinaryReader(fs))
                {
                    byte[] magicBytes = br.ReadBytes(4);
                    string magic = Encoding.ASCII.GetString(magicBytes);
                    if (magic != MAGIC)
                    {
                        MessageBox.Show("Định dạng file không hợp lệ (magic mismatch).");
                        return false;
                    }

                    byte version = br.ReadByte();
                    if (version != VERSION)
                    {
                        MessageBox.Show("Phiên bản file không hỗ trợ.");
                        return false;
                    }

                    int namelen = br.ReadInt32();
                    if (namelen <= 0 || namelen > 1024) { MessageBox.Show("Filename in header invalid."); return false; }
                    byte[] nameBytes = br.ReadBytes(namelen);
                    originalName = Encoding.UTF8.GetString(nameBytes);

                    int klen = br.ReadInt32();
                    if (klen <= 0 || klen > 8192) { MessageBox.Show("File khóa không hợp lệ."); return false; }
                    encryptedAesKey = br.ReadBytes(klen);
                    iv = br.ReadBytes(16);
                    long clen = br.ReadInt64();
                    if (clen < 0 || clen > fs.Length) { MessageBox.Show("Kích thước ciphertext không hợp lệ."); return false; }
                    ciphertext = br.ReadBytes((int)clen);
                    expectedHmac = br.ReadBytes(32);

                    // orig hash field
                    int hlen = br.ReadInt32();
                    if (hlen > 0 && hlen < 4096)
                    {
                        byte[] hashBytes = br.ReadBytes(hlen);
                        origHashFromFile = Encoding.ASCII.GetString(hashBytes);
                    }
                }

                // RSA decrypt AES key with provided private key
                byte[] aesKey;
                string privateXml = File.ReadAllText(privateKeyPath);
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.PersistKeyInCsp = false;
                    rsa.FromXmlString(privateXml);
                    try
                    {
                        aesKey = rsa.Decrypt(encryptedAesKey, true); // OAEP
                    }
                    catch (CryptographicException)
                    {
                        MessageBox.Show("Không thể giải RSA-encrypted AES key. Kiểm tra private key có đúng không?");
                        return false;
                    }
                }

                // Verify HMAC
                byte[] computedHmac;
                using (var h = new HMACSHA256(aesKey))
                using (var msForH = new MemoryStream())
                {
                    msForH.Write(iv, 0, iv.Length);
                    msForH.Write(ciphertext, 0, ciphertext.Length);
                    computedHmac = h.ComputeHash(msForH.ToArray());
                }

                if (!CryptographicEquals(computedHmac, expectedHmac))
                {
                    MessageBox.Show("Kiểm tra toàn vẹn thất bại (HMAC không khớp). File có thể bị thay đổi hoặc private key không đúng.");
                    return false;
                }

                // Decrypt ciphertext
                byte[] plaintext;
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = aesKey;
                    aes.IV = iv;

                    using (var ms = new MemoryStream())
                    using (var crypto = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(ciphertext, 0, ciphertext.Length);
                        crypto.FlushFinalBlock();
                        plaintext = ms.ToArray();
                    }
                }

                // Compute SHA256 and compare with origHashFromFile if present
                string decHash = ComputeSHA256Hex(plaintext);
                string message = "Giải mã thành công.\nSHA256 của file giải mã: " + decHash;
                if (!string.IsNullOrEmpty(origHashFromFile))
                {
                    message += "\nSHA256 lưu trong .enc: " + origHashFromFile;
                    if (string.Equals(decHash, origHashFromFile, StringComparison.OrdinalIgnoreCase))
                        message += "\n=> HASH TRÙNG KHỚP: file phục hồi nguyên vẹn.";
                    else
                        message += "\n=> HASH KHÔNG TRÙNG: file đã bị thay đổi hoặc key sai (nội dung khác).";
                }

                // Write out file with original name next to .enc location
                string outDir = Path.GetDirectoryName(encFilePath);
                string outFile = Path.Combine(outDir, originalName ?? (Path.GetFileName(encFilePath) + ".dec"));
                File.WriteAllBytes(outFile, plaintext);

                MessageBox.Show(message + "\nFile xuất: " + outFile);
                return true;
            }
            catch (CryptographicException ce)
            {
                MessageBox.Show("Lỗi giải mã mật mã: " + ce.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return false;
            }
        }

        private bool CryptographicEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i];
            return diff == 0;
        }

        public bool EncryptFileTarget(string inputFilePath, string publicKeyPath, string outputFilePath)
        {
            try
            {
                if (!File.Exists(inputFilePath)) return false;

                // 1. Đọc file gốc và tính hash gốc (để kiểm tra toàn vẹn mức cao)
                byte[] plaintext = File.ReadAllBytes(inputFilePath);
                string origHash = "";
                using (SHA256 sha = SHA256.Create())
                {
                    byte[] h = sha.ComputeHash(plaintext);
                    StringBuilder sb = new StringBuilder();
                    foreach (var b in h) sb.AppendFormat("{0:x2}", b);
                    origHash = sb.ToString();
                }

                byte[] aesKey, aesIV, ciphertext;

                // 2. Tạo khóa AES ngẫu nhiên và Mã hóa nội dung
                using (Aes aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.GenerateKey();
                    aes.GenerateIV();
                    aesKey = aes.Key;
                    aesIV = aes.IV;

                    using (var ms = new MemoryStream())
                    using (var crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        crypto.Write(plaintext, 0, plaintext.Length);
                        crypto.FlushFinalBlock();
                        ciphertext = ms.ToArray();
                    }
                }

                // 3. Mã hóa khóa AES bằng Public Key (từ file XML)
                byte[] encryptedAesKey;
                string publicXml = File.ReadAllText(publicKeyPath);
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicXml);
                    encryptedAesKey = rsa.Encrypt(aesKey, true);
                }

                // --- KHẮC PHỤC LỖI HMAC TẠI ĐÂY ---
                // 4. Tính toán HMAC thực sự (Hash của IV + Ciphertext)
                byte[] hmac;
                using (var h = new HMACSHA256(aesKey))
                using (var msForH = new MemoryStream())
                {
                    msForH.Write(aesIV, 0, aesIV.Length);
                    msForH.Write(ciphertext, 0, ciphertext.Length);
                    hmac = h.ComputeHash(msForH.ToArray());
                }

                // 5. Ghi file theo đúng cấu trúc chuẩn của DecryptFile
                string originalName = Path.GetFileName(inputFilePath);
                byte[] nameBytes = Encoding.UTF8.GetBytes(originalName);

                using (var fs = new FileStream(outputFilePath, FileMode.Create))
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(Encoding.ASCII.GetBytes("ORCL")); // Magic
                    bw.Write((byte)1);                         // Version
                    bw.Write((int)nameBytes.Length);
                    bw.Write(nameBytes);                       // Filename
                    bw.Write((int)encryptedAesKey.Length);
                    bw.Write(encryptedAesKey);                 // Encrypted Key
                    bw.Write(aesIV);                           // IV
                    bw.Write((long)ciphertext.Length);
                    bw.Write(ciphertext);                      // Ciphertext

                    // Ghi HMAC thật (không dùng dummy nữa)
                    bw.Write(hmac);

                    // Ghi Hash gốc (để DecryptFile so sánh cuối cùng)
                    byte[] hashBytes = Encoding.ASCII.GetBytes(origHash);
                    bw.Write((int)hashBytes.Length);
                    bw.Write(hashBytes);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi mã hóa: " + ex.Message);
                return false;
            }
        }
    }
}
