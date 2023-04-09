using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Shared.Constants;

namespace TorneioLeft4Dead2.Shared.Extensions;

public static class MemoryCacheExtensions
{
    public static void RemoveAllKeys(this IMemoryCache memoryCache)
    {
        memoryCache.Remove(MemoryCacheKeys.Jogadores);
        memoryCache.Remove(MemoryCacheKeys.Confrontos);
        memoryCache.Remove(MemoryCacheKeys.Times);
        memoryCache.Remove(MemoryCacheKeys.Classificacao);
        memoryCache.Remove(MemoryCacheKeys.Playoffs);
    }
}