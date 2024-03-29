﻿using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Models;

namespace TorneioLeft4Dead2.Jogadores.Servicos;

public interface IServicoSenhaJogador
{
    Task<SenhaJogadorModel> GerarSenhaAsync(string steamId);
    Task<bool> AutenticadoAsync(AutenticarJogadorCommand command);
}