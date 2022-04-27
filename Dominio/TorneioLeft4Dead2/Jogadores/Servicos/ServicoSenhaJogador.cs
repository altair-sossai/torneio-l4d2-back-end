using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Models;
using TorneioLeft4Dead2.Jogadores.Repositorios;

namespace TorneioLeft4Dead2.Jogadores.Servicos
{
    public class ServicoSenhaJogador : IServicoSenhaJogador
    {
        private readonly IRepositorioSenhaJogador _repositorioSenhaJogador;

        public ServicoSenhaJogador(IRepositorioSenhaJogador repositorioSenhaJogador)
        {
            _repositorioSenhaJogador = repositorioSenhaJogador;
        }

        public async Task<SenhaJogadorModel> GerarSenhaAsync(string steamId)
        {
            return await _repositorioSenhaJogador.GerarSenhaAsync(steamId);
        }

        public async Task<bool> VerificarAutenticacaoAsync(AutenticarJogadorCommand command)
        {
            return await _repositorioSenhaJogador.VerificarAutenticacaoAsync(command);
        }
    }
}