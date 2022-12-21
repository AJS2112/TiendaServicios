using MediatR;
using TiendaServicios.Api.CarritoCompra.Models;
using TiendaServicios.Api.CarritoCompra.Repository;

namespace TiendaServicios.Api.CarritoCompra.Application
{
    public class Nuevo
    {
        public class Command : IRequest
        {
            public DateTime? FechaCreacion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ContextoCarrito _contexto;

            public Handler(ContextoCarrito contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacion
                };
                _contexto.CarritoSesiones.Add(carritoSesion);
                var value = await _contexto.SaveChangesAsync();

                if (value == 0)
                {
                    throw new Exception("Errores en la creacion del carrito de compras");
                }

                int idCarritoSesion = carritoSesion.CarritoSesionId;

                foreach (var item in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = idCarritoSesion,
                        ProductoSeleccionado = item
                    };

                    _contexto.CarritoSesionDetalles.Add(detalleSesion);
                }
                value = await _contexto.SaveChangesAsync();
                if (value == 0)
                {
                    throw new Exception("Errores en la insercion de detalles del carrito de compras");
                }

                return Unit.Value;
            }
        }
    }
}
