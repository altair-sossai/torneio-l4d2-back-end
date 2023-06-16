using AutoMapper;
using TorneioLeft4Dead2.Estatisticas.Models;
using TorneioLeft4Dead2.Jogadores.Entidades;

namespace TorneioLeft4Dead2.Estatisticas.Profiles;

public class EstatisticasProfile : Profile
{
    public EstatisticasProfile()
    {
        CreateMap<JogadorEntity, JogadorModel>();
    }
}