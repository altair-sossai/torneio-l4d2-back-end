using System;
using System.Collections.Generic;
using TorneioLeft4Dead2.Confrontos.Enums;

namespace TorneioLeft4Dead2.Playoffs.Commands
{
    public class PlayoffsCommand
    {
        public PlayoffsCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int Rodada { get; set; }
        public int Ordem { get; set; }
        public string CodigoTimeA { get; set; }
        public string CodigoTimeB { get; set; }
        public List<Confronto> Confrontos { get; set; }

        public class Confronto
        {
            public int? CodigoCampanha { get; set; }
            public DateTime? Data { get; set; }
            public StatusConfronto Status { get; set; }
            public int PontosConquistadosTimeA { get; set; }
            public int PontosConquistadosTimeB { get; set; }
            public int PenalidadeTimeA { get; set; }
            public int PenalidadeTimeB { get; set; }
            public string Observacoes { get; set; }
        }
    }
}