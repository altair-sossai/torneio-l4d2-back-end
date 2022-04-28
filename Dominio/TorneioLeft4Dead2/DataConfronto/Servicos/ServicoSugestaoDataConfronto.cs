using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Enums;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.DataConfronto.Servicos
{
    public class ServicoSugestaoDataConfronto : IServicoSugestaoDataConfronto
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioSugestaoDataConfronto _repositorioSugestaoDataConfronto;
        private readonly IValidator<ResponderSugestaoDataCommand> _responderSugestaoDataValidator;
        private readonly IServicoTime _servicoTime;
        private readonly IValidator<NovaSugestaoDataCommand> _sugerirNovaDataValidator;
        private readonly IValidator<SugestaoDataConfrontoEntity> _validator;

        public ServicoSugestaoDataConfronto(IMapper mapper,
            IRepositorioSugestaoDataConfronto repositorioSugestaoDataConfronto,
            IRepositorioConfronto repositorioConfronto,
            IServicoTime servicoTime,
            IValidator<SugestaoDataConfrontoEntity> validator,
            IValidator<NovaSugestaoDataCommand> sugerirNovaDataValidator,
            IValidator<ResponderSugestaoDataCommand> responderSugestaoDataValidator)
        {
            _mapper = mapper;
            _repositorioSugestaoDataConfronto = repositorioSugestaoDataConfronto;
            _repositorioConfronto = repositorioConfronto;
            _servicoTime = servicoTime;
            _validator = validator;
            _sugerirNovaDataValidator = sugerirNovaDataValidator;
            _responderSugestaoDataValidator = responderSugestaoDataValidator;
        }

        public async Task<List<SugestaoDataConfrontoModel>> ObterPorConfrontoAsync(Guid confrontoId)
        {
            var sugestoes = await _repositorioSugestaoDataConfronto.ObterPorConfrontoAsync(confrontoId);
            var models = sugestoes.Select(_mapper.Map<SugestaoDataConfrontoModel>).ToList();

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
            await _sugerirNovaDataValidator.ValidateAndThrowAsync(command);

            var confronto = await _repositorioConfronto.ObterPorIdAsync(command.ConfrontoId);
            var timeA = await _servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
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
            await _responderSugestaoDataValidator.ValidateAndThrowAsync(command);

            var confronto = await _repositorioConfronto.ObterPorIdAsync(command.ConfrontoId);
            var timeA = await _servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
            var sugestao = await _repositorioSugestaoDataConfronto.ObterPorIdAsync(command.SugestaoId);

            if (command.SteamId == timeA.Capitao.SteamId)
                sugestao.RespostaTimeA = (int) command.Resposta;
            else
                sugestao.RespostaTimeB = (int) command.Resposta;

            await _repositorioSugestaoDataConfronto.SalvarAsync(sugestao);
        }

        public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
        {
            await _repositorioSugestaoDataConfronto.ExcluirPorConfrontoAsync(confrontoId);
        }

        private async Task<SugestaoDataConfrontoEntity> SalvarAsync(Guid confrontoId, SugestaoDataConfrontoCommand command)
        {
            var entity = _mapper.Map<SugestaoDataConfrontoEntity>(command);

            entity.ConfrontoId = confrontoId;

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioSugestaoDataConfronto.SalvarAsync(entity);

            return entity;
        }
    }
}