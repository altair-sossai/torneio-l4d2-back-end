using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Times.Entidades
{
    public class TimeEntity : TableEntity
    {
        public TimeEntity()
        {
            PartitionKey = "shared";
        }

        public string Nome
        {
            get => RowKey;
            set => RowKey = value;
        }

        public int PontosGerais => QuantidadeVitorias * 3 + QuantidadeEmpates;
        public int QuantidadePartidasRealizadas { get; set; }
        public int QuantidadeVitorias { get; set; }
        public int QuantidadeEmpates { get; set; }
        public int QuantidadeDerrotas { get; set; }
        public int TotalPontosConquistados { get; set; }
        public int TotalPontosSofridos { get; set; }
        public int TotalPenalidades { get; set; }
        public int SaldoTotalPontos => TotalPontosConquistados - TotalPontosSofridos - TotalPenalidades;
    }
}