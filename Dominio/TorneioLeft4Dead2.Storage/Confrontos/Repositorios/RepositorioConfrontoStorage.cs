using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Confrontos.Repositorios
{
    public class RepositorioConfrontoStorage : BaseRepository<ConfrontoEntity>, IRepositorioConfronto
    {
        private const string TableName = "Confrontos";

        public RepositorioConfrontoStorage(UnitOfWorkStorage unitOfWork, IMemoryCache memoryCache)
            : base(unitOfWork, TableName, memoryCache)
        {
        }

        public async Task<ConfrontoEntity> ObterPorIdAsync(Guid confrontoId)
        {
            return await GetByRowKeyAsync(confrontoId.ToString().ToLower());
        }

        public async Task<List<ConfrontoEntity>> ObterConfrontosAsync()
        {
            return (await GetAllAsync(QueryCommand.Default))
                .OrderBy(o => o.Rodada)
                .ThenBy(t => t.Data ?? DateTime.Now)
                .ThenBy(t => t.RowKey)
                .ToList();
        }

        public async Task<ConfrontoEntity> SalvarAsync(ConfrontoEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirTudoAsync()
        {
            await DeleteAllAsync();
        }

        public async Task ExcluirAsync(Guid confrontoId)
        {
            await DeleteAsync(confrontoId.ToString().ToLower());
        }
    }
}