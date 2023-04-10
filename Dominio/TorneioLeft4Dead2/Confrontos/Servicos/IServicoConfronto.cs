using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Confrontos.Commands;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Models;

namespace TorneioLeft4Dead2.Confrontos.Servicos;

public interface IServicoConfronto
{
    Task<ConfrontoEntity> ObterPorIdAsync(Guid confrontoId);
    Task<List<RodadaModel>> ObterRodadasAsync();
    Task<List<ConfrontoModel>> ObterConfrontosAsync();
    Task<ConfrontoEntity> SalvarAsync(ConfrontoCommand command);
    Task AgendarConfrontoAsync(Guid confrontoId);
    Task GerarConfrontosAsync();
    Task ExcluirAsync(Guid confrontoId);
    Task LimparCampanhasAsync();
    Task<List<CampanhaEntity>> SortearCampanhasAsync();
}