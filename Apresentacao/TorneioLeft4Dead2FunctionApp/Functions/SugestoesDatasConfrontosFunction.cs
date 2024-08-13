using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Confrontos.Servicos;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Servicos;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class SugestoesDatasConfrontosFunction(
    IServicoSenhaJogador servicoSenhaJogador,
    IServicoSugestaoDataConfronto servicoSugestaoDataConfronto,
    IServicoConfronto servicoConfronto)
{
    [Function($"{nameof(SugestoesDatasConfrontosFunction)}_{nameof(SugerirNovaDataAsync)}")]
    public async Task<HttpResponseData> SugerirNovaDataAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/{confrontoId:guid}/sugestao-data")] HttpRequestData httpRequest,
        Guid confrontoId)
    {
        try
        {
            var currentUser = httpRequest.BuildAutenticarJogadorCommand();
            if (currentUser == null || !await servicoSenhaJogador.AutenticadoAsync(currentUser))
                return httpRequest.Unauthorized();

            var command = await httpRequest.DeserializeBodyAsync<NovaSugestaoDataCommand>();

            command.SteamId = currentUser.SteamId;
            command.ConfrontoId = confrontoId;

            await servicoSugestaoDataConfronto.SugerirNovaDataAsync(command);
            await servicoConfronto.AgendarConfrontoAsync(confrontoId);

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

    [Function($"{nameof(SugestoesDatasConfrontosFunction)}_{nameof(ResponderSugestaoDataAsync)}")]
    public async Task<HttpResponseData> ResponderSugestaoDataAsync([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "confrontos/{confrontoId:guid}/sugestao-data/{sugestaoId}/responder")] HttpRequestData httpRequest,
        Guid confrontoId, Guid sugestaoId)
    {
        try
        {
            var currentUser = httpRequest.BuildAutenticarJogadorCommand();
            if (currentUser == null || !await servicoSenhaJogador.AutenticadoAsync(currentUser))
                return httpRequest.Unauthorized();

            var command = await httpRequest.DeserializeBodyAsync<ResponderSugestaoDataCommand>();

            command.ConfrontoId = confrontoId;
            command.SugestaoId = sugestaoId;
            command.SteamId = currentUser.SteamId;

            await servicoSugestaoDataConfronto.ResponderSugestaoDataAsync(command);
            await servicoConfronto.AgendarConfrontoAsync(confrontoId);

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

    [Function($"{nameof(SugestoesDatasConfrontosFunction)}_{nameof(ExcluirSugestaoDataAsync)}")]
    public async Task<HttpResponseData> ExcluirSugestaoDataAsync([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "confrontos/{confrontoId:guid}/sugestao-data/{sugestaoId}")] HttpRequestData httpRequest,
        Guid confrontoId, Guid sugestaoId)
    {
        try
        {
            var currentUser = httpRequest.BuildAutenticarJogadorCommand();
            if (currentUser == null || !await servicoSenhaJogador.AutenticadoAsync(currentUser))
                return httpRequest.Unauthorized();

            await servicoSugestaoDataConfronto.ExcluirSugestaoDataAsync(confrontoId, sugestaoId, currentUser.SteamId);
            await servicoConfronto.AgendarConfrontoAsync(confrontoId);

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