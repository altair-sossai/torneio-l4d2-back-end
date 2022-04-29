using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios
{
    public class RepositorioTimeStorage : BaseRepository<TimeEntity>, IRepositorioTime
    {
        private const string TableName = "Times";

        public RepositorioTimeStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<TimeEntity> ObterPorCodigoAsync(string codigo)
        {
            return await GetByRowKeyAsync(codigo);
        }

        public async Task<List<TimeEntity>> ObterTimesAsync()
        {
            var entities = await GetAllAsync(QueryCommand.Default);

            return entities.OrderBy(o => o.Codigo).ToList();
        }

        public async Task<List<TimeEntity>> ObterClassificacaoAsync()
        {
            var entities = await GetAllAsync(QueryCommand.Default);
            var classificados = entities
                .OrderByDescending(o => o.PontosGerais)
                .ThenByDescending(t => t.SaldoTotalPontos)
                .ToList();

            return classificados;
        }

        public async Task<TimeEntity> SalvarAsync(TimeEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task ExcluirAsync(string codigo)
        {
            await DeleteAsync(codigo);
        }
    }
}