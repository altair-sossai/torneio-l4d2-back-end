using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Playoffs.Commands;
using TorneioLeft4Dead2.Playoffs.Entidades;
using TorneioLeft4Dead2.Playoffs.Models;

namespace TorneioLeft4Dead2.Playoffs.Servicos;

public interface IServicoPlayoffs
{
    Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId);
    Task<List<RodadaModel>> ObterRodadasAsync();
    Task<List<PlayoffsModel>> ObterPlayoffsAsync();
    Task<PlayoffsEntity> SalvarAsync(PlayoffsCommand command);
    Task ExcluirAsync(Guid playoffsId);
}