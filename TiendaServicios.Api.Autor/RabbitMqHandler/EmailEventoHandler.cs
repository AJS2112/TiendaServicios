using System.Reflection.Metadata.Ecma335;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventQueue;

namespace TiendaServicios.Api.Autor.RabbitMqHandler
{
    public class EmailEventoHandler : IEventoHandler<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoHandler> _logger;

        public EmailEventoHandler(ILogger<EmailEventoHandler> logger)
        {
            _logger = logger;
        }
    
        public Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation($"Este es el valor que consumo desde rabbitmq {@event.Titulo}");

            return Task.CompletedTask;
        }
    }
}
