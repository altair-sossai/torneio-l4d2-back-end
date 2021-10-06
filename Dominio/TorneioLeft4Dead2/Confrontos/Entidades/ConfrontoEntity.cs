using System;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Confrontos.Entidades
{
    public class ConfrontoEntity : TableEntity
    {
        public ConfrontoEntity()
        {
            PartitionKey = "shared";
            Id = Guid.NewGuid();
        }


        public Guid Id
        {
            get => Guid.TryParse(RowKey, out var id) ? id : Guid.Empty;
            set => RowKey = value.ToString().ToLower();
        }

        public int Rodada { get; set; }
        public DateTime? Data { get; set; }
        public int Status { get; set; }
        public int? CodigoCampanha { get; set; }
        public string CodigoTimeA { get; set; }
        public string CodigoTimeB { get; set; }

        public string CodigoTimeVencedor
        {
            get
            {
                var pontosTimeA = PontosConquistadosTimeA - PenalidadeTimeA;
                var pontosTimeB = PontosConquistadosTimeB - PenalidadeTimeB;

                if (pontosTimeA > pontosTimeB)
                    return CodigoTimeA;

                if (pontosTimeB > pontosTimeA)
                    return CodigoTimeB;

                return null;
            }
        }

        public int PontosConquistadosTimeA { get; set; }
        public int PontosConquistadosTimeB { get; set; }
        public int PenalidadeTimeA { get; set; }
        public int PenalidadeTimeB { get; set; }
        public string Observacoes { get; set; }

        public void ZerarPontuacao()
        {
            PontosConquistadosTimeA = 0;
            PontosConquistadosTimeB = 0;
            PenalidadeTimeA = 0;
            PenalidadeTimeB = 0;
        }
    }
}