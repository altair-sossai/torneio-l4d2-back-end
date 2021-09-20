using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Repositorios
{
    public interface IRepositorioTime
    {
        Task<TimeEntity> ObterPorCodigoAsync(string codigo);
        Task<List<TimeEntity>> ObterTimesAsync();
        Task<TimeEntity> SalvarAsync(TimeEntity entity);
        Task ExcluirAsync(string codigo);
    }
}