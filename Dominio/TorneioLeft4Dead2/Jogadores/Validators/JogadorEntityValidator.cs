using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Entidades;

namespace TorneioLeft4Dead2.Jogadores.Validators
{
    public class JogadorEntityValidator : AbstractValidator<JogadorEntity>
    {
        public JogadorEntityValidator()
        {
            RuleFor(r => r.SteamId)
                .NotEmpty();

            RuleFor(r => r.Nome)
                .NotEmpty();

            RuleFor(r => r.UrlFotoPerfil)
                .NotEmpty();

            RuleFor(r => r.UrlPerfilSteam)
                .NotEmpty();

            RuleFor(r => r.TotalHoras)
                .GreaterThanOrEqualTo(0)
                .When(r => r.TotalHoras.HasValue);
        }
    }
}