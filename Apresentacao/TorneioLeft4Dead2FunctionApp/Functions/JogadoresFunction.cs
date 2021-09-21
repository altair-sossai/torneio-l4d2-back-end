using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2.Times.Repositorios;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class JogadoresFunction
    {
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;
        private readonly IServicoJogador _servicoJogador;

        public JogadoresFunction(IServicoJogador servicoJogador,
            IRepositorioJogador repositorioJogador,
            IRepositorioTimeJogador repositorioTimeJogador)
        {
            _servicoJogador = servicoJogador;
            _repositorioJogador = repositorioJogador;
            _repositorioTimeJogador = repositorioTimeJogador;
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
            var jogadores = await _repositorioJogador.ObterJogadoresAsync();
            var vinculos = await _repositorioTimeJogador.ObterTodosAsync();
            var indisponiveis = vinculos.Select(v => v.Jogador).ToHashSet();
            var disponiveis = jogadores.Where(jogador => !indisponiveis.Contains(jogador.SteamId)).ToList();

            return await httpRequest.OkAsync(disponiveis);
        }

        [Function(nameof(JogadoresFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "jogadores")] HttpRequestData httpRequest)
        {
            try
            {
                var command = await httpRequest.DeserializeBodyAsync<JogadorCommand>();
                var entity = await _servicoJogador.SalvarAsync(command);

                return await httpRequest.OkAsync(entity);
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
                await _repositorioJogador.ExcluirAsync(steamId);

                return httpRequest.Ok();
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }
    }
}