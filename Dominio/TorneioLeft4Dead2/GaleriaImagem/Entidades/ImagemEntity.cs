using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Flurl;

namespace TorneioLeft4Dead2.GaleriaImagem.Entidades
{
    public class ImagemEntity
    {
        public ImagemEntity(BlobContainerClient blobContainerClient, BlobItem blobItem)
        {
            NomeArquivo = blobItem.Name;
            UrlDownload = Url.Combine(blobContainerClient.Uri.AbsoluteUri, blobItem.Name);
        }

        public string NomeArquivo { get; }
        public string UrlDownload { get; }
    }
}