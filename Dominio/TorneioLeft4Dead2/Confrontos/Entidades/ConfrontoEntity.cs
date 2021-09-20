using System;
using Microsoft.Azure.Cosmos.Table;
using TorneioLeft4Dead2.Confrontos.Enums;

namespace TorneioLeft4Dead2.Confrontos.Entidades
{
    public class ConfrontoEntity : TableEntity
    {
        public ConfrontoEntity()
        {
            PartitionKey = "shared";
        }

        public int Id
        {
            get => int.TryParse(RowKey, out var id) ? id : 0;
            set => RowKey = value.ToString();
        }

        public int Rodada { get; set; }
        public DateTime Data { get; set; }
        public StatusConfronto Status { get; set; }
        public string Campanha { get; set; }
        public string TimeA { get; set; }
        public string TimeB { get; set; }
        public int PontosConquistadosTimeA { get; set; }
        public int PontosConquistadosTimeB { get; set; }
        public int PenalidadeTimeA { get; set; }
        public int PenalidadeTimeB { get; set; }
        public string MotivoPenalidadeTimeA { get; set; }
        public string MotivoPenalidadeTimeB { get; set; }
    }
}