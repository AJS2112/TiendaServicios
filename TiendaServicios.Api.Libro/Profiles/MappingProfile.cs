using AutoMapper;
using TiendaServicios.Api.Libro.Controllers;
using TiendaServicios.Api.Libro.Dtos;
using TiendaServicios.Api.Libro.Models;

namespace TiendaServicios.Api.Libro.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LibreriaMaterial, LibreriaMaterialDto>();
        }
    }
}
