using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Factura
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Direccion { get; set; }
        public string Barrio { get; set; }
        public string Ciudad { get; set; }
        public string CP { get; set; }
        public string Depto { get; set; }
    }
}
