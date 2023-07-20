using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.DataConfronto.Repositorios;

public class RepositorioPeriodoConfrontoStorage : BaseTableStorageRepository<PeriodoConfrontoEntity>, IRepositorioPeriodoConfronto
{
    private const string TableName = "PeriodosConfrontos";

    public RepositorioPeriodoConfrontoStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<PeriodoConfrontoEntity> ObterPorConfrontoAsync(Guid confrontoId)
    {
        return await FindAsync(confrontoId);
    }

    public async Task SalvarAsync(PeriodoConfrontoEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }
}