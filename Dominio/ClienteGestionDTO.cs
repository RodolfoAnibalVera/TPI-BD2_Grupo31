using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ClienteGestionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int DNI { get; set; }
        public string Email { get; set; }
        public int IdUsuario { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string CP { get; set; }
    }
}
