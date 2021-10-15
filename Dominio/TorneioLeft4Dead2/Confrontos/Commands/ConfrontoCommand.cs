using System;
using TorneioLeft4Dead2.Confrontos.Enums;

namespace TorneioLeft4Dead2.Confrontos.Commands
{
    public class ConfrontoCommand
    {
        public ConfrontoCommand()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public int? Rodada { get; set; }
        public DateTime? Data { get; set; }
        public StatusConfronto Status { get; set; }
        public int? CodigoCampanha { get; set; }
        public string CodigoTimeA { get; set; }
        public string CodigoTimeB { get; set; }
        public int? PontosConquistadosTimeA { get; set; }
        public int? PontosConquistadosTimeB { get; set; }
        public int? PenalidadeTimeA { get; set; }
        public int? PenalidadeTimeB { get; set; }
        public int? PenalidadePontosGeraisTimeA { get; set; }
        public int? PenalidadePontosGeraisTimeB { get; set; }
        public string Observacoes { get; set; }
    }
}