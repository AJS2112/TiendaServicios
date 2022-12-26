using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.Events;

namespace TiendaServicios.RabbitMQ.Bus.BusRabbit
{
    public interface IEventoHandler<in TEvent> : IEventoHandler where TEvent : Evento
    {
        Task Handle(TEvent @event);
    }

    public interface IEventoHandler { }
}
