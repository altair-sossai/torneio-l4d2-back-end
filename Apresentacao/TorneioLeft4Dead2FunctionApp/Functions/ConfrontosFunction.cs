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

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class ConfrontosFunction
    {
        private readonly AuthContext _authContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IServicoConfronto _servicoConfronto;

        public ConfrontosFunction(IServicoConfronto servicoConfronto,
            AuthContext authContext,
            IMemoryCache memoryCache)
        {
            _servicoConfronto = servicoConfronto;
            _authContext = authContext;
            _memoryCache = memoryCache;
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(GetAll))]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos")] HttpRequestData httpRequest)
        {
            var models = await _servicoConfronto.ObterConfrontosAsync();

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/{confrontoId:guid}")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            var model = await _servicoConfronto.ObterPorIdAsync(confrontoId);

            if (model == null)
                return httpRequest.NotFound();

            return await httpRequest.OkAsync(model);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Rodadas))]
        public async Task<HttpResponseData> Rodadas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "confrontos/rodadas")] HttpRequestData httpRequest)
        {
            var models = await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Rodadas, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

                return _servicoConfronto.ObterRodadasAsync();
            });

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos")] HttpRequestData httpRequest)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                var command = await httpRequest.DeserializeBodyAsync<ConfrontoCommand>();
                var entity = await _servicoConfronto.SalvarAsync(command);

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

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Gerar))]
        public async Task<HttpResponseData> Gerar([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "confrontos/gerar")] HttpRequestData httpRequest)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                await _servicoConfronto.GerarConfrontosAsync();

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

        [Function(nameof(ConfrontosFunction) + "_" + nameof(Delete))]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "confrontos/{confrontoId:guid}")] HttpRequestData httpRequest,
            Guid confrontoId)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                await _servicoConfronto.ExcluirAsync(confrontoId);

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