using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Autor.Models;

namespace TiendaServicios.Api.Autor.Repository
{
    public class ContextoAutor : DbContext
    {
        public ContextoAutor(DbContextOptions<ContextoAutor> options):base(options){}

        public DbSet<AutorLibro> AutoresLibros { get; set; }
        public DbSet<GradoAcademico> GradosAcademicos { get; set; }

    }
}
