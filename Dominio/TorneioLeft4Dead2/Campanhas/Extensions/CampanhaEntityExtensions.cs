using System;
using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Playoffs.Entidades;

namespace TorneioLeft4Dead2.Campanhas.Extensions;

public static class CampanhaEntityExtensions
{
    public static Dictionary<int, CampanhaEntity> ToDictionary(this IEnumerable<CampanhaEntity> entities)
    {
        return entities.ToDictionary(k => k.Codigo, v => v);
    }

    public static void RemoverCampanhasJaEscolhidas(this List<CampanhaEntity> campanhas, PlayoffsEntity playoff)
    {
        var campanhasJaEscolhidas = new[]
        {
            playoff.CodigoCampanhaExcluidaTimeA,
            playoff.CodigoCampanhaExcluidaTimeB,
            playoff.Confronto01CodigoCampanha,
            playoff.Confronto02CodigoCampanha
        };

        foreach (var campanhaEscolhida in campanhasJaEscolhidas)
        {
            var campanha = campanhas.FirstOrDefault(f => f.Codigo == campanhaEscolhida);
            if (campanha == null)
                continue;

            campanhas.Remove(campanha);
        }
    }

    public static CampanhaEntity Sortear(this IEnumerable<CampanhaEntity> campanhas)
    {
        return campanhas.MinBy(_ => Guid.NewGuid());
    }
}