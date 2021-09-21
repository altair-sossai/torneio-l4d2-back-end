using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Repositorios
{
    public interface IRepositorioTimeJogador
    {
        Task<List<TimeJogadorEntity>> ObterTodosAsync();
        Task<List<TimeJogadorEntity>> ObterPorTimeAsync(string codigo);
        Task<TimeJogadorEntity> SalvarAsync(TimeJogadorEntity entity);
        Task DesvincularJogadorAsync(string codigo, string steamId);
        Task ExcluirPorJogadorAsync(string steamId);
        Task ExcluirPorTimeAsync(string codigo);
    }
}