using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Playoffs.Entidades;

namespace TorneioLeft4Dead2.Playoffs.Repositorios;

public interface IRepositorioPlayoffs
{
    Task<PlayoffsEntity> ObterPorIdAsync(Guid playoffsId);
    Task<List<PlayoffsEntity>> ObterPlayoffsAsync();
    Task SalvarAsync(PlayoffsEntity entity);
    Task ExcluirAsync(Guid playoffsId);
}