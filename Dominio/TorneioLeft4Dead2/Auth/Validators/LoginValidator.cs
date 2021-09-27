using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Auth.Commands;
using TorneioLeft4Dead2.Auth.Repositories;

namespace TorneioLeft4Dead2.Auth.Validators
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUserRepository _userRepository;

        public LoginValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage("Informe o E-mail")
                .EmailAddress()
                .WithMessage("Informe um E-mail válido");

            RuleFor(r => r.Password)
                .NotEmpty()
                .WithMessage("Informe a Senha");

            RuleFor(r => r.Password)
                .MustAsync(AutenticadoAsync)
                .WithMessage("E-mail e/ou senha inválido");
        }

        private async Task<bool> AutenticadoAsync(LoginCommand command, string senha, CancellationToken cancellationToken)
        {
            return await _userRepository.AuthAsync(command.Email, senha);
        }
    }
}