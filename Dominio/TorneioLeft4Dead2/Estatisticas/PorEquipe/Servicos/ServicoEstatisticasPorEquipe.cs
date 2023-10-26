﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2.Estatisticas.PorEquipe.Models;
using TorneioLeft4Dead2.Playoffs.Servicos;
using TorneioLeft4Dead2.PlayStats.Services;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.Estatisticas.PorEquipe.Servicos;

public class ServicoEstatisticasPorEquipe : IServicoEstatisticasPorEquipe
{
    private readonly IMatchesService _matchesService;
    private readonly IServicoConfronto _servicoConfronto;
    private readonly IServicoPlayoffs _servicoPlayoffs;
    private readonly IServicoTime _servicoTime;

    public ServicoEstatisticasPorEquipe(IServicoTime servicoTime,
        IServicoConfronto servicoConfronto,
        IServicoPlayoffs servicoPlayoffs,
        IMatchesService matchesService)
    {
        _servicoTime = servicoTime;
        _servicoConfronto = servicoConfronto;
        _servicoPlayoffs = servicoPlayoffs;
        _matchesService = matchesService;
    }

    public async Task<List<EquipeModel>> ObterEstatisticasAsync()
    {
        var equipes = new List<EquipeModel>();

        foreach (var time in await _servicoTime.ObterTimesAsync())
            equipes.Add(new EquipeModel(time));

        foreach (var rodada in await _servicoConfronto.ObterRodadasAsync())
        foreach (var confronto in rodada.Confrontos)
        {
            if (confronto.Status != (int)StatusConfronto.Realizado || string.IsNullOrEmpty(confronto.InicioEstatistica) || string.IsNullOrEmpty(confronto.FimEstatistica))
                continue;

            await VincularConfrontosAsync(equipes, confronto.CodigoTimeA, confronto.CodigoTimeB, confronto.InicioEstatistica, confronto.FimEstatistica);
        }

        foreach (var rodada in await _servicoPlayoffs.ObterRodadasAsync())
        foreach (var playoff in rodada.Playoffs)
        foreach (var confronto in playoff.Confrontos)
        {
            if (confronto.Status != StatusConfronto.Realizado || string.IsNullOrEmpty(confronto.InicioEstatistica) || string.IsNullOrEmpty(confronto.FimEstatistica))
                continue;

            await VincularConfrontosAsync(equipes, playoff.CodigoTimeA, playoff.CodigoTimeB, confronto.InicioEstatistica, confronto.FimEstatistica);
        }

        return equipes;
    }

    private async Task VincularConfrontosAsync(IReadOnlyCollection<EquipeModel> equipes, string timeA, string timeB, string inicioEstatistica, string fimEstatistica)
    {
        var matches = await _matchesService.BetweenAsync(inicioEstatistica, fimEstatistica);

        foreach (var match in matches)
        {
            var equipeA = equipes.First(f => f.Time.Codigo == timeA);
            var equipeB = equipes.First(f => f.Time.Codigo == timeB);

            equipeA.AddConfronto(match, equipeB.Time);
            equipeB.AddConfronto(match, equipeA.Time);
        }
    }
}