using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Enums;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.DataConfronto.Servicos;

public class ServicoSugestaoDataConfronto(
    IMapper mapper,
    IRepositorioSugestaoDataConfronto repositorioSugestaoDataConfronto,
    IRepositorioConfronto repositorioConfronto,
    IServicoTime servicoTime,
    IValidator<SugestaoDataConfrontoEntity> validator,
    IValidator<NovaSugestaoDataCommand> sugerirNovaDataValidator,
    IValidator<ResponderSugestaoDataCommand> responderSugestaoDataValidator)
    : IServicoSugestaoDataConfronto
{
    public async Task<List<SugestaoDataConfrontoModel>> ObterPorConfrontoAsync(Guid confrontoId)
    {
        var sugestoes = await repositorioSugestaoDataConfronto.ObterPorConfrontoAsync(confrontoId);
        var models = sugestoes.Select(mapper.Map<SugestaoDataConfrontoModel>).ToList();

        return models;
    }

    public async Task<List<SugestaoDataConfrontoEntity>> SalvarAsync(Guid confrontoId, List<SugestaoDataConfrontoCommand> commands)
    {
        var entities = new List<SugestaoDataConfrontoEntity>();

        foreach (var command in commands)
        {
            var entity = await SalvarAsync(confrontoId, command);

            entities.Add(entity);
        }

        return entities;
    }

    public async Task SugerirNovaDataAsync(NovaSugestaoDataCommand command)
    {
        await sugerirNovaDataValidator.ValidateAndThrowAsync(command);

        var confronto = await repositorioConfronto.ObterPorIdAsync(command.ConfrontoId);
        var timeA = await servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
        var cadastradoPor = command.SteamId == timeA.Capitao.SteamId ? CadastradoPor.TimeA : CadastradoPor.TimeB;

        var sugestaoCommand = new SugestaoDataConfrontoCommand
        {
            Data = command.Data,
            CadastradoPor = cadastradoPor,
            RespostaTimeA = cadastradoPor == CadastradoPor.TimeA ? RespostaTime.Aceitou : RespostaTime.SemResposta,
            RespostaTimeB = cadastradoPor == CadastradoPor.TimeB ? RespostaTime.Aceitou : RespostaTime.SemResposta
        };

        await SalvarAsync(command.ConfrontoId, sugestaoCommand);
    }

    public async Task ResponderSugestaoDataAsync(ResponderSugestaoDataCommand command)
    {
        await responderSugestaoDataValidator.ValidateAndThrowAsync(command);

        var confronto = await repositorioConfronto.ObterPorIdAsync(command.ConfrontoId);
        var timeA = await servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
        var sugestao = await repositorioSugestaoDataConfronto.ObterPorIdAsync(command.SugestaoId);

        if (command.SteamId == timeA.Capitao.SteamId)
            sugestao.RespostaTimeA = (int)command.Resposta;
        else
            sugestao.RespostaTimeB = (int)command.Resposta;

        await repositorioSugestaoDataConfronto.SalvarAsync(sugestao);
    }

    public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
    {
        await repositorioSugestaoDataConfronto.ExcluirPorConfrontoAsync(confrontoId);
    }

    public async Task ExcluirSugestaoDataAsync(Guid confrontoId, Guid sugestaoId, string steamId)
    {
        var confronto = await repositorioConfronto.ObterPorIdAsync(confrontoId);
        if (confronto is not { Status: (int)StatusConfronto.Aguardando })
            return;

        var timeA = await servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
        var timeB = await servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeB);
        if (timeA.Capitao.SteamId != steamId && timeB.Capitao.SteamId != steamId)
            return;

        var sugestao = await repositorioSugestaoDataConfronto.ObterPorIdAsync(sugestaoId);

        if (sugestao.ConfrontoId != confrontoId || sugestao.CadastradoPor == (int)CadastradoPor.Administrador)
            return;

        if (timeA.Capitao.SteamId == steamId && sugestao.CadastradoPor != (int)CadastradoPor.TimeA)
            return;

        if (timeB.Capitao.SteamId == steamId && sugestao.CadastradoPor != (int)CadastradoPor.TimeB)
            return;

        await repositorioSugestaoDataConfronto.ExcluirPorIdAsync(sugestaoId);
    }

    private async Task<SugestaoDataConfrontoEntity> SalvarAsync(Guid confrontoId, SugestaoDataConfrontoCommand command)
    {
        var entity = mapper.Map<SugestaoDataConfrontoEntity>(command);

        entity.ConfrontoId = confrontoId;

        await validator.ValidateAndThrowAsync(entity);
        await repositorioSugestaoDataConfronto.SalvarAsync(entity);

        return entity;
    }
}