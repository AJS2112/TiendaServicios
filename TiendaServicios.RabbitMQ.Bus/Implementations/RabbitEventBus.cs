using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.Comands;
using TiendaServicios.RabbitMQ.Bus.Events;

namespace TiendaServicios.RabbitMQ.Bus.Implementations
{
    public class RabbitEventBus : IRabbitEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _types;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public RabbitEventBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _handlers = new Dictionary<string, List<Type>>();
            _types = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task EnviarComando<T>(T comando) where T : Comando
        {
            return _mediator.Send(comando);
        }

        public void Publish<T>(T evento) where T : Evento
        {
            var factory = new ConnectionFactory() { HostName = "rabbit-efrete-web" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = evento.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonSerializer.Serialize(evento);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventName, null, body);
            }
        }

        public void Subscribe<T, TH>()
            where T : Evento
            where TH : IEventoHandler<T>
        {
            var eventoNombre = typeof(T).Name;
            var manejadorEventoTipo = typeof(TH);

            if (!_types.Contains(typeof(T)))
            {
                _types.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventoNombre))
            {
                _handlers.Add(eventoNombre, new List<Type>());
            }

            if (_handlers[eventoNombre].Any(x => x.GetType() == manejadorEventoTipo))
            {
                throw new ArgumentException($"El handler {manejadorEventoTipo.Name} fue registrado anteriormente por {eventoNombre}");
            }

            _handlers[eventoNombre].Add(manejadorEventoTipo);


            var factory = new ConnectionFactory()
            {
                HostName = "rabbit-efrete-web",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(eventoNombre, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Delegate;

            channel.BasicConsume(eventoNombre, true, consumer);

        }

        private async Task Consumer_Delegate(object sender, BasicDeliverEventArgs e)
        {
            var nombreEvento = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body.ToArray());

            try
            {
                if (_handlers.ContainsKey(nombreEvento))
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var subscriptions = _handlers[nombreEvento];
                        foreach (var item in subscriptions)
                        {
                            var handler = scope.ServiceProvider.GetService(item); //Activator.CreateInstance(item);
                            if (handler == null) continue;

                            var tipoEvento = _types.SingleOrDefault(x => x.Name == nombreEvento);
                            var eventoDS = JsonSerializer.Deserialize(message, tipoEvento);

                            var tipoConcreto = typeof(IEventoHandler<>).MakeGenericType(tipoEvento);

                            await (Task)tipoConcreto.GetMethod("Handle").Invoke(handler, new object[] { eventoDS });
                        }
                    }
                    
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
