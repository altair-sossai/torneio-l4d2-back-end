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

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class PlayoffsFunction
    {
        private readonly AuthContext _authContext;
        private readonly IMemoryCache _memoryCache;
        private readonly IServicoPlayoffs _servicoPlayoffs;

        public PlayoffsFunction(IServicoPlayoffs servicoPlayoffs,
            AuthContext authContext,
            IMemoryCache memoryCache)
        {
            _servicoPlayoffs = servicoPlayoffs;
            _authContext = authContext;
            _memoryCache = memoryCache;
        }

        [Function(nameof(PlayoffsFunction) + "_" + nameof(GetAll))]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs")] HttpRequestData httpRequest)
        {
            var models = await _servicoPlayoffs.ObterPlayoffsAsync();

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(PlayoffsFunction) + "_" + nameof(Get))]
        public async Task<HttpResponseData> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs/{playoffsId:guid}")] HttpRequestData httpRequest,
            Guid playoffsId)
        {
            var model = await _servicoPlayoffs.ObterPorIdAsync(playoffsId);

            if (model == null)
                return httpRequest.NotFound();

            return await httpRequest.OkAsync(model);
        }

        [Function(nameof(PlayoffsFunction) + "_" + nameof(Rodadas))]
        public async Task<HttpResponseData> Rodadas([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "playoffs/rodadas")] HttpRequestData httpRequest)
        {
            var models = await _memoryCache.GetOrCreateAsync(MemoryCacheKeys.Rodadas, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

                return _servicoPlayoffs.ObterRodadasAsync();
            });

            return await httpRequest.OkAsync(models);
        }

        [Function(nameof(PlayoffsFunction) + "_" + nameof(Post))]
        public async Task<HttpResponseData> Post([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "playoffs")] HttpRequestData httpRequest)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                var command = await httpRequest.DeserializeBodyAsync<PlayoffsCommand>();
                var entity = await _servicoPlayoffs.SalvarAsync(command);

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

        [Function(nameof(PlayoffsFunction) + "_" + nameof(Delete))]
        public async Task<HttpResponseData> Delete([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "playoffs/{playoffsId:guid}")] HttpRequestData httpRequest,
            Guid playoffsId)
        {
            try
            {
                var claimsPrincipal = httpRequest.CurrentUser();
                if (claimsPrincipal == null)
                    return httpRequest.Unauthorized();

                await _authContext.FillUserAsync(claimsPrincipal);
                _authContext.GrantPermission();

                await _servicoPlayoffs.ExcluirAsync(playoffsId);

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