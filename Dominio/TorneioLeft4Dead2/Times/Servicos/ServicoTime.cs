using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Extensions;
using TorneioLeft4Dead2.Times.Models;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Times.Servicos;

public class ServicoTime(
    IMapper mapper,
    IValidator<TimeEntity> validator,
    IRepositorioTime repositorioTime,
    IRepositorioTimeJogador repositorioTimeJogador,
    IRepositorioJogador repositorioJogador,
    IServicoJogador servicoJogador)
    : IServicoTime
{
    public async Task<TimeModel> ObterPorCodigoAsync(string codigo)
    {
        var entity = await repositorioTime.ObterPorCodigoAsync(codigo);
        if (entity == null)
            return null;

        var jogadores = await servicoJogador.ObterPorTimeAsync(codigo);
        var model = mapper.Map<TimeModel>(entity);

        model.Jogadores = jogadores;

        return model;
    }

    public async Task<List<TimeModel>> ObterTimesAsync()
    {
        var entities = await repositorioTime.ObterTimesAsync();
        if (entities == null)
            return null;

        var times = mapper.Map<List<TimeModel>>(entities);
        var jogadores = await repositorioJogador.ObterJogadoresAsync();
        var timesJogadores = await repositorioTimeJogador.ObterTodosAsync();

        times.Vincular(timesJogadores, jogadores);

        return times;
    }

    public async Task<List<TimeEntity>> ObterClassificacaoAsync()
    {
        return await repositorioTime.ObterClassificacaoAsync();
    }

    public async Task<TimeEntity> SalvarAsync(TimeEntity entity)
    {
        await validator.ValidateAndThrowAsync(entity);
        await repositorioTime.SalvarAsync(entity);

        return entity;
    }

    public async Task<TimeEntity> SalvarAsync(TimeCommand command)
    {
        var entity = await ObterPorCodigoAsync(command.Codigo) ?? new TimeEntity();

        mapper.Map(command, entity);

        return await SalvarAsync(entity);
    }

    public async Task ExcluirAsync(string codigo)
    {
        await repositorioTime.ExcluirAsync(codigo);
        await repositorioTimeJogador.ExcluirPorTimeAsync(codigo);
    }
}