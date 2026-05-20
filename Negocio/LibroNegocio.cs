using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class LibroNegocio
    {
        public List<Libro> Listar()
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   SELECT L.Id, L.Titulo, L.ISBN, L.Stock, L.PrecioVenta, L.ImagenUrl, L.BestSeller,
                   A.Id AS IdAutor, A.Nombre AS AutorNombre,
                   E.Id AS IdEditorial, E.Nombre AS EditorialNombre,
                   C.Id AS IdCategoria, C.Nombre AS CategoriaNombre
                   FROM LIBROS L
                   LEFT JOIN AUTORES A ON L.IdAutor = A.Id
                   LEFT JOIN EDITORIALES E ON L.IdEditorial = E.Id
                   LEFT JOIN CATEGORIAS C ON L.IdCategoria = C.Id
                   WHERE L.Activo = 1
                   ORDER BY L.Titulo
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro libro = new Libro
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Titulo = datos.Lector["Titulo"]?.ToString() ?? "",
                        ISBN = datos.Lector["ISBN"]?.ToString() ?? "",
                        Stock = datos.Lector["Stock"] != DBNull.Value ? Convert.ToInt32(datos.Lector["Stock"]) : 0,
                        PrecioVenta = datos.Lector["PrecioVenta"] != DBNull.Value ? Convert.ToDecimal(datos.Lector["PrecioVenta"]) : 0,
                        ImagenUrl = datos.Lector["ImagenUrl"]?.ToString() ?? "",
                        BestSeller = Convert.ToBoolean(datos.Lector["BestSeller"]),
                        Autor = new Autor
                        {
                            Id = datos.Lector["IdAutor"] != DBNull.Value ? Convert.ToInt32(datos.Lector["IdAutor"]) : 0,
                            Nombre = datos.Lector["AutorNombre"]?.ToString() ?? ""
                        },
                        Editorial = new Editorial
                        {
                            Id = datos.Lector["IdEditorial"] != DBNull.Value ? Convert.ToInt32(datos.Lector["IdEditorial"]) : 0,
                            Nombre = datos.Lector["EditorialNombre"]?.ToString() ?? ""
                        },
                        Categoria = new Categoria
                        {
                            Id = datos.Lector["IdCategoria"] != DBNull.Value ? Convert.ToInt32(datos.Lector["IdCategoria"]) : 0,
                            Nombre = datos.Lector["CategoriaNombre"]?.ToString() ?? ""
                        }
                    };

                    lista.Add(libro);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public List<Libro> ListarPorCategoria(int idCategoria)
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                l.*, 
                c.Id AS IdCategoria,
                c.Nombre AS CategoriaNombre,
                e.Id AS IdEditorial,
                e.Nombre AS EditorialNombre,
                a.Id AS IdAutor,
                a.Nombre AS AutorNombre
            FROM LIBROS l
            INNER JOIN CATEGORIAS c ON l.IdCategoria = c.Id
            LEFT JOIN EDITORIALES e ON l.IdEditorial = e.Id
            LEFT JOIN AUTORES a ON l.IdAutor = a.Id
            WHERE l.IdCategoria = @idCategoria
        ");

                datos.setearParametro("@idCategoria", idCategoria);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro aux = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                        ISBN = datos.Lector["ISBN"].ToString(),
                        Idioma = datos.Lector["Idioma"].ToString(),
                        AnioEdicion = datos.Lector["AnioEdicion"] != DBNull.Value ? (int)datos.Lector["AnioEdicion"] : 0,
                        Paginas = datos.Lector["Paginas"] != DBNull.Value ? (int)datos.Lector["Paginas"] : 0,
                        Stock = (int)datos.Lector["Stock"],
                        Activo = (bool)datos.Lector["Activo"],
                        BestSeller = (bool)datos.Lector["BestSeller"],
                        PrecioCompra = datos.Lector["PrecioCompra"] != DBNull.Value ? (decimal)datos.Lector["PrecioCompra"] : 0,
                        PrecioVenta = datos.Lector["PrecioVenta"] != DBNull.Value ? (decimal)datos.Lector["PrecioVenta"] : 0,
                        PorcentajeGanancia = datos.Lector["PorcentajeGanancia"] != DBNull.Value ? (decimal)datos.Lector["PorcentajeGanancia"] : 0,
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString(),

                        Categoria = new Categoria
                        {
                            Id = (int)datos.Lector["IdCategoria"],
                            Nombre = datos.Lector["CategoriaNombre"].ToString()
                        }
                    };

                    // Editorial
                    if (datos.Lector["IdEditorial"] != DBNull.Value)
                    {
                        aux.Editorial = new Editorial
                        {
                            Id = (int)datos.Lector["IdEditorial"],
                            Nombre = datos.Lector["EditorialNombre"].ToString()
                        };
                    }

                    // Autor
                    if (datos.Lector["IdAutor"] != DBNull.Value)
                    {
                        aux.Autor = new Autor
                        {
                            Id = (int)datos.Lector["IdAutor"],
                            Nombre = datos.Lector["AutorNombre"].ToString()
                        };
                    }

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Libro> Buscar(string termino)
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                l.*,
                c.Id AS IdCategoria,
                c.Nombre AS CategoriaNombre,
                e.Id AS IdEditorial,
                e.Nombre AS EditorialNombre,
                a.Id AS IdAutor,
                a.Nombre AS AutorNombre
            FROM LIBROS l
            INNER JOIN CATEGORIAS c ON l.IdCategoria = c.Id
            LEFT JOIN EDITORIALES e ON l.IdEditorial = e.Id
            LEFT JOIN AUTORES a ON l.IdAutor = a.Id
            WHERE l.Activo = 1 AND
                (
                    l.Titulo LIKE @q
                    OR l.ISBN LIKE @q
                    OR a.Nombre LIKE @q
                    OR e.Nombre LIKE @q
                )
            ORDER BY l.Titulo;
        ");

                datos.setearParametro("@q", "%" + termino + "%");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro aux = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                        ISBN = datos.Lector["ISBN"].ToString(),
                        Idioma = datos.Lector["Idioma"].ToString(),
                        AnioEdicion = datos.Lector["AnioEdicion"] != DBNull.Value ? (int)datos.Lector["AnioEdicion"] : 0,
                        Paginas = datos.Lector["Paginas"] != DBNull.Value ? (int)datos.Lector["Paginas"] : 0,
                        Stock = (int)datos.Lector["Stock"],
                        Activo = (bool)datos.Lector["Activo"],
                        BestSeller = (bool)datos.Lector["BestSeller"],
                        PrecioCompra = datos.Lector["PrecioCompra"] != DBNull.Value ? (decimal)datos.Lector["PrecioCompra"] : 0,
                        PrecioVenta = datos.Lector["PrecioVenta"] != DBNull.Value ? (decimal)datos.Lector["PrecioVenta"] : 0,
                        PorcentajeGanancia = datos.Lector["PorcentajeGanancia"] != DBNull.Value ? (decimal)datos.Lector["PorcentajeGanancia"] : 0,
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString(),

                        Categoria = new Categoria
                        {
                            Id = (int)datos.Lector["IdCategoria"],
                            Nombre = datos.Lector["CategoriaNombre"].ToString()
                        }
                    };

                    // Editorial si existe
                    if (datos.Lector["IdEditorial"] != DBNull.Value)
                    {
                        aux.Editorial = new Editorial
                        {
                            Id = (int)datos.Lector["IdEditorial"],
                            Nombre = datos.Lector["EditorialNombre"].ToString()
                        };
                    }

                    // Autor si existe
                    if (datos.Lector["IdAutor"] != DBNull.Value)
                    {
                        aux.Autor = new Autor
                        {
                            Id = (int)datos.Lector["IdAutor"],
                            Nombre = datos.Lector["AutorNombre"].ToString()
                        };
                    }

                    lista.Add(aux);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public List<Libro> listarGrilla()
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                l.Id,
                l.Titulo,
                l.Descripcion,
                l.ISBN,
                l.Idioma,
                l.AnioEdicion,
                l.Paginas,
                l.Stock,
                l.Activo,
                l.BestSeller,
                l.PrecioCompra,
                l.PrecioVenta,
                l.PorcentajeGanancia,
                l.ImagenUrl,

                c.Id AS IdCategoria,
                c.Nombre AS CategoriaNombre,

                e.Id AS IdEditorial,
                e.Nombre AS EditorialNombre,
                e.Pais AS EditorialPais,

                a.Id AS IdAutor,
                a.Nombre AS AutorNombre,
                a.Nacionalidad AS AutorNacionalidad

            FROM LIBROS l
            INNER JOIN CATEGORIAS c ON l.IdCategoria = c.Id
            LEFT JOIN EDITORIALES e ON l.IdEditorial = e.Id
            LEFT JOIN AUTORES a ON l.IdAutor = a.Id
        ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro aux = new Libro();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = datos.Lector["Titulo"].ToString();
                    aux.Descripcion = datos.Lector["Descripcion"].ToString();
                    aux.ISBN = datos.Lector["ISBN"].ToString();
                    aux.Idioma = datos.Lector["Idioma"].ToString();
                    aux.AnioEdicion = datos.Lector["AnioEdicion"] != DBNull.Value ? (int)datos.Lector["AnioEdicion"] : 0;
                    aux.Paginas = datos.Lector["Paginas"] != DBNull.Value ? (int)datos.Lector["Paginas"] : 0;
                    aux.Stock = (int)datos.Lector["Stock"];
                    aux.Activo = (bool)datos.Lector["Activo"];
                    aux.BestSeller = (bool)datos.Lector["BestSeller"];
                    aux.PrecioCompra = datos.Lector["PrecioCompra"] != DBNull.Value ? (decimal)datos.Lector["PrecioCompra"] : 0;
                    aux.PrecioVenta = datos.Lector["PrecioVenta"] != DBNull.Value ? (decimal)datos.Lector["PrecioVenta"] : 0;
                    aux.PorcentajeGanancia = datos.Lector["PorcentajeGanancia"] != DBNull.Value ? (decimal)datos.Lector["PorcentajeGanancia"] : 0;
                    aux.ImagenUrl = datos.Lector["ImagenUrl"].ToString();

                    aux.Categoria = new Categoria
                    {
                        Id = (int)datos.Lector["IdCategoria"],
                        Nombre = datos.Lector["CategoriaNombre"].ToString()
                    };

                    if (datos.Lector["IdEditorial"] != DBNull.Value)
                    {
                        aux.Editorial = new Editorial
                        {
                            Id = (int)datos.Lector["IdEditorial"],
                            Nombre = datos.Lector["EditorialNombre"].ToString(),
                            Pais = datos.Lector["EditorialPais"].ToString()
                        };
                    }

                    if (datos.Lector["IdAutor"] != DBNull.Value)
                    {
                        aux.Autor = new Autor
                        {
                            Id = (int)datos.Lector["IdAutor"],
                            Nombre = datos.Lector["AutorNombre"].ToString(),
                            Nacionalidad = datos.Lector["AutorNacionalidad"].ToString()
                        };
                    }

                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void AgregarLibro(Libro nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            INSERT INTO LIBROS
            (Titulo, Descripcion, ISBN, Idioma, AnioEdicion, Paginas, Stock, Activo, BestSeller,
             PrecioCompra, PrecioVenta, PorcentajeGanancia, ImagenUrl, IdEditorial, IdAutor, IdCategoria)
            VALUES
            (@Titulo, @Descripcion, @ISBN, @Idioma, @AnioEdicion, @Paginas, @Stock, @Activo, @BestSeller,
             @PrecioCompra, @PrecioVenta, @PorcentajeGanancia, @ImagenUrl, @IdEditorial, @IdAutor, @IdCategoria)
        ");

                datos.setearParametro("@Titulo", nuevo.Titulo);
                datos.setearParametro("@Descripcion", nuevo.Descripcion);
                datos.setearParametro("@ISBN", nuevo.ISBN);
                datos.setearParametro("@Idioma", nuevo.Idioma);
                datos.setearParametro("@AnioEdicion", nuevo.AnioEdicion);
                datos.setearParametro("@Paginas", nuevo.Paginas);
                datos.setearParametro("@Stock", nuevo.Stock);
                datos.setearParametro("@Activo", nuevo.Activo);
                datos.setearParametro("@BestSeller", nuevo.BestSeller);
                datos.setearParametro("@PrecioCompra", nuevo.PrecioCompra);
                datos.setearParametro("@PrecioVenta", nuevo.PrecioVenta);
                datos.setearParametro("@PorcentajeGanancia", nuevo.PorcentajeGanancia);
                datos.setearParametro("@ImagenUrl", nuevo.ImagenUrl);

                datos.setearParametro("@IdEditorial", nuevo.Editorial?.Id);
                datos.setearParametro("@IdAutor", nuevo.Autor?.Id);
                datos.setearParametro("@IdCategoria", nuevo.Categoria.Id);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void ModificarLibro(Libro libro)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            UPDATE LIBROS SET
                Titulo = @Titulo,
                Descripcion = @Descripcion,
                ISBN = @ISBN,
                Idioma = @Idioma,
                AnioEdicion = @AnioEdicion,
                Paginas = @Paginas,
                Stock = @Stock,
                Activo = @Activo,
                BestSeller = @BestSeller,
                PrecioCompra = @PrecioCompra,
                PrecioVenta = @PrecioVenta,
                PorcentajeGanancia = @PorcentajeGanancia,
                ImagenUrl = @ImagenUrl,
                IdEditorial = @IdEditorial,
                IdAutor = @IdAutor,
                IdCategoria = @IdCategoria
            WHERE Id = @Id
        ");

                datos.setearParametro("@Titulo", libro.Titulo);
                datos.setearParametro("@Descripcion", libro.Descripcion);
                datos.setearParametro("@ISBN", libro.ISBN);
                datos.setearParametro("@Idioma", libro.Idioma);
                datos.setearParametro("@AnioEdicion", libro.AnioEdicion);
                datos.setearParametro("@Paginas", libro.Paginas);
                datos.setearParametro("@Stock", libro.Stock);
                datos.setearParametro("@Activo", libro.Activo);
                datos.setearParametro("@BestSeller", libro.BestSeller);
                datos.setearParametro("@PrecioCompra", libro.PrecioCompra);
                datos.setearParametro("@PrecioVenta", libro.PrecioVenta);
                datos.setearParametro("@PorcentajeGanancia", libro.PorcentajeGanancia);
                datos.setearParametro("@ImagenUrl", libro.ImagenUrl);

                datos.setearParametro("@IdEditorial", libro.Editorial?.Id);
                datos.setearParametro("@IdAutor", libro.Autor?.Id);
                datos.setearParametro("@IdCategoria", libro.Categoria.Id);

                datos.setearParametro("@Id", libro.Id);

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
                datos.setearConsulta("DELETE FROM LIBROS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
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

        public void Desactivar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("UPDATE Libros SET Activo = 0 WHERE Id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public List<Libro> Filtrar(string campo, string criterio, string filtro, string estado)
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"
        SELECT 
            L.Id, L.Titulo, L.ISBN, L.Stock,
            L.PrecioCompra, L.PrecioVenta,
            L.PorcentajeGanancia, L.Activo
        FROM LIBROS L
        WHERE 1=1 ";

                // ---- ESTADO ----
                if (estado == "1")           // activos
                    consulta += " AND L.Activo = 1 ";
                else if (estado == "0")      // inactivos
                    consulta += " AND L.Activo = 0 ";

                bool requiereFiltro = false;
                bool requiereNum = false;
                decimal num = 0;

                // ---- FILTROS ----
                if (!string.IsNullOrEmpty(campo) && !string.IsNullOrEmpty(criterio))
                {
                    switch (campo)
                    {
                        // --- Campos texto ---
                        case "Titulo":
                        case "ISBN":
                            requiereFiltro = true;

                            switch (criterio)
                            {
                                case "Contiene":
                                    consulta += $" AND {campo} LIKE @filtro ";
                                    filtro = "%" + filtro + "%";
                                    break;

                                case "Empieza con":
                                    consulta += $" AND {campo} LIKE @filtro ";
                                    filtro = filtro + "%";
                                    break;

                                case "Termina con":
                                    consulta += $" AND {campo} LIKE @filtro ";
                                    filtro = "%" + filtro;
                                    break;

                                case "Igual a":
                                    consulta += $" AND {campo} = @filtro ";
                                    break;
                            }
                            break;

                        // --- Campos numéricos ---
                        case "PrecioVenta":
                        case "Stock":
                            if (decimal.TryParse(filtro, out num))
                            {
                                requiereNum = true;

                                switch (criterio)
                                {
                                    case "Mayor que":
                                        consulta += $" AND {campo} > @num ";
                                        break;

                                    case "Menor que":
                                        consulta += $" AND {campo} < @num ";
                                        break;

                                    case "Igual a":
                                        consulta += $" AND {campo} = @num ";
                                        break;
                                }
                            }
                            break;
                    }
                }

                datos.setearConsulta(consulta);

                if (requiereFiltro)
                    datos.setearParametro("@filtro", filtro);

                if (requiereNum)
                    datos.setearParametro("@num", num);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro lib = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        ISBN = datos.Lector["ISBN"].ToString(),
                        Stock = (int)datos.Lector["Stock"],
                        PrecioCompra = (decimal)datos.Lector["PrecioCompra"],
                        PrecioVenta = (decimal)datos.Lector["PrecioVenta"],
                        PorcentajeGanancia = (decimal)datos.Lector["PorcentajeGanancia"],
                        Activo = (bool)datos.Lector["Activo"]
                    };

                    lista.Add(lib);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar libros: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public Libro ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            Libro libro = null;

            try
            {
                datos.setearConsulta(@"
            SELECT 
                l.Id, l.Titulo, l.Descripcion, l.ISBN, l.Idioma,
                l.AnioEdicion, l.Paginas, l.Stock, l.Activo,
                l.BestSeller, l.PrecioCompra, l.PrecioVenta,
                l.PorcentajeGanancia, l.ImagenUrl,

                e.Id AS IdEditorial, e.Nombre AS EditorialNombre,
                a.Id AS IdAutor, a.Nombre AS AutorNombre,
                c.Id AS IdCategoria, c.Nombre AS CategoriaNombre

            FROM LIBROS l
            LEFT JOIN EDITORIALES e ON l.IdEditorial = e.Id
            LEFT JOIN AUTORES a ON l.IdAutor = a.Id
            LEFT JOIN CATEGORIAS c ON l.IdCategoria = c.Id
            WHERE l.Id = @Id
        ");

                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    libro = new Libro();
                    libro.Id = Convert.ToInt32(datos.Lector["Id"]);
                    libro.Titulo = datos.Lector["Titulo"].ToString();
                    libro.Descripcion = datos.Lector["Descripcion"].ToString();
                    libro.ISBN = datos.Lector["ISBN"].ToString();
                    libro.Idioma = datos.Lector["Idioma"].ToString();
                    libro.AnioEdicion = Convert.ToInt32(datos.Lector["AnioEdicion"]);
                    libro.Paginas = Convert.ToInt32(datos.Lector["Paginas"]);
                    libro.Stock = Convert.ToInt32(datos.Lector["Stock"]);
                    libro.Activo = Convert.ToBoolean(datos.Lector["Activo"]);
                    libro.BestSeller = Convert.ToBoolean(datos.Lector["BestSeller"]);
                    libro.PrecioCompra = Convert.ToDecimal(datos.Lector["PrecioCompra"]);
                    libro.PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]);
                    libro.PorcentajeGanancia = Convert.ToDecimal(datos.Lector["PorcentajeGanancia"]);
                    libro.ImagenUrl = datos.Lector["ImagenUrl"].ToString();


                    libro.Editorial = new Editorial();
                    libro.Editorial.Id = Convert.ToInt32(datos.Lector["IdEditorial"]);
                    libro.Editorial.Nombre = datos.Lector["EditorialNombre"].ToString();


                    libro.Autor = new Autor();
                    libro.Autor.Id = Convert.ToInt32(datos.Lector["IdAutor"]);
                    libro.Autor.Nombre = datos.Lector["AutorNombre"].ToString();


                    libro.Categoria = new Categoria();
                    libro.Categoria.Id = Convert.ToInt32(datos.Lector["IdCategoria"]);
                    libro.Categoria.Nombre = datos.Lector["CategoriaNombre"].ToString();
                }

                return libro;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public List<Libro> ListarOfertas()
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   SELECT L.Id, L.Titulo, L.ISBN, L.Stock, L.PrecioVenta, L.ImagenUrl, L.BestSeller,
                   L.PorcentajeGanancia,
                   A.Id AS IdAutor, A.Nombre AS AutorNombre,
                   E.Id AS IdEditorial, E.Nombre AS EditorialNombre,
                   C.Id AS IdCategoria, C.Nombre AS CategoriaNombre
                   FROM LIBROS L
                   LEFT JOIN AUTORES A ON L.IdAutor = A.Id
                   LEFT JOIN EDITORIALES E ON L.IdEditorial = E.Id
                   LEFT JOIN CATEGORIAS C ON L.IdCategoria = C.Id
                   WHERE L.Activo = 1
                   AND L.PorcentajeGanancia < 40
                   ORDER BY L.Titulo
                ");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro libro = new Libro
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        Titulo = datos.Lector["Titulo"]?.ToString() ?? "",
                        ISBN = datos.Lector["ISBN"]?.ToString() ?? "",
                        Stock = datos.Lector["Stock"] != DBNull.Value ? Convert.ToInt32(datos.Lector["Stock"]) : 0,
                        PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]),
                        ImagenUrl = datos.Lector["ImagenUrl"]?.ToString() ?? "",
                        BestSeller = Convert.ToBoolean(datos.Lector["BestSeller"]),
                        PorcentajeGanancia = Convert.ToDecimal(datos.Lector["PorcentajeGanancia"]),
                        Autor = new Autor
                        {
                            Id = Convert.ToInt32(datos.Lector["IdAutor"]),
                            Nombre = datos.Lector["AutorNombre"]?.ToString() ?? ""
                        },
                        Editorial = new Editorial
                        {
                            Id = Convert.ToInt32(datos.Lector["IdEditorial"]),
                            Nombre = datos.Lector["EditorialNombre"]?.ToString() ?? ""
                        },
                        Categoria = new Categoria
                        {
                            Id = Convert.ToInt32(datos.Lector["IdCategoria"]),
                            Nombre = datos.Lector["CategoriaNombre"]?.ToString() ?? ""
                        }
                    };

                    lista.Add(libro);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }

        public List<Libro> ListarBestSellers()
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"SELECT Id, Titulo, Descripcion, PrecioVenta, ImagenUrl, Stock, BestSeller
                               FROM Libros
                               WHERE BestSeller = 1 AND Activo = 1");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro libro = new Libro
                    {
                        Id = (int)datos.Lector["Id"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Descripcion = datos.Lector["Descripcion"].ToString(),
                        PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]),
                        ImagenUrl = datos.Lector["ImagenUrl"].ToString(),
                        Stock = Convert.ToInt32(datos.Lector["Stock"]),
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

        public List<Libro> ListarPopulares()
        {
            List<Libro> lista = new List<Libro>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(
                    "SELECT TOP 12 L.Id, L.Titulo, L.Descripcion, L.PrecioVenta, L.ImagenUrl, L.Stock, " +
                    "SUM(PD.Cantidad) AS TotalVendida " +
                    "FROM PEDIDOS_DETALLE PD " +
                    "INNER JOIN LIBROS L ON L.Id = PD.IdLibro " +
                    "WHERE L.Activo = 1 " +
                    "GROUP BY L.Id, L.Titulo, L.Descripcion, L.PrecioVenta, L.ImagenUrl, L.Stock " +
                    "ORDER BY TotalVendida DESC"
                );

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Libro libro = new Libro();

                    libro.Id = Convert.ToInt32(datos.Lector["Id"]);
                    libro.Titulo = datos.Lector["Titulo"].ToString();
                    libro.Descripcion = datos.Lector["Descripcion"].ToString();
                    libro.PrecioVenta = Convert.ToDecimal(datos.Lector["PrecioVenta"]);
                    libro.ImagenUrl = datos.Lector["ImagenUrl"].ToString();
                    libro.Stock = Convert.ToInt32(datos.Lector["Stock"]);

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
