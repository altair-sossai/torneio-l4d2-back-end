using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Auth.Entities;
using TorneioLeft4Dead2.Auth.Jwt.Extensions;
using TorneioLeft4Dead2.Auth.Repositories;

namespace TorneioLeft4Dead2.Auth.Context
{
    public class AuthContext
    {
        private readonly IUserRepository _userRepository;

        public AuthContext(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserEntity CurrentUser { get; private set; }

        public async Task FillUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null)
                return;

            var userId = claimsPrincipal.GetGuid("userId");
            var currentUser = await _userRepository.FindUserAsync(userId);

            CurrentUser = currentUser;
        }

        public void GrantPermission()
        {
            if (CurrentUser == null)
                throw new UnauthorizedAccessException();
        }
    }
}