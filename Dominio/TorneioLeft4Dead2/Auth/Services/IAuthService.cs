using System.Threading.Tasks;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Jwt.Models;

namespace TorneioLeft4Dead2.Auth.Services;

public interface IAuthService
{
    Task<PrettyToken> AuthAsync(LoginCommand command);
}