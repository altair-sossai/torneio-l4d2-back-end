using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Times.Servicos;

public class ServicoTimeJogador(
    IMapper mapper,
    IValidator<TimeJogadorEntity> validator,
    IRepositorioTimeJogador repositorioTimeJogador)
    : IServicoTimeJogador
{
    public async Task<TimeJogadorEntity> SalvarAsync(TimeJogadorCommand command)
    {
        var entity = mapper.Map<TimeJogadorEntity>(command);
        var jogadores = await repositorioTimeJogador.ObterPorTimeAsync(command.Time);

        entity.Ordem = jogadores.Count;

        await validator.ValidateAndThrowAsync(entity);
        await repositorioTimeJogador.SalvarAsync(entity);

        return entity;
    }

    public async Task DesvincularJogadorAsync(string codigo, string steamId)
    {
        await repositorioTimeJogador.DesvincularJogadorAsync(codigo, steamId);
    }
}