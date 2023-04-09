using System;
using TorneioLeft4Dead2.DataConfronto.Enums;

namespace TorneioLeft4Dead2.DataConfronto.Commands;

public class ResponderSugestaoDataCommand
{
    public Guid ConfrontoId { get; set; }
    public Guid SugestaoId { get; set; }
    public string SteamId { get; set; }
    public RespostaTime Resposta { get; set; }
}