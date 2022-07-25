using System.Collections.Generic;
using System.Linq;

namespace TorneioLeft4Dead2.Playoffs.Models
{
    public class RodadaModel
    {
        public RodadaModel(KeyValuePair<int, List<PlayoffsModel>> keyValuePair)
        {
            Rodada = keyValuePair.Key;
            Playoffs = keyValuePair.Value.OrderBy(o => o.Ordem).ToList();
        }

        public int Rodada { get; }
        public List<PlayoffsModel> Playoffs { get; }
    }
}