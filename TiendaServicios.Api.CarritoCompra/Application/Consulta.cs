using MediatR;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Dtos;
using TiendaServicios.Api.CarritoCompra.Repository;
using TiendaServicios.Api.CarritoCompra.ResourceInterfaces;

namespace TiendaServicios.Api.CarritoCompra.Application
{
    public class Consulta
    {
        public class Command : IRequest<CarritoDto>
        {
            public int CarritoSesionId { get; set; }
        }

        public class Handler : IRequestHandler<Command, CarritoDto>
        {
            private readonly ContextoCarrito _contexto;
            private readonly ILibrosService _librosService;
            public Handler(ContextoCarrito contexto, ILibrosService librosService)
            {
                _contexto = contexto;
                _librosService = librosService;
            }

            public async Task<CarritoDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var carritoSesion = await _contexto.CarritoSesiones.FirstOrDefaultAsync(x => x.CarritoSesionId == request.CarritoSesionId);
                var carritoSesionDetalle = await _contexto.CarritoSesionDetalles.Where(x => x.CarritoSesionId == request.CarritoSesionId).ToListAsync();

                var listaCarritoDto = new List<CarritoDetalleDto>();    

                foreach (var libro in carritoSesionDetalle)
                {
                    var response = await _librosService.GetLibro(new Guid(libro.ProductoSeleccionado));
                    if (response.resultado)
                    {
                        var objetoLibro = response.libro;
                        var carritoDetalle = new CarritoDetalleDto
                        {
                            TituloLibro = objetoLibro.Titulo,
                            FechaPublicacion = objetoLibro.FechaPublicacion,
                            LibroId = objetoLibro.LibreriaMaterialId.Value
                        };
                        listaCarritoDto.Add(carritoDetalle);
                    }
                }

                var carritoSesionDto = new CarritoDto
                {
                    CarritoId = carritoSesion.CarritoSesionId,
                    FechaCreacionSesion = carritoSesion.FechaCreacion,
                    ListaProductos = listaCarritoDto
                };

                return carritoSesionDto;

            }
        }
    }
}
