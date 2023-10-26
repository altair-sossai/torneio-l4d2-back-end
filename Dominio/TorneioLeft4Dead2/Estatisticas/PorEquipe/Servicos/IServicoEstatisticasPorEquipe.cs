using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Estatisticas.PorEquipe.Models;

namespace TorneioLeft4Dead2.Estatisticas.PorEquipe.Servicos;

public interface IServicoEstatisticasPorEquipe
{
    Task<List<EquipeModel>> ObterEstatisticasAsync();
}