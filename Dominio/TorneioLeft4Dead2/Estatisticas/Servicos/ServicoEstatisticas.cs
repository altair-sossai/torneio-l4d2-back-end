﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Estatisticas.Models;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Playoffs.Repositorios;
using TorneioLeft4Dead2.PlayStats.Models;
using TorneioLeft4Dead2.PlayStats.Services;

namespace TorneioLeft4Dead2.Estatisticas.Servicos;

public class ServicoEstatisticas : IServicoEstatisticas
{
    private readonly IMapper _mapper;
    private readonly IMatchesService _matchesService;
    private readonly IRepositorioConfronto _repositorioConfronto;
    private readonly IRepositorioJogador _repositorioJogador;
    private readonly IRepositorioPlayoffs _repositorioPlayoffs;

    public ServicoEstatisticas(IMapper mapper,
        IRepositorioJogador repositorioJogador,
        IRepositorioConfronto repositorioConfronto,
        IRepositorioPlayoffs repositorioPlayoffs,
        IMatchesService matchesService)
    {
        _mapper = mapper;
        _repositorioJogador = repositorioJogador;
        _repositorioConfronto = repositorioConfronto;
        _repositorioPlayoffs = repositorioPlayoffs;
        _matchesService = matchesService;
    }

    public async Task<List<JogadorModel>> ObterEstatisticasAsync()
    {
        var jogadores = _mapper
            .Map<List<JogadorModel>>(await _repositorioJogador.ObterJogadoresAsync())
            .ToDictionary(d => d.SteamId, v => v);

        await foreach (var match in MatchesFase01Async())
            AtualizarDadoJogadores(match, jogadores);

        await foreach (var match in MatchesFasePlayoffAsync())
            AtualizarDadoJogadores(match, jogadores);

        return jogadores.Values.ToList();
    }

    private async IAsyncEnumerable<Match> MatchesFase01Async()
    {
        foreach (var confronto in await _repositorioConfronto.ObterConfrontosAsync())
        {
            if (string.IsNullOrEmpty(confronto.InicioEstatistica) || string.IsNullOrEmpty(confronto.FimEstatistica))
                continue;

            foreach (var match in await _matchesService.BetweenAsync(confronto.InicioEstatistica, confronto.FimEstatistica))
                yield return match;
        }
    }

    private async IAsyncEnumerable<Match> MatchesFasePlayoffAsync()
    {
        foreach (var playoff in await _repositorioPlayoffs.ObterPlayoffsAsync())
        foreach (var confronto in playoff.Confrontos)
        {
            if (string.IsNullOrEmpty(confronto.InicioEstatistica) || string.IsNullOrEmpty(confronto.FimEstatistica))
                continue;

            foreach (var match in await _matchesService.BetweenAsync(confronto.InicioEstatistica, confronto.FimEstatistica))
                yield return match;
        }
    }

    private static void AtualizarDadoJogadores(Match match, IReadOnlyDictionary<string, JogadorModel> jogadores)
    {
        foreach (var team in match.Teams)
        foreach (var player in team.Players)
        {
            if (!jogadores.ContainsKey(player.CommunityId))
                continue;

            var jogador = jogadores[player.CommunityId];

            jogador.Died += player.Died;
            jogador.Incaps += player.Incaps;
            jogador.DmgTaken += player.DmgTaken;
            jogador.Common += player.Common;
            jogador.SiKilled += player.SiKilled;
            jogador.SiDamage += player.SiDamage;
            jogador.TankDamage += player.TankDamage;
            jogador.RockEats += player.RockEats;
            jogador.WitchDamage += player.WitchDamage;
            jogador.Skeets += player.Skeets;
            jogador.Levels += player.Levels;
            jogador.Crowns += player.Crowns;
            jogador.FfGiven += player.FfGiven;
            jogador.DmgTotal += player.DmgTotal;
            jogador.DmgTank += player.DmgTank;
            jogador.DmgSpit += player.DmgSpit;
            jogador.HunterDpDmg += player.HunterDpDmg;
            jogador.MvpSiDamage += player.MvpSiDamage;
            jogador.MvpCommon += player.MvpCommon;
            jogador.LvpFfGiven += player.LvpFfGiven;
        }
    }
}