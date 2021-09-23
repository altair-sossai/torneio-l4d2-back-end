using System.Linq;
using TorneioLeft4Dead2.Steam.SteamUser.Responses;

namespace TorneioLeft4Dead2.Steam.SteamUser.Extensions
{
    public static class GetPlayerSummariesResponseExtensions
    {
        public static GetPlayerSummariesResponse.PlayerInfo Player(this GetPlayerSummariesResponse response)
        {
            return response?.Response?.Players?.FirstOrDefault();
        }
    }
}