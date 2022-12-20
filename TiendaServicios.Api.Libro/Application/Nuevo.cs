using FluentValidation;
using MediatR;
using System.Drawing;
using TiendaServicios.Api.Libro.Models;
using TiendaServicios.Api.Libro.Repository;

namespace TiendaServicios.Api.Libro.Application
{
    public class Nuevo
    {
        public class Command : IRequest
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Command>
        {
            public EjecutaValidacion()
            {
                RuleFor(c => c.Titulo).NotEmpty();
                RuleFor(c => c.FechaPublicacion).NotEmpty();
                RuleFor(c => c.AutorLibro).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ContextoLibreria _contexto;

            public Handler(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro
                };

                _contexto.LibreriasMateriales.Add(libro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No fue posible insertar el libro");
            }
        }
    }
}
