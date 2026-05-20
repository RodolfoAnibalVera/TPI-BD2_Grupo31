using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class AutorNegocio
    {
        public List<Autor> Listar()
        {
            List<Autor> lista = new List<Autor>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre FROM AUTORES");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Autor autor = new Autor
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString()
                    };

                    lista.Add(autor);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar autores: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Autor autor)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO AUTORES (Nombre, Nacionalidad) VALUES (@nombre, @nac)");
                datos.setearParametro("@nombre", autor.Nombre);
                datos.setearParametro("@nac", autor.Nacionalidad);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar autor: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Modificar(Autor autor)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE AUTORES SET Nombre = @nombre, Nacionalidad = @nac WHERE Id = @id");
                datos.setearParametro("@nombre", autor.Nombre);
                datos.setearParametro("@nac", autor.Nacionalidad);
                datos.setearParametro("@id", autor.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar autor: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Autor> ListarGrilla()
        {
            List<Autor> lista = new List<Autor>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Nacionalidad FROM AUTORES");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Autor a = new Autor
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Nacionalidad = datos.Lector["Nacionalidad"].ToString()
                    };
                    lista.Add(a);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar autores: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
            return lista;
        }

        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM AUTORES WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar autor: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Autor> Buscar(string termino)
        {
            List<Autor> lista = new List<Autor>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT Id, Nombre, Nacionalidad FROM AUTORES WHERE Nombre LIKE @q");
                datos.setearParametro("@q", "%" + termino + "%");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    lista.Add(new Autor
                    {
                        Id = (int)datos.Lector["Id"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Nacionalidad = datos.Lector["Nacionalidad"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar autores: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }
    }
}
