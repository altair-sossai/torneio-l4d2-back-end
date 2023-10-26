using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Estatisticas.PorJogador.Models;

namespace TorneioLeft4Dead2.Estatisticas.PorJogador.Servicos;

public interface IServicoEstatisticasPorJogador
{
    Task<List<JogadorModel>> ObterEstatisticasAsync();
}