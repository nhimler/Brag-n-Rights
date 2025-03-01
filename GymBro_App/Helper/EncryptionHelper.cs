using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace GymBro_App.Helper
{
    public class EncryptionHelper
    {
        private readonly byte[] _encryptionKey;

        public EncryptionHelper(IConfiguration configuration)
        {
            string keyBase64 = configuration["EncryptionKey"];
            if (string.IsNullOrEmpty(keyBase64))
                throw new Exception("Encryption key is missing.");

            _encryptionKey = Convert.FromBase64String(keyBase64);

            if (_encryptionKey.Length != 32) // Ensure it's exactly 32 bytes
                throw new Exception("Encryption key must be exactly 32 bytes.");
        }

        public string EncryptToken(string token)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey; // Use the correct byte array
                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
                        cs.Write(tokenBytes, 0, tokenBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    byte[] encryptedToken = ms.ToArray();
                    return Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(encryptedToken);
                }
            }
        }

        public string DecryptToken(string encryptedData)
        {
            string[] parts = encryptedData.Split(':');
            if (parts.Length != 2) return null;

            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] encryptedToken = Convert.FromBase64String(parts[1]);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _encryptionKey; // Use the correct byte array
                aes.IV = iv;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedToken, 0, encryptedToken.Length);
                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
    }
}
