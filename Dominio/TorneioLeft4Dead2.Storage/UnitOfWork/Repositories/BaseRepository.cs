using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;

namespace TorneioLeft4Dead2.Storage.UnitOfWork.Repositories
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

        protected async Task<T> GetByRowKeyAsync(Guid rowKey)
        {
            return await GetByRowKeyAsync(rowKey.ToString().ToLower());
        }

        protected async Task<T> GetByRowKeyAsync(string rowKey)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            var tableQuery = new TableQuery<T>().Where(filter);
            var entities = cloudTable.ExecuteQuery(tableQuery);

            return entities.FirstOrDefault();
        }

        protected async Task<List<T>> GetAllFromPartitionKeyAsync(Guid partitionKey)
        {
            return await GetAllFromPartitionKeyAsync(partitionKey.ToString().ToLower());
        }

        private async Task<List<T>> GetAllFromPartitionKeyAsync(string partitionKey)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            var tableQuery = new TableQuery<T>().Where(filter);

            return cloudTable.ExecuteQuery(tableQuery).ToList();
        }

        protected async Task<List<T>> GetAllAsync(QueryCommand queryCommand)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var tableQuery = queryCommand.BuildTableQuery<T>();
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

        protected async Task DeleteAsync(Guid rowKey)
        {
            await DeleteAsync(rowKey.ToString().ToLower());
        }

        protected async Task DeleteAsync(string rowKey)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var filter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey);
            var tableQuery = new TableQuery<T>().Where(filter);

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
                await DeleteAsync(entity);
        }

        protected async Task DeleteAllFromPartitionKeyAsync(Guid partitionKey)
        {
            await DeleteAllFromPartitionKeyAsync(partitionKey.ToString().ToLower());
        }

        private async Task DeleteAllFromPartitionKeyAsync(string partitionKey)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            var tableQuery = new TableQuery<T>().Where(filter);

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
                await DeleteAsync(entity);
        }

        protected async Task DeleteAsync(T entity)
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var tableOperation = TableOperation.Delete(entity);

            await cloudTable.ExecuteAsync(tableOperation);
        }

        protected async Task DeleteAllAsync()
        {
            var cloudTable = await _unitOfWork.GetTableReferenceAsync(_tableName);
            var tableQuery = new TableQuery<T>();

            foreach (var entity in cloudTable.ExecuteQuery(tableQuery))
            {
                var tableOperation = TableOperation.Delete(entity);
                await cloudTable.ExecuteAsync(tableOperation);
            }
        }
    }
}