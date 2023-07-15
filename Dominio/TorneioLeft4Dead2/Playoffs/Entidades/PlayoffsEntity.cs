using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Azure;
using Azure.Data.Tables;
using TorneioLeft4Dead2.Confrontos.Enums;
using TorneioLeft4Dead2.Playoffs.Enums;

namespace TorneioLeft4Dead2.Playoffs.Entidades;

public class PlayoffsEntity : ITableEntity
{
    public PlayoffsEntity()
    {
        PartitionKey = "shared";
        Id = Guid.NewGuid();
        IniciarConfrontos();
    }

    public Guid Id
    {
        get => Guid.TryParse(RowKey, out var id) ? id : Guid.Empty;
        set => RowKey = value.ToString().ToLower();
    }

    public int Rodada { get; set; }
    public int Ordem { get; set; }
    public string CodigoTimeA { get; set; }
    public string CodigoTimeB { get; set; }
    public int? CodigoCampanhaExcluidaTimeA { get; set; }
    public int? CodigoCampanhaExcluidaTimeB { get; set; }
    public int QuantidadeVitoriasTimeA { get; set; }
    public int QuantidadeVitoriasTimeB { get; set; }
    public int QuantidadeConfrontosRealizados => QuantidadeVitoriasTimeA + QuantidadeVitoriasTimeB;

    public bool VitoriaTimeA => QuantidadeVitoriasTimeA >= 2;
    public bool VitoriaTimeB => QuantidadeVitoriasTimeB >= 2;

    public int Status
    {
        get
        {
            if (QuantidadeConfrontosRealizados == 0)
                return (int)StatusPlayoffs.Aguardando;

            if (string.IsNullOrEmpty(CodigoTimeVencedor))
                return (int)StatusPlayoffs.EmAndamento;

            return (int)StatusPlayoffs.Finalizado;
        }
    }

    public string CodigoTimeVencedor
    {
        get
        {
            if (QuantidadeVitoriasTimeA >= 2)
                return CodigoTimeA;

            if (QuantidadeVitoriasTimeB >= 2)
                return CodigoTimeB;

            return null;
        }
    }

    public string CodigoTimePerdedor
    {
        get
        {
            if (QuantidadeVitoriasTimeA >= 2)
                return CodigoTimeB;

            if (QuantidadeVitoriasTimeB >= 2)
                return CodigoTimeA;

            return null;
        }
    }

    public int? Confronto01CodigoCampanha { get; set; }
    public DateTime? Confronto01Data { get; set; }
    public int? Confronto01Status { get; set; }
    public string Confronto01InicioEstatistica { get; set; }
    public string Confronto01FimEstatistica { get; set; }
    public int? Confronto01PontosConquistadosTimeA { get; set; }
    public int? Confronto01PontosConquistadosTimeB { get; set; }
    public int? Confronto01PenalidadeTimeA { get; set; }
    public int? Confronto01PenalidadeTimeB { get; set; }
    public bool? Confronto01TimeAVenceu { get; set; }
    public bool? Confronto01TimeBVenceu { get; set; }
    public string Confronto01Observacoes { get; set; }

    public int? Confronto02CodigoCampanha { get; set; }
    public DateTime? Confronto02Data { get; set; }
    public int? Confronto02Status { get; set; }
    public string Confronto02InicioEstatistica { get; set; }
    public string Confronto02FimEstatistica { get; set; }
    public int? Confronto02PontosConquistadosTimeA { get; set; }
    public int? Confronto02PontosConquistadosTimeB { get; set; }
    public int? Confronto02PenalidadeTimeA { get; set; }
    public int? Confronto02PenalidadeTimeB { get; set; }
    public bool? Confronto02TimeAVenceu { get; set; }
    public bool? Confronto02TimeBVenceu { get; set; }
    public string Confronto02Observacoes { get; set; }

    public int? Confronto03CodigoCampanha { get; set; }
    public DateTime? Confronto03Data { get; set; }
    public int? Confronto03Status { get; set; }
    public string Confronto03InicioEstatistica { get; set; }
    public string Confronto03FimEstatistica { get; set; }
    public int? Confronto03PontosConquistadosTimeA { get; set; }
    public int? Confronto03PontosConquistadosTimeB { get; set; }
    public int? Confronto03PenalidadeTimeA { get; set; }
    public int? Confronto03PenalidadeTimeB { get; set; }
    public bool? Confronto03TimeAVenceu { get; set; }
    public bool? Confronto03TimeBVenceu { get; set; }
    public string Confronto03Observacoes { get; set; }

