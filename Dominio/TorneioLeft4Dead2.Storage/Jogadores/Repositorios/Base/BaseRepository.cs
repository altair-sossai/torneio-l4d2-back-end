using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using TorneioLeft4Dead2.Storage.UnitOfWork;

namespace TorneioLeft4Dead2.Storage.Jogadores.Repositorios.Base
{
    public abstract class BaseRepository<T>
        where T : TableEntity, new()
    {
        private readonly string _tableName;
        private readonly UnitOfWorkStorage _unitOfWork;

        protected BaseRepository(UnitOfWorkStorage unitOfWork, string tableName)
        {
            _unitOfWork = unitOfWork;
            _tableName = tableName;
        }

        protected async Task<List<T>> GetAllAsync()
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);

            var tableQuery = new TableQuery<T>();
            var query = cloudTable.ExecuteQuery(tableQuery);
            var entities = query.ToList();

            return entities;
        }

        protected async Task<T> InsertOrMergeAsync(T entity)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);

            var tableOperation = TableOperation.InsertOrMerge(entity);
            var tableResult = await cloudTable.ExecuteAsync(tableOperation);
            var result = tableResult.Result as T;

            return result;
        }

        protected async Task DeleteAsync(string rowKey)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            var tableQuery = new TableQuery<T>().Where(filter);

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
            {
                var tableOperation = TableOperation.Delete(entity);
                await cloudTable.ExecuteAsync(tableOperation);
            }
        }
    }
}