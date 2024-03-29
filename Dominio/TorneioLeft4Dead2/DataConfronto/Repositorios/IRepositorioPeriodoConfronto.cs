﻿using System;
using System.Threading.Tasks;
using TorneioLeft4Dead2.DataConfronto.Entidades;

namespace TorneioLeft4Dead2.DataConfronto.Repositorios;

public interface IRepositorioPeriodoConfronto
{
    Task<PeriodoConfrontoEntity> ObterPorConfrontoAsync(Guid confrontoId);
    Task SalvarAsync(PeriodoConfrontoEntity entity);
}