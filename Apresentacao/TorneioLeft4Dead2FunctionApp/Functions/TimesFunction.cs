using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions
{
    public class TimesFunction
    {
        private readonly IRepositorioTime _repositorioTime;
        private readonly IServicoTime _servicoTime;

        public TimesFunction(IServicoTime servicoTime,
            IRepositorioTime repositorioTime)
        {
            _servicoTime = servicoTime;
            _repositorioTime = repositorioTime;
        }

        [Function(nameof(TimesFunction) + "_" + nameof(GetAll))]
        public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "times")] HttpRequestData httpRequest)
        {
            var entities = await _repositorioTime.ObterTimesAsync();

            return await httpRequest.OkAsync(entities);
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
    }
}