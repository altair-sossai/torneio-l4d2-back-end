using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Campanhas.Extensions;
using TorneioLeft4Dead2.Playoffs.Models;
using TorneioLeft4Dead2.Times.Extensions;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Playoffs.Extensions
{
    public static class PlayoffsModelExtensions
    {
        public static List<RodadaModel> Rodadas(this List<PlayoffsModel> playoffs)
        {
            return playoffs
                .AgruparPorRodada()
                .Select(rodada => new RodadaModel(rodada))
                .OrderBy(o => o.Rodada)
                .ToList();
        }

        private static Dictionary<int, List<PlayoffsModel>> AgruparPorRodada(this List<PlayoffsModel> playoffs)
        {
            var rodadas = new Dictionary<int, List<PlayoffsModel>>();

            foreach (var item in playoffs)
            {
                if (!rodadas.ContainsKey(item.Rodada))
                    rodadas.Add(item.Rodada, new List<PlayoffsModel>());

                rodadas[item.Rodada].Add(item);
            }

            return rodadas;
        }

        public static void Vincular(this List<PlayoffsModel> playoffs, IEnumerable<CampanhaEntity> campanhas)
        {
            var dictionary = campanhas.ToDictionary();

            foreach (var item in playoffs)
            {
                foreach (var confront in item.Confrontos)
                {
                    if (!confront.CodigoCampanha.HasValue)
                        continue;

                    confront.Campanha = dictionary[confront.CodigoCampanha.Value];
                }
            }
        }

        public static void Vincular(this List<PlayoffsModel> playoffs, IEnumerable<TimeModel> times)
        {
            var dictionary = times.ToDictionary();

            foreach (var item in playoffs)
            {
                item.TimeA = dictionary[item.CodigoTimeA];
                item.TimeB = dictionary[item.CodigoTimeB];

                if (!string.IsNullOrEmpty(item.CodigoTimeVencedor))
                    item.TimeVencedor = dictionary[item.CodigoTimeVencedor];

                if (!string.IsNullOrEmpty(item.CodigoTimePerdedor))
                    item.TimePerdedor = dictionary[item.CodigoTimePerdedor];
            }
        }
    }
}