﻿using System;
using System.Linq;
using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Confrontos.Entidades
{
    public class ConfrontoEntity : TableEntity
    {
        public int Rodada
        {
            get => int.TryParse(PartitionKey, out var rodada) ? rodada : 0;
            set => PartitionKey = value.ToString();
        }

        public DateTime? Data { get; set; }
        public int Status { get; set; }
        public int CodigoCampanha { get; set; }

        public string CodigoTimeA
        {
            get => RowKey?.Split('_').FirstOrDefault();
            set => RowKey = $"{value}_{CodigoTimeB}";
        }

        public string CodigoTimeB
        {
            get => RowKey?.Split('_').LastOrDefault();
            set => RowKey = $"{CodigoTimeA}_{value}";
        }

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
    }
}