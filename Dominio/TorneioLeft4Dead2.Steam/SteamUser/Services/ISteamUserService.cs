using System.Threading.Tasks;
using Refit;
using TorneioLeft4Dead2.Steam.SteamUser.Responses;

namespace TorneioLeft4Dead2.Steam.SteamUser.Services
{
    public interface ISteamUserService
    {
        [Get("/ISteamUser/ResolveVanityURL/v0001")]
        Task<ResolveVanityUrlResponse> ResolveVanityUrlAsync([AliasAs("key")] string key, [AliasAs("vanityurl")] string vanityUrl);

        [Get("/ISteamUser/GetPlayerSummaries/v0002")]
        Task<GetPlayerSummariesResponse> GetPlayerSummariesAsync([AliasAs("key")] string key, [AliasAs("steamids")] string steamIds);
    }
}