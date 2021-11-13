using System.Collections.Generic;
using TorneioLeft4Dead2.GaleriaImagem.Entidades;

namespace TorneioLeft4Dead2.GaleriaImagem.Repositorios
{
    public interface IRepositorioGaleriaImagem
    {
        IAsyncEnumerable<ImagemEntity> ObterImagensAsync(string galeriaId);
    }
}