using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Jwt;
using TorneioLeft4Dead2.Auth.Jwt.Models;
using TorneioLeft4Dead2.Auth.Repositories;

namespace TorneioLeft4Dead2.Auth.Services;

public class AuthService(
    IUserRepository userRepository,
    IValidator<LoginCommand> loginValidator)
    : IAuthService
{
    public async Task<PrettyToken> AuthAsync(LoginCommand command)
    {
        await loginValidator.ValidateAndThrowAsync(command);

        var user = await userRepository.FindUserAsync(command.Email);

        return UsuarioJwtService.CreateNewToken(user);
    }
}