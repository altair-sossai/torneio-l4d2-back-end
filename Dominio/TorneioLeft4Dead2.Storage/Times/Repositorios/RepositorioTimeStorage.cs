using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios;

public class RepositorioTimeStorage : BaseTableStorageRepository<TimeEntity>, IRepositorioTime
{
    private const string TableName = "Times";

    public RepositorioTimeStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<TimeEntity> ObterPorCodigoAsync(string codigo)
    {
        return await FindAsync(codigo);
    }

    public async Task<List<TimeEntity>> ObterTimesAsync()
    {
        return await GetAllAsync()
            .OrderBy(o => o.Codigo)
            .ToListAsync();
    }

    public async Task<List<TimeEntity>> ObterClassificacaoAsync()
    {
        var classificados = await GetAllAsync()
            .OrderByDescending(o => o.PontosGerais)
            .ThenByDescending(t => t.SaldoTotalPontos)
            .ToListAsync();

        return classificados;
    }

    public async Task SalvarAsync(TimeEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task ExcluirAsync(string codigo)
    {
        await DeleteAsync(codigo);
    }
}