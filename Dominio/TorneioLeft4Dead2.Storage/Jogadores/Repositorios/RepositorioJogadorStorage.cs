using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Jogadores.Repositorios;

public class RepositorioJogadorStorage : BaseTableStorageRepository<JogadorEntity>, IRepositorioJogador
{
    private const string TableName = "Jogadores";

    public RepositorioJogadorStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<List<JogadorEntity>> ObterJogadoresAsync()
    {
        return await GetAllAsync()
            .OrderBy(o => o.Nome)
            .ToListAsync();
    }

    public async Task SalvarAsync(JogadorEntity entity)
    {
        await AddOrUpdateAsync(entity);
    }

    public async Task ExcluirAsync(string steamId)
    {
        await DeleteAsync(steamId);
    }
}