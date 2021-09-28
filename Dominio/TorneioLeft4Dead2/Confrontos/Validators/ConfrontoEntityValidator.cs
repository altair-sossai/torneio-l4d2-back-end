using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Enums;

namespace TorneioLeft4Dead2.Confrontos.Validators
{
    public class ConfrontoEntityValidator : AbstractValidator<ConfrontoEntity>
    {
        public ConfrontoEntityValidator()
        {
            RuleFor(r => r.Id)
                .NotEmpty();

            RuleFor(r => r.Rodada)
                .NotEmpty();

            RuleFor(r => r.CodigoCampanha)
                .NotEmpty();

            RuleFor(r => r.CodigoTimeA)
                .NotEmpty()
                .NotEqual(r => r.CodigoTimeB);

            RuleFor(r => r.CodigoTimeB)
                .NotEmpty();

            When(w => w.Status == (int) StatusConfronto.Realizado, () =>
            {
                RuleFor(r => r.Data)
                    .NotNull();
            });
        }
    }
}