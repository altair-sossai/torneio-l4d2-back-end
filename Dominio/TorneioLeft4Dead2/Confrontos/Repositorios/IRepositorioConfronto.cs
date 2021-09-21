using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Entidades;

namespace TorneioLeft4Dead2.Confrontos.Repositorios
{
    public interface IRepositorioConfronto
    {
        Task<List<ConfrontoEntity>> ObterConfrontosAsync();
        Task<ConfrontoEntity> SalvarAsync(ConfrontoEntity entity);
        Task ExcluirTudoAsync();
    }
}