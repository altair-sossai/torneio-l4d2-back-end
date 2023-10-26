using System.Collections.Generic;
using TorneioLeft4Dead2.PlayStats.Models;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Estatisticas.PorEquipe.Models;

public class EquipeModel
{
    public EquipeModel(TimeModel time)
    {
        Time = time;
    }

    public TimeModel Time { get; }
    public List<ConfrontoModel> Confrontos { get; set; } = new();

    public void AddConfronto(Match match, TimeEntity adversario)
    {
        var confronto = new ConfrontoModel(match, Time, adversario);

        Confrontos.Add(confronto);
    }
}