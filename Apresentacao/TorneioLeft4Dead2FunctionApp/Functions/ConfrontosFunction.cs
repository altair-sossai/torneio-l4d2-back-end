using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Confrontos.Commands;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2.Shared.Constants;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class ConfrontosFunction(
    IServicoConfronto servicoConfronto,
    IAuthContext authContext,
    IMemoryCache memoryCache)
{
    [Function($"{nameof(ConfrontosFunction)}_{nameof(GetAllAsync)}")]
    public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos")] HttpRequestData httpRequest)
    {
        var models = await servicoConfronto.ObterConfrontosAsync();

        return await httpRequest.OkAsync(models);
    }

    [Function($"{nameof(ConfrontosFunction)}_{nameof(GetAsync)}")]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/{confrontoId:guid}")] HttpRequestData httpRequest,
        Guid confrontoId)
    {
        var model = await servicoConfronto.ObterPorIdAsync(confrontoId);

        if (model == null)
            return httpRequest.NotFound();

        return await httpRequest.OkAsync(model);
    }

    [Function($"{nameof(ConfrontosFunction)}_{nameof(RodadasAsync)}")]
    public async Task<HttpResponseData> RodadasAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/rodadas")] HttpRequestData httpRequest)
    {
        var models = await memoryCache.GetOrCreateAsync(MemoryCacheKeys.Confrontos, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return servicoConfronto.ObterRodadasAsync();
        });

        return await httpRequest.OkAsync(models);
    }

    [Function($"{nameof(ConfrontosFunction)}_{nameof(PostAsync)}")]
    public async Task<HttpResponseData> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<ConfrontoCommand>();
            var entity = await servicoConfronto.SalvarAsync(command);

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

    [Function($"{nameof(ConfrontosFunction)}_{nameof(GerarAsync)}")]
    public async Task<HttpResponseData> GerarAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/gerar")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoConfronto.GerarConfrontosAsync();

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

    [Function($"{nameof(ConfrontosFunction)}_{nameof(LimparCampanhasAsync)}")]
    public async Task<HttpResponseData> LimparCampanhasAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/limpar-campanhas")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoConfronto.LimparCampanhasAsync();

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

    [Function($"{nameof(ConfrontosFunction)}_{nameof(SortearCampanhasAsync)}")]
    public async Task<HttpResponseData> SortearCampanhasAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/sortear-campanhas")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var campanhas = await servicoConfronto.SortearCampanhasAsync();

            return await httpRequest.OkAsync(campanhas);
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

    [Function($"{nameof(ConfrontosFunction)}_{nameof(DeleteAsync)}")]
    public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "confrontos/{confrontoId:guid}")] HttpRequestData httpRequest,
        Guid confrontoId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoConfronto.ExcluirAsync(confrontoId);

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