    [IgnoreDataMember]
    public List<Confronto> Confrontos { get; set; }

    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public void IniciarConfrontos()
    {
        var confrontos = new List<Confronto>
        {
            new()
            {
                CodigoCampanha = Confronto01CodigoCampanha,
                Data = Confronto01Data,
                Status = Confronto01Status ?? 0,
                InicioEstatistica = Confronto01InicioEstatistica,
                FimEstatistica = Confronto01FimEstatistica,
                PontosConquistadosTimeA = Confronto01PontosConquistadosTimeA ?? 0,
                PontosConquistadosTimeB = Confronto01PontosConquistadosTimeB ?? 0,
                PenalidadeTimeA = Confronto01PenalidadeTimeA ?? 0,
                PenalidadeTimeB = Confronto01PenalidadeTimeB ?? 0,
                Observacoes = Confronto01Observacoes
            },
            new()
            {
                CodigoCampanha = Confronto02CodigoCampanha,
                Data = Confronto02Data,
                Status = Confronto02Status ?? 0,
                InicioEstatistica = Confronto02InicioEstatistica,
                FimEstatistica = Confronto02FimEstatistica,
                PontosConquistadosTimeA = Confronto02PontosConquistadosTimeA ?? 0,
                PontosConquistadosTimeB = Confronto02PontosConquistadosTimeB ?? 0,
                PenalidadeTimeA = Confronto02PenalidadeTimeA ?? 0,
                PenalidadeTimeB = Confronto02PenalidadeTimeB ?? 0,
                Observacoes = Confronto02Observacoes
            },
            new()
            {
                CodigoCampanha = Confronto03CodigoCampanha,
                Data = Confronto03Data,
                Status = Confronto03Status ?? 0,
                InicioEstatistica = Confronto03InicioEstatistica,
                FimEstatistica = Confronto03FimEstatistica,
                PontosConquistadosTimeA = Confronto03PontosConquistadosTimeA ?? 0,
                PontosConquistadosTimeB = Confronto03PontosConquistadosTimeB ?? 0,
                PenalidadeTimeA = Confronto03PenalidadeTimeA ?? 0,
                PenalidadeTimeB = Confronto03PenalidadeTimeB ?? 0,
                Observacoes = Confronto03Observacoes
            }
        };

        Confrontos = confrontos;
    }

