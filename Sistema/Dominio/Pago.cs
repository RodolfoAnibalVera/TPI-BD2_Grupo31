using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pago
    {
        public int Id { get; set; }
        public Pedido Pedido { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; }
        public string Estado { get; set; }
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }
    }
}
