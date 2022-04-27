using System;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.DataConfronto.Entidades
{
    public class SugestaoDataConfrontoEntity : TableEntity
    {
        public SugestaoDataConfrontoEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid ConfrontoId
        {
            get => Guid.TryParse(PartitionKey, out var confrontoId) ? confrontoId : Guid.Empty;
            set => PartitionKey = value.ToString().ToLower();
        }

        public Guid Id
        {
            get => Guid.TryParse(RowKey, out var id) ? id : Guid.Empty;
            set => RowKey = value.ToString().ToLower();
        }

        public DateTime Data { get; set; }
        public int CadastradoPor { get; set; }
        public int RespostaTimeA { get; set; }
        public int RespostaTimeB { get; set; }
    }
}