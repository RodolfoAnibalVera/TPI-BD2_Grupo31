using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CarritoNegocio
    {
        public decimal ObtenerTotalCarrito(int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"
                    SELECT SUM(ci.Cantidad * ci.PrecioUnitario)
                    FROM CARRITO_ITEMS ci
                    INNER JOIN CARRITOS c ON c.Id = ci.IdCarrito
                    WHERE c.IdCliente = @idCliente AND c.Activo = 1";

                datos.setearConsulta(consulta);
                datos.setearParametro("@idCliente", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read() && datos.Lector[0] != DBNull.Value)
                    return Convert.ToDecimal(datos.Lector[0]);

                return 0m;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public int AsegurarCarritoActivo(int idCliente)
        {
            // Buscar existente
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT Id FROM CARRITOS WHERE IdCliente=@c AND Activo=1");
                datos.setearParametro("@c", idCliente);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                    return Convert.ToInt32(datos.Lector[0]);
            }
            finally { datos.cerrarConexion(); }

            // Crear si no existe
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("INSERT INTO CARRITOS (IdCliente,Activo) VALUES (@c,1)");
                datos.setearParametro("@c", idCliente);
                datos.ejecutarAccion();
            }
            catch
            {
                datos.cerrarConexion();
                return 0;
            }
            finally { datos.cerrarConexion(); }

            // Devolver Id creado
            //AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TOP 1 Id FROM CARRITOS WHERE IdCliente=@c AND Activo=1 ORDER BY Id DESC");
                datos.setearParametro("@c", idCliente);
                datos.ejecutarLectura();
                return datos.Lector.Read() ? Convert.ToInt32(datos.Lector[0]) : 0;
            }
            finally { datos.cerrarConexion(); }
        }

        //  NUEVO CODIGO DESDE ACA //

        public CarritoCompra ObtenerOCrearCarritoActivo(string cookieId, int? idCliente = null)
        {
            CarritoCompra carrito = ObtenerCarritoActivo(cookieId, idCliente);

            if (carrito != null)
                return carrito;

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   INSERT INTO CARRITOS (IdCliente, CookieId)
                   OUTPUT INSERTED.Id, INSERTED.Creado, INSERTED.Actualizado
                   VALUES (@idCliente, @cookieId)
                ");
                datos.setearParametro("@idCliente", idCliente);
                datos.setearParametro("@cookieId", cookieId);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    carrito = new CarritoCompra
                    {
                        Id = (int)datos.Lector["Id"],
                        IdCliente = idCliente,
                        CookieId = cookieId,
                        Creado = (DateTime)datos.Lector["Creado"],
                        Actualizado = (DateTime)datos.Lector["Actualizado"],
                        Activo = true,
                        Items = new List<ItemCarrito>()
                    };
                }

                datos.cerrarConexion();
                return carrito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CarritoCompra ObtenerCarritoActivo(string cookieId, int? idCliente = null)
        {
            CarritoCompra carrito = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   SELECT Id, IdCliente, CookieId, Creado, Actualizado, Activo
                   FROM CARRITOS
                   WHERE Activo = 1 AND 
                   (CookieId = @cookieId OR (IdCliente = @idCliente AND @idCliente IS NOT NULL))
                ");
                datos.setearParametro("@cookieId", cookieId);
                datos.setearParametro("@idCliente", idCliente);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    carrito = new CarritoCompra
                    {
                        Id = (int)datos.Lector["Id"],
                        IdCliente = datos.Lector["IdCliente"] as int?,
                        CookieId = datos.Lector["CookieId"].ToString(),
                        Creado = (DateTime)datos.Lector["Creado"],
                        Actualizado = (DateTime)datos.Lector["Actualizado"],
                        Activo = (bool)datos.Lector["Activo"],
                        Items = new List<ItemCarrito>()
                    };
                }

                datos.cerrarConexion();

                if (carrito != null)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta(@"
                       SELECT ci.Id, ci.IdCarrito, ci.IdLibro, ci.Cantidad, ci.PrecioUnitario,
                       l.Titulo, l.ImagenUrl, l.Stock, l.BestSeller,
                       a.Id AS IdAutor, a.Nombre AS NombreAutor, a.Nacionalidad,
                       e.Id AS IdEditorial, e.Nombre AS NombreEditorial, e.Pais
                       FROM CARRITO_ITEMS ci
                       INNER JOIN LIBROS l ON l.Id = ci.IdLibro
                       INNER JOIN AUTORES a ON a.Id = l.IdAutor
                       INNER JOIN EDITORIALES e ON e.Id = l.IdEditorial
                       WHERE ci.IdCarrito = @idCarrito
                    ");
                    datos.setearParametro("@idCarrito", carrito.Id);
                    datos.ejecutarLectura();

                    while (datos.Lector.Read())
                    {
                        Autor autor = new Autor
                        {
                            Id = (int)datos.Lector["IdAutor"],
                            Nombre = datos.Lector["NombreAutor"].ToString(),
                            Nacionalidad = datos.Lector["Nacionalidad"].ToString()
                        };

                        Editorial editorial = new Editorial
                        {
                            Id = (int)datos.Lector["IdEditorial"],
                            Nombre = datos.Lector["NombreEditorial"].ToString(),
                            Pais = datos.Lector["Pais"].ToString()
                        };

                        Libro libro = new Libro
                        {
                            Id = (int)datos.Lector["IdLibro"],
                            Titulo = datos.Lector["Titulo"].ToString(),
                            ImagenUrl = datos.Lector["ImagenUrl"].ToString(),
                            Stock = (int)datos.Lector["Stock"],
                            BestSeller = (bool)datos.Lector["BestSeller"],
                            Autor = autor,
                            Editorial = editorial
                        };

                        ItemCarrito item = new ItemCarrito
                        {
                            Id = (int)datos.Lector["Id"],
                            IdCarrito = (int)datos.Lector["IdCarrito"],
                            IdLibro = (int)datos.Lector["IdLibro"],
                            Cantidad = (int)datos.Lector["Cantidad"],
                            PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"],
                            Libro = libro
                        };

                        carrito.Items.Add(item);
                    }

                    datos.cerrarConexion();
                }

                return carrito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AgregarItem(int idCarrito, int idLibro, int cantidadSolicitada, decimal precioUnitario)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();

                //  Obtener stock actual del libro
                datos.setearConsulta("SELECT Stock FROM LIBROS WHERE Id = @idLibro AND Activo = 1");
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                {
                    datos.cerrarConexion();
                    return false; // Libro no existe o no está activo
                }

                int stockDisponible = (int)datos.Lector["Stock"];
                datos.cerrarConexion();

                if (stockDisponible == 0)
                    return false; // Sin stock

                //  Obtener cantidad actual en el carrito
                datos = new AccesoDatos();
                datos.setearConsulta("SELECT Cantidad FROM CARRITO_ITEMS WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarLectura();

                int cantidadActual = 0;
                bool existe = false;

                if (datos.Lector.Read())
                {
                    cantidadActual = (int)datos.Lector["Cantidad"];
                    existe = true;
                }
                datos.cerrarConexion();

                int cantidadTotal = cantidadActual + cantidadSolicitada;
                int cantidadFinal = Math.Min(cantidadTotal, stockDisponible);
                int cantidadAAgregar = cantidadFinal - cantidadActual;

                if (cantidadAAgregar <= 0)
                    return false; // Ya alcanzó el máximo permitido

                //  Insertar o actualizar
                datos = new AccesoDatos();

                if (existe)
                {
                    datos.setearConsulta(@"UPDATE CARRITO_ITEMS
                                   SET Cantidad = Cantidad + @cantidad
                                   WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro");
                    datos.setearParametro("@cantidad", cantidadAAgregar);
                }
                else
                {
                    datos.setearConsulta(@"INSERT INTO CARRITO_ITEMS (IdCarrito, IdLibro, Cantidad, PrecioUnitario)
                                   VALUES (@idCarrito, @idLibro, @cantidad, @precioUnitario)");
                    datos.setearParametro("@cantidad", cantidadAAgregar);
                    datos.setearParametro("@precioUnitario", precioUnitario);
                }

                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                //  Actualizar fecha Actualizado
                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE CARRITOS SET Actualizado = SYSUTCDATETIME() WHERE Id = @idCarrito");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IncrementarItem(int idCarrito, int idLibro)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //  Obtener stock del libro
                datos.setearConsulta("SELECT Stock FROM LIBROS WHERE Id = @idLibro AND Activo = 1");
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarLectura();

                if (!datos.Lector.Read())
                {
                    datos.cerrarConexion();
                    return; // Libro no existe o está inactivo
                }

                int stock = (int)datos.Lector["Stock"];
                datos.cerrarConexion();

                if (stock == 0)
                    return; 

                //  Obtener cantidad actual en el carrito
                datos = new AccesoDatos();
                datos.setearConsulta("SELECT Cantidad FROM CARRITO_ITEMS WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarLectura();

                int cantidadActual = 0;
                if (datos.Lector.Read())
                    cantidadActual = (int)datos.Lector["Cantidad"];

                datos.cerrarConexion();

                if (cantidadActual >= stock)
                    return;

                //  Incrementar
                datos = new AccesoDatos();
                datos.setearConsulta(@"UPDATE CARRITO_ITEMS
                               SET Cantidad = Cantidad + 1
                               WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                //Actualizar fecha Actualizado
                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE CARRITOS SET Actualizado = SYSUTCDATETIME() WHERE Id = @idCarrito");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DisminuirItem(int idCarrito, int idLibro)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   UPDATE CARRITO_ITEMS
                   SET Cantidad = Cantidad - 1
                   WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro AND Cantidad > 1
                ");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE CARRITOS SET Actualizado = SYSUTCDATETIME() WHERE Id = @idCarrito");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void EliminarItem(int idCarrito, int idLibro)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   DELETE FROM CARRITO_ITEMS
                   WHERE IdCarrito = @idCarrito AND IdLibro = @idLibro
                ");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.setearParametro("@idLibro", idLibro);
                datos.ejecutarAccion();
                datos.cerrarConexion();

                datos = new AccesoDatos();
                datos.setearConsulta("UPDATE CARRITOS SET Actualizado = SYSUTCDATETIME() WHERE Id = @idCarrito");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AsignarClienteAlCarrito(string cookieId, int idCliente)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // 1) Ver si el cliente ya tiene un carrito activo
                datos.setearConsulta("SELECT COUNT(*) FROM CARRITOS WHERE IdCliente = @idCliente AND Activo = 1");
                datos.setearParametro("@idCliente", idCliente);
                datos.ejecutarLectura();

                bool yaTieneCarrito = false;
                if (datos.Lector.Read())
                {
                    int cantidad = (int)datos.Lector[0];
                    yaTieneCarrito = cantidad > 0;
                }
                datos.cerrarConexion();

                // Si ya tiene carrito activo, no tocamos nada (así evitamos el error)
                if (yaTieneCarrito)
                    return;

                // 2) Si NO tiene carrito activo, asignamos el carrito de la cookie
                datos = new AccesoDatos();
                datos.setearConsulta(@"
           UPDATE CARRITOS
           SET IdCliente = @idCliente,
               Actualizado = SYSUTCDATETIME()
           WHERE CookieId = @cookieId AND Activo = 1 AND IdCliente IS NULL
        ");
                datos.setearParametro("@idCliente", idCliente);
                datos.setearParametro("@cookieId", cookieId);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Esto es para usar en Finalizar Compra
        public void DesactivarCarrito(int idCarrito)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   UPDATE CARRITOS
                   SET Activo = 0,
                   Actualizado = SYSUTCDATETIME()
                   WHERE Id = @idCarrito
                ");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DescontarStockPorCarrito(int idCarrito)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                   SELECT IdLibro, Cantidad
                   FROM CARRITO_ITEMS
                   WHERE IdCarrito = @idCarrito
                ");
                datos.setearParametro("@idCarrito", idCarrito);
                datos.ejecutarLectura();

                var items = new List<Tuple<int, int>>(); // IdLibro, Cantidad

                while (datos.Lector.Read())
                {
                    int idLibro = (int)datos.Lector["IdLibro"];
                    int cantidad = (int)datos.Lector["Cantidad"];
                    items.Add(Tuple.Create(idLibro, cantidad));
                }

                datos.cerrarConexion();

                foreach (var item in items)
                {
                    datos = new AccesoDatos();
                    datos.setearConsulta(@"
                       UPDATE LIBROS
                       SET Stock = Stock - @cantidad
                       WHERE Id = @idLibro AND Activo = 1 AND Stock >= @cantidad
                    ");
                    datos.setearParametro("@idLibro", item.Item1);
                    datos.setearParametro("@cantidad", item.Item2);
                    datos.ejecutarAccion();
                    datos.cerrarConexion();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void FusionarCarritos(string cookieId, int idCliente)
        {
            CarritoCompra carritoAnonimo = ObtenerCarritoActivo(cookieId, null);
            if (carritoAnonimo == null) return;

            CarritoCompra carritoCliente = ObtenerCarritoActivo(null, idCliente);
            if (carritoCliente == null)
            {
                AsignarClienteAlCarrito(cookieId, idCliente);
                return;
            }

            foreach (var item in carritoAnonimo.Items)
            {
                AgregarItem(carritoCliente.Id, item.IdLibro, item.Cantidad, item.PrecioUnitario);
            }

            DesactivarCarrito(carritoAnonimo.Id);
        }
    }
}
