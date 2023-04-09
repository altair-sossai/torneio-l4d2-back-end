using TorneioLeft4Dead2.Jogadores.Extensions;

namespace TorneioLeft4Dead2.Jogadores.Commands;

public class JogadorCommand
{
    public string User { get; set; }
    public string Login => this.Login();
    public string SteamId => this.SteamId();
}