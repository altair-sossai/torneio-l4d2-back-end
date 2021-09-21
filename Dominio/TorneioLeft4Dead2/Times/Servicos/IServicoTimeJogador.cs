using System.Threading.Tasks;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public interface IServicoTimeJogador
    {
        Task<TimeJogadorEntity> SalvarAsync(TimeJogadorCommand command);
        Task DesvincularJogadorAsync(string codigo, string steamId);
    }
}