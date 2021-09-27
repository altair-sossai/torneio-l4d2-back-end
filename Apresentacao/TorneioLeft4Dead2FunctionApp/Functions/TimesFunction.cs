using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Auth.Context;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class TimesFunction
    {
        private readonly AuthContext _authContext;
        private readonly IServicoTime _servicoTime;
        private readonly IServicoTimeJogador _servicoTimeJogador;

        public TimesFunction(IServicoTime servicoTime,
            IServicoTimeJogador servicoTimeJogador,
            AuthContext authContext)
        {
            _servicoTime = servicoTime;
            _servicoTimeJogador = servicoTimeJogador;
            _authContext = authContext;
        }

        [Function(nameof(TimesFunction) + "_" + nameof(GetAll))]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times")] HttpRequestData httpRequest)
        {
            var models = await _servicoTime.ObterTimesAsync();

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times/{codigo}")] HttpRequestData httpRequest,
            string codigo)
        {
            var model = await _servicoTime.ObterPorCodigoAsync(codigo);

            if (model == null)
                return httpRequest.NotFound();

            return await httpRequest.OkAsync(model);
        }

        [Function(nameof(TimesFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times")] HttpRequestData httpRequest)
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

        [Function(nameof(TimesFunction) + "_" + nameof(VincularJogador))]
        public async Task<HttpResponseData> VincularJogador([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "times/vincular-jogador")] HttpRequestData httpRequest)
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

        [Function(nameof(TimesFunction) + "_" + nameof(Delete))]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}")] HttpRequestData httpRequest,
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

        [Function(nameof(TimesFunction) + "_" + nameof(DesvincularJogador))]
        public async Task<HttpResponseData> DesvincularJogador([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "times/{codigo}/desvincular-jogador/{steamId}")] HttpRequestData httpRequest,
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
}