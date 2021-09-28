using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Jogadores.Servicos;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;
using TorneioLeft4Dead2.Times.Extensions;
using TorneioLeft4Dead2.Times.Models;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Times.Servicos
{
    public class ServicoTime : IServicoTime
    {
        private readonly IMapper _mapper;
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly IRepositorioTime _repositorioTime;
        private readonly IRepositorioTimeJogador _repositorioTimeJogador;
        private readonly IServicoJogador _servicoJogador;
        private readonly IValidator<TimeEntity> _validator;

        public ServicoTime(IMapper mapper,
            IValidator<TimeEntity> validator,
            IRepositorioTime repositorioTime,
            IRepositorioTimeJogador repositorioTimeJogador,
            IRepositorioJogador repositorioJogador,
            IServicoJogador servicoJogador)
        {
            _mapper = mapper;
            _validator = validator;
            _repositorioTime = repositorioTime;
            _repositorioTimeJogador = repositorioTimeJogador;
            _repositorioJogador = repositorioJogador;
            _servicoJogador = servicoJogador;
        }

        public async Task<TimeModel> ObterPorCodigoAsync(string codigo)
        {
            var entity = await _repositorioTime.ObterPorCodigoAsync(codigo);
            if (entity == null)
                return null;

            var jogadores = await _servicoJogador.ObterPorTimeAsync(codigo);
            var model = _mapper.Map<TimeModel>(entity);

            model.Jogadores = jogadores;

            return model;
        }

        public async Task<List<TimeModel>> ObterTimesAsync()
        {
            var entities = await _repositorioTime.ObterTimesAsync();
            if (entities == null)
                return null;

            var times = _mapper.Map<List<TimeModel>>(entities);
            var jogadores = await _repositorioJogador.ObterJogadoresAsync();
            var timesJogadores = await _repositorioTimeJogador.ObterTodosAsync();

            times.Vincular(timesJogadores, jogadores);

            return times;
        }

        public async Task<TimeEntity> SalvarAsync(TimeEntity entity)
        {
            await _validator.ValidateAndThrowAsync(entity);
            await _repositorioTime.SalvarAsync(entity);

            return entity;
        }

        public async Task<TimeEntity> SalvarAsync(TimeCommand command)
        {
            var entity = await ObterPorCodigoAsync(command.Codigo) ?? new TimeEntity();

            _mapper.Map(command, entity);

            return await SalvarAsync(entity);
        }

        public async Task ExcluirAsync(string codigo)
        {
            await _repositorioTime.ExcluirAsync(codigo);
            await _repositorioTimeJogador.ExcluirPorTimeAsync(codigo);
        }
    }
}