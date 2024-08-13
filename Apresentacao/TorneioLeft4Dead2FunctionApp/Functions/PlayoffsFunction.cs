using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Playoffs.Commands;
using TorneioLeft4Dead2.Playoffs.Servicos;
using TorneioLeft4Dead2.Shared.Constants;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class PlayoffsFunction(
    IServicoPlayoffs servicoPlayoffs,
    IAuthContext authContext,
    IMemoryCache memoryCache)
{
    [Function($"{nameof(PlayoffsFunction)}_{nameof(GetAllAsync)}")]
    public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs")] HttpRequestData httpRequest)
    {
        var models = await servicoPlayoffs.ObterPlayoffsAsync();

        return await httpRequest.OkAsync(models);
    }

    [Function($"{nameof(PlayoffsFunction)}_{nameof(GetAsync)}")]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs/{playoffsId:guid}")] HttpRequestData httpRequest,
        Guid playoffsId)
    {
        var model = await servicoPlayoffs.ObterPorIdAsync(playoffsId);

        if (model == null)
            return httpRequest.NotFound();

        return await httpRequest.OkAsync(model);
    }

    [Function($"{nameof(PlayoffsFunction)}_{nameof(RodadasAsync)}")]
    public async Task<HttpResponseData> RodadasAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs/rodadas")] HttpRequestData httpRequest)
    {
        var models = await memoryCache.GetOrCreateAsync(MemoryCacheKeys.Playoffs, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return servicoPlayoffs.ObterRodadasAsync();
        });

        return await httpRequest.OkAsync(models);
    }

    [Function($"{nameof(PlayoffsFunction)}_{nameof(PostAsync)}")]
    public async Task<HttpResponseData> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "playoffs")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<PlayoffsCommand>();
            var entity = await servicoPlayoffs.SalvarAsync(command);

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

    [Function($"{nameof(PlayoffsFunction)}_{nameof(DeleteAsync)}")]
    public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "playoffs/{playoffsId:guid}")] HttpRequestData httpRequest,
        Guid playoffsId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoPlayoffs.ExcluirAsync(playoffsId);

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