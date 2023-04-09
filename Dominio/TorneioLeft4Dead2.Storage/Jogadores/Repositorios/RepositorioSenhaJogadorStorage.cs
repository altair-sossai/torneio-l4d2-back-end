using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Models;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Jogadores.Repositorios;

public class RepositorioSenhaJogadorStorage : BaseTableStorageRepository<SenhaJogadorEntity>, IRepositorioSenhaJogador
{
    private const string TableName = "SenhasJogadores";

    public RepositorioSenhaJogadorStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<SenhaJogadorModel> GerarSenhaAsync(string steamId)
    {
        var model = new SenhaJogadorModel(steamId);
        var entity = new SenhaJogadorEntity
        {
            SteamId = model.SteamId,
            SenhaCriptografada = model.SenhaCriptografada
        };

        await AddOrUpdateAsync(entity);

        return model;
    }

    public async Task<bool> AutenticadoAsync(AutenticarJogadorCommand command)
    {
        var entity = await FindAsync(command.SteamId);
        if (entity == null)
            return false;

        return entity.SenhaCriptografada == command.SenhaCriptografada;
    }
}