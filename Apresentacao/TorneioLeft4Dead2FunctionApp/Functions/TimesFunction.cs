using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Shared.Constants;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class TimesFunction
{
    private readonly IAuthContext _authContext;
    private readonly IMemoryCache _memoryCache;
    private readonly IServicoTime _servicoTime;
    private readonly IServicoTimeJogador _servicoTimeJogador;

    public TimesFunction(IServicoTime servicoTime,
        IServicoTimeJogador servicoTimeJogador,
        IAuthContext authContext,
        IMemoryCache memoryCache)
    {
        _servicoTime = servicoTime;
        _servicoTimeJogador = servicoTimeJogador;
        _authContext = authContext;
        _memoryCache = memoryCache;
    }

    [Function(nameof(TimesFunction) + "_" + nameof(GetAllAsync))]
    public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times")] HttpRequestData httpRequest)
    {
        var models = await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Times, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return _servicoTime.ObterTimesAsync();
        });

        return await httpRequest.OkAsync(models);
    }

    [Function(nameof(TimesFunction) + "_" + nameof(ClassificacaoAsync))]
    public async Task<HttpResponseData> ClassificacaoAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "classificacao")] HttpRequestData httpRequest)
    {
        var models = await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Classificacao, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return _servicoTime.ObterClassificacaoAsync();
        });

        return await httpRequest.OkAsync(models);
    }

    [Function(nameof(TimesFunction) + "_" + nameof(GetAsync))]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times/{codigo}")] HttpRequestData httpRequest,
        string codigo)
    {
        var model = await _servicoTime.ObterPorCodigoAsync(codigo);

        if (model == null)
            return httpRequest.NotFound();

        return await httpRequest.OkAsync(model);
    }

    [Function(nameof(TimesFunction) + "_" + nameof(PostAsync))]
    public async Task<HttpResponseData> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await _authContext.FillUserAsync(claimsPrincipal);
            _authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<TimeCommand>();
            var entity = await _servicoTime.SalvarAsync(command);

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

    [Function(nameof(TimesFunction) + "_" + nameof(VincularJogadorAsync))]
    public async Task<HttpResponseData> VincularJogadorAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times/vincular-jogador")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await _authContext.FillUserAsync(claimsPrincipal);
            _authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<TimeJogadorCommand>();
            var entity = await _servicoTimeJogador.SalvarAsync(command);

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

    [Function(nameof(TimesFunction) + "_" + nameof(DeleteAsync))]
    public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}")] HttpRequestData httpRequest,
        string codigo)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await _authContext.FillUserAsync(claimsPrincipal);
            _authContext.GrantPermission();

            await _servicoTime.ExcluirAsync(codigo);

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

    [Function(nameof(TimesFunction) + "_" + nameof(DesvincularJogadorAsync))]
    public async Task<HttpResponseData> DesvincularJogadorAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}/desvincular-jogador/{steamId}")] HttpRequestData httpRequest,
        string codigo, string steamId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await _authContext.FillUserAsync(claimsPrincipal);
            _authContext.GrantPermission();

            await _servicoTimeJogador.DesvincularJogadorAsync(codigo, steamId);

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