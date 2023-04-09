using System;
using Azure;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.Jogadores.Entidades;

public class JogadorEntity : ITableEntity
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
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}