using System.Linq;
using TorneioLeft4Dead2.Steam.PlayerService.Responses;

namespace TorneioLeft4Dead2.Steam.PlayerService.Extensions
{
    public static class GetOwnedGamesResponseExtensions
    {
        public static GetOwnedGamesResponse.GameInfo Left4Dead2(this GetOwnedGamesResponse response)
        {
            return response?.Response?.Games?.FirstOrDefault(f => f.AppId == 550);
        }
    }
}