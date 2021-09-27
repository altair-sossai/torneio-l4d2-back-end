using System;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Auth.Entities;

namespace TorneioLeft4Dead2.Auth.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> FindUserAsync(Guid id);
        Task<UserEntity> FindUserAsync(string email);
        Task<bool> AuthAsync(string email, string password);
    }
}