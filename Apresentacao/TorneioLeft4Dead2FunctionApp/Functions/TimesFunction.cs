using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class TimesFunction
    {
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly IRepositorioTime _repositorioTime;
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;
        private readonly IServicoTime _servicoTime;
        private readonly IServicoTimeJogador _servicoTimeJogador;

        public TimesFunction(IServicoTime servicoTime,
            IServicoTimeJogador servicoTimeJogador,
            IRepositorioJogador repositorioJogador,
            IRepositorioTime repositorioTime,
            IRepositorioTimeJogador repositorioTimeJogador)
        {
            _servicoTime = servicoTime;
            _servicoTimeJogador = servicoTimeJogador;
            _repositorioJogador = repositorioJogador;
            _repositorioTime = repositorioTime;
            _repositorioTimeJogador = repositorioTimeJogador;
        }

        [Function(nameof(TimesFunction) + "_" + nameof(GetAll))]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times")] HttpRequestData httpRequest)
        {
            var entities = await _repositorioTime.ObterTimesAsync();

            return await httpRequest.OkAsync(entities);
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Jogadores))]
        public async Task<HttpResponseData> Jogadores([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times-jogadores")] HttpRequestData httpRequest)
        {
            var jogadores = await _repositorioJogador.ObterJogadoresAsync();
            var times = await _repositorioTime.ObterTimesAsync();
            var vinculos = await _repositorioTimeJogador.ObterTodosAsync();

            return await httpRequest.OkAsync(new
            {
                jogadores, times, vinculos
            });
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times/{codigo}")] HttpRequestData httpRequest,
            string codigo)
        {
            var entity = await _repositorioTime.ObterPorCodigoAsync(codigo);

            if (entity == null)
                return httpRequest.NotFound();

            return await httpRequest.OkAsync(entity);
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times")] HttpRequestData httpRequest)
        {
            try
            {
                var command = await httpRequest.DeserializeBodyAsync<TimeCommand>();
                var entity = await _servicoTime.SalvarAsync(command);

                return await httpRequest.OkAsync(entity);
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }

        [Function(nameof(TimesFunction) + "_" + nameof(VincularJogador))]
        public async Task<HttpResponseData> VincularJogador([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times/vincular-jogador")] HttpRequestData httpRequest)
        {
            try
            {
                var command = await httpRequest.DeserializeBodyAsync<TimeJogadorCommand>();
                var entity = await _servicoTimeJogador.SalvarAsync(command);

                return await httpRequest.OkAsync(entity);
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Delete))]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}")] HttpRequestData httpRequest,
            string codigo)
        {
            try
            {
                await _servicoTime.ExcluirAsync(codigo);

                return httpRequest.Ok();
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }

        [Function(nameof(TimesFunction) + "_" + nameof(DesvincularJogador))]
        public async Task<HttpResponseData> DesvincularJogador([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}/desvincular-jogador/{steamId}")] HttpRequestData httpRequest,
            string codigo, string steamId)
        {
            try
            {
                await _servicoTimeJogador.DesvincularJogadorAsync(codigo, steamId);

                return httpRequest.Ok();
            }
            catch (Exception exception)
            {
                return await httpRequest.BadRequestAsync(exception);
            }
        }
    }
}