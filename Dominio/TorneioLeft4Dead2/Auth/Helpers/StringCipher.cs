using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TorneioLeft4Dead2.Auth.Helpers;

public static class StringCipher
{
    public static string Encrypt(string plainText, string key)
    {
        using var aes = Aes.Create();

        aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32));
        aes.IV = new byte[16];

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        {
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
        }

        return Convert.ToBase64String(ms.ToArray());
    }
}