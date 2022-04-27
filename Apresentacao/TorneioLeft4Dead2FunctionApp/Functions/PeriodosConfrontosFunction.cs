using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class PeriodosConfrontosFunction
    {
        private readonly AuthContext _authContext;
        private readonly IServicoPeriodoConfronto _servicoPeriodoConfronto;

        public PeriodosConfrontosFunction(IServicoPeriodoConfronto servicoPeriodoConfronto,
            AuthContext authContext)
        {
            _servicoPeriodoConfronto = servicoPeriodoConfronto;
            _authContext = authContext;
        }

        [Function(nameof(PeriodosConfrontosFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/{confrontoId:guid}/periodo")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            var model = await _servicoPeriodoConfronto.ObterPorConfrontoAsync(confrontoId)
                        ?? PeriodoConfrontoModel.Empty(confrontoId);

            return await httpRequest.OkAsync(model);
        }

        [Function(nameof(PeriodosConfrontosFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/{confrontoId:guid}/periodo")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                var command = await httpRequest.DeserializeBodyAsync<PeriodoConfrontoCommand>();
                var entity = await _servicoPeriodoConfronto.SalvarAsync(confrontoId, command);

                return await httpRequest.OkAsync(entity);
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