using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;
using TorneioLeft4Dead2.Jogadores.Extensions;
using TorneioLeft4Dead2.Jogadores.Repositorios;
using TorneioLeft4Dead2.Steam.Context;
using TorneioLeft4Dead2.Steam.PlayerService.Services;
using TorneioLeft4Dead2.Steam.SteamUser.Services;
using TorneioLeft4Dead2.Times.Repositorios;

namespace TorneioLeft4Dead2.Jogadores.Servicos;

public class ServicoJogador : IServicoJogador
{
    private readonly IPlayerService _playerService;
    private readonly IRepositorioJogador _repositorioJogador;
    private readonly IRepositorioTimeJogador _repositorioTimeJogador;
    private readonly ISteamUserService _steamUserService;
    private readonly IValidator<JogadorEntity> _validator;

    public ServicoJogador(IValidator<JogadorEntity> validator,
        IRepositorioJogador repositorioJogador,
        IRepositorioTimeJogador repositorioTimeJogador,
        ISteamUserService steamUserService,
        IPlayerService playerService)
    {
        _validator = validator;
        _repositorioJogador = repositorioJogador;
        _repositorioTimeJogador = repositorioTimeJogador;
        _steamUserService = steamUserService;
        _playerService = playerService;
    }

    public async Task<List<JogadorEntity>> JogadoresDisponiveisAsync()
    {
        var jogadores = await _repositorioJogador.ObterJogadoresAsync();
        var vinculos = await _repositorioTimeJogador.ObterTodosAsync();
        var indisponiveis = vinculos.Select(v => v.Jogador).ToHashSet();
        var disponiveis = jogadores.Where(jogador => !indisponiveis.Contains(jogador.SteamId)).ToList();

        return disponiveis;
    }

    public async Task<List<JogadorEntity>> ObterPorTimeAsync(string codigo)
    {
        var timesJogadores = await _repositorioTimeJogador.ObterPorTimeAsync(codigo);
        var jogadores = (await _repositorioJogador.ObterJogadoresAsync()).ToDictionary();

        return timesJogadores.Select(s => jogadores[s.Jogador]).ToList();
    }

    public async Task AtualizarJogadoresAsync()
    {
        var jogadores = await _repositorioJogador.ObterJogadoresAsync();

        foreach (var jogador in jogadores)
            try
            {
                var command = new JogadorCommand { User = jogador.SteamId };

                await SalvarAsync(command);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
    }

    public async Task<JogadorEntity> SalvarAsync(JogadorCommand command)
    {
        var steamId = await ResolveSteamIdAsync(command.Login) ?? command.SteamId;
        var entity = await BuildEntityAsync(steamId);

        if (entity == null)
            throw new Exception("Usuário não localizado");

        await _validator.ValidateAsync(entity);
        await _repositorioJogador.SalvarAsync(entity);

        return entity;
    }

    public async Task ExcluirAsync(string steamId)
    {
        await _repositorioJogador.ExcluirAsync(steamId);
        await _repositorioTimeJogador.ExcluirPorJogadorAsync(steamId);
    }

    private async Task<string> ResolveSteamIdAsync(string login)
    {
        if (string.IsNullOrEmpty(login))
            return await Task.FromResult((string)null);

        var response = await _steamUserService.ResolveVanityUrlAsync(SteamContext.ApiKey, login);

        return response is { Response: { Success: 1 } } ? response.Response.SteamId : null;
    }

    private async Task<JogadorEntity> BuildEntityAsync(string steamId)
    {
        if (string.IsNullOrEmpty(steamId))
            return await Task.FromResult((JogadorEntity)null);

        var playerSummariesResponse = await _steamUserService.GetPlayerSummariesAsync(SteamContext.ApiKey, steamId);
        var ownedGamesResponse = await _playerService.GetOwnedGamesAsync(SteamContext.ApiKey, steamId);

        var entity = new JogadorEntity();

        entity.Update(playerSummariesResponse);
        entity.Update(ownedGamesResponse);

        return entity;
    }
}