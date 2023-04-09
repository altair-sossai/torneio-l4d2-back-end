using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Repositorios;

namespace TorneioLeft4Dead2.DataConfronto.Servicos;

public class ServicoPeriodoConfronto : IServicoPeriodoConfronto
{
    private readonly IMapper _mapper;
    private readonly IRepositorioPeriodoConfronto _repositorioPeriodoConfronto;
    private readonly IServicoSugestaoDataConfronto _servicoSugestaoDataConfronto;
    private readonly IValidator<PeriodoConfrontoEntity> _validator;

    public ServicoPeriodoConfronto(IMapper mapper,
        IRepositorioPeriodoConfronto repositorioPeriodoConfronto,
        IServicoSugestaoDataConfronto servicoSugestaoDataConfronto,
        IValidator<PeriodoConfrontoEntity> validator)
    {
        _mapper = mapper;
        _repositorioPeriodoConfronto = repositorioPeriodoConfronto;
        _servicoSugestaoDataConfronto = servicoSugestaoDataConfronto;
        _validator = validator;
    }

    public async Task<PeriodoConfrontoModel> ObterPorConfrontoAsync(Guid confrontoId)
    {
        var confronto = await _repositorioPeriodoConfronto.ObterPorConfrontoAsync(confrontoId);
        if (confronto == null)
            return null;

        var model = _mapper.Map<PeriodoConfrontoModel>(confronto);

        model.Sugestoes = await _servicoSugestaoDataConfronto.ObterPorConfrontoAsync(confrontoId);

        return model;
    }

    public async Task<PeriodoConfrontoEntity> SalvarAsync(Guid confrontoId, PeriodoConfrontoCommand command)
    {
        var entity = _mapper.Map<PeriodoConfrontoEntity>(command);

        entity.ConfrontoId = confrontoId;

        await _validator.ValidateAndThrowAsync(entity);
        await _repositorioPeriodoConfronto.SalvarAsync(entity);
        await _servicoSugestaoDataConfronto.ExcluirPorConfrontoAsync(entity.ConfrontoId);
        await _servicoSugestaoDataConfronto.SalvarAsync(confrontoId, command.Sugestoes);

        return entity;
    }
}