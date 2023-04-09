using System.Threading.Tasks;
using Refit;
using TorneioLeft4Dead2.Steam.PlayerService.Responses;

namespace TorneioLeft4Dead2.Steam.PlayerService.Services;

public interface IPlayerService
{
    [Get("/IPlayerService/GetOwnedGames/v0001")]
    Task<GetOwnedGamesResponse> GetOwnedGamesAsync([AliasAs("key")] string key, [AliasAs("steamid")] string steamId);
}