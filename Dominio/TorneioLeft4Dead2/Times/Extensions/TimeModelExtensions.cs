using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Extensions;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Times.Extensions
{
    public static class TimeModelExtensions
    {
        public static void Vincular(this IEnumerable<TimeModel> models, IEnumerable<TimeJogadorEntity> timesJogadores, IEnumerable<JogadorEntity> jogadores)
        {
            var times = timesJogadores.AgruparPorTimes();
            var dictionary = jogadores.ToDictionary();

            foreach (var model in models.Where(time => times.ContainsKey(time.Codigo)))
                model.Jogadores = times[model.Codigo].Select(s => dictionary[s.Jogador]).ToList();
        }
    }
}