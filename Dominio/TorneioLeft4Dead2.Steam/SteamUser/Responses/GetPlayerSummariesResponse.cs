using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TorneioLeft4Dead2.Steam.SteamUser.Responses;

public class GetPlayerSummariesResponse
{
    [JsonPropertyName("response")]
    public ResponseInfo Response { get; set; }

    public class ResponseInfo
    {
        [JsonPropertyName("players")]
        public List<PlayerInfo> Players { get; set; }
    }

    public class PlayerInfo
    {
        [JsonPropertyName("steamid")]
        public string SteamId { get; set; }

        [JsonPropertyName("communityvisibilitystate")]
        public int CommunityVisibilityState { get; set; }

        [JsonPropertyName("profilestate")]
        public int ProfileState { get; set; }

        [JsonPropertyName("personaname")]
        public string PersonaName { get; set; }

        [JsonPropertyName("profileurl")]
        public string ProfileUrl { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("avatarmedium")]
        public string AvatarMedium { get; set; }

        [JsonPropertyName("avatarfull")]
        public string AvatarFull { get; set; }

        [JsonPropertyName("avatarhash")]
        public string AvatarHash { get; set; }

        [JsonPropertyName("lastlogoff")]
        public int LastLogoff { get; set; }

        [JsonPropertyName("personastate")]
        public int PersonaState { get; set; }

        [JsonPropertyName("realname")]
        public string RealName { get; set; }

        [JsonPropertyName("primaryclanid")]
        public string PrimaryClanId { get; set; }

        [JsonPropertyName("timecreated")]
        public int TimeCreated { get; set; }

        [JsonPropertyName("personastateflags")]
        public int PersonaStateFlags { get; set; }

        [JsonPropertyName("loccountrycode")]
        public string LocCountryCode { get; set; }

        [JsonPropertyName("locstatecode")]
        public string LocStateCode { get; set; }

        [JsonPropertyName("loccityid")]
        public int LocCityId { get; set; }
    }
}