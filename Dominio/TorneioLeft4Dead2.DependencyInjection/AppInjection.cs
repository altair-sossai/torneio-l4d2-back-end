using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TorneioLeft4Dead2.PlayStats.Context;
using TorneioLeft4Dead2.Steam.Context;

namespace TorneioLeft4Dead2.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection services)
    {
        var assemblies = new[]
        {
            Assembly.Load("TorneioLeft4Dead2"),
            Assembly.Load("TorneioLeft4Dead2.Storage")
        };

        services.AddMemoryCache();
        services.AddAutoMapper(assemblies);
        services.AddValidatorsFromAssemblies(assemblies);

        services.Scan(scan => scan.FromAssemblies(assemblies).AddClasses().AsImplementedInterfaces());

        services.AddScoped(_ => SteamContext.PlayerService);
        services.AddScoped(_ => SteamContext.SteamUserService);

        services.AddScoped(_ => PlayStatsContext.MatchesService);
    }
}