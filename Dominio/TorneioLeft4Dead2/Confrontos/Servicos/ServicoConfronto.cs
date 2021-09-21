using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Builders;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Confrontos.Servicos
{
    public class ServicoConfronto : IServicoConfronto
    {
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioTime _repositorioTime;

        public ServicoConfronto(IRepositorioConfronto repositorioConfronto,
            IRepositorioTime repositorioTime)
        {
            _repositorioConfronto = repositorioConfronto;
            _repositorioTime = repositorioTime;
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