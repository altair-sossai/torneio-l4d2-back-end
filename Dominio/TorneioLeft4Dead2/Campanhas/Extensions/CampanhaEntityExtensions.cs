using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Campanhas.Entidades;

namespace TorneioLeft4Dead2.Campanhas.Extensions
{
    public static class CampanhaEntityExtensions
    {
        public static Dictionary<int, CampanhaEntity> ToDictionary(this IEnumerable<CampanhaEntity> entities)
        {
            return entities.ToDictionary(k => k.Codigo, v => v);
        }
    }
}