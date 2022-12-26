namespace TiendaServicos.Api.Gateway.ResourceModels
{
    public class LibroResourceModel
    {
        public Guid? LibreriaMaterialId { get; set; }
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public Guid? AutorLibro { get; set; }

        public AutorResourceModel AutorData { get; set; }
    }
}
