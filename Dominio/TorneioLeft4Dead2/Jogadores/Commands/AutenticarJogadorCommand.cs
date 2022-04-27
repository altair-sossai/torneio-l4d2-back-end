using System;
using System.Linq;
using System.Text;
using TorneioLeft4Dead2.Auth.Helpers;

namespace TorneioLeft4Dead2.Jogadores.Commands
{
    public class AutenticarJogadorCommand
    {
        public AutenticarJogadorCommand(string authentication)
        {
            var base64 = Convert.FromBase64String(authentication);
            var plaintext = Encoding.UTF8.GetString(base64);
            var strings = plaintext.Split(':', 2);

            SteamId = strings.FirstOrDefault();
            Senha = strings.LastOrDefault();
        }

        public string SteamId { get; }
        public string Senha { get; }
        public string SenhaCriptografada => PasswordHelper.Encrypt(Senha);
    }
}