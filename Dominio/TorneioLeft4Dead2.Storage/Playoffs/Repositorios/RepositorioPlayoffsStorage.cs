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
            var entity = await GetByRowKeyAsync(playoffsId.ToString().ToLower());

            entity?.IniciarConfrontos();

            return entity;
        }

        public async Task<List<PlayoffsEntity>> ObterPlayoffsAsync()
        {
            var entities = (await GetAllAsync(QueryCommand.Default))
                .OrderBy(o => o.Rodada)
                .ThenBy(t => t.RowKey)
                .ToList();

            entities.ForEach(entity => entity.IniciarConfrontos());

            return entities;
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