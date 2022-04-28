using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;

namespace TorneioLeft4Dead2.DataConfronto.Servicos
{
    public interface IServicoSugestaoDataConfronto
    {
        Task<List<SugestaoDataConfrontoModel>> ObterPorConfrontoAsync(Guid confrontoId);
        Task<List<SugestaoDataConfrontoEntity>> SalvarAsync(Guid confrontoId, List<SugestaoDataConfrontoCommand> commands);
        Task SugerirNovaDataAsync(NovaSugestaoDataCommand command);
        Task ExcluirPorConfrontoAsync(Guid confrontoId);
    }
}