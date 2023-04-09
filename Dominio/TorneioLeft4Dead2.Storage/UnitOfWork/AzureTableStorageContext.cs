using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;

namespace TorneioLeft4Dead2.Storage.UnitOfWork;

public class AzureTableStorageContext : IAzureTableStorageContext
{
    private static readonly HashSet<string> CreatedTables = new();
    private readonly IConfiguration _configuration;
    private TableServiceClient _tableServiceClient;

    public AzureTableStorageContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private string ConnectionString => _configuration.GetValue<string>("AzureWebJobsStorage")!;
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