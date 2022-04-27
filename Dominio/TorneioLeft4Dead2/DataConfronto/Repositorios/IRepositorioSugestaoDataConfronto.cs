using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;

namespace TorneioLeft4Dead2.DataConfronto.Repositorios
{
    public interface IRepositorioSugestaoDataConfronto
    {
        Task<List<SugestaoDataConfrontoEntity>> ObterPorConfrontoAsync(Guid confrontoId);
        Task<SugestaoDataConfrontoEntity> SalvarAsync(SugestaoDataConfrontoEntity entity);
        Task ExcluirPorConfrontoAsync(Guid confrontoId);
    }
}