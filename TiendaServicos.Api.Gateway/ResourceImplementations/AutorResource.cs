using System.Text.Json;
using TiendaServicos.Api.Gateway.ResourceInterfaces;
using TiendaServicos.Api.Gateway.ResourceModels;

namespace TiendaServicos.Api.Gateway.ResourceImplementations
{
    public class AutorResource : IAutorResource
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AutorResourceModel> _logger;

        public AutorResource(IHttpClientFactory httpClientFactory, ILogger<AutorResourceModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<(bool resultado, AutorResourceModel autor, string errorMessage)> GetAutor(Guid autorId)
        {
            try
            {
                var cliente = _httpClientFactory.CreateClient("AutorService");
                var response = await cliente.GetAsync($"/Autor/{autorId}");
                if (response.IsSuccessStatusCode)
                {
                    var contenido = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                    var resultado = JsonSerializer.Deserialize<AutorResourceModel>(contenido, options);
                    return (true, resultado, null);
                } else
                {
                    return (false, null, response.ReasonPhrase);
                }
            }
            catch (Exception e)
            {

                _logger.LogError(e.ToString());
                return (false, null, e.Message);
            }
        }
    }
}
