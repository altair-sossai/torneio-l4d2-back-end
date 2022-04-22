using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Jogadores.Entidades
{
    public class SenhaJogadorEntity : TableEntity
    {
        public SenhaJogadorEntity()
        {
            PartitionKey = "shared";
        }

        public string SteamId
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string SenhaCriptografada { get; set; }
    }
}