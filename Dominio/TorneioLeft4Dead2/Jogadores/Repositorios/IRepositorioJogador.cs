using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Entidades;

namespace TorneioLeft4Dead2.Jogadores.Repositorios
{
    public interface IRepositorioJogador
    {
        Task<List<JogadorEntity>> ObterJogadoresAsync();
        Task<JogadorEntity> SalvarAsync(JogadorEntity entity);
        Task ExcluirAsync(string steamId);
    }
}