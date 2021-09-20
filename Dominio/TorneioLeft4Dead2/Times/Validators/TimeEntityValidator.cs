using FluentValidation;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Validators
{
    public class TimeEntityValidator : AbstractValidator<TimeEntity>
    {
        public TimeEntityValidator()
        {
            RuleFor(r => r.Codigo)
                .NotEmpty();

            RuleFor(r => r.Nome)
                .NotEmpty();
        }
    }
}