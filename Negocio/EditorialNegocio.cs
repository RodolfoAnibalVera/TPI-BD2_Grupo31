using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class EditorialNegocio
    {
        public List<Editorial> Listar()
        {
            List<Editorial> lista = new List<Editorial>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Pais FROM EDITORIALES");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Editorial editorial = new Editorial
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Pais = datos.Lector["Pais"].ToString()
                    };

                    lista.Add(editorial);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar editoriales: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Editorial editorial)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO EDITORIALES (Nombre, Pais) VALUES (@nom, @pais)");
                datos.setearParametro("@nom", editorial.Nombre);
                datos.setearParametro("@pais", editorial.Pais ?? (object)DBNull.Value);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(Editorial editorial)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE EDITORIALES SET Nombre = @nom, Pais = @pais WHERE Id = @id");
                datos.setearParametro("@nom", editorial.Nombre);
                datos.setearParametro("@pais", editorial.Pais ?? (object)DBNull.Value);
                datos.setearParametro("@id", editorial.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("DELETE FROM EDITORIALES WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex) { throw ex; }
            finally { datos.cerrarConexion(); }
        }
    }
}
