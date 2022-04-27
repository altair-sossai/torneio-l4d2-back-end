using System;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.DataConfronto.Entidades
{
    public class PeriodoConfrontoEntity : TableEntity
    {
        public PeriodoConfrontoEntity()
        {
            PartitionKey = "shared";
        }

        public Guid ConfrontoId
        {
            get => Guid.TryParse(RowKey, out var confrontoId) ? confrontoId : Guid.Empty;
            set => RowKey = value.ToString().ToLower();
        }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}