using System;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace TorneioLeft4Dead2.Auth.Entities
{
    public sealed class UserEntity : TableEntity
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
    }
}