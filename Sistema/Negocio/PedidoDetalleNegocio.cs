using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PedidoDetalleNegocio
    {
        public List<PedidoDetalle> Listar()
        {
            List<PedidoDetalle> lista = new List<PedidoDetalle>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                SELECT 
                    PD.Id,
                    PD.IdPedido,
                    PD.IdLibro,
                    PD.Cantidad,
                    PD.PrecioUnitario,
                    L.Titulo,
                    L.PrecioVenta
                FROM PEDIDOS_DETALLE PD
                INNER JOIN LIBROS L ON L.Id = PD.IdLibro");

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    PedidoDetalle aux = new PedidoDetalle();
                    aux.Id = (int)datos.Lector["Id"];

                    // Pedido
                    aux.Pedido = new Pedido();
                    aux.Pedido.Id = (int)datos.Lector["IdPedido"];

                    // Libro
                    aux.Libro = new Libro();
                    aux.Libro.Id = (int)datos.Lector["IdLibro"];
                    aux.Libro.Titulo = datos.Lector["Titulo"].ToString();
                    aux.Libro.PrecioVenta = (decimal)datos.Lector["PrecioVenta"];

                    // Otros
                    aux.Cantidad = (int)datos.Lector["Cantidad"];
                    aux.PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar pedido detalle: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public List<PedidoDetalleDTO> ListarPorPedido(int idPedido)
        {
            List<PedidoDetalleDTO> lista = new List<PedidoDetalleDTO>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT PD.Id,
                   PD.IdLibro,
                   L.Titulo,
                   PD.Cantidad,
                   PD.PrecioUnitario,
                   (PD.Cantidad * PD.PrecioUnitario) AS Subtotal
            FROM PEDIDOS_DETALLE PD
            INNER JOIN Libros L ON L.Id = PD.IdLibro
            WHERE PD.IdPedido = @idPedido
        ");

                datos.setearParametro("@idPedido", idPedido);

                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    PedidoDetalleDTO item = new PedidoDetalleDTO
                    {
                        Id = (int)datos.Lector["Id"],
                        IdLibro = (int)datos.Lector["IdLibro"],
                        Titulo = datos.Lector["Titulo"].ToString(),
                        Cantidad = (int)datos.Lector["Cantidad"],
                        PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"],
                        Subtotal = (decimal)datos.Lector["Subtotal"]
                    };

                    lista.Add(item);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }



        public void Agregar(PedidoDetalle nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                INSERT INTO PEDIDOS_DETALLE (IdPedido, IdLibro, Cantidad, PrecioUnitario)
                VALUES (@pedido, @libro, @cant, @precio)");

                datos.setearParametro("@pedido", nuevo.Pedido.Id);
                datos.setearParametro("@libro", nuevo.Libro.Id);
                datos.setearParametro("@cant", nuevo.Cantidad);
                datos.setearParametro("@precio", nuevo.PrecioUnitario);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar detalle: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }


        public void Modificar(PedidoDetalle modificado)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                UPDATE PEDIDOS_DETALLE
                SET IdLibro = @libro,
                    Cantidad = @cant,
                    PrecioUnitario = @precio
                WHERE Id = @id");

                datos.setearParametro("@libro", modificado.Libro.Id);
                datos.setearParametro("@cant", modificado.Cantidad);
                datos.setearParametro("@precio", modificado.PrecioUnitario);
                datos.setearParametro("@id", modificado.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar detalle: " + ex.Message);
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
                datos.setearConsulta("DELETE FROM PEDIDOS_DETALLE WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar detalle: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public int ObtenerIdLibro(int idDetalle)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("SELECT IdLibro FROM PEDIDOS_DETALLE WHERE Id = @idDetalle");
                datos.setearParametro("@idDetalle", idDetalle);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return (int)datos.Lector["IdLibro"];
                }

                return 0; // No encontrado
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener IdLibro: " + ex.Message);
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

    }
}
