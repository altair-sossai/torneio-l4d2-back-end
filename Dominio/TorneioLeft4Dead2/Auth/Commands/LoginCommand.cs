using TorneioLeft4Dead2.Auth.Helpers;

namespace TorneioLeft4Dead2.Auth.Commands;

public class LoginCommand
{
    private string _email;
    private string _password;

    public string Email
    {
        get => _email;
        set => _email = value?.Trim().ToLower();
    }

    public string Password
    {
        get => _password;
        set => _password = PasswordHelper.Encrypt(value);
    }
}