using FluentValidation;
using MediatR;
using TiendaServicios.Api.Autor.Models;
using TiendaServicios.Api.Autor.Repository;

namespace TiendaServicios.Api.Autor.Application
{
    public class Nuevo
    {
        public class Command : IRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            public readonly ContextoAutor _contexto;

            public Handler(ContextoAutor contexto)
            {
                _contexto = contexto;
            }

            public class EjecutaValidacion : AbstractValidator<Command>
            {
                public EjecutaValidacion()
                {
                    RuleFor(x => x.Nombre).NotEmpty();
                    RuleFor(x => x.Apellido).NotEmpty();    
                }
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var autorLibro = new AutorLibro
                {
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    FechaNacimiento = request.FechaNacimiento,
                    AutorLibroGuid = Guid.NewGuid().ToString()
                };

                _contexto.AutoresLibros.Add(autorLibro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor>0)
                {
                    return Unit.Value;
                }

                throw new Exception("No fue posible insertar el autor");
            }
        }
    }
}
