using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.GaleriaImagem.Repositorios;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class GaleriaImagemFunction
    {
        private readonly IRepositorioGaleriaImagem _repositorioGaleriaImagem;

        public GaleriaImagemFunction(IRepositorioGaleriaImagem repositorioGaleriaImagem)
        {
            _repositorioGaleriaImagem = repositorioGaleriaImagem;
        }

        [Function(nameof(GaleriaImagemFunction) + "_" + nameof(Imagens))]
        public async Task<HttpResponseData> Imagens([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "galeria-imagens/{galeriaId}/imagens")] HttpRequestData httpRequest,
            string galeriaId)
        {
            var imagens = await _repositorioGaleriaImagem.ObterImagensAsync(galeriaId).ToListAsync();

            return await httpRequest.OkAsync(imagens.OrderBy(o=>o.NomeArquivo).ToList());
        }
    }
}