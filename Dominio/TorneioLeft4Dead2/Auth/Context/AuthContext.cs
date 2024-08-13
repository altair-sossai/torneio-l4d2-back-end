using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Auth.Entities;
using TorneioLeft4Dead2.Auth.Jwt.Extensions;
using TorneioLeft4Dead2.Auth.Repositories;

namespace TorneioLeft4Dead2.Auth.Context;

public class AuthContext(IUserRepository userRepository)
    : IAuthContext
{
    private UserEntity CurrentUser { get; set; }

    public async Task FillUserAsync(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
            return;

        var userId = claimsPrincipal.GetGuid("userId");
        var currentUser = await userRepository.FindUserAsync(userId);

        CurrentUser = currentUser;
    }

    public void GrantPermission()
    {
        if (CurrentUser == null)
            throw new UnauthorizedAccessException();
    }
}