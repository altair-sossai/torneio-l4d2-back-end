namespace TorneioLeft4Dead2.Estatisticas.PorEquipe.Models;

public class JogadorModel
{
    public string SteamId { get; set; }
    public string Nome { get; set; }
    public string UrlFotoPerfil { get; set; }
    public string UrlPerfilSteam { get; set; }
    public int PointsMvpSiDamage { get; set; }
    public int PointsMvpCommon { get; set; }
    public int PointsLvpFfGiven { get; set; }
}