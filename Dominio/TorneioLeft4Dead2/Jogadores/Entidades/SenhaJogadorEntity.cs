using System;
using Azure;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.Jogadores.Entidades;

public class SenhaJogadorEntity : ITableEntity
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
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}