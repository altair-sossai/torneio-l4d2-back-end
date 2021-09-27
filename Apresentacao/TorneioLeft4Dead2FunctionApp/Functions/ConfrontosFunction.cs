using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class ConfrontosFunction
    {
        private readonly AuthContext _authContext;
        private readonly IServicoConfronto _servicoConfronto;

        public ConfrontosFunction(IServicoConfronto servicoConfronto,
            AuthContext authContext)
        {
            _servicoConfronto = servicoConfronto;
            _authContext = authContext;
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
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                await _servicoConfronto.GerarConfrontosAsync();

                return httpRequest.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return httpRequest.Unauthorized();
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }
    }
}