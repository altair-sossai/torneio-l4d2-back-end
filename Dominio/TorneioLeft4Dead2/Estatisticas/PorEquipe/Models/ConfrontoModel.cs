using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.PlayStats.Models;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Estatisticas.PorEquipe.Models;

public class ConfrontoModel
{
    public ConfrontoModel(Match match, TimeModel time, TimeEntity adversario)
    {
        Adversario = adversario;

        foreach (var team in match.Teams)
        foreach (var player in team.Players)
        {
            var jogador = time.Jogadores.FirstOrDefault(f => f.SteamId == player.CommunityId);
            if (jogador == null)
                continue;

            var model = Jogadores.FirstOrDefault(f => f.SteamId == player.CommunityId);
            if (model == null)
            {
                model = new JogadorModel
                {
                    SteamId = jogador.SteamId,
                    Nome = jogador.Nome,
                    UrlFotoPerfil = jogador.UrlFotoPerfil,
                    UrlPerfilSteam = jogador.UrlPerfilSteam
                };

                Jogadores.Add(model);
            }

            model.PointsMvpSiDamage += player.PointsMvpSiDamage;
            model.PointsMvpCommon += player.PointsMvpCommon;
            model.PointsLvpFfGiven += player.PointsLvpFfGiven;
        }
    }

    public TimeEntity Adversario { get; set; }
    public List<JogadorModel> Jogadores { get; set; } = new();
}