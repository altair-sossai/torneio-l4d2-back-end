using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public interface IServicoTime
    {
        Task<TimeModel> ObterPorCodigoAsync(string codigo);
        Task<List<TimeModel>> ObterTimesAsync();
        Task<TimeEntity> SalvarAsync(TimeEntity command);
        Task<TimeEntity> SalvarAsync(TimeCommand command);
        Task ExcluirAsync(string codigo);
    }
}