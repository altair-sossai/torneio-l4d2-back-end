using System;

namespace TorneioLeft4Dead2.DataConfronto.Commands
{
    public class SugestaoDataConfrontoCommand
    {
        public DateTime Data { get; set; }
        public int CadastradoPor { get; set; }
        public int RespostaTimeA { get; set; }
        public int RespostaTimeB { get; set; }
    }
}