using System.Collections.Generic;
using System.Threading.Tasks;
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

        public RepositorioJogadorStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<List<JogadorEntity>> ObterJogadoresAsync()
        {
            const string nome = nameof(JogadorEntity.Nome);

            var queryCommand = QueryCommand.Default
                .OrderBy(nome);

            return await GetAllAsync(queryCommand);
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