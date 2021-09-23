using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Entidades;

namespace TorneioLeft4Dead2.Confrontos.Validators
{
    public class ConfrontoEntityValidator : AbstractValidator<ConfrontoEntity>
    {
        public ConfrontoEntityValidator()
        {
            RuleFor(r => r.Campanha)
                .NotEmpty();

            RuleFor(r => r.TimeA)
                .NotEmpty();

            RuleFor(r => r.TimeB)
                .NotEmpty();
        }
    }
}