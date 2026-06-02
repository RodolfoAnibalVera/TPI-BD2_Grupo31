using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class GestionClienteNegocio
    {
        public List<ClienteGestionDTO> Listar()
        {
            List<ClienteGestionDTO> lista = new List<ClienteGestionDTO>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    SELECT 
                        Id,
                        Nombre,
                        Apellido,
                        DNI,
                        Email,
                        IdUsuario,
                        Telefono,
                        Direccion,
                        CP
                    FROM CLIENTES");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    ClienteGestionDTO c = new ClienteGestionDTO();

                    c.Id = (int)datos.Lector["Id"];
                    c.Nombre = datos.Lector["Nombre"].ToString();
                    c.Apellido = datos.Lector["Apellido"].ToString();
                    c.Email = datos.Lector["Email"].ToString();
                    c.Telefono = datos.Lector["Telefono"].ToString();
                    c.Direccion = datos.Lector["Direccion"].ToString();
                    c.CP = datos.Lector["CP"].ToString();

                    // Evitar el error de formato:
                    c.DNI = datos.Lector["DNI"] == DBNull.Value || datos.Lector["DNI"].ToString() == ""
                           ? 0
                           : Convert.ToInt32(datos.Lector["DNI"]);

                    c.IdUsuario = datos.Lector["IdUsuario"] == DBNull.Value || datos.Lector["IdUsuario"].ToString() == ""
                           ? 0
                           : Convert.ToInt32(datos.Lector["IdUsuario"]);

                    lista.Add(c);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar clientes: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public ClienteGestionDTO ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Apellido, DNI, Email, IdUsuario, Telefono, Direccion, CP FROM CLIENTES WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return new ClienteGestionDTO
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = int.Parse(datos.Lector["DNI"].ToString()),
                        Email = datos.Lector["Email"].ToString(),
                        IdUsuario = (int)datos.Lector["IdUsuario"],
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Direccion = datos.Lector["Direccion"].ToString(),
                        CP = datos.Lector["CP"].ToString()
                    };
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(ClienteGestionDTO cli)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    INSERT INTO CLIENTES (Nombre, Apellido, DNI, Email, IdUsuario, Telefono, Direccion, CP)
                    VALUES (@Nombre, @Apellido, @DNI, @Email, @IdUsuario, @Telefono, @Direccion, @CP)");

                datos.setearParametro("@Nombre", cli.Nombre);
                datos.setearParametro("@Apellido", cli.Apellido);
                datos.setearParametro("@DNI", cli.DNI);
                datos.setearParametro("@Email", cli.Email);
                datos.setearParametro("@IdUsuario", cli.IdUsuario);
                datos.setearParametro("@Telefono", cli.Telefono);
                datos.setearParametro("@Direccion", cli.Direccion);
                datos.setearParametro("@CP", cli.CP);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Modificar(ClienteGestionDTO cli)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                    UPDATE CLIENTES 
                    SET Nombre = @Nombre, Apellido = @Apellido, DNI = @DNI,
                        Email = @Email, IdUsuario = @IdUsuario, Telefono = @Telefono,
                        Direccion = @Direccion, CP = @CP
                    WHERE Id = @Id");

                datos.setearParametro("@Id", cli.Id);
                datos.setearParametro("@Nombre", cli.Nombre);
                datos.setearParametro("@Apellido", cli.Apellido);
                datos.setearParametro("@DNI", cli.DNI);
                datos.setearParametro("@Email", cli.Email);
                datos.setearParametro("@IdUsuario", cli.IdUsuario);
                datos.setearParametro("@Telefono", cli.Telefono);
                datos.setearParametro("@Direccion", cli.Direccion);
                datos.setearParametro("@CP", cli.CP);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM CLIENTES WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
