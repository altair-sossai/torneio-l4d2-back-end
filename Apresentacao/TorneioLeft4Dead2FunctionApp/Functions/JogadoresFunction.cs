using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class JogadoresFunction
    {
        private readonly AuthContext _authContext;
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly IServicoJogador _servicoJogador;

        public JogadoresFunction(IServicoJogador servicoJogador,
            IRepositorioJogador repositorioJogador,
            AuthContext authContext)
        {
            _servicoJogador = servicoJogador;
            _repositorioJogador = repositorioJogador;
            _authContext = authContext;
        }

        [Function(nameof(JogadoresFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores")] HttpRequestData httpRequest)
        {
            var entities = await _repositorioJogador.ObterJogadoresAsync();

            return await httpRequest.OkAsync(entities);
        }

        [Function(nameof(JogadoresFunction) + "_" + nameof(Disponiveis))]
        public async Task<HttpResponseData> Disponiveis([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "jogadores/disponiveis")] HttpRequestData httpRequest)
        {
            var entities = await _servicoJogador.JogadoresDisponiveisAsync();

            return await httpRequest.OkAsync(entities);
        }

        [Function(nameof(JogadoresFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jogadores")] HttpRequestData httpRequest)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                var command = await httpRequest.DeserializeBodyAsync<JogadorCommand>();
                var entity = await _servicoJogador.SalvarAsync(command);

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

        [Function(nameof(JogadoresFunction) + "_" + nameof(Delete))]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "jogadores/{steamId}")] HttpRequestData httpRequest,
            string steamId)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                await _servicoJogador.ExcluirAsync(steamId);

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

        [Function(nameof(JogadoresFunction) + "_" + nameof(AtualizarJogadores))]
        public async void AtualizarJogadores([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo)
        {
            await _servicoJogador.AtualizarJogadoresAsync();
        }
    }
}