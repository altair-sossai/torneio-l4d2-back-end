namespace TorneioLeft4Dead2.Jogadores.Commands
{
    public class JogadorCommand
    {
        public string SteamId { get; set; }
        public string Nome { get; set; }
        public string UrlFotoPerfil { get; set; }
        public string UrlPerfilSteam { get; set; }
        public int? TotalHoras { get; set; }
    }
}