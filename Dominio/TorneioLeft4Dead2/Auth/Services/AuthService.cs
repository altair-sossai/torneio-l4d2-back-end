using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Jwt;
using TorneioLeft4Dead2.Auth.Jwt.Models;
using TorneioLeft4Dead2.Auth.Repositories;

namespace TorneioLeft4Dead2.Auth.Services;

public class AuthService : IAuthService
{
    private readonly IValidator<LoginCommand> _loginValidator;
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository,
        IValidator<LoginCommand> loginValidator)
    {
        _userRepository = userRepository;
        _loginValidator = loginValidator;
    }

    public async Task<PrettyToken> AuthAsync(LoginCommand command)
    {
        await _loginValidator.ValidateAndThrowAsync(command);

        var user = await _userRepository.FindUserAsync(command.Email);

        return UsuarioJwtService.CreateNewToken(user);
    }
}