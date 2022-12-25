using System.Text.Json;
using TiendaServicios.Api.CarritoCompra.ResourceInterfaces;
using TiendaServicios.Api.CarritoCompra.ResourceModels;

namespace TiendaServicios.Api.CarritoCompra.ResourceServices
{
    public class LibrosService : ILibrosService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger<LibrosService> _logger;

        public LibrosService(IHttpClientFactory httpClient, ILogger<LibrosService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
    
        public async Task<(bool resultado, LibroResource libro, string errorMessage)> GetLibro(Guid libroId)
        {
            try
            {
                var cliente = _httpClient.CreateClient("Libros");
                var response = await cliente.GetAsync($"api/LibrosMateriales/{libroId}");
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<LibroResource>(contenido, options);

                    return (true, resultado, null);
                }

                return (false, null, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
