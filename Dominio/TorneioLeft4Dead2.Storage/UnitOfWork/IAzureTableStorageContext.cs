using System.Threading.Tasks;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.Storage.UnitOfWork;

public interface IAzureTableStorageContext
{
    Task<TableClient> GetTableClientAsync(string tableName);
}