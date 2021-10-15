using Microsoft.Azure.Cosmos.Table;

namespace TorneioLeft4Dead2.Times.Entidades
{
    public class TimeEntity : TableEntity
    {
        public TimeEntity()
        {
            PartitionKey = "shared";
        }

        public string Codigo
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string Nome { get; set; }
        public int PontosGerais => QuantidadeVitorias * 3 + QuantidadeEmpates - TotalPenalidadePontosGerais;
        public int QuantidadePartidasRealizadas { get; set; }
        public int QuantidadeVitorias { get; set; }
        public int QuantidadeEmpates { get; set; }
        public int QuantidadeDerrotas { get; set; }
        public int TotalPontosConquistados { get; set; }
        public int TotalPontosSofridos { get; set; }
        public int TotalPenalidades { get; set; }
        public int TotalPenalidadePontosGerais { get; set; }
        public int SaldoTotalPontos => TotalPontosConquistados - TotalPontosSofridos - TotalPenalidades;

        public void ZerarPontuacao()
        {
            QuantidadePartidasRealizadas = 0;
            QuantidadeVitorias = 0;
            QuantidadeEmpates = 0;
            QuantidadeDerrotas = 0;
            TotalPontosConquistados = 0;
            TotalPontosSofridos = 0;
            TotalPenalidades = 0;
        }
    }
}