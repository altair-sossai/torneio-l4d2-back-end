using System.Collections.Generic;

namespace TorneioLeft4Dead2.Confrontos.Models;

public class RodadaModel(KeyValuePair<int, List<ConfrontoModel>> keyValuePair)
{
    public int Rodada { get; } = keyValuePair.Key;
    public List<ConfrontoModel> Confrontos { get; } = keyValuePair.Value;
}