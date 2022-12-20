using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TiendaServicios.Api.Autor.Dtos;
using TiendaServicios.Api.Autor.Models;
using TiendaServicios.Api.Autor.Repository;

namespace TiendaServicios.Api.Autor.Application
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>>
        {

        }

        public class Handler : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            public readonly ContextoAutor _contexto;
            public readonly IMapper _mapper;

            public Handler(ContextoAutor contexto, IMapper mapper)
            {
                _contexto = contexto;
                _mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores = await _contexto.AutoresLibros.ToListAsync();
                var autoresDto = _mapper.Map<List<AutorLibro>, List< AutorDto> > (autores);
                return autoresDto;
            }
        }
    }
}
