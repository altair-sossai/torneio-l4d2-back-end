using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TorneioLeft4Dead2.Auth.Entities;
using TorneioLeft4Dead2.Auth.Repositories;
using TorneioLeft4Dead2.Storage.UnitOfWork;
using TorneioLeft4Dead2.Storage.UnitOfWork.Commands;
using TorneioLeft4Dead2.Storage.UnitOfWork.Repositories;

namespace TorneioLeft4Dead2.Storage.Auth.Repositories
{
    public class UserRepositoryStorage : BaseRepository<UserEntity>, IUserRepository
    {
        private const string TableName = "Users";

        public UserRepositoryStorage(UnitOfWorkStorage unitOfWork, IMemoryCache memoryCache)
            : base(unitOfWork, TableName, memoryCache)
        {
        }

        public async Task<UserEntity> FindUserAsync(Guid id)
        {
            var rowKey = id.ToString().ToLower();

            return await GetByRowKeyAsync(rowKey);
        }

        public async Task<UserEntity> FindUserAsync(string email)
        {
            var queryCommand = QueryCommand.Default
                .Where(nameof(UserEntity.Email), email);

            return (await GetAllAsync(queryCommand))
                .FirstOrDefault();
        }

        public async Task<bool> AuthAsync(string email, string password)
        {
            var user = await FindUserAsync(email);

            if (user == null)
                return false;

            return user.Password == password;
        }
    }
}