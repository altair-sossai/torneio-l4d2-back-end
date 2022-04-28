using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios
{
    public class RepositorioSugestaoDataConfrontoStorage : BaseRepository<SugestaoDataConfrontoEntity>, IRepositorioSugestaoDataConfronto
    {
        private const string TableName = "SugestoesDatasConfrontos";

        public RepositorioSugestaoDataConfrontoStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<SugestaoDataConfrontoEntity> ObterPorIdAsync(Guid sugestaoId)
        {
            return await GetByRowKeyAsync(sugestaoId);
        }

        public async Task<List<SugestaoDataConfrontoEntity>> ObterPorConfrontoAsync(Guid confrontoId)
        {
            var entities = await GetAllFromPartitionKeyAsync(confrontoId);

            return entities.OrderBy(o => o.Data).ToList();
        }

        public async Task<SugestaoDataConfrontoEntity> SalvarAsync(SugestaoDataConfrontoEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
        {
            await DeleteAllFromPartitionKeyAsync(confrontoId);
        }
    }
}