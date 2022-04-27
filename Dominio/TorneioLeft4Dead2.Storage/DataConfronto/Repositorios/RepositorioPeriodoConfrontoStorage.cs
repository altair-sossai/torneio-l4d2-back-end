using System;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios
{
    public class RepositorioPeriodoConfrontoStorage : BaseRepository<PeriodoConfrontoEntity>, IRepositorioPeriodoConfronto
    {
        private const string TableName = "PeriodosConfrontos";

        public RepositorioPeriodoConfrontoStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<PeriodoConfrontoEntity> ObterPorConfrontoAsync(Guid confrontoId)
        {
            return await GetByRowKeyAsync(confrontoId);
        }

        public async Task<PeriodoConfrontoEntity> SalvarAsync(PeriodoConfrontoEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }
    }
}