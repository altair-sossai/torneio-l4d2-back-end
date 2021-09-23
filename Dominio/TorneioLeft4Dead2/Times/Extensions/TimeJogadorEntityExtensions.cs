using System.Collections.Generic;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Extensions
{
    public static class TimeJogadorEntityExtensions
    {
        public static Dictionary<string, List<TimeJogadorEntity>> AgruparPorTimes(this IEnumerable<TimeJogadorEntity> entities)
        {
            var dictionary = new Dictionary<string, List<TimeJogadorEntity>>();

            foreach (var entity in entities)
            {
                if (!dictionary.ContainsKey(entity.Time))
                    dictionary.Add(entity.Time, new List<TimeJogadorEntity>());

                dictionary[entity.Time].Add(entity);
            }

            return dictionary;
        }
    }
}