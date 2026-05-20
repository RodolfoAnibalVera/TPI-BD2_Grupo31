using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public int IdTipoUsuario { get; set; }
        public string Rol { get; set; }   // Nombre del rol desde Tipos_Usuario
        public bool Activo { get; set; }
    }

}
