namespace TorneioLeft4Dead2.Shared.Helpers;

public static class StringHelper
{
    public static string FirstLetterLowerCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        if (value.Length == 1)
            return value.ToLower();

        return char.ToLower(value[0]) + value[1..];
    }
}