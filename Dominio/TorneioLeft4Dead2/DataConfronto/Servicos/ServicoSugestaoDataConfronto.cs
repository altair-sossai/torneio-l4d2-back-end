using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Repositorios;

namespace TorneioLeft4Dead2.DataConfronto.Servicos
{
    public class ServicoSugestaoDataConfronto : IServicoSugestaoDataConfronto
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioSugestaoDataConfronto _repositorioSugestaoDataConfronto;
        private readonly IValidator<SugestaoDataConfrontoEntity> _validator;

        public ServicoSugestaoDataConfronto(IMapper mapper,
            IRepositorioSugestaoDataConfronto repositorioSugestaoDataConfronto,
            IValidator<SugestaoDataConfrontoEntity> validator)
        {
            _mapper = mapper;
            _repositorioSugestaoDataConfronto = repositorioSugestaoDataConfronto;
            _validator = validator;
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

        public async Task<SugestaoDataConfrontoEntity> SalvarAsync(Guid confrontoId, SugestaoDataConfrontoCommand command)
        {
            var entity = _mapper.Map<SugestaoDataConfrontoEntity>(command);

            entity.ConfrontoId = confrontoId;

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioSugestaoDataConfronto.SalvarAsync(entity);

            return entity;
        }

        public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
        {
            await _repositorioSugestaoDataConfronto.ExcluirPorConfrontoAsync(confrontoId);
        }
    }
}