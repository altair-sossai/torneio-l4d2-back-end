using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Models;

namespace TorneioLeft4Dead2.Confrontos.Servicos
{
    public interface IServicoConfronto
    {
        Task<List<RodadaModel>> ObterRodadasAsync();
        Task<List<ConfrontoModel>> ObterConfrontosAsync();
        Task GerarConfrontosAsync();
    }
}