using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class ConfrontosFunction
    {
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IServicoConfronto _servicoConfronto;

        public ConfrontosFunction(IServicoConfronto servicoConfronto,
            IRepositorioConfronto repositorioConfronto)
        {
            _servicoConfronto = servicoConfronto;
            _repositorioConfronto = repositorioConfronto;
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos")] HttpRequestData httpRequest)
        {
            var entities = await _repositorioConfronto.ObterConfrontosAsync();

            return await httpRequest.OkAsync(entities);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(GerarConfrontos))]
        public async Task<HttpResponseData> GerarConfrontos([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "gerar-confrontos")] HttpRequestData httpRequest)
        {
            try
            {
                await _servicoConfronto.GerarConfrontosAsync();

                return httpRequest.Ok();
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }
    }
}