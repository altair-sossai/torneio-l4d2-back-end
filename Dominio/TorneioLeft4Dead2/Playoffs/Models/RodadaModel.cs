using System.Collections.Generic;

namespace TorneioLeft4Dead2.Playoffs.Models
{
    public class RodadaModel
    {
        public RodadaModel(KeyValuePair<int, List<PlayoffsModel>> keyValuePair)
        {
            Rodada = keyValuePair.Key;
            Playoffs = keyValuePair.Value;
        }

        public int Rodada { get; }
        public List<PlayoffsModel> Playoffs { get; }
    }
}