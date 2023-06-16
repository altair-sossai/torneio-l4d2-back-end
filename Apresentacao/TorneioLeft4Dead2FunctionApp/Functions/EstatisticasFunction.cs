using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Estatisticas.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class EstatisticasFunction
{
    private readonly IServicoEstatisticas _servicoEstatisticas;

    public EstatisticasFunction(IServicoEstatisticas servicoEstatisticas)
    {
        _servicoEstatisticas = servicoEstatisticas;
    }

    [Function(nameof(EstatisticasFunction) + "_" + nameof(GetAsync))]
    public async Task<HttpResponseData> GetAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "estatisticas")] HttpRequestData httpRequest)
    {
        var models = await _servicoEstatisticas.ObterEstatisticasAsync();

        return await httpRequest.OkAsync(models);
    }
}