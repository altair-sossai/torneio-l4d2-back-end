using System;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Confrontos.Entidades
{
    public class ConfrontoEntity : TableEntity
    {
        public int Rodada
        {
            get => int.TryParse(PartitionKey, out var rodada) ? rodada : 0;
            set => PartitionKey = value.ToString();
        }

        public DateTime? Data { get; set; }
        public int Status { get; set; }
        public int Campanha { get; set; }

        public string TimeA
        {
            get => RowKey?.Split('_').FirstOrDefault();
            set => RowKey = $"{value}_{TimeB}";
        }

        public string TimeB
        {
            get => RowKey?.Split('_').LastOrDefault();
            set => RowKey = $"{TimeA}_{value}";
        }

        public int PontosConquistadosTimeA { get; set; }
        public int PontosConquistadosTimeB { get; set; }
        public int PenalidadeTimeA { get; set; }
        public int PenalidadeTimeB { get; set; }
        public string MotivoPenalidadeTimeA { get; set; }
        public string MotivoPenalidadeTimeB { get; set; }
    }
}