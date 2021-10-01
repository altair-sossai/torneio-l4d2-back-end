using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Campanhas.Repositorios;
using TorneioLeft4Dead2.Confrontos.Builders;
using TorneioLeft4Dead2.Confrontos.Commands;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Extensions;
using TorneioLeft4Dead2.Confrontos.Models;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Times.Extensions;
using TorneioLeft4Dead2.Times.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.Confrontos.Servicos
{
    public class ServicoConfronto : IServicoConfronto
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioCampanha _repositorioCampanha;
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioTime _repositorioTime;
        private readonly IServicoTime _servicoTime;
        private readonly IValidator<ConfrontoEntity> _validator;

        public ServicoConfronto(IMapper mapper,
            IValidator<ConfrontoEntity> validator,
            IRepositorioConfronto repositorioConfronto,
            IRepositorioCampanha repositorioCampanha,
            IServicoTime servicoTime,
            IRepositorioTime repositorioTime)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioConfronto = repositorioConfronto;
            _repositorioCampanha = repositorioCampanha;
            _servicoTime = servicoTime;
            _repositorioTime = repositorioTime;
        }

        public async Task<ConfrontoEntity> ObterPorIdAsync(Guid confrontoId)
        {
            return await _repositorioConfronto.ObterPorIdAsync(confrontoId);
        }

        public async Task<List<RodadaModel>> ObterRodadasAsync()
        {
            var confrontos = await ObterConfrontosAsync();

            return confrontos.Rodadas();
        }

        public async Task<List<ConfrontoModel>> ObterConfrontosAsync()
        {
            var entities = await _repositorioConfronto.ObterConfrontosAsync();
            var confrontos = _mapper.Map<List<ConfrontoModel>>(entities);

            var campanhas = await _repositorioCampanha.ObterCampanhasAsync();
            confrontos.Vincular(campanhas);

            var times = await _servicoTime.ObterTimesAsync();
            confrontos.Vincular(times);

            return confrontos;
        }

        public async Task<ConfrontoEntity> SalvarAsync(ConfrontoCommand command)
        {
            var entity = _mapper.Map<ConfrontoEntity>(command);

            if (entity.Status == (int) StatusConfronto.Aguardando)
                entity.ZerarPontuacao();

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioConfronto.ExcluirAsync(entity.Id);
            await _repositorioConfronto.SalvarAsync(entity);

            await AtualizarPlacarAsync();

            return entity;
        }

        public async Task GerarConfrontosAsync()
        {
            var times = await _repositorioTime.ObterTimesAsync();
            var builder = new ConfrontosBuilder(times);

            await _repositorioConfronto.ExcluirTudoAsync();

            foreach (var entity in builder.Build())
                await _repositorioConfronto.SalvarAsync(entity);
        }

        public async Task ExcluirAsync(Guid confrontoId)
        {
            await _repositorioConfronto.ExcluirAsync(confrontoId);
        }

        private async Task AtualizarPlacarAsync()
        {
            var times = (await _repositorioTime.ObterTimesAsync()).ToDictionary();
            var confrontos = await _repositorioConfronto.ObterConfrontosAsync();

            foreach (var (_, time) in times)
                time.ZerarPontuacao();

            foreach (var confronto in confrontos)
            {
                if (confronto.Status == (int) StatusConfronto.Aguardando)
                    continue;

                var timeA = times[confronto.CodigoTimeA];
                var timeB = times[confronto.CodigoTimeB];

                timeA.QuantidadePartidasRealizadas++;
                timeA.TotalPontosConquistados += confronto.PontosConquistadosTimeA;
                timeA.TotalPontosSofridos += confronto.PontosConquistadosTimeB;
                timeA.TotalPenalidades += confronto.PenalidadeTimeA;

                timeB.QuantidadePartidasRealizadas++;
                timeB.TotalPontosConquistados += confronto.PontosConquistadosTimeB;
                timeB.TotalPontosSofridos += confronto.PontosConquistadosTimeA;
                timeB.TotalPenalidades += confronto.PenalidadeTimeB;

                if (confronto.CodigoTimeVencedor == timeA.Codigo)
                {
                    timeA.QuantidadeVitorias++;
                    timeB.QuantidadeDerrotas++;
                }
                else if (confronto.CodigoTimeVencedor == timeB.Codigo)
                {
                    timeA.QuantidadeDerrotas++;
                    timeB.QuantidadeVitorias++;
                }
                else
                {
                    timeA.QuantidadeEmpates++;
                    timeB.QuantidadeEmpates++;
                }
            }

            foreach (var (_, time) in times)
                await _servicoTime.SalvarAsync(time);
        }
    }
}