using System;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;

namespace TorneioLeft4Dead2.DataConfronto.Servicos
{
    public interface IServicoPeriodoConfronto
    {
        Task<PeriodoConfrontoModel> ObterPorConfrontoAsync(Guid confrontoId);
        Task<PeriodoConfrontoEntity> SalvarAsync(Guid confrontoId, PeriodoConfrontoCommand command);
    }
}