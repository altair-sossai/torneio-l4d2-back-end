using System;

namespace TorneioLeft4Dead2.Auth.Helpers;

public static class PasswordHelper
{
    private static readonly string Key = Environment.GetEnvironmentVariable("PasswordKey");

    public static string Encrypt(string password)
    {
        return string.IsNullOrEmpty(password) ? null : StringCipher.Encrypt(password, Key);
    }
}