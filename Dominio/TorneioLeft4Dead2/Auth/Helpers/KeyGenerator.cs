using System;
using System.Linq;

namespace TorneioLeft4Dead2.Auth.Helpers
{
    public static class KeyGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-[];'\\,./<>?:\"{}|";
        private static readonly Random Random = new();

        public static string RandomString(int length)
        {
            var chars = Enumerable.Repeat(Chars, length)
                .Select(s => s[Random.Next(s.Length)])
                .ToArray();

            return new string(chars);
        }
    }
}