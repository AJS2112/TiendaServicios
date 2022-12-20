using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Dtos;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Repository;

namespace TiendaServicios.Api.Libro.Application
{
    public class ConsultaFiltro
    {
        public class Query: IRequest<LibreriaMaterialDto>
        {
            public Guid? LibroId { get; set; }
        }

        public class Handler : IRequestHandler<Query, LibreriaMaterialDto>
        {
            private readonly ContextoLibreria _contexto;
            private readonly IMapper _mapper;

            public Handler(ContextoLibreria contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }
    
            public async Task<LibreriaMaterialDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales.Where(x => x.LibreriaMaterialId == request.LibroId).FirstOrDefaultAsync();

                if (libro == null)
                {
                    throw new Exception("No se encontró el libro");
                }

                var libroDto = _mapper.Map<LibreriaMaterial, LibreriaMaterialDto>(libro);

                return libroDto;
            }
        }
    }
}
