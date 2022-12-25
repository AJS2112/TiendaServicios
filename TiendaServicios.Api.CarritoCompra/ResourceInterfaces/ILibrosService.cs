using TiendaServicios.Api.CarritoCompra.ResourceModels;

namespace TiendaServicios.Api.CarritoCompra.ResourceInterfaces
{
    public interface ILibrosService
    {
        Task<(bool resultado, LibroResource libro, string errorMessage)> GetLibro(Guid libroId);
    }
}
