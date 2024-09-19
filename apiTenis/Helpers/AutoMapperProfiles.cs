using apiTenis.Entities;
using apiTenis.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace apiTenis.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<IdentityUser, UsuarioDTO>();

            CreateMap<Entities.Jugador, Models.JugadorDTO>().ReverseMap();
            CreateMap<Entities.Torneo, Models.TorneoDTO>()
                .ForMember(x => x.JugadorGanadorName, x => x.MapFrom(y => $"{y.JugadorGanador.Apellido} {y.JugadorGanador.Nombre}"))
                .ReverseMap();
            CreateMap<TorneoCrearDTO, Torneo>();
        }    
    }
}
