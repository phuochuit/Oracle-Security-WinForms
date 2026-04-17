using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;

// Namespace của BouncyCastle 2.0
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators; // Quan trọng: chứa Asn1SignatureFactory
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Asn1.X509;

namespace ConnectOracle
{
    public class PdfSignerHelper
    {
        // 1. Sinh cặp khóa RSA và lưu ra file XML
        public void GenerateKeys(string pubPath, string priPath)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                File.WriteAllText(pubPath, rsa.ToXmlString(false)); // Public Key
                File.WriteAllText(priPath, rsa.ToXmlString(true));  // Private Key
            }
        }

        // 2. Ký số vào file PDF
        public void SignPdf(string sourcePdf, string destPdf, string privateKeyXml, string signerName, string reason)
        {
            // A. Tạo Certificate tự ký từ RSA Private Key (Style mới cho BC 2.0)
            var keyPair = CreateBouncyCastleKeyPair(privateKeyXml);
            var cert = CreateSelfSignedCertificate(keyPair, signerName);

            // Tạo chuỗi chứng thực (List)
            ICollection<Org.BouncyCastle.X509.X509Certificate> chain = new List<Org.BouncyCastle.X509.X509Certificate>();
            chain.Add(cert);

            // B. Mở PDF và Ký
            using (FileStream fs = new FileStream(destPdf, FileMode.Create))
            {
                PdfReader reader = new PdfReader(sourcePdf);
                PdfStamper stamper = PdfStamper.CreateSignature(reader, fs, '\0');

                // Tạo giao diện chữ ký (Visual)
                PdfSignatureAppearance appearance = stamper.SignatureAppearance;
                appearance.Reason = reason;
                appearance.Location = "Vietnam";
                appearance.Contact = signerName;
                // Vị trí chữ ký (Trang 1, tọa độ X, Y, W, H)
                appearance.SetVisibleSignature(new Rectangle(100, 50, 300, 150), 1, "SignatureField");

                appearance.Layer2Text = $"Ký bởi: {signerName}\nNgày: {DateTime.Now:dd/MM/yyyy HH:mm}";

                // Tạo chữ ký số (Crypto)
                // Lưu ý: PrivateKeySignature cần thuật toán băm rõ ràng
                IExternalSignature pks = new PrivateKeySignature(keyPair.Private, "SHA-256");

                MakeSignature.SignDetached(appearance, pks, chain, null, null, null, 0, CryptoStandard.CMS);

                stamper.Close();
                reader.Close();
            }
        }

        // --- HÀM HỖ TRỢ (ĐÃ SỬA CHO BOUNCY CASTLE 2.0) ---

        private AsymmetricCipherKeyPair CreateBouncyCastleKeyPair(string privateKeyXml)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKeyXml);
                return DotNetUtilities.GetRsaKeyPair(rsa);
            }
        }

        private Org.BouncyCastle.X509.X509Certificate CreateSelfSignedCertificate(AsymmetricCipherKeyPair keyPair, string subjectName)
        {
            // Sử dụng X509V3CertificateGenerator (Vẫn còn trong 2.0 nhưng cách dùng khác)
            X509V3CertificateGenerator certGen = new X509V3CertificateGenerator();

            // Random generator chuẩn mới
            SecureRandom random = new SecureRandom();

            // Serial Number
            BigInteger serialNumber = BigInteger.ProbablePrime(120, new Random());
            certGen.SetSerialNumber(serialNumber);
            certGen.SetIssuerDN(new X509Name($"CN={subjectName}"));
            certGen.SetSubjectDN(new X509Name($"CN={subjectName}"));

            // Thời hạn
            certGen.SetNotBefore(DateTime.UtcNow.Date);
            certGen.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));

            certGen.SetPublicKey(keyPair.Public);

            // --- KHÁC BIỆT LỚN NHẤT Ở ĐÂY ---
            // Thay vì SetSignatureAlgorithm, ta phải tạo ISignatureFactory
            ISignatureFactory signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", keyPair.Private, random);

            // Truyền factory vào hàm Generate
            return certGen.Generate(signatureFactory);
        }
    }
}