using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Models;
using TorneioLeft4Dead2.Jogadores.Repositorios;

namespace TorneioLeft4Dead2.Jogadores.Servicos;

public class ServicoSenhaJogador(IRepositorioSenhaJogador repositorioSenhaJogador)
    : IServicoSenhaJogador
{
    public async Task<SenhaJogadorModel> GerarSenhaAsync(string steamId)
    {
        return await repositorioSenhaJogador.GerarSenhaAsync(steamId);
    }

    public async Task<bool> AutenticadoAsync(AutenticarJogadorCommand command)
    {
        return await repositorioSenhaJogador.AutenticadoAsync(command);
    }
}