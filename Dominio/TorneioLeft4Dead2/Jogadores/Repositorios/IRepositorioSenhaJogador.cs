using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Models;

namespace TorneioLeft4Dead2.Jogadores.Repositorios
{
    public interface IRepositorioSenhaJogador
    {
        Task<SenhaJogadorModel> GerarSenhaAsync(string steamId);
    }
}