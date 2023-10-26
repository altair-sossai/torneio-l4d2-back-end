using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Estatisticas.PorJogador.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class EstatisticasFunction
{
    private readonly IServicoEstatisticasPorJogador _servicoEstatisticasPorJogador;

    public EstatisticasFunction(IServicoEstatisticasPorJogador servicoEstatisticasPorJogador)
    {
        _servicoEstatisticasPorJogador = servicoEstatisticasPorJogador;
    }

    [Function(nameof(EstatisticasFunction) + "_" + nameof(PorJogadorAsync))]
    public async Task<HttpResponseData> PorJogadorAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "estatisticas/por-jogador")] HttpRequestData httpRequest)
    {
        var models = await _servicoEstatisticasPorJogador.ObterEstatisticasAsync();

        return await httpRequest.OkAsync(models);
    }
}