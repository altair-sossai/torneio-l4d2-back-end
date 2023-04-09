using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Shared.Extensions;

namespace TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

public abstract class BaseTableStorageRepository
{
    private static readonly HashSet<string> CreatedTables = new();
    private readonly IMemoryCache _memoryCache;

    private readonly IAzureTableStorageContext _tableContext;
    private readonly string _tableName;

    private TableClient _tableClient;

    protected BaseTableStorageRepository(string tableName,
        IAzureTableStorageContext tableContext,
        IMemoryCache memoryCache)
    {
        _tableName = tableName;
        _tableContext = tableContext;
        _memoryCache = memoryCache;

        CreateIfNotExistsAsync().Wait();
    }

    protected TableClient TableClient => _tableClient ??= _tableContext.GetTableClientAsync(_tableName).Result;

    private async Task CreateIfNotExistsAsync()
    {
        if (CreatedTables.Contains(_tableName))
            return;

        await TableClient.CreateIfNotExistsAsync();

        CreatedTables.Add(_tableName);
    }

    protected Task DeleteAsync(Guid rowKey)
    {
        return DeleteAsync(rowKey.ToString().ToLower());
    }

    protected Task DeleteAsync(string rowKey)
    {
        return DeleteAsync("shared", rowKey);
    }

    protected Task DeleteAsync(string partitionKey, string rowKey)
    {
        _memoryCache.RemoveAllKeys();

        return TableClient.DeleteEntityAsync(partitionKey, rowKey);
    }
}

public abstract class BaseTableStorageRepository<TEntity> : BaseTableStorageRepository
    where TEntity : class, ITableEntity, new()
{
    private readonly IMemoryCache _memoryCache;

    protected BaseTableStorageRepository(string tableName, IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(tableName, tableContext, memoryCache)
    {
        _memoryCache = memoryCache;
    }

    protected ValueTask<TEntity> FindAsync(Guid rowKey)
    {
        return FindAsync(rowKey.ToString().ToLower());
    }

    protected ValueTask<TEntity> FindAsync(string rowKey)
    {
        return FindAsync("shared", rowKey);
    }

    private ValueTask<TEntity> FindAsync(string partitionKey, string rowKey)
    {
        return TableClient.QueryAsync<TEntity>(q => q.PartitionKey == partitionKey && q.RowKey == rowKey).FirstOrDefaultAsync();
    }

    protected IAsyncEnumerable<TEntity> GetAllAsync()
    {
        return TableClient.QueryAsync<TEntity>();
    }

    protected IAsyncEnumerable<TEntity> GetAllAsync(Guid partitionKey)
    {
        return GetAllAsync(partitionKey.ToString().ToLower());
    }

    private IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey)
    {
        return TableClient.QueryAsync<TEntity>(q => q.PartitionKey == partitionKey);
    }

    protected Task AddOrUpdateAsync(TEntity entity)
    {
        _memoryCache.RemoveAllKeys();

        return TableClient.UpsertEntityAsync(entity);
    }

    protected async Task DeleteAllAsync()
    {
        await foreach (var entity in TableClient.QueryAsync<TEntity>())
            await DeleteAsync(entity);
    }

    protected async Task DeleteAllAsync(Guid partitionKey)
    {
        await DeleteAllAsync(partitionKey.ToString().ToLower());
    }

    private async Task DeleteAllAsync(string partitionKey)
    {
        await foreach (var entity in GetAllAsync(partitionKey))
            await DeleteAsync(entity);
    }

    protected Task DeleteAsync(TEntity entity)
    {
        return DeleteAsync(entity.PartitionKey, entity.RowKey);
    }
}