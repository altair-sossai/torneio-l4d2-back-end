using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Storage.UnitOfWork
{
    public class UnitOfWorkStorage
    {
        private static readonly HashSet<string> CreatedTables = new();
        private readonly string _connectionString;
        private readonly TableClientConfiguration _tableClientConfiguration = new();
        private CloudStorageAccount _cloudStorageAccount;

        public UnitOfWorkStorage(string connectionString)
        {
            _connectionString = connectionString;
        }

        private CloudStorageAccount CloudStorageAccount => _cloudStorageAccount ??= CloudStorageAccount.Parse(_connectionString);
        private CloudTableClient CloudTableClient => CloudStorageAccount.CreateCloudTableClient(_tableClientConfiguration);

        public async Task<CloudTable> GetTableReferenceAsync(string tableName)
        {
            var cloudTable = CloudTableClient.GetTableReference(tableName);

            if (CreatedTables.Contains(tableName))
                return cloudTable;

            await cloudTable.CreateIfNotExistsAsync();

            CreatedTables.Add(tableName);

            return cloudTable;
        }
    }
}