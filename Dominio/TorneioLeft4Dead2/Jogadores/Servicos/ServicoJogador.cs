using System;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Steam.Context;
using TorneioLeft4Dead2.Steam.PlayerService.Services;
using TorneioLeft4Dead2.Steam.SteamUser.Services;

namespace TorneioLeft4Dead2.Jogadores.Servicos
{
    public class ServicoJogador : IServicoJogador
    {
        private readonly IPlayerService _playerService;
        private readonly IRepositorioJogador _repositorioJogador;
        private readonly string _steamApiKey = SteamContext.ApiKey;
        private readonly ISteamUserService _steamUserService;
        private readonly IValidator<JogadorEntity> _validator;

        public ServicoJogador(IValidator<JogadorEntity> validator,
            IRepositorioJogador repositorioJogador,
            ISteamUserService steamUserService,
            IPlayerService playerService)
        {
            _validator = validator;
            _repositorioJogador = repositorioJogador;
            _steamUserService = steamUserService;
            _playerService = playerService;
        }

        public async Task<JogadorEntity> SalvarAsync(JogadorCommand command)
        {
            var steamId = await ResolveSteamIdAsync(command.Login) ?? command.SteamId;
            var entity = await BuildEntityAsync(steamId);

            if (entity == null)
                throw new Exception("Usuário não localizado");

            await _validator.ValidateAsync(entity);

            return await _repositorioJogador.SalvarAsync(entity);
        }

        private async Task<string> ResolveSteamIdAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                return await Task.FromResult((string) null);

            var response = await _steamUserService.ResolveVanityUrlAsync(_steamApiKey, login);

            return response is {Response: {Success: 1}} ? response.Response.SteamId : null;
        }

        private async Task<JogadorEntity> BuildEntityAsync(string steamId)
        {
            if (string.IsNullOrEmpty(steamId))
                return await Task.FromResult((JogadorEntity) null);

            var playerSummariesResponse = await _steamUserService.GetPlayerSummariesAsync(_steamApiKey, steamId);
            var ownedGamesResponse = await _playerService.GetOwnedGamesAsync(_steamApiKey, steamId);

            var entity = new JogadorEntity();

            entity.Update(playerSummariesResponse);
            entity.Update(ownedGamesResponse);

            return entity;
        }
    }
}