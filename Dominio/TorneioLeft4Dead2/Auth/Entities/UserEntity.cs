using System;
using System.Text.Json.Serialization;
using Azure;
using Azure.Data.Tables;

namespace TorneioLeft4Dead2.Auth.Entities;

public sealed class UserEntity : ITableEntity
{
    public UserEntity()
    {
        PartitionKey = "shared";
        Id = Guid.NewGuid();
    }

    public Guid Id
    {
        get => Guid.TryParse(RowKey, out var id) ? id : Guid.Empty;
        set => RowKey = value.ToString().ToLower();
    }

    public string Name { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public string Password { get; set; }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}