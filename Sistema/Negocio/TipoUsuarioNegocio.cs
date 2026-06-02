using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class TipoUsuarioNegocio
    {
        public List<TipoUsuario> Listar()
        {
            List<TipoUsuario> lista = new List<TipoUsuario>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta("SELECT Id, Rol, Descripcion FROM TIPOS_USUARIO");
            datos.ejecutarLectura();

            while (datos.Lector.Read())
            {
                lista.Add(new TipoUsuario
                {
                    Id = (int)datos.Lector["Id"],
                    Rol = datos.Lector["Rol"].ToString(),
                    Descripcion = datos.Lector["Descripcion"].ToString()
                });
            }

            return lista;
        }
    }
}
