using System.Collections.Generic;
using System.Linq;
using TorneioLeft4Dead2.GaleriaImagem.Entidades;
using TorneioLeft4Dead2.GaleriaImagem.Repositorios;
using TorneioLeft4Dead2.Storage.UnitOfWork;

namespace TorneioLeft4Dead2.Storage.GaleriaImagem.Repositorios
{
    public class RepositorioGaleriaImagemStorage : IRepositorioGaleriaImagem
    {
        private readonly UnitOfWorkStorage _unitOfWork;

        public RepositorioGaleriaImagemStorage(UnitOfWorkStorage unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async IAsyncEnumerable<ImagemEntity> ObterImagensAsync(string galeriaId)
        {
            var blobServiceClient = _unitOfWork.BlobServiceClient;
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(galeriaId);
            var blobs = blobContainerClient.GetBlobsAsync();

            foreach (var blobItem in await blobs.ToListAsync())
                yield return new ImagemEntity(blobContainerClient, blobItem);
        }
    }
}