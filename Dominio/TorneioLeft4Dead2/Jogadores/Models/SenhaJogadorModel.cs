using TorneioLeft4Dead2.Auth.Helpers;

namespace TorneioLeft4Dead2.Jogadores.Models;

public class SenhaJogadorModel(string steamId)
{
    public string SteamId { get; } = steamId;
    public string SenhaDescriptografada { get; } = KeyGenerator.RandomString(12);
    public string SenhaCriptografada => PasswordHelper.Encrypt(SenhaDescriptografada);
}