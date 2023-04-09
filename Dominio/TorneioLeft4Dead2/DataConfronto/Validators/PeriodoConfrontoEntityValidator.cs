using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Entidades;

namespace TorneioLeft4Dead2.DataConfronto.Validators;

public class PeriodoConfrontoEntityValidator : AbstractValidator<PeriodoConfrontoEntity>
{
    private readonly IRepositorioConfronto _repositorioConfronto;

    public PeriodoConfrontoEntityValidator(IRepositorioConfronto repositorioConfronto)
    {
        _repositorioConfronto = repositorioConfronto;

        RuleFor(r => r.ConfrontoId)
            .NotEmpty()
            .MustAsync(ConfrontoComStatusAguardandoAsync)
            .WithMessage("Apenas confrontos com status de 'Aguardando' podem ter as datas alteradas");

        RuleFor(r => r.Fim)
            .GreaterThan(f => f.Inicio);
    }

    private async Task<bool> ConfrontoComStatusAguardandoAsync(Guid confrontoId, CancellationToken cancellationToken)
    {
        var confronto = await _repositorioConfronto.ObterPorIdAsync(confrontoId);
        if (confronto == null)
            return false;

        return confronto.Status == (int)StatusConfronto.Aguardando;
    }
}