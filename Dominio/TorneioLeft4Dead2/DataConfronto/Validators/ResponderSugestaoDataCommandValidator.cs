using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Repositorios;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.DataConfronto.Validators
{
    public class ResponderSugestaoDataCommandValidator : AbstractValidator<ResponderSugestaoDataCommand>
    {
        private readonly IRepositorioConfronto _repositorioConfronto;
        private readonly IRepositorioSugestaoDataConfronto _repositorioSugestaoDataConfronto;
        private readonly IServicoTime _servicoTime;

        public ResponderSugestaoDataCommandValidator(IRepositorioConfronto repositorioConfronto,
            IRepositorioSugestaoDataConfronto repositorioSugestaoDataConfronto,
            IServicoTime servicoTime)
        {
            _repositorioConfronto = repositorioConfronto;
            _repositorioSugestaoDataConfronto = repositorioSugestaoDataConfronto;
            _servicoTime = servicoTime;

            RuleFor(r => r.ConfrontoId)
                .NotEmpty()
                .MustAsync(ConfrontoComStatusAguardandoAsync)
                .WithMessage("Apenas confrontos com status de 'Aguardando' podem ter as datas alteradas");

            RuleFor(r => r.SugestaoId)
                .NotEmpty()
                .MustAsync(ExisteNoConfrontoAsync)
                .WithMessage("Sugestão de data inválida");

            RuleFor(r => r.SteamId)
                .NotEmpty()
                .NotNull()
                .MustAsync(ExisteComoCapitaoEmUmaDasEquipesDoConfrontoAsync)
                .WithMessage("Não é um capitão de equipe válido");

            RuleFor(r => r.Resposta)
                .IsInEnum();
        }

        private async Task<bool> ConfrontoComStatusAguardandoAsync(Guid confrontoId, CancellationToken cancellationToken)
        {
            var confronto = await _repositorioConfronto.ObterPorIdAsync(confrontoId);
            if (confronto == null)
                return false;

            return confronto.Status == (int) StatusConfronto.Aguardando;
        }

        private async Task<bool> ExisteNoConfrontoAsync(ResponderSugestaoDataCommand command, Guid sugestaoId, CancellationToken cancellationToken)
        {
            var sugestao = await _repositorioSugestaoDataConfronto.ObterPorIdAsync(sugestaoId);
            if (sugestao == null)
                return false;

            return sugestao.ConfrontoId == command.ConfrontoId;
        }

        private async Task<bool> ExisteComoCapitaoEmUmaDasEquipesDoConfrontoAsync(ResponderSugestaoDataCommand command, string steamId, CancellationToken cancellationToken)
        {
            var confronto = await _repositorioConfronto.ObterPorIdAsync(command.ConfrontoId);
            if (confronto == null)
                return false;

            var timeA = await _servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeA);
            var timeB = await _servicoTime.ObterPorCodigoAsync(confronto.CodigoTimeB);

            return timeA.Capitao.SteamId == steamId
                   || timeB.Capitao.SteamId == steamId;
        }
    }
}