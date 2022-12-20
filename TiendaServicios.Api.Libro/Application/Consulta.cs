using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Dtos;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Repository;

namespace TiendaServicios.Api.Libro.Application
{
    public class Consulta
    {
        public class Query : IRequest<List<LibreriaMaterialDto>> { }

        public class Handler : IRequestHandler<Query, List<LibreriaMaterialDto>>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Handler(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<LibreriaMaterialDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var libros = await _contexto.LibreriasMateriales.ToListAsync();
                var librosDto = _mapper.Map<List<LibreriaMaterial>,List<LibreriaMaterialDto>>(libros);
                return librosDto;
            }
        }
    }
}
