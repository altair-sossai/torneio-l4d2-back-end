using AutoMapper;
using TorneioLeft4Dead2.Times.Commands;
using TorneioLeft4Dead2.Times.Entidades;

namespace TorneioLeft4Dead2.Times.Profiles
{
    public class TimeProfile : Profile
    {
        public TimeProfile()
        {
            CreateMap<TimeCommand, TimeEntity>();
            CreateMap<TimeJogadorCommand, TimeJogadorEntity>();
        }
    }
}