using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public class ServicoTimeJogador : IServicoTimeJogador
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;
        private readonly IValidator<TimeJogadorEntity> _validator;

        public ServicoTimeJogador(IMapper mapper,
            IValidator<TimeJogadorEntity> validator,
            IRepositorioTimeJogador repositorioTimeJogador)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioTimeJogador = repositorioTimeJogador;
        }

        public async Task<TimeJogadorEntity> SalvarAsync(TimeJogadorCommand command)
        {
            var entity = _mapper.Map<TimeJogadorEntity>(command);
            var jogadores = await _repositorioTimeJogador.ObterPorTimeAsync(command.Time);

            entity.Ordem = jogadores.Count;

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioTimeJogador.SalvarAsync(entity);

            return entity;
        }

        public async Task DesvincularJogadorAsync(string codigo, string steamId)
        {
            await _repositorioTimeJogador.DesvincularJogadorAsync(codigo, steamId);
        }
    }
}