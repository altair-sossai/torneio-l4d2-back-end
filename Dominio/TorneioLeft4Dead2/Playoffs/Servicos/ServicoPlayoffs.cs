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

namespace TorneioLeft4Dead2.Playoffs.Servicos
{
    public class ServicoPlayoffs : IServicoPlayoffs
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioCampanha _repositorioCampanha;
        private readonly IRepositorioPlayoffs _repositorioPlayoffs;
        private readonly IServicoTime _servicoTime;
        private readonly IValidator<PlayoffsEntity> _validator;

        public ServicoPlayoffs(IMapper mapper,
            IValidator<PlayoffsEntity> validator,
            IRepositorioPlayoffs repositorioPlayoffs,
            IRepositorioCampanha repositorioCampanha,
            IServicoTime servicoTime)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioPlayoffs = repositorioPlayoffs;
            _repositorioCampanha = repositorioCampanha;
            _servicoTime = servicoTime;
        }

        public async Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId)
        {
            return await _repositorioPlayoffs.ObterPorIdAsync(playoffsId);
        }

        public async Task<List<RodadaModel>> ObterRodadasAsync()
        {
            var playoffs = await ObterPlayoffsAsync();

            return playoffs.Rodadas();
        }

        public async Task<List<PlayoffsModel>> ObterPlayoffsAsync()
        {
            var entities = await _repositorioPlayoffs.ObterPlayoffsAsync();
            var playoffs = _mapper.Map<List<PlayoffsModel>>(entities);

            var campanhas = await _repositorioCampanha.ObterCampanhasAsync();
            playoffs.Vincular(campanhas);

            var times = await _servicoTime.ObterTimesAsync();
            playoffs.Vincular(times);

            return playoffs;
        }

        public async Task<PlayoffsEntity> SalvarAsync(PlayoffsCommand command)
        {
            var entity = _mapper.Map<PlayoffsEntity>(command);
            entity.AtualizarDadosConfrontos();

            await SortearTerceiraCampanhaAsync(entity);

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioPlayoffs.ExcluirAsync(entity.Id);
            await _repositorioPlayoffs.SalvarAsync(entity);

            return entity;
        }

        public async Task ExcluirAsync(Guid playoffsId)
        {
            await _repositorioPlayoffs.ExcluirAsync(playoffsId);
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

            var campanhas = await _repositorioCampanha.ObterCampanhasAsync();
            campanhas.RemoverCampanhasJaEscolhidas(entity);

            var campanha = campanhas.Sortear();
            entity.Confronto03CodigoCampanha = campanha.Codigo;
        }
    }
}