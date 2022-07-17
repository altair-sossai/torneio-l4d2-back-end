using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Playoffs.Entidades;
using TorneioLeft4Dead2.Playoffs.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Playoffs.Repositorios
{
    public class RepositorioPlayoffsStorage : BaseRepository<PlayoffsEntity>, IRepositorioPlayoffs
    {
        private const string TableName = "Playoffs";

        public RepositorioPlayoffsStorage(UnitOfWorkStorage unitOfWork, IMemoryCache memoryCache)
            : base(unitOfWork, TableName, memoryCache)
        {
        }

        public async Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId)
        {
            return await GetByRowKeyAsync(playoffsId.ToString().ToLower());
        }

        public async Task<List<PlayoffsEntity>> ObterPlayoffsAsync()
        {
            return (await GetAllAsync(QueryCommand.Default))
                .OrderBy(o => o.Rodada)
                .ThenBy(t => t.RowKey)
                .ToList();
        }

        public async Task<PlayoffsEntity> SalvarAsync(PlayoffsEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirAsync(Guid playoffsId)
        {
            await DeleteAsync(playoffsId.ToString().ToLower());
        }
    }
}