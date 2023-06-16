using System.Text.Json;
using Refit;
using TorneioLeft4Dead2.PlayStats.Services;

namespace TorneioLeft4Dead2.PlayStats.Context;

public static class PlayStatsContext
{
    private const string BaseUrl = "https://l4d2-playstats-api.azurewebsites.net";

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    public static IMatchesService MatchesService => CreateService<IMatchesService>();

    private static T CreateService<T>()
    {
        return RestService.For<T>(BaseUrl, Settings);
    }
}