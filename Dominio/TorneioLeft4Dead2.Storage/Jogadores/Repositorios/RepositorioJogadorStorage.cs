using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Jogadores.Repositorios
{
    public class RepositorioJogadorStorage : BaseRepository<JogadorEntity>, IRepositorioJogador
    {
        private const string TableName = "Jogadores";

        public RepositorioJogadorStorage(UnitOfWorkStorage unitOfWork, IMemoryCache memoryCache)
            : base(unitOfWork, TableName, memoryCache)
        {
        }

        public async Task<List<JogadorEntity>> ObterJogadoresAsync()
        {
            var entities = await GetAllAsync(QueryCommand.Default);

            return entities.OrderBy(o => o.Nome).ToList();
        }

        public async Task<JogadorEntity> SalvarAsync(JogadorEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirAsync(string steamId)
        {
            await DeleteAsync(steamId);
        }
    }
}