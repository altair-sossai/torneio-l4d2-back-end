using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Repositorios;

namespace TorneioLeft4Dead2.DataConfronto.Validators
{
    public class SugestaoDataConfrontoEntityValidator : AbstractValidator<SugestaoDataConfrontoEntity>
    {
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioPeriodoConfronto _repositorioPeriodoConfronto;

        public SugestaoDataConfrontoEntityValidator(IRepositorioConfronto repositorioConfronto,
            IRepositorioPeriodoConfronto repositorioPeriodoConfronto)
        {
            _repositorioConfronto = repositorioConfronto;
            _repositorioPeriodoConfronto = repositorioPeriodoConfronto;

            RuleFor(r => r.ConfrontoId)
                .NotEmpty()
                .MustAsync(ConfrontoComStatusAguardandoAsync)
                .WithMessage("Apenas confrontos com status de 'Aguardando' podem ter as datas alteradas");

            RuleFor(r => r.Id)
                .NotEmpty();

            RuleFor(r => r.Data)
                .MustAsync(DentroDoPeriodoDoConfrontoAsync)
                .WithMessage("A data não pode estar fora do período do confronto");
        }

        private async Task<bool> ConfrontoComStatusAguardandoAsync(Guid confrontoId, CancellationToken cancellationToken)
        {
            var confronto = await _repositorioConfronto.ObterPorIdAsync(confrontoId);
            if (confronto == null)
                return false;

            return confronto.Status == (int) StatusConfronto.Aguardando;
        }

        private async Task<bool> DentroDoPeriodoDoConfrontoAsync(SugestaoDataConfrontoEntity entity, DateTime data, CancellationToken cancellationToken)
        {
            var periodo = await _repositorioPeriodoConfronto.ObterPorConfrontoAsync(entity.ConfrontoId);
            if (periodo == null)
                return false;

            return data >= periodo.Inicio && data <= periodo.Fim;
        }
    }
}