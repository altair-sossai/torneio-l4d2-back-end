using AutoMapper;
using TorneioLeft4Dead2.Playoffs.Commands;
using TorneioLeft4Dead2.Playoffs.Entidades;
using TorneioLeft4Dead2.Playoffs.Models;

namespace TorneioLeft4Dead2.Playoffs.Profiles
{
    public class PlayoffsProfile : Profile
    {
        public PlayoffsProfile()
        {
            CreateMap<PlayoffsEntity, PlayoffsModel>();
            CreateMap<PlayoffsEntity.Confronto, PlayoffsModel.Confronto>();

            CreateMap<PlayoffsCommand, PlayoffsEntity>();
            CreateMap<PlayoffsCommand.Confronto, PlayoffsEntity.Confronto>();
        }
    }
}