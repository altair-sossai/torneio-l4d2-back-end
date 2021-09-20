using System.Text.Json.Serialization;

namespace TorneioLeft4Dead2.Steam.SteamUser.Responses
{
    public class ResolveVanityUrlResponse
    {
        [JsonPropertyName("response")]
        public ResponseInfo Response { get; set; }

        public class ResponseInfo
        {
            [JsonPropertyName("steamid")]
            public string SteamId { get; set; }

            [JsonPropertyName("success")]
            public int Success { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}