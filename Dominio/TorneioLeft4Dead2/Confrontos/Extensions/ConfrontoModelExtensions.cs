using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Campanhas.Extensions;
using TorneioLeft4Dead2.Confrontos.Models;
using TorneioLeft4Dead2.Times.Extensions;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Confrontos.Extensions
{
    public static class ConfrontoModelExtensions
    {
        public static List<RodadaModel> Rodadas(this List<ConfrontoModel> confrontos)
        {
            return confrontos
                .AgruparPorRodada()
                .Select(rodada => new RodadaModel(rodada))
                .OrderBy(o => o.Rodada)
                .ToList();
        }

        private static Dictionary<int, List<ConfrontoModel>> AgruparPorRodada(this List<ConfrontoModel> confrontos)
        {
            var rodadas = new Dictionary<int, List<ConfrontoModel>>();

            foreach (var confronto in confrontos)
            {
                if (!rodadas.ContainsKey(confronto.Rodada))
                    rodadas.Add(confronto.Rodada, new List<ConfrontoModel>());

                rodadas[confronto.Rodada].Add(confronto);
            }

            return rodadas;
        }

        public static void Vincular(this List<ConfrontoModel> confrontos, IEnumerable<CampanhaEntity> campanhas)
        {
            var dictionary = campanhas.ToDictionary();

            foreach (var confronto in confrontos)
                confronto.Campanha = dictionary[confronto.CodigoCampanha];
        }

        public static void Vincular(this List<ConfrontoModel> confrontos, IEnumerable<TimeModel> times)
        {
            var dictionary = times.ToDictionary();

            foreach (var confronto in confrontos)
            {
                confronto.TimeA = dictionary[confronto.CodigoTimeA];
                confronto.TimeB = dictionary[confronto.CodigoTimeB];

                if (!string.IsNullOrEmpty(confronto.CodigoTimeVencedor))
                    confronto.TimeVencedor = dictionary[confronto.CodigoTimeVencedor];
            }
        }
    }
}