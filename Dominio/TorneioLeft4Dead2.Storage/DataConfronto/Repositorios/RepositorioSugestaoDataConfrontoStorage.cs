using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios;

public class RepositorioSugestaoDataConfrontoStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<SugestaoDataConfrontoEntity>(TableName, tableContext, memoryCache), IRepositorioSugestaoDataConfronto
{
    private const string TableName = "SugestoesDatasConfrontos";

    public async Task<SugestaoDataConfrontoEntity> ObterPorIdAsync(Guid sugestaoId)
    {
        return await FindAsync(sugestaoId);
    }

    public async Task<List<SugestaoDataConfrontoEntity>> ObterPorConfrontoAsync(Guid confrontoId)
    {
        return await GetAllAsync(confrontoId)
            .OrderBy(o => o.Data)
            .ToListAsync();
    }

    public async Task SalvarAsync(SugestaoDataConfrontoEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task ExcluirPorConfrontoAsync(Guid confrontoId)
    {
        await DeleteAllAsync(confrontoId);
    }

    public async Task ExcluirPorIdAsync(Guid sugestaoId)
    {
        await DeleteAsync(sugestaoId);
    }
}