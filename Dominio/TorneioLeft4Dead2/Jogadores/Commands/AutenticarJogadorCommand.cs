using System;
using System.Linq;
using System.Text;
using TorneioLeft4Dead2.Auth.Helpers;

namespace TorneioLeft4Dead2.Jogadores.Commands;

public class AutenticarJogadorCommand
{
    public string SteamId { get; set; }
    public string Senha { get; set; }
    public string SenhaCriptografada => PasswordHelper.Encrypt(Senha);

    public static AutenticarJogadorCommand Parse(string authentication)
    {
        var base64 = Convert.FromBase64String(authentication);
        var plaintext = Encoding.UTF8.GetString(base64);
        var strings = plaintext.Split(':', 2);

        var command = new AutenticarJogadorCommand
        {
            SteamId = strings.FirstOrDefault(),
            Senha = strings.LastOrDefault()
        };

        return command;
    }
}