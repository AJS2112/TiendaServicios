using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.RabbitMQ.Bus.Events;

namespace TiendaServicios.RabbitMQ.Bus.EventQueue
{
    public class EmailEventoQueue : Evento
    {
        public EmailEventoQueue(string destinatario, string titulo, string contenido)
        {
            Destinatario = destinatario;
            Titulo = titulo;
            Contenido = contenido;
        }

        public string Destinatario { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
    }
}
