using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.Storage.Jogadores.Repositorios.Base;
using TorneioLeft4Dead2.Storage.UnitOfWork;

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
            return (await GetAllAsync())
                .OrderBy(o => o.Rodada)
                .ThenBy(tb => tb.RowKey)
                .ToList();
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