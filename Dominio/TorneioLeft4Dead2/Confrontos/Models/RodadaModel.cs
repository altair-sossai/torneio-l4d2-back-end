using System.Collections.Generic;

namespace TorneioLeft4Dead2.Confrontos.Models;

public class RodadaModel
{
    public RodadaModel(KeyValuePair<int, List<ConfrontoModel>> keyValuePair)
    {
        Rodada = keyValuePair.Key;
        Confrontos = keyValuePair.Value;
    }

    public int Rodada { get; }
    public List<ConfrontoModel> Confrontos { get; }
}