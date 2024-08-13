using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Repositorios;

namespace TorneioLeft4Dead2.DataConfronto.Servicos;

public class ServicoPeriodoConfronto(
    IMapper mapper,
    IRepositorioPeriodoConfronto repositorioPeriodoConfronto,
    IServicoSugestaoDataConfronto servicoSugestaoDataConfronto,
    IValidator<PeriodoConfrontoEntity> validator)
    : IServicoPeriodoConfronto
{
    public async Task<PeriodoConfrontoModel> ObterPorConfrontoAsync(Guid confrontoId)
    {
        var confronto = await repositorioPeriodoConfronto.ObterPorConfrontoAsync(confrontoId);
        if (confronto == null)
            return null;

        var model = mapper.Map<PeriodoConfrontoModel>(confronto);

        model.Sugestoes = await servicoSugestaoDataConfronto.ObterPorConfrontoAsync(confrontoId);

        return model;
    }

    public async Task<PeriodoConfrontoEntity> SalvarAsync(Guid confrontoId, PeriodoConfrontoCommand command)
    {
        var entity = mapper.Map<PeriodoConfrontoEntity>(command);

        entity.ConfrontoId = confrontoId;

        await validator.ValidateAndThrowAsync(entity);
        await repositorioPeriodoConfronto.SalvarAsync(entity);
        await servicoSugestaoDataConfronto.ExcluirPorConfrontoAsync(entity.ConfrontoId);
        await servicoSugestaoDataConfronto.SalvarAsync(confrontoId, command.Sugestoes);

        return entity;
    }
}