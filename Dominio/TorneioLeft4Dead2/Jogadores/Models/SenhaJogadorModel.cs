using TorneioLeft4Dead2.Auth.Helpers;

namespace TorneioLeft4Dead2.Jogadores.Models
{
    public class SenhaJogadorModel
    {
        public SenhaJogadorModel(string steamId)
        {
            SteamId = steamId;
            SenhaDescriptografada = KeyGenerator.RandomString(12);
        }

        public string SteamId { get; }
        public string SenhaDescriptografada { get; }
        public string SenhaCriptografada => PasswordHelper.Encrypt(SenhaDescriptografada);
    }
}