    public void AtualizarDadosConfrontos()
    {
        QuantidadeVitoriasTimeA = Confrontos.Count(w => w.TimeAVenceu);
        QuantidadeVitoriasTimeB = Confrontos.Count(w => w.TimeBVenceu);

        var confronto01 = Confrontos.Count >= 1 ? Confrontos[0] : null;
        Confronto01CodigoCampanha = confronto01?.CodigoCampanha;
        Confronto01Data = confronto01?.Data;
        Confronto01Status = confronto01?.Status;
        Confronto01InicioEstatistica = confronto01?.InicioEstatistica;
        Confronto01FimEstatistica = confronto01?.FimEstatistica;

        if (Confronto01Status == (int?)StatusConfronto.Aguardando)
        {
            Confronto01PontosConquistadosTimeA = 0;
            Confronto01PontosConquistadosTimeB = 0;
            Confronto01PenalidadeTimeA = 0;
            Confronto01PenalidadeTimeB = 0;
        }
        else
        {
            Confronto01PontosConquistadosTimeA = confronto01?.PontosConquistadosTimeA;
            Confronto01PontosConquistadosTimeB = confronto01?.PontosConquistadosTimeB;
            Confronto01PenalidadeTimeA = confronto01?.PenalidadeTimeA;
            Confronto01PenalidadeTimeB = confronto01?.PenalidadeTimeB;
        }

        Confronto01TimeAVenceu = confronto01?.TimeAVenceu;
        Confronto01TimeBVenceu = confronto01?.TimeBVenceu;
        Confronto01Observacoes = confronto01?.Observacoes;

        var confronto02 = Confrontos.Count >= 2 ? Confrontos[1] : null;
        Confronto02CodigoCampanha = confronto02?.CodigoCampanha;
        Confronto02Data = confronto02?.Data;
        Confronto02Status = confronto02?.Status;
        Confronto02InicioEstatistica = confronto02?.InicioEstatistica;
        Confronto02FimEstatistica = confronto02?.FimEstatistica;

        if (Confronto02Status == (int?)StatusConfronto.Aguardando)
        {
            Confronto02PontosConquistadosTimeA = 0;
            Confronto02PontosConquistadosTimeB = 0;
            Confronto02PenalidadeTimeA = 0;
            Confronto02PenalidadeTimeB = 0;
        }
        else
        {
            Confronto02PontosConquistadosTimeA = confronto02?.PontosConquistadosTimeA;
            Confronto02PontosConquistadosTimeB = confronto02?.PontosConquistadosTimeB;
            Confronto02PenalidadeTimeA = confronto02?.PenalidadeTimeA;
            Confronto02PenalidadeTimeB = confronto02?.PenalidadeTimeB;
        }

        Confronto02TimeAVenceu = confronto02?.TimeAVenceu;
        Confronto02TimeBVenceu = confronto02?.TimeBVenceu;
        Confronto02Observacoes = confronto02?.Observacoes;

        var confronto03 = Confrontos.Count >= 3 ? Confrontos[2] : null;
        Confronto03CodigoCampanha = confronto03?.CodigoCampanha;

        if (QuantidadeConfrontosRealizados == 2 && (VitoriaTimeA || VitoriaTimeB))
        {
            Confronto03Data = DateTime.Now;
            Confronto03Status = (int?)StatusConfronto.Cancelado;
            Confronto03PontosConquistadosTimeA = 0;
            Confronto03PontosConquistadosTimeB = 0;
            Confronto03PenalidadeTimeA = 0;
            Confronto03PenalidadeTimeB = 0;
            Confronto03TimeAVenceu = false;
            Confronto03TimeBVenceu = false;
            Confronto03Observacoes = "O terceiro jogo não é necessário, vitória por 2x0";
        }
        else
        {
            Confronto03Data = confronto03?.Data;
            Confronto03Status = confronto03?.Status;
            Confronto03InicioEstatistica = confronto03?.InicioEstatistica;
            Confronto03FimEstatistica = confronto03?.FimEstatistica;

            if (Confronto03Status == (int?)StatusConfronto.Aguardando)
            {
                Confronto03PontosConquistadosTimeA = 0;
                Confronto03PontosConquistadosTimeB = 0;
                Confronto03PenalidadeTimeA = 0;
                Confronto03PenalidadeTimeB = 0;
            }
            else
            {
                Confronto03PontosConquistadosTimeA = confronto03?.PontosConquistadosTimeA;
                Confronto03PontosConquistadosTimeB = confronto03?.PontosConquistadosTimeB;
                Confronto03PenalidadeTimeA = confronto03?.PenalidadeTimeA;
                Confronto03PenalidadeTimeB = confronto03?.PenalidadeTimeB;
            }

            Confronto03TimeAVenceu = confronto03?.TimeAVenceu;
            Confronto03TimeBVenceu = confronto03?.TimeBVenceu;
            Confronto03Observacoes = confronto03?.Observacoes;
        }
    }

    public class Confronto
    {
        public int? CodigoCampanha { get; set; }
        public DateTime? Data { get; set; }
        public int Status { get; set; }
        public string InicioEstatistica { get; set; }
        public string FimEstatistica { get; set; }
        public int PontosConquistadosTimeA { get; set; }
        public int PontosConquistadosTimeB { get; set; }
        public int PenalidadeTimeA { get; set; }
        public int PenalidadeTimeB { get; set; }

        public bool TimeAVenceu
        {
            get
            {
                var pontosTimeA = PontosConquistadosTimeA - PenalidadeTimeA;
                var pontosTimeB = PontosConquistadosTimeB - PenalidadeTimeB;

                return pontosTimeA > pontosTimeB;
            }
        }

        public bool TimeBVenceu
        {
            get
            {
                var pontosTimeA = PontosConquistadosTimeA - PenalidadeTimeA;
                var pontosTimeB = PontosConquistadosTimeB - PenalidadeTimeB;

                return pontosTimeB > pontosTimeA;
            }
        }

        public string Observacoes { get; set; }
    }
}