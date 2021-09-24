using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TorneioLeft4Dead2.Campanhas.Repositorios;
using TorneioLeft4Dead2.Confrontos.Builders;
using TorneioLeft4Dead2.Confrontos.Extensions;
using TorneioLeft4Dead2.Confrontos.Models;
using TorneioLeft4Dead2.Confrontos.Repositorios;
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

        public ServicoConfronto(IMapper mapper,
            IRepositorioConfronto repositorioConfronto,
            IRepositorioCampanha repositorioCampanha,
            IServicoTime servicoTime,
            IRepositorioTime repositorioTime)
        {
            _mapper = mapper;
            _repositorioConfronto = repositorioConfronto;
            _repositorioCampanha = repositorioCampanha;
            _servicoTime = servicoTime;
            _repositorioTime = repositorioTime;
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

        public async Task GerarConfrontosAsync()
        {
            var times = await _repositorioTime.ObterTimesAsync();
            var builder = new ConfrontosBuilder(times);

            await _repositorioConfronto.ExcluirTudoAsync();

            foreach (var entity in builder.Build())
                await _repositorioConfronto.SalvarAsync(entity);
        }
    }
}