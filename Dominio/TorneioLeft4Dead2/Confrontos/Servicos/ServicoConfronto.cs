using System.Threading.Tasks;
using TorneioLeft4Dead2.Campanhas.Repositorios;
using TorneioLeft4Dead2.Confrontos.Builders;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Confrontos.Servicos
{
    public class ServicoConfronto : IServicoConfronto
    {
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioTime _repositorioTime;
        private readonly IRepositorioCampanha _repositorioCampanha;

        public ServicoConfronto(IRepositorioConfronto repositorioConfronto,
            IRepositorioTime repositorioTime,
            IRepositorioCampanha repositorioCampanha)
        {
            _repositorioConfronto = repositorioConfronto;
            _repositorioTime = repositorioTime;
            _repositorioCampanha = repositorioCampanha;
        }

        public async Task GerarConfrontosAsync()
        {
            var times = await _repositorioTime.ObterTimesAsync();
            var campanhas = await _repositorioCampanha.ObterCampanhasAsync();
            var builder = new ConfrontosBuilder(times, campanhas);

            await _repositorioConfronto.ExcluirTudoAsync();

            foreach (var entity in builder.Build())
                await _repositorioConfronto.SalvarAsync(entity);
        }
    }
}