using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Estatisticas.Models;

namespace TorneioLeft4Dead2.Estatisticas.Servicos;

public interface IServicoEstatisticas
{
    Task<List<JogadorModel>> ObterEstatisticasAsync();
}