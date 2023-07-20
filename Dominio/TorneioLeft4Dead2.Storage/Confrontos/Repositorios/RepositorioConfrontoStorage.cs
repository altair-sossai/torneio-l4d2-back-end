using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Confrontos.Repositorios;

public class RepositorioConfrontoStorage : BaseTableStorageRepository<ConfrontoEntity>, IRepositorioConfronto
{
    private const string TableName = "Confrontos";

    public RepositorioConfrontoStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<ConfrontoEntity> ObterPorIdAsync(Guid confrontoId)
    {
        return await FindAsync(confrontoId);
    }

    public async Task<List<ConfrontoEntity>> ObterConfrontosAsync()
    {
        return await GetAllAsync()
            .OrderBy(o => o.Rodada)
            .ThenBy(t => t.Data ?? DateTime.Now)
            .ThenBy(t => t.RowKey)
            .ToListAsync();
    }

    public async Task SalvarAsync(ConfrontoEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task ExcluirTudoAsync()
    {
        await DeleteAllAsync();
    }

    public async Task ExcluirAsync(Guid confrontoId)
    {
        await DeleteAsync(confrontoId);
    }
}