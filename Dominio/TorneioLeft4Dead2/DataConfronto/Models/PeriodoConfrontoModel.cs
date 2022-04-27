using System;
using System.Collections.Generic;

namespace TorneioLeft4Dead2.DataConfronto.Models
{
    public class PeriodoConfrontoModel
    {
        public PeriodoConfrontoModel()
        {
            Sugestoes = new List<SugestaoDataConfrontoModel>();
        }

        public Guid ConfrontoId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public List<SugestaoDataConfrontoModel> Sugestoes { get; set; }
    }
}