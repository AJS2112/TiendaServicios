using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Models;

namespace TiendaServicios.Api.Libro.Repository
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria(DbContextOptions options) : base(options){}

        public DbSet<LibreriaMaterial> LibreriasMateriales { get; set; }
    }
}
