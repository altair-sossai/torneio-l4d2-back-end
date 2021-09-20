using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Repositorios;

namespace TorneioLeft4Dead2.Jogadores.Servicos
{
    public class ServicoJogador : IServicoJogador
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly IValidator<JogadorEntity> _validator;

        public ServicoJogador(IMapper mapper,
            IValidator<JogadorEntity> validator,
            IRepositorioJogador repositorioJogador)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioJogador = repositorioJogador;
        }

        public async Task<JogadorEntity> SalvarAsync(JogadorCommand command)
        {
            var entity = _mapper.Map<JogadorEntity>(command);

            await _validator.ValidateAndThrowAsync(entity);

            return await _repositorioJogador.SalvarAsync(entity);
        }
    }
}