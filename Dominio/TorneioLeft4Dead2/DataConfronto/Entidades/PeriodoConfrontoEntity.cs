using System;
using Azure;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.DataConfronto.Entidades;

public class PeriodoConfrontoEntity : ITableEntity
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
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}