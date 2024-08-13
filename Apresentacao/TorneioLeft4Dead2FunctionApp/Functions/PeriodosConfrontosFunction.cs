using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Models;
using TorneioLeft4Dead2.DataConfronto.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class PeriodosConfrontosFunction(
    IServicoPeriodoConfronto servicoPeriodoConfronto,
    IAuthContext authContext)
{
    [Function($"{nameof(PeriodosConfrontosFunction)}_{nameof(GetAsync)}")]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/{confrontoId:guid}/periodo")] HttpRequestData httpRequest,
        Guid confrontoId)
    {
        var model = await servicoPeriodoConfronto.ObterPorConfrontoAsync(confrontoId)
                    ?? PeriodoConfrontoModel.Empty(confrontoId);

        return await httpRequest.OkAsync(model);
    }

    [Function($"{nameof(PeriodosConfrontosFunction)}_{nameof(PostAsync)}")]
    public async Task<HttpResponseData> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/{confrontoId:guid}/periodo")] HttpRequestData httpRequest,
        Guid confrontoId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<PeriodoConfrontoCommand>();
            var entity = await servicoPeriodoConfronto.SalvarAsync(confrontoId, command);

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