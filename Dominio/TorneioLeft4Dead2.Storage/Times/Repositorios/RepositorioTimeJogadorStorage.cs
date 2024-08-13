using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios;

public class RepositorioTimeJogadorStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
    : BaseTableStorageRepository<TimeJogadorEntity>(TableName, tableContext, memoryCache), IRepositorioTimeJogador
{
    private const string TableName = "TimesJogadores";

    public async Task<List<TimeJogadorEntity>> ObterTodosAsync()
    {
        return await GetAllAsync()
            .OrderBy(o => o.Time)
            .ThenBy(t => t.Ordem)
            .ToListAsync();
    }

    public async Task<List<TimeJogadorEntity>> ObterPorTimeAsync(string codigo)
    {
        var filter = $@"Time eq '{codigo}'";

        return await TableClient
            .QueryAsync<TimeJogadorEntity>(filter)
            .OrderBy(o => o.Ordem)
            .ToListAsync();
    }

    public async Task SalvarAsync(TimeJogadorEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task DesvincularJogadorAsync(string codigo, string steamId)
    {
        var rowKey = $"{codigo}_{steamId}";

        await DeleteAsync(rowKey);
    }

    public async Task ExcluirPorJogadorAsync(string steamId)
    {
        var filter = $@"Jogador eq '{steamId}'";

        await foreach (var entity in TableClient.QueryAsync<TimeJogadorEntity>(filter))
            await DeleteAsync(entity);
    }

    public async Task ExcluirPorTimeAsync(string codigo)
    {
        var filter = $@"Time eq '{codigo}'";

        await foreach (var entity in TableClient.QueryAsync<TimeJogadorEntity>(filter))
            await DeleteAsync(entity);
    }
}