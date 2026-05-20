using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ClienteNegocio
    {
        public int ValidarLogin(string email, string password)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT C.Id FROM USUARIOS U INNER JOIN CLIENTES C ON C.IdUsuario=U.Id " +
                    "WHERE U.Email=@e AND U.Contrasena=@p AND U.Activo=1");
                datos.setearParametro("@e", email);
                datos.setearParametro("@p", password);
                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector[0]) : 0;
            }
            finally { datos.cerrarConexion(); }
        }

        // === REGISTRO =======================================================
        // >0 IdCliente  | -1 email en uso | -2 DNI en uso | 0 error
        public int RegistrarCliente(string nombre, string apellido, int dni, string email,
                                    string telefono, string direccion, string cp, string password)
        {
            // 1) Validaciones simples en DB
            if (ExisteEmail(email)) return -1;
            if (ExisteDNI(dni)) return -2;

            // 2) Obtener rol Cliente
            int idRolCliente = ObtenerIdRolCliente();
            if (idRolCliente <= 0) return 0;

            // 3) Insert USUARIO
            int idUsuario = InsertUsuario(email, password, idRolCliente);
            if (idUsuario <= 0) return 0;

            // 4) Insert CLIENTE (si falla, limpio el usuario creado)
            int idCliente = InsertCliente(nombre, apellido, dni, email, idUsuario, telefono, direccion, cp);
            if (idCliente <= 0)
            {
                // rollback manual y simple
                EliminarUsuario(idUsuario);
                return 0;
            }

            return idCliente;
        }

        // ======= PRIVADOS (SQL simple) ======================================
        private bool ExisteEmail(string email)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT 1 FROM USUARIOS WHERE Email=@e " +
                    "UNION ALL SELECT 1 FROM CLIENTES WHERE Email=@e");
                datos.setearParametro("@e", email);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.cerrarConexion(); }
        }

        private bool ExisteDNI(int dni)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT 1 FROM CLIENTES WHERE DNI=@dni");
                datos.setearParametro("@dni", dni);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.cerrarConexion(); }
        }

        private int ObtenerIdRolCliente()
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TOP 1 Id FROM TIPOS_USUARIO WHERE Rol='Cliente'");
                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector[0]) : 0;
            }
            finally { datos.cerrarConexion(); }
        }

        private int InsertUsuario(string email, string password, int idRol)
        {
            // INSERT + SCOPE_IDENTITY en dos pasos simples
            var ins = new AccesoDatos();
            try
            {
                ins.setearConsulta(
                    "INSERT INTO USUARIOS (NombreUsuario,Contrasena,Email,IdTipoUsuario,Activo) " +
                    "VALUES (@nu,@pw,@em,@rol,1)");
                ins.setearParametro("@nu", email);
                ins.setearParametro("@pw", password);
                ins.setearParametro("@em", email);
                ins.setearParametro("@rol", idRol);
                ins.ejecutarAccion();
            }
            catch
            {
                ins.cerrarConexion();
                return 0;
            }
            finally { ins.cerrarConexion(); }

            var sel = new AccesoDatos();
            try
            {
                sel.setearConsulta("SELECT TOP 1 Id FROM USUARIOS WHERE Email=@em ORDER BY Id DESC");
                sel.setearParametro("@em", email);
                sel.ejecutarLectura();
                return sel.Lector.Read() ? Convert.ToInt32(sel.Lector[0]) : 0;
            }
            finally { sel.cerrarConexion(); }
        }

        private int InsertCliente(string nombre, string apellido, int dni, string email,
                                  int idUsuario, string tel, string dir, string cp)
        {
            var ins = new AccesoDatos();
            try
            {
                ins.setearConsulta(
                    "INSERT INTO CLIENTES (Nombre,Apellido,DNI,Email,IdUsuario,Telefono,Direccion,CP) " +
                    "VALUES (@n,@a,@dni,@em,@idu,@tel,@dir,@cp)");
                ins.setearParametro("@n", nombre);
                ins.setearParametro("@a", apellido);
                ins.setearParametro("@dni", dni);
                ins.setearParametro("@em", email);
                ins.setearParametro("@idu", idUsuario);
                ins.setearParametro("@tel", (object)tel ?? DBNull.Value);
                ins.setearParametro("@dir", (object)dir ?? DBNull.Value);
                ins.setearParametro("@cp", (object)cp ?? DBNull.Value);
                ins.ejecutarAccion();
            }
            catch
            {
                ins.cerrarConexion();
                return 0;
            }
            finally { ins.cerrarConexion(); }

            var sel = new AccesoDatos();
            try
            {
                sel.setearConsulta("SELECT TOP 1 Id FROM CLIENTES WHERE IdUsuario=@idu ORDER BY Id DESC");
                sel.setearParametro("@idu", idUsuario);
                sel.ejecutarLectura();
                return sel.Lector.Read() ? Convert.ToInt32(sel.Lector[0]) : 0;
            }
            finally { sel.cerrarConexion(); }
        }

        private void EliminarUsuario(int idUsuario)
        {
            var del = new AccesoDatos();
            try
            {
                del.setearConsulta("DELETE FROM USUARIOS WHERE Id=@id");
                del.setearParametro("@id", idUsuario);
                del.ejecutarAccion();
            }
            finally { del.cerrarConexion(); }
        }

        public Cliente ObtenerClientePorId(int idCliente)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "SELECT c.Id, c.Nombre, c.Apellido, c.DNI, c.Email, c.Telefono, c.Direccion, c.CP, u.IdTipoUsuario " +
                    "FROM CLIENTES c " +
                    "INNER JOIN USUARIOS u ON c.IdUsuario = u.Id " +
                    "WHERE c.Id = @id");

                datos.setearParametro("@id", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    Cliente cli = new Cliente();
                    cli.Id = (int)datos.Lector["Id"];
                    cli.Nombre = datos.Lector["Nombre"].ToString();
                    cli.Apellido = datos.Lector["Apellido"].ToString();
                    cli.DNI = (int)datos.Lector["DNI"];
                    cli.Email = datos.Lector["Email"].ToString();
                    cli.Telefono = datos.Lector["Telefono"] != DBNull.Value ? datos.Lector["Telefono"].ToString() : null;
                    cli.Direccion = datos.Lector["Direccion"] != DBNull.Value ? datos.Lector["Direccion"].ToString() : null;
                    cli.CP = datos.Lector["CP"] != DBNull.Value ? datos.Lector["CP"].ToString() : null;
                    cli.ClienteUsuario = new Usuario
                    {
                        IdTipoUsuario = new TipoUsuario
                        {
                            Id = (int)datos.Lector["IdTipoUsuario"]
                        }
                    };
                    return cli;
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool CambiarPasswordPorEmail(string email, string nuevaPassword)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(
                    "UPDATE USUARIOS SET Contrasena = @pw " +
                    "WHERE Email = @em AND Activo = 1");
                datos.setearParametro("@pw", nuevaPassword);
                datos.setearParametro("@em", email);
                datos.ejecutarAccion();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public bool ValidarYActualizarCliente(Cliente cliente, out string mensajeError)
        {
            var datos = new AccesoDatos();
            mensajeError = "";

            try
            {
                // Validar DNI duplicado
                datos.setearConsulta("SELECT 1 FROM CLIENTES WHERE DNI=@dni AND Id<>@id");
                datos.setearParametro("@dni", cliente.DNI);
                datos.setearParametro("@id", cliente.Id);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    mensajeError = "El DNI ya está registrado.";
                    return false;
                }

                datos.cerrarConexion();

                // Actualizar cliente
                datos.setearConsulta(@"UPDATE CLIENTES SET 
                        Nombre=@n, Apellido=@a, DNI=@dni, 
                        Telefono=@t, Direccion=@d, CP=@cp 
                        WHERE Id=@id");
                datos.setearParametro("@n", cliente.Nombre);
                datos.setearParametro("@a", cliente.Apellido);
                datos.setearParametro("@dni", cliente.DNI);
                datos.setearParametro("@t", cliente.Telefono);
                datos.setearParametro("@d", cliente.Direccion);
                datos.setearParametro("@cp", cliente.CP);
                datos.setearParametro("@id", cliente.Id);
                datos.ejecutarAccion();

                return true;
            }
            catch (Exception ex)
            {
                mensajeError = "Error al actualizar: " + ex.Message;
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
