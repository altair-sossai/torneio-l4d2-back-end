using FluentValidation;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Validators;

public class TimeEntityValidator : AbstractValidator<TimeEntity>
{
    public TimeEntityValidator()
    {
        RuleFor(r => r.Codigo)
            .NotEmpty()
            .MaximumLength(20)
            .Matches(@"^[\w-]+$");

        RuleFor(r => r.Nome)
            .NotEmpty()
            .MaximumLength(50);
    }
}