using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Confrontos.Repositorios;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.Times.Servicos;

namespace TorneioLeft4Dead2.DataConfronto.Validators;

public class NovaSugestaoDataCommandValidator : AbstractValidator<NovaSugestaoDataCommand>
{
    private readonly IRepositorioConfronto _repositorioConfronto;
    private readonly IServicoTime _servicoTime;

    public NovaSugestaoDataCommandValidator(IRepositorioConfronto repositorioConfronto,
        IServicoTime servicoTime)
    {
        _repositorioConfronto = repositorioConfronto;
        _servicoTime = servicoTime;

        RuleFor(r => r.SteamId)
            .NotEmpty()
            .NotNull()
            .MustAsync(ExisteComoCapitaoEmUmaDasEquipesDoConfrontoAsync)
            .WithMessage("Não é um capitão de equipe válido");
    }

    private async Task<bool> ExisteComoCapitaoEmUmaDasEquipesDoConfrontoAsync(NovaSugestaoDataCommand command, string steamId, CancellationToken cancellationToken)
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