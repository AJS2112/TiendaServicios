using System.Diagnostics;
using System.Text.Json;
using TiendaServicos.Api.Gateway.ResourceInterfaces;
using TiendaServicos.Api.Gateway.ResourceModels;

namespace TiendaServicos.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly ILogger<LibroHandler> _logger;
        private readonly IAutorResource _autorResource;


        public LibroHandler(ILogger<LibroHandler> logger, IAutorResource autorResource)
        {
            _logger = logger;
            _autorResource = autorResource;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia el request");
            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                var resultado = JsonSerializer.Deserialize<LibroResourceModel>(contenido, options);

                var responseAutor = await _autorResource.GetAutor(resultado.AutorLibro ?? Guid.Empty);
                if (responseAutor.resultado)
                {
                    var objetoAutor = responseAutor.autor;
                    resultado.AutorData = objetoAutor;

                    var resultadoStr = JsonSerializer.Serialize(resultado);
                    response.Content = new StringContent(resultadoStr, System.Text.Encoding.UTF8, "application/json");

                }
            }

            _logger.LogInformation($"Este proceso demoró {tiempo.ElapsedMilliseconds}ms");

            return response;
        }
    }
}
