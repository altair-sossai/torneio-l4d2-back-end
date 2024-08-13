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

public class ServicoJogador(
    IValidator<JogadorEntity> validator,
    IRepositorioJogador repositorioJogador,
    IRepositorioTimeJogador repositorioTimeJogador,
    ISteamUserService steamUserService,
    IPlayerService playerService)
    : IServicoJogador
{
    public async Task<List<JogadorEntity>> JogadoresDisponiveisAsync()
    {
        var jogadores = await repositorioJogador.ObterJogadoresAsync();
        var vinculos = await repositorioTimeJogador.ObterTodosAsync();
        var indisponiveis = vinculos.Select(v => v.Jogador).ToHashSet();
        var disponiveis = jogadores.Where(jogador => !indisponiveis.Contains(jogador.SteamId)).ToList();

        return disponiveis;
    }

    public async Task<List<JogadorEntity>> ObterPorTimeAsync(string codigo)
    {
        var timesJogadores = await repositorioTimeJogador.ObterPorTimeAsync(codigo);
        var jogadores = (await repositorioJogador.ObterJogadoresAsync()).ToDictionary();

        return timesJogadores.Select(s => jogadores[s.Jogador]).ToList();
    }

    public async Task AtualizarJogadoresAsync()
    {
        var jogadores = await repositorioJogador.ObterJogadoresAsync();

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

        await validator.ValidateAsync(entity);
        await repositorioJogador.SalvarAsync(entity);

        return entity;
    }

    public async Task SortearCapitaesAsync()
    {
        var timesJogadores = await repositorioTimeJogador.ObterTodosAsync();
        var timesJogadoresCapitaes = timesJogadores.Where(w => w.Ordem == 0).ToList();
        var capitaes = timesJogadoresCapitaes.Select(s => s.Jogador).OrderBy(_ => Guid.NewGuid()).ToList();

        for (var i = 0; i < timesJogadoresCapitaes.Count; i++)
        {
            var timeJogador = timesJogadoresCapitaes[i];
            await repositorioTimeJogador.DesvincularJogadorAsync(timeJogador.Time, timeJogador.Jogador);

            timeJogador.Jogador = capitaes[i];
            await repositorioTimeJogador.SalvarAsync(timeJogador);
        }
    }

    public async Task SortearSuportesAsync()
    {
        var timesJogadores = await repositorioTimeJogador.ObterTodosAsync();
        var timesJogadoresSuportes = timesJogadores.Where(w => w.Ordem == 1).ToList();
        var suportes = timesJogadoresSuportes.Select(s => s.Jogador).OrderBy(_ => Guid.NewGuid()).ToList();

        for (var i = 0; i < timesJogadoresSuportes.Count; i++)
        {
            var timeJogador = timesJogadoresSuportes[i];
            await repositorioTimeJogador.DesvincularJogadorAsync(timeJogador.Time, timeJogador.Jogador);

            timeJogador.Jogador = suportes[i];
            await repositorioTimeJogador.SalvarAsync(timeJogador);
        }
    }

    public async Task ExcluirAsync(string steamId)
    {
        await repositorioJogador.ExcluirAsync(steamId);
        await repositorioTimeJogador.ExcluirPorJogadorAsync(steamId);
    }

    private async Task<string> ResolveSteamIdAsync(string login)
    {
        if (string.IsNullOrEmpty(login))
            return await Task.FromResult((string)null);

        var response = await steamUserService.ResolveVanityUrlAsync(SteamContext.ApiKey, login);

        return response is { Response: { Success: 1 } } ? response.Response.SteamId : null;
    }

    private async Task<JogadorEntity> BuildEntityAsync(string steamId)
    {
        if (string.IsNullOrEmpty(steamId))
            return await Task.FromResult((JogadorEntity)null);

        var playerSummariesResponse = await steamUserService.GetPlayerSummariesAsync(SteamContext.ApiKey, steamId);
        var ownedGamesResponse = await playerService.GetOwnedGamesAsync(SteamContext.ApiKey, steamId);

        var entity = new JogadorEntity();

        entity.Update(playerSummariesResponse);
        entity.Update(ownedGamesResponse);

        return entity;
    }
}