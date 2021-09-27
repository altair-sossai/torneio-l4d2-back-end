using System;
using System.Text.Json;
using Refit;
using TorneioLeft4Dead2.Steam.PlayerService.Services;
using TorneioLeft4Dead2.Steam.SteamUser.Services;

namespace TorneioLeft4Dead2.Steam.Context
{
    public static class SteamContext
    {
        private const string BaseUrl = "https://api.steampowered.com";
        public static readonly string ApiKey = Environment.GetEnvironmentVariable("SteamApiKey");

        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        private static readonly RefitSettings Settings = new()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(Options)
        };


        public static IPlayerService PlayerService => CreateService<IPlayerService>();
        public static ISteamUserService SteamUserService => CreateService<ISteamUserService>();

        private static T CreateService<T>()
        {
            return RestService.For<T>(BaseUrl, Settings);
        }
    }
}