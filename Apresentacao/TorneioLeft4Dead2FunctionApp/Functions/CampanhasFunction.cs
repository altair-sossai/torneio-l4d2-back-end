using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Campanhas.Repositorios;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class CampanhasFunction
{
    private readonly IRepositorioCampanha _repositorioCampanha;

    public CampanhasFunction(IRepositorioCampanha repositorioCampanha)
    {
        _repositorioCampanha = repositorioCampanha;
    }

    [Function(nameof(CampanhasFunction) + "_" + nameof(GetAsync))]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "campanhas/{codigo}")] HttpRequestData httpRequest,
        int codigo)
    {
        var entity = await _repositorioCampanha.ObterPorIdAsync(codigo);

        if (entity == null)
            return httpRequest.NotFound();

        return await httpRequest.OkAsync(entity);
    }

    [Function(nameof(CampanhasFunction) + "_" + nameof(GetAllAsync))]
    public async Task<HttpResponseData> GetAllAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "campanhas")] HttpRequestData httpRequest)
    {
        var entities = await _repositorioCampanha.ObterCampanhasAsync();

        return await httpRequest.OkAsync(entities);
    }
}