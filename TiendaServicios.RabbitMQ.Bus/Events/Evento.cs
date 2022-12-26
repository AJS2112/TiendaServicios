using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaServicios.RabbitMQ.Bus.Events
{
    public abstract class Evento
    {
        public DateTime Timestamp { get; protected set; }

        public Evento()
        {
            Timestamp = DateTime.Now;
        }
    }
}
