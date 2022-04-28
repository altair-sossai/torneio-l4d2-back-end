using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Servicos;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class SugestoesDatasConfrontosFunction
    {
        private readonly IServicoSenhaJogador _servicoSenhaJogador;
        private readonly IServicoSugestaoDataConfronto _servicoSugestaoDataConfronto;

        public SugestoesDatasConfrontosFunction(IServicoSenhaJogador servicoSenhaJogador,
            IServicoSugestaoDataConfronto servicoSugestaoDataConfronto)
        {
            _servicoSenhaJogador = servicoSenhaJogador;
            _servicoSugestaoDataConfronto = servicoSugestaoDataConfronto;
        }

        [Function(nameof(SugestoesDatasConfrontosFunction) + "_" + nameof(SugerirNovaData))]
        public async Task<HttpResponseData> SugerirNovaData([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/{confrontoId:guid}/sugerir-nova-data")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            try
            {
                var currentUser = httpRequest.BuildAutenticarJogadorCommand();
                if (currentUser == null || !await _servicoSenhaJogador.AutenticadoAsync(currentUser))
                    return httpRequest.Unauthorized();

                var command = await httpRequest.DeserializeBodyAsync<NovaSugestaoDataCommand>();

                command.SteamId = currentUser.SteamId;
                command.ConfrontoId = confrontoId;

                await _servicoSugestaoDataConfronto.SugerirNovaDataAsync(command);

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

        [Function(nameof(SugestoesDatasConfrontosFunction) + "_" + nameof(ResponderSugestaoData))]
        public async Task<HttpResponseData> ResponderSugestaoData([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/{confrontoId:guid}/responder-sugestao-data")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            try
            {
                var currentUser = httpRequest.BuildAutenticarJogadorCommand();
                if (currentUser == null || !await _servicoSenhaJogador.AutenticadoAsync(currentUser))
                    return httpRequest.Unauthorized();

                var command = await httpRequest.DeserializeBodyAsync<ResponderSugestaoDataCommand>();

                command.SteamId = currentUser.SteamId;
                command.ConfrontoId = confrontoId;

                await _servicoSugestaoDataConfronto.ResponderSugestaoDataAsync(command);

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