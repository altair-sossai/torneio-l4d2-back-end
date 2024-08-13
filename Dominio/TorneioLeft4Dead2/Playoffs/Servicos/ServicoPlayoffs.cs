using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Campanhas.Extensions;
using TorneioLeft4Dead2.Campanhas.Repositorios;
using TorneioLeft4Dead2.Playoffs.Commands;
using TorneioLeft4Dead2.Playoffs.Entidades;
using TorneioLeft4Dead2.Playoffs.Extensions;
using TorneioLeft4Dead2.Playoffs.Models;
using TorneioLeft4Dead2.Playoffs.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.Playoffs.Servicos;

public class ServicoPlayoffs(
    IMapper mapper,
    IValidator<PlayoffsEntity> validator,
    IRepositorioPlayoffs repositorioPlayoffs,
    IRepositorioCampanha repositorioCampanha,
    IServicoTime servicoTime)
    : IServicoPlayoffs
{
    public async Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId)
    {
        return await repositorioPlayoffs.ObterPorIdAsync(playoffsId);
    }

    public async Task<List<RodadaModel>> ObterRodadasAsync()
    {
        var playoffs = await ObterPlayoffsAsync();

        return playoffs.Rodadas();
    }

    public async Task<List<PlayoffsModel>> ObterPlayoffsAsync()
    {
        var entities = await repositorioPlayoffs.ObterPlayoffsAsync();
        var playoffs = mapper.Map<List<PlayoffsModel>>(entities);

        var campanhas = await repositorioCampanha.ObterCampanhasAsync();
        playoffs.Vincular(campanhas);

        var times = await servicoTime.ObterTimesAsync();
        playoffs.Vincular(times);

        return playoffs;
    }

    public async Task<PlayoffsEntity> SalvarAsync(PlayoffsCommand command)
    {
        var entity = mapper.Map<PlayoffsEntity>(command);
        entity.AtualizarDadosConfrontos();

        await SortearTerceiraCampanhaAsync(entity);

        await validator.ValidateAndThrowAsync(entity);
        await repositorioPlayoffs.ExcluirAsync(entity.Id);
        await repositorioPlayoffs.SalvarAsync(entity);

        return entity;
    }

    public async Task ExcluirAsync(Guid playoffsId)
    {
        await repositorioPlayoffs.ExcluirAsync(playoffsId);
    }

    private async Task SortearTerceiraCampanhaAsync(PlayoffsEntity entity)
    {
        if (string.IsNullOrEmpty(entity.CodigoTimeA)
            || string.IsNullOrEmpty(entity.CodigoTimeB)
            || !entity.CodigoCampanhaExcluidaTimeA.HasValue
            || !entity.CodigoCampanhaExcluidaTimeB.HasValue
            || !entity.Confronto01CodigoCampanha.HasValue
            || !entity.Confronto02CodigoCampanha.HasValue
            || entity.Confronto03CodigoCampanha.HasValue)
            return;

        var campanhas = await repositorioCampanha.ObterCampanhasAsync();
        campanhas.RemoverCampanhasJaEscolhidas(entity);

        var campanha = campanhas
            .ComQuatroMapasOuMais()
            .Sortear();

        entity.Confronto03CodigoCampanha = campanha.Codigo;
    }
}