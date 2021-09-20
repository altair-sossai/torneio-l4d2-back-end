using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Jogadores.Entidades
{
    public class JogadorEntity : TableEntity
    {
        public JogadorEntity()
        {
            PartitionKey = "shared";
        }

        public string SteamId
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string Nome { get; set; }
        public string UrlFotoPerfil { get; set; }
        public string UrlPerfilSteam { get; set; }
        public int? TotalHoras { get; set; }
    }
}