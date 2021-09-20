using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Storage.Jogadores.Repositorios.Base;
using TorneioLeft4Dead2.Storage.UnitOfWork;

namespace TorneioLeft4Dead2.Storage.Jogadores.Repositorios
{
    public class RepositorioJogadorStorage : BaseRepository<JogadorEntity>, IRepositorioJogador
    {
        private const string TableName = "Jogadores";

        public RepositorioJogadorStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<List<JogadorEntity>> ObterJogadoresAsync()
        {
            return await GetAllAsync();
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