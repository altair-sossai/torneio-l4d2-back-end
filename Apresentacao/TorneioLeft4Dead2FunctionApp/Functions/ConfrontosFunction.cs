using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class ConfrontosFunction
    {
        private readonly IServicoConfronto _servicoConfronto;

        public ConfrontosFunction(IServicoConfronto servicoConfronto)
        {
            _servicoConfronto = servicoConfronto;
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos")] HttpRequestData httpRequest)
        {
            var models = await _servicoConfronto.ObterConfrontosAsync();

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Rodadas))]
        public async Task<HttpResponseData> Rodadas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/rodadas")] HttpRequestData httpRequest)
        {
            var models = await _servicoConfronto.ObterRodadasAsync();

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Gerar))]
        public async Task<HttpResponseData> Gerar([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/gerar")] HttpRequestData httpRequest)
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