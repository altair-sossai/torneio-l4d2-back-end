using AutoMapper;
using TorneioLeft4Dead2.Confrontos.Entidades;
using TorneioLeft4Dead2.Confrontos.Models;

namespace TorneioLeft4Dead2.Confrontos.Profiles
{
    public class ConfrontoProfile : Profile
    {
        public ConfrontoProfile()
        {
            CreateMap<ConfrontoEntity, ConfrontoModel>();
        }
    }
}