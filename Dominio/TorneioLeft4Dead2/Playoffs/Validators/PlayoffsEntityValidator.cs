using System.Linq;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Playoffs.Entidades;

namespace TorneioLeft4Dead2.Playoffs.Validators;

public class PlayoffsEntityValidator : AbstractValidator<PlayoffsEntity>
{
    public PlayoffsEntityValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty();

        RuleFor(r => r.Rodada)
            .NotEmpty();

        RuleFor(r => r.Ordem)
            .NotEmpty();

        When(w => w.Confronto01Status is (int)StatusConfronto.Realizado or (int)StatusConfronto.Cancelado, () =>
        {
            RuleFor(r => r.Confronto01CodigoCampanha)
                .NotEmpty();

            RuleFor(r => r.Confronto01Data)
                .NotNull();

            RuleFor(r => r.Confronto01Status)
                .NotNull();

            RuleFor(r => r.Confronto01PontosConquistadosTimeA)
                .NotNull();

            RuleFor(r => r.Confronto01PontosConquistadosTimeB)
                .NotNull();

            RuleFor(r => r.Confronto01PenalidadeTimeA)
                .NotNull();

            RuleFor(r => r.Confronto01PenalidadeTimeB)
                .NotNull();

            RuleFor(r => r.Confronto01TimeAVenceu)
                .NotNull();

            RuleFor(r => r.Confronto01TimeBVenceu)
                .NotNull();
        });

        When(w => w.Confronto02Status is (int)StatusConfronto.Realizado or (int)StatusConfronto.Cancelado, () =>
        {
            RuleFor(r => r.Confronto02CodigoCampanha)
                .NotEmpty();

            RuleFor(r => r.Confronto02Data)
                .NotNull();

            RuleFor(r => r.Confronto02Status)
                .NotNull();

            RuleFor(r => r.Confronto02PontosConquistadosTimeA)
                .NotNull();

            RuleFor(r => r.Confronto02PontosConquistadosTimeB)
                .NotNull();

            RuleFor(r => r.Confronto02PenalidadeTimeA)
                .NotNull();

            RuleFor(r => r.Confronto02PenalidadeTimeB)
                .NotNull();

            RuleFor(r => r.Confronto02TimeAVenceu)
                .NotNull();

            RuleFor(r => r.Confronto02TimeBVenceu)
                .NotNull();
        });

        When(w => w.Confronto03Status is (int)StatusConfronto.Realizado or (int)StatusConfronto.Cancelado, () =>
        {
            RuleFor(r => r.Confronto03CodigoCampanha)
                .NotEmpty();

            RuleFor(r => r.Confronto03Data)
                .NotNull();

            RuleFor(r => r.Confronto03Status)
                .NotNull();

            RuleFor(r => r.Confronto03PontosConquistadosTimeA)
                .NotNull();

            RuleFor(r => r.Confronto03PontosConquistadosTimeB)
                .NotNull();

            RuleFor(r => r.Confronto03PenalidadeTimeA)
                .NotNull();

            RuleFor(r => r.Confronto03PenalidadeTimeB)
                .NotNull();

            RuleFor(r => r.Confronto03TimeAVenceu)
                .NotNull();

            RuleFor(r => r.Confronto03TimeBVenceu)
                .NotNull();
        });

        RuleFor(r => r.Confronto01Status)
            .NotEqual((int?)StatusConfronto.Aguardando)
            .When(w => w.Confronto02Status != (int?)StatusConfronto.Aguardando);

        RuleFor(r => r.Confronto01Status)
            .NotEqual((int?)StatusConfronto.Aguardando)
            .When(w => w.Confronto03Status != (int?)StatusConfronto.Aguardando);

        RuleFor(r => r.Confronto02Status)
            .NotEqual((int?)StatusConfronto.Aguardando)
            .When(w => w.Confronto03Status != (int?)StatusConfronto.Aguardando);

        RuleFor(r => r.CodigoCampanhaExcluidaTimeA)
            .Must(CampanhaNaoSelecionada)
            .When(w => w.CodigoCampanhaExcluidaTimeA.HasValue);

        RuleFor(r => r.CodigoCampanhaExcluidaTimeB)
            .Must(CampanhaNaoSelecionada)
            .When(w => w.CodigoCampanhaExcluidaTimeB.HasValue);
    }

    private static bool CampanhaNaoSelecionada(PlayoffsEntity entity, int? campanha)
    {
        return entity.Confrontos.All(c => c.CodigoCampanha != campanha);
    }
}