using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Playoffs.Entidades;
using TorneioLeft4Dead2.Playoffs.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Playoffs.Repositorios;

public class RepositorioPlayoffsStorage : BaseTableStorageRepository<PlayoffsEntity>, IRepositorioPlayoffs
{
    private const string TableName = "Playoffs";

    public RepositorioPlayoffsStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId)
    {
        var entity = await FindAsync(playoffsId);

        entity?.IniciarConfrontos();

        return entity;
    }

    public async Task<List<PlayoffsEntity>> ObterPlayoffsAsync()
    {
        var entities = await GetAllAsync()
            .OrderBy(o => o.Rodada)
            .ThenBy(t => t.Ordem)
            .ToListAsync();

        entities.ForEach(entity => entity.IniciarConfrontos());

        return entities;
    }

    public async Task SalvarAsync(PlayoffsEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task ExcluirAsync(Guid playoffsId)
    {
        await DeleteAsync(playoffsId);
    }
}