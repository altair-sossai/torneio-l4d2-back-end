using System.Linq;
using Microsoft.Azure.Cosmos.Table;
using TorneioLeft4Dead2.Steam.PlayerService.Responses;
using TorneioLeft4Dead2.Steam.SteamUser.Responses;

namespace TorneioLeft4Dead2.Jogadores.Entidades
{
    public class JogadorEntity : TableEntity
    {
        public JogadorEntity()
        {
            PartitionKey = "shared";
        }

        public string SteamId
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string Nome { get; set; }
        public string UrlFotoPerfil { get; set; }
        public string UrlPerfilSteam { get; set; }
        public int? TotalHoras { get; set; }

        public void Update(GetPlayerSummariesResponse response)
        {
            var player = response?.Response?.Players?.FirstOrDefault();

            Update(player);
        }

        private void Update(GetPlayerSummariesResponse.PlayerInfo player)
        {
            if (player == null)
                return;

            SteamId = player.SteamId;
            Nome = player.PersonaName;
            UrlFotoPerfil = player.AvatarFull;
            UrlPerfilSteam = player.ProfileUrl;
        }

        public void Update(GetOwnedGamesResponse response)
        {
            var gameInfo = response?.Response?.Games?.FirstOrDefault(f => f.AppId == 550);

            Update(gameInfo);
        }

        private void Update(GetOwnedGamesResponse.GameInfo gameInfo)
        {
            TotalHoras = gameInfo?.PlayTimeForever / 60;
        }
    }
}