using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Times.Entidades
{
    public class TimeJogadorEntity : TableEntity
    {
        public TimeJogadorEntity()
        {
            PartitionKey = "shared";
        }

        public string Time
        {
            get => RowKey?.Split('_').FirstOrDefault();
            set => RowKey = $"{value}_{Jogador}";
        }

        public string Jogador
        {
            get => RowKey?.Split('_').LastOrDefault();
            set => RowKey = $"{Time}_{value}";
        }

        public int Ordem { get; set; }
    }
}