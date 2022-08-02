using System;
using System.Collections.Generic;
using TorneioLeft4Dead2.Campanhas.Entidades;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Playoffs.Enums;
using TorneioLeft4Dead2.Times.Models;

namespace TorneioLeft4Dead2.Playoffs.Models
{
    public class PlayoffsModel
    {
        public Guid Id { get; set; }
        public int Rodada { get; set; }
        public int Ordem { get; set; }
        public string CodigoTimeA { get; set; }
        public string CodigoTimeB { get; set; }
        public TimeModel TimeA { get; set; }
        public TimeModel TimeB { get; set; }
        public int? CodigoCampanhaExcluidaTimeA { get; set; }
        public int? CodigoCampanhaExcluidaTimeB { get; set; }
        public CampanhaEntity CampanhaExcluidaTimeA { get; set; }
        public CampanhaEntity CampanhaExcluidaTimeB { get; set; }
        public int QuantidadeVitoriasTimeA { get; set; }
        public int QuantidadeVitoriasTimeB { get; set; }
        public int QuantidadeConfrontosRealizados { get; set; }
        public StatusPlayoffs Status { get; set; }
        public string CodigoTimeVencedor { get; set; }
        public string CodigoTimePerdedor { get; set; }
        public TimeModel TimeVencedor { get; set; }
        public TimeModel TimePerdedor { get; set; }
        public List<Confronto> Confrontos { get; set; }

        public class Confronto
        {
            public int? CodigoCampanha { get; set; }
            public CampanhaEntity Campanha { get; set; }
            public DateTime? Data { get; set; }
            public StatusConfronto Status { get; set; }
            public int PontosConquistadosTimeA { get; set; }
            public int PontosConquistadosTimeB { get; set; }
            public int PenalidadeTimeA { get; set; }
            public int PenalidadeTimeB { get; set; }
            public bool TimeAVenceu { get; set; }
            public bool TimeBVenceu { get; set; }
            public string Observacoes { get; set; }
        }
    }
}