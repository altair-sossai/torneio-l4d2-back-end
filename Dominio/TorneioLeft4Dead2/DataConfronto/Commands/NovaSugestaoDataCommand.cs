using System;

namespace TorneioLeft4Dead2.DataConfronto.Commands
{
    public class NovaSugestaoDataCommand
    {
        public Guid ConfrontoId { get; set; }
        public string SteamId { get; set; }
        public DateTime Data { get; set; }
    }
}