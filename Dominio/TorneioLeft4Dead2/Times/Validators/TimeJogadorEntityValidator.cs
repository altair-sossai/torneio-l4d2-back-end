using FluentValidation;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Validators;

public class TimeJogadorEntityValidator : AbstractValidator<TimeJogadorEntity>
{
    public TimeJogadorEntityValidator()
    {
        RuleFor(r => r.Time)
            .NotEmpty();

        RuleFor(r => r.Jogador)
            .NotEmpty();
    }
}