using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }

        public TipoUsuario IdTipoUsuario { get; set; }
        public bool Activo { get; set; }
    }
}
