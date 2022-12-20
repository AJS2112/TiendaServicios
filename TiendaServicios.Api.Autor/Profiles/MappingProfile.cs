using AutoMapper;
using TiendaServicios.Api.Autor.Dtos;
using TiendaServicios.Api.Autor.Models;

namespace TiendaServicios.Api.Autor.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AutorLibro, AutorDto>();
        }
    }
}
