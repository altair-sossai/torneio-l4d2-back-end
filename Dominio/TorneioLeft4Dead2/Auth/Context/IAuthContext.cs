using System.Security.Claims;
using System.Threading.Tasks;

namespace TorneioLeft4Dead2.Auth.Context;

public interface IAuthContext
{
    Task FillUserAsync(ClaimsPrincipal claimsPrincipal);
    void GrantPermission();
}