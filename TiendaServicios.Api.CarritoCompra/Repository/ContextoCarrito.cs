using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Models;

namespace TiendaServicios.Api.CarritoCompra.Repository
{
    public class ContextoCarrito : DbContext
    {
        public ContextoCarrito(DbContextOptions options) : base(options){}

        public DbSet<CarritoSesion> CarritoSesiones { get; set; }
        public DbSet<CarritoSesionDetalle> CarritoSesionDetalles { get; set; }
    }
}
