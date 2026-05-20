using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class UsuarioNegocio
    {
        public List<UsuarioDTO> ListarConTipo()
        {
            List<UsuarioDTO> lista = new List<UsuarioDTO>();
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
                SELECT u.Id, u.NombreUsuario, u.Email, t.Rol, u.Activo 
                FROM USUARIOS u
                INNER JOIN TIPOS_USUARIO t ON t.Id = u.IdTipoUsuario");

            datos.ejecutarLectura();

            while (datos.Lector.Read())
            {
                lista.Add(new UsuarioDTO
                {
                    Id = (int)datos.Lector["Id"],
                    NombreUsuario = datos.Lector["NombreUsuario"].ToString(),
                    Email = datos.Lector["Email"].ToString(),
                    Rol = datos.Lector["Rol"].ToString(),
                    Activo = (bool)datos.Lector["Activo"]
                });
            }

            return lista;
        }

        public void Agregar(Usuario u)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
                INSERT INTO USUARIOS (NombreUsuario, Contrasena, Email, IdTipoUsuario, Activo)
                VALUES (@user, @pass, @mail, @tipo, @act)");

            datos.setearParametro("@user", u.NombreUsuario);
            datos.setearParametro("@pass", u.Contrasena);
            datos.setearParametro("@mail", u.Email);
            datos.setearParametro("@tipo", u.IdTipoUsuario.Id);
            datos.setearParametro("@act", u.Activo);

            datos.ejecutarAccion();
        }

        public void Modificar(Usuario u)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta(@"
                UPDATE USUARIOS SET
                    NombreUsuario=@user,
                    Contrasena=@pass,
                    Email=@mail,
                    IdTipoUsuario=@tipo,
                    Activo=@act
                WHERE Id=@id");

            datos.setearParametro("@user", u.NombreUsuario);
            datos.setearParametro("@pass", u.Contrasena);
            datos.setearParametro("@mail", u.Email);
            datos.setearParametro("@tipo", u.IdTipoUsuario.Id);
            datos.setearParametro("@act", u.Activo);
            datos.setearParametro("@id", u.Id);

            datos.ejecutarAccion();
        }

        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta("DELETE FROM USUARIOS WHERE Id=@id");
            datos.setearParametro("@id", id);

            datos.ejecutarAccion();
        }

        public int ObtenerIdTipo(int idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();

            datos.setearConsulta("SELECT IdTipoUsuario FROM USUARIOS WHERE Id=@id");
            datos.setearParametro("@id", idUsuario);

            datos.ejecutarLectura();

            if (datos.Lector.Read())
                return (int)datos.Lector["IdTipoUsuario"];

            return 0;
        }


    }
}
