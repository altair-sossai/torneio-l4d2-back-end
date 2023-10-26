using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using TorneioLeft4Dead2.Estatisticas.PorEquipe.Servicos;
using TorneioLeft4Dead2.Estatisticas.PorJogador.Servicos;
using TorneioLeft4Dead2FunctionApp.Extensions;

namespace TorneioLeft4Dead2FunctionApp.Functions;

public class EstatisticasFunction
{
    private readonly IServicoEstatisticasPorEquipe _servicoEstatisticasPorEquipe;
    private readonly IServicoEstatisticasPorJogador _servicoEstatisticasPorJogador;

    public EstatisticasFunction(IServicoEstatisticasPorJogador servicoEstatisticasPorJogador,
        IServicoEstatisticasPorEquipe servicoEstatisticasPorEquipe)
    {
        _servicoEstatisticasPorJogador = servicoEstatisticasPorJogador;
        _servicoEstatisticasPorEquipe = servicoEstatisticasPorEquipe;
    }

    [Function(nameof(EstatisticasFunction) + "_" + nameof(PorJogadorAsync))]
    public async Task<HttpResponseData> PorJogadorAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "estatisticas/por-jogador")] HttpRequestData httpRequest)
    {
        var models = await _servicoEstatisticasPorJogador.ObterEstatisticasAsync();

        return await httpRequest.OkAsync(models);
    }

    [Function(nameof(EstatisticasFunction) + "_" + nameof(PorEquipeAsync))]
    public async Task<HttpResponseData> PorEquipeAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "estatisticas/por-equipe")] HttpRequestData httpRequest)
    {
        var models = await _servicoEstatisticasPorEquipe.ObterEstatisticasAsync();

        return await httpRequest.OkAsync(models);
    }
}