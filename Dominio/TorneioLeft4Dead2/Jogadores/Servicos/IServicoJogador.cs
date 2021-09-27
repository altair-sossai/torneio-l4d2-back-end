using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;

namespace TorneioLeft4Dead2.Jogadores.Servicos
{
    public interface IServicoJogador
    {
        Task<List<JogadorEntity>> JogadoresDisponiveisAsync();
        Task<List<JogadorEntity>> ObterPorTimeAsync(string codigo);
        Task AtualizarJogadoresAsync();
        Task<JogadorEntity> SalvarAsync(JogadorCommand command);
        Task ExcluirAsync(string steamId);
    }
}