using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class DetalleNegocio
    {
        public Libro ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Libro libro = null;

            try
            {
                datos.setearConsulta(@"
                   SELECT l.Id, l.Titulo, l.Descripcion, l.ISBN, l.Idioma, l.AnioEdicion, l.Paginas, l.Stock, l.Activo,
                   l.PrecioCompra, l.PrecioVenta, l.PorcentajeGanancia, l.ImagenUrl, l.BestSeller,
                   a.Id AS IdAutor, a.Nombre AS NombreAutor, a.Nacionalidad,
                   e.Id AS IdEditorial, e.Nombre AS NombreEditorial, e.Pais,
                   c.Id AS CategoriaId, c.Nombre AS CategoriaNombre
                   FROM Libros l
                   INNER JOIN Categorias c ON l.IdCategoria = c.Id
                   INNER JOIN Autores a ON l.IdAutor = a.Id
                   INNER JOIN Editoriales e ON l.IdEditorial = e.Id
                   WHERE l.Id = @id
                ");

                datos.setearParametro("@id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    libro = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                        ISBN = datos.Lector["ISBN"].ToString(),
                        Idioma = datos.Lector["Idioma"].ToString(),
                        AnioEdicion = Convert.ToInt32(datos.Lector["AnioEdicion"]),
                        Paginas = Convert.ToInt32(datos.Lector["Paginas"]),
                        Stock = Convert.ToInt32(datos.Lector["Stock"]),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        PrecioCompra = Convert.ToDecimal(datos.Lector["PrecioCompra"]),
                        PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]),
                        PorcentajeGanancia = Convert.ToDecimal(datos.Lector["PorcentajeGanancia"]),
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString(),
                        BestSeller = Convert.ToBoolean(datos.Lector["BestSeller"]),
                        Autor = new Autor
                        {
                            Id = Convert.ToInt32(datos.Lector["IdAutor"]),
                            Nombre = datos.Lector["NombreAutor"].ToString(),
                            Nacionalidad = datos.Lector["Nacionalidad"].ToString()
                        },
                        Editorial = new Editorial
                        {
                            Id = Convert.ToInt32(datos.Lector["IdEditorial"]),
                            Nombre = datos.Lector["NombreEditorial"].ToString(),
                            Pais = datos.Lector["Pais"].ToString()
                        },
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt32(datos.Lector["CategoriaId"]),
                            Nombre = datos.Lector["CategoriaNombre"].ToString()
                        }
                    };
                }

                return libro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public List<Libro> ListarSugeridosPorCategoria(int idCategoria, int idLibroActual)
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"SELECT TOP 4 Id, Titulo, ImagenUrl, BestSeller
                       FROM Libros 
                       WHERE IdCategoria = @idCategoria AND Id <> @idLibro AND Activo = 1");
                datos.setearParametro("@idCategoria", idCategoria);
                datos.setearParametro("@idLibro", idLibroActual);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro libro = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString(),
                        BestSeller = Convert.ToBoolean(datos.Lector["BestSeller"])
                    };

                    lista.Add(libro);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
