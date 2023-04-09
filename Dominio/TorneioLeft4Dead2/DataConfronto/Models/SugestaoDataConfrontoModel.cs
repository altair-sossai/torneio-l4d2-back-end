using System;
using TorneioLeft4Dead2.DataConfronto.Enums;

namespace TorneioLeft4Dead2.DataConfronto.Models;

public class SugestaoDataConfrontoModel
{
    public Guid Id { get; set; }
    public Guid ConfrontoId { get; set; }
    public DateTime Data { get; set; }
    public CadastradoPor CadastradoPor { get; set; }
    public RespostaTime RespostaTimeA { get; set; }
    public RespostaTime RespostaTimeB { get; set; }
}