using System;
using System.Collections.Generic;

namespace TorneioLeft4Dead2.DataConfronto.Commands
{
    public class PeriodoConfrontoCommand
    {
        public PeriodoConfrontoCommand()
        {
            Sugestoes = new List<SugestaoDataConfrontoCommand>();
        }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public List<SugestaoDataConfrontoCommand> Sugestoes { get; set; }
    }
}