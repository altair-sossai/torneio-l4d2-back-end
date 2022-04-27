using AutoMapper;
using TorneioLeft4Dead2.DataConfronto.Commands;
using TorneioLeft4Dead2.DataConfronto.Entidades;
using TorneioLeft4Dead2.DataConfronto.Models;

namespace TorneioLeft4Dead2.DataConfronto.Profiles
{
    public class DataConfrontoProfile : Profile
    {
        public DataConfrontoProfile()
        {
            CreateMap<PeriodoConfrontoCommand, PeriodoConfrontoEntity>();
            CreateMap<SugestaoDataConfrontoCommand, SugestaoDataConfrontoEntity>();

            CreateMap<PeriodoConfrontoEntity, PeriodoConfrontoModel>();
            CreateMap<SugestaoDataConfrontoEntity, SugestaoDataConfrontoModel>();
        }
    }
}