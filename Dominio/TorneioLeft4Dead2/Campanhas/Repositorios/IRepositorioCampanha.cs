using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Campanhas.Entidades;

namespace TorneioLeft4Dead2.Campanhas.Repositorios
{
    public interface IRepositorioCampanha
    {
        Task<CampanhaEntity> ObterPorIdAsync(int codigo);
        Task<List<CampanhaEntity>> ObterCampanhasAsync();
    }
}