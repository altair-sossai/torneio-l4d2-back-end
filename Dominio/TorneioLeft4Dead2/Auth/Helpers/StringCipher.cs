using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TorneioLeft4Dead2.Auth.Helpers
{
    public static class StringCipher
    {
        private const int Size = 256;
        private static readonly string CryptographyKey = Environment.GetEnvironmentVariable("CryptographyKey");
        private static readonly byte[] VectorBytes = Encoding.ASCII.GetBytes(CryptographyKey);

        public static string Encrypt(string content, string key)
        {
            var bytes = Encoding.UTF8.GetBytes(content.Trim());
            using var passwordDeriveBytes = new PasswordDeriveBytes(key, null);
            var keyBytes = passwordDeriveBytes.GetBytes(Size / 8);
            using var rijndaelManaged = new RijndaelManaged {Mode = CipherMode.CBC};
            using var cryptoTransform = rijndaelManaged.CreateEncryptor(keyBytes, VectorBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            var cipherTextBytes = memoryStream.ToArray();
            return Convert.ToBase64String(cipherTextBytes);
        }
    }
}