using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> Listar()
        {
            List<Categoria> lista = new List<Categoria>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Activo FROM CATEGORIAS ORDER BY Nombre;");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Categoria cat = new Categoria();
                    cat.Id = (int)datos.Lector["Id"];
                    cat.Nombre = datos.Lector["Nombre"].ToString();
                    cat.Activo = datos.Lector["Activo"] != DBNull.Value && Convert.ToBoolean(datos.Lector["Activo"]);
                    lista.Add(cat);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public void Agregar(Categoria nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("INSERT INTO CATEGORIAS (Nombre, Activo) VALUES (@nombre, @activo)");
                datos.setearParametro("@nombre", nueva.Nombre);
                datos.setearParametro("@activo", nueva.Activo);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Categoria cat)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("UPDATE CATEGORIAS SET Nombre = @nombre, Activo = @activo WHERE Id = @id");
                datos.setearParametro("@nombre", cat.Nombre);
                datos.setearParametro("@activo", cat.Activo);
                datos.setearParametro("@id", cat.Id);
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
                datos.setearConsulta("DELETE FROM CATEGORIAS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

    }
}
