using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Models;

namespace TiendaServicios.Api.Libro.Repository
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria()
        {

        }
        public ContextoLibreria(DbContextOptions options) : base(options){}

        public virtual DbSet<LibreriaMaterial> LibreriasMateriales { get; set; }
    }
}
