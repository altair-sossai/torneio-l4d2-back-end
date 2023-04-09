using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TorneioLeft4Dead2.Confrontos.Entidades;

namespace TorneioLeft4Dead2.Confrontos.Repositorios;

public interface IRepositorioConfronto
{
    Task<ConfrontoEntity> ObterPorIdAsync(Guid confrontoId);
    Task<List<ConfrontoEntity>> ObterConfrontosAsync();
    Task SalvarAsync(ConfrontoEntity entity);
    Task ExcluirTudoAsync();
    Task ExcluirAsync(Guid confrontoId);
}