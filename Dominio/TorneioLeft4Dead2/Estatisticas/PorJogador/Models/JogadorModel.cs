namespace TorneioLeft4Dead2.Estatisticas.PorJogador.Models;

public class JogadorModel
{
    public string SteamId { get; set; }
    public string Nome { get; set; }
    public string UrlFotoPerfil { get; set; }
    public string UrlPerfilSteam { get; set; }

    /* PlayStats */
    public int Died { get; set; }
    public int Incaps { get; set; }
    public int DmgTaken { get; set; }
    public int Common { get; set; }
    public int SiKilled { get; set; }
    public int SiDamage { get; set; }
    public int TankDamage { get; set; }
    public int RockEats { get; set; }
    public int WitchDamage { get; set; }
    public int Skeets { get; set; }
    public int Levels { get; set; }
    public int Crowns { get; set; }
    public int FfGiven { get; set; }
    public int DmgTotal { get; set; }
    public int DmgTank { get; set; }
    public int DmgSpit { get; set; }
    public int HunterDpDmg { get; set; }
    public int MvpSiDamage { get; set; }
    public int MvpCommon { get; set; }
    public int LvpFfGiven { get; set; }
}