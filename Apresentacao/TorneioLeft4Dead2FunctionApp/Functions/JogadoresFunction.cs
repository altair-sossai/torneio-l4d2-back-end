using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2.Shared.Constants;
using TorneioLeft4Dead2.Times.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class JogadoresFunction(
    IServicoJogador servicoJogador,
    IServicoSenhaJogador servicoSenhaJogador,
    IServicoTime servicoTime,
    IRepositorioJogador repositorioJogador,
    IAuthContext authContext,
    IMemoryCache memoryCache)
{
    [Function($"{nameof(JogadoresFunction)}_{nameof(GetAsync)}")]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores")] HttpRequestData httpRequest)
    {
        var entities = await memoryCache.GetOrCreateAsync(MemoryCacheKeys.Jogadores, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            return repositorioJogador.ObterJogadoresAsync();
        });

        return await httpRequest.OkAsync(entities);
    }

    [Function($"{nameof(JogadoresFunction)}_{nameof(DisponiveisAsync)}")]
    public async Task<HttpResponseData> DisponiveisAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores/disponiveis")] HttpRequestData httpRequest)
    {
        var entities = await servicoJogador.JogadoresDisponiveisAsync();

        return await httpRequest.OkAsync(entities);
    }

    [Function($"{nameof(JogadoresFunction)}_{nameof(CapitaesAsync)}")]
    public async Task<HttpResponseData> CapitaesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores/capitaes")] HttpRequestData httpRequest)
    {
        var times = await servicoTime.ObterTimesAsync();
        var capitaes = times.Select(t => t.Capitao).Where(capitao => capitao != null).ToList();

        return await httpRequest.OkAsync(capitaes);
    }

    [Function($"{nameof(JogadoresFunction)}_{nameof(SuportesAsync)}")]
    public async Task<HttpResponseData> SuportesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores/suportes")] HttpRequestData httpRequest)
    {
        var times = await servicoTime.ObterTimesAsync();
        var suportes = times.Select(t => t.Suporte).Where(suporte => suporte != null).ToList();

        return await httpRequest.OkAsync(suportes);
    }

    [Function($"{nameof(JogadoresFunction)}_{nameof(PostAsync)}")]
    public async Task<HttpResponseData> PostAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jogadores")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var command = await httpRequest.DeserializeBodyAsync<JogadorCommand>();
            var entity = await servicoJogador.SalvarAsync(command);

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

    [Function($"{nameof(JogadoresFunction)}_{nameof(SortearCapitaesAsync)}")]
    public async Task<HttpResponseData> SortearCapitaesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "jogadores/sortear-capitaes")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoJogador.SortearCapitaesAsync();

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

    [Function($"{nameof(JogadoresFunction)}_{nameof(SortearSuportesAsync)}")]
    public async Task<HttpResponseData> SortearSuportesAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "jogadores/sortear-suportes")] HttpRequestData httpRequest)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoJogador.SortearSuportesAsync();

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

    [Function($"{nameof(JogadoresFunction)}_{nameof(GerarSenhaAsync)}")]
    public async Task<HttpResponseData> GerarSenhaAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jogadores/{steamId}/gerar-senha")] HttpRequestData httpRequest,
        string steamId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            var model = await servicoSenhaJogador.GerarSenhaAsync(steamId);

            return await httpRequest.OkAsync(model);
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

    [Function($"{nameof(JogadoresFunction)}_{nameof(VerificarAutenticacaoAsync)}")]
    public async Task<HttpResponseData> VerificarAutenticacaoAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jogadores/verificar-autenticacao")] HttpRequestData httpRequest)
    {
        try
        {
            var command = await httpRequest.DeserializeBodyAsync<AutenticarJogadorCommand>();
            if (command == null)
                return httpRequest.Unauthorized();

            var autenticado = await servicoSenhaJogador.AutenticadoAsync(command);

            return await httpRequest.OkAsync(new { autenticado });
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

    [Function($"{nameof(JogadoresFunction)}_{nameof(DeleteAsync)}")]
    public async Task<HttpResponseData> DeleteAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "jogadores/{steamId}")] HttpRequestData httpRequest,
        string steamId)
    {
        try
        {
            var claimsPrincipal = httpRequest.CurrentUser();
            if (claimsPrincipal == null)
                return httpRequest.Unauthorized();

            await authContext.FillUserAsync(claimsPrincipal);
            authContext.GrantPermission();

            await servicoJogador.ExcluirAsync(steamId);

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

#if !DEBUG
    [Function(nameof(JogadoresFunction) + "_" + nameof(AtualizarJogadoresAsync))]
    public async Task AtualizarJogadoresAsync([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo)
    {
        await _servicoJogador.AtualizarJogadoresAsync();
    }
#endif
}