using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TorneioLeft4Dead2.Steam.PlayerService.Responses;

public class GetOwnedGamesResponse
{
    [JsonPropertyName("response")]
    public ResponseInfo Response { get; set; }

    public class ResponseInfo
    {
        [JsonPropertyName("game_count")]
        public int GameCount { get; set; }

        [JsonPropertyName("games")]
        public List<GameInfo> Games { get; set; }
    }

    public class GameInfo
    {
        [JsonPropertyName("appid")]
        public int AppId { get; set; }

        [JsonPropertyName("playtime_2weeks")]
        public int PlayTime2Weeks { get; set; }

        [JsonPropertyName("playtime_forever")]
        public int PlayTimeForever { get; set; }

        [JsonPropertyName("playtime_windows_forever")]
        public int PlayTimeWindowsForever { get; set; }

        [JsonPropertyName("playtime_mac_forever")]
        public int PlayTimeMacForever { get; set; }

        [JsonPropertyName("playtime_linux_forever")]
        public int PlayTimeLinuxForever { get; set; }
    }
}