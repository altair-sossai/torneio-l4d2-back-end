using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Entidades;

namespace TorneioLeft4Dead2.Confrontos.Validators
{
    public class ConfrontoEntityValidator : AbstractValidator<ConfrontoEntity>
    {
        public ConfrontoEntityValidator()
        {
            RuleFor(r => r.CodigoCampanha)
                .NotEmpty();

            RuleFor(r => r.CodigoTimeA)
                .NotEmpty();

            RuleFor(r => r.CodigoTimeB)
                .NotEmpty();
        }
    }
}