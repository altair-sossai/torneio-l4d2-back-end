using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Steam.PlayerService.Extensions;
using TorneioLeft4Dead2.Steam.PlayerService.Responses;
using TorneioLeft4Dead2.Steam.SteamUser.Extensions;
using TorneioLeft4Dead2.Steam.SteamUser.Responses;

namespace TorneioLeft4Dead2.Jogadores.Extensions
{
    public static class JogadorEntityExtensions
    {
        public static Dictionary<string, JogadorEntity> ToDictionary(this IEnumerable<JogadorEntity> entities)
        {
            return entities.ToDictionary(k => k.SteamId, v => v);
        }

        public static void Update(this JogadorEntity entity, GetPlayerSummariesResponse response)
        {
            var player = response?.Player();

            entity?.Update(player);
        }

        private static void Update(this JogadorEntity entity, GetPlayerSummariesResponse.PlayerInfo player)
        {
            if (entity == null || player == null)
                return;

            entity.SteamId = player.SteamId;
            entity.Nome = player.PersonaName;
            entity.UrlFotoPerfil = player.AvatarFull;
            entity.UrlPerfilSteam = player.ProfileUrl;
        }

        public static void Update(this JogadorEntity entity, GetOwnedGamesResponse response)
        {
            var gameInfo = response?.Left4Dead2();

            entity.Update(gameInfo);
        }

        private static void Update(this JogadorEntity entity, GetOwnedGamesResponse.GameInfo gameInfo)
        {
            if (entity == null || gameInfo == null)
                return;

            entity.TotalHoras = gameInfo.PlayTimeForever / 60;
        }
    }
}