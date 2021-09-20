﻿using System.Threading.Tasks;
using TorneioLeft4Dead2.Jogadores.Commands;
using TorneioLeft4Dead2.Jogadores.Entidades;

namespace TorneioLeft4Dead2.Jogadores.Servicos
{
    public interface IServicoJogador
    {
        Task<JogadorEntity> SalvarAsync(JogadorCommand command);
    }
}