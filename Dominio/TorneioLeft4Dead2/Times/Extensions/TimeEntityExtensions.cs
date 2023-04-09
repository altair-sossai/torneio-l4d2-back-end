using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Extensions;

public static class TimeEntityExtensions
{
    public static Dictionary<string, TimeEntity> ToDictionary(this IEnumerable<TimeEntity> times)
    {
        return times.ToDictionary(k => k.Codigo, v => v);
    }
}