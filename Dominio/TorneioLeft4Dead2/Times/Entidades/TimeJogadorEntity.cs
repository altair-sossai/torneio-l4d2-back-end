using System;
using System.Linq;
using Azure;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.Times.Entidades;

public class TimeJogadorEntity : ITableEntity
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

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}