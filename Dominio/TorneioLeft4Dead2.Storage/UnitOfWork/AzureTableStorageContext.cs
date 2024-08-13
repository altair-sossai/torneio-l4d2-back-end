using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace TorneioLeft4Dead2.Storage.UnitOfWork;

public class AzureTableStorageContext(IConfiguration configuration)
    : IAzureTableStorageContext
{
    private static readonly HashSet<string> CreatedTables = new();
    private TableServiceClient _tableServiceClient;

    private string ConnectionString => configuration.GetValue<string>("AzureWebJobsStorage")!;
    private TableServiceClient TableServiceClient => _tableServiceClient ??= new TableServiceClient(ConnectionString);

    public async Task<TableClient> GetTableClientAsync(string tableName)
    {
        var tableClient = TableServiceClient.GetTableClient(tableName);

        if (CreatedTables.Contains(tableName))
            return tableClient;

        await tableClient.CreateIfNotExistsAsync();
        CreatedTables.Add(tableName);

        return tableClient;
    }
}