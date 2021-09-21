using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using TorneioLeft4Dead2.Storage.Jogadores.Repositorios.Base;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Storage.Times.Repositorios
{
    public class RepositorioTimeJogadorStorage : BaseRepository<TimeJogadorEntity>, IRepositorioTimeJogador
    {
        private const string TableName = "TimesJogadores";

        public RepositorioTimeJogadorStorage(UnitOfWorkStorage unitOfWork)
            : base(unitOfWork, TableName)
        {
        }

        public async Task<List<TimeJogadorEntity>> ObterTodosAsync()
        {
            return (await GetAllAsync()).OrderBy(o => o.Ordem).ToList();
        }

        public async Task<List<TimeJogadorEntity>> ObterPorTimeAsync(string codigo)
        {
            var cloudTable = await UnitOfWork.GetTableReferenceAsync(TableName);

            var filter = TableQuery.GenerateFilterCondition("Time", QueryComparisons.Equal, codigo);
            var tableQuery = new TableQuery<TimeJogadorEntity>().Where(filter);
            var query = cloudTable.ExecuteQuery(tableQuery);
            var entities = query.ToList();

            return entities;
        }

        public async Task<TimeJogadorEntity> SalvarAsync(TimeJogadorEntity entity)
        {
            return await InsertOrMergeAsync(entity);
        }

        public async Task DesvincularJogadorAsync(string codigo, string steamId)
        {
            var rowKey = $"{codigo}_{steamId}";

            await DeleteAsync(rowKey);
        }

        public async Task ExcluirPorJogadorAsync(string steamId)
        {
            var cloudTable = await UnitOfWork.GetTableReferenceAsync(TableName);
            var filter = TableQuery.GenerateFilterCondition("Jogador", QueryComparisons.Equal, steamId);
            var tableQuery = new TableQuery<TimeJogadorEntity>().Where(filter);

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
            {
                var tableOperation = TableOperation.Delete(entity);
                await cloudTable.ExecuteAsync(tableOperation);
            }
        }

        public async Task ExcluirPorTimeAsync(string codigo)
        {
            var cloudTable = await UnitOfWork.GetTableReferenceAsync(TableName);
            var filter = TableQuery.GenerateFilterCondition("Time", QueryComparisons.Equal, codigo);
            var tableQuery = new TableQuery<TimeJogadorEntity>().Where(filter);

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
            {
                var tableOperation = TableOperation.Delete(entity);
                await cloudTable.ExecuteAsync(tableOperation);
            }
        }
    }
}