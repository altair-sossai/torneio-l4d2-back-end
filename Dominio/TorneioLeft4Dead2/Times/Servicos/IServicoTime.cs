using System.Threading.Tasks;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public interface IServicoTime
    {
        Task<TimeEntity> SalvarAsync(TimeCommand command);
        Task ExcluirAsync(string codigo);
    }
}