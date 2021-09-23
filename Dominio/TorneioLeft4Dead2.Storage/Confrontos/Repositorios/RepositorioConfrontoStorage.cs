using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Confrontos.Repositorios
{
    public class RepositorioConfrontoStorage : BaseRepository<ConfrontoEntity>, IRepositorioConfronto
    {
        private const string TableName = "Confrontos";

        public RepositorioConfrontoStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<List<ConfrontoEntity>> ObterConfrontosAsync()
        {
            const string rodada = nameof(ConfrontoEntity.Rodada);
            const string rowKey = nameof(ConfrontoEntity.RowKey);

            var queryCommand = QueryCommand.Default
                .OrderBy(rodada, rowKey);

            return await GetAllAsync(queryCommand);
        }

        public async Task<ConfrontoEntity> SalvarAsync(ConfrontoEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirTudoAsync()
        {
            await DeleteAllAsync();
        }
    }
}