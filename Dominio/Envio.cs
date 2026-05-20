using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Envio
    {
        public int Id { get; set; }
        public Pedido Pedido { get; set; }
        public string MetodoDeEnvio { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string EstadoEnvio { get; set; }
        public string Observaciones { get; set; }
    }
}
