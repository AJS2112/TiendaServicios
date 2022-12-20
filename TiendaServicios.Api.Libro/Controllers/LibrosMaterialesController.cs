using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaServicios.Api.Libro.Application;
using TiendaServicios.Api.Libro.Dtos;

namespace TiendaServicios.Api.Libro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosMaterialesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibrosMaterialesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibreriaMaterialDto>>> ObtenerLibros()
        {
            return await _mediator.Send(new Consulta.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LibreriaMaterialDto>> ObtenerLibroPorId(Guid? id)
        {
            return await _mediator.Send(new ConsultaFiltro.Query() { LibroId = id });
        }
    }
}
