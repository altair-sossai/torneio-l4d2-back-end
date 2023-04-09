using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios;

public class RepositorioSugestaoDataConfrontoStorage : BaseTableStorageRepository<SugestaoDataConfrontoEntity>, IRepositorioSugestaoDataConfronto
{
    private const string TableName = "SugestoesDatasConfrontos";

    public RepositorioSugestaoDataConfrontoStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

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