using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public class ServicoTime : IServicoTime
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioTime _repositorioTime;
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;
        private readonly IValidator<TimeEntity> _validator;

        public ServicoTime(IMapper mapper,
            IValidator<TimeEntity> validator,
            IRepositorioTime repositorioTime,
            IRepositorioTimeJogador repositorioTimeJogador)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioTime = repositorioTime;
            _repositorioTimeJogador = repositorioTimeJogador;
        }

        public async Task<TimeEntity> SalvarAsync(TimeCommand command)
        {
            var entity = _mapper.Map<TimeEntity>(command);

            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioTime.SalvarAsync(entity);

            return entity;
        }

        public async Task ExcluirAsync(string codigo)
        {
            await _repositorioTime.ExcluirAsync(codigo);
            await _repositorioTimeJogador.ExcluirPorTimeAsync(codigo);
        }
    }
}