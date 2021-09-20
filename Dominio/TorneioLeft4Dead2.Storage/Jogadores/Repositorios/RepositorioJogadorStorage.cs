using System.Collections.Generic;
using System.Linq;
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
            var entities = await GetAllAsync();

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