using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Entities;
using TorneioLeft4Dead2.Auth.Repositories;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Auth.Repositories;

public class UserRepositoryStorage : BaseTableStorageRepository<UserEntity>, IUserRepository
{
    private const string TableName = "Users";

    public UserRepositoryStorage(IAzureTableStorageContext tableContext, IMemoryCache memoryCache)
        : base(TableName, tableContext, memoryCache)
    {
    }

    public async Task<UserEntity> FindUserAsync(Guid userId)
    {
        return await FindAsync(userId);
    }

    public async Task<UserEntity> FindUserAsync(string email)
    {
        var filter = $@"Email eq '{email}'";

        return await TableClient.QueryAsync<UserEntity>(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> AuthAsync(string email, string password)
    {
        var user = await FindUserAsync(email);

        if (user == null)
            return false;

        return user.Password == password;
    }
}