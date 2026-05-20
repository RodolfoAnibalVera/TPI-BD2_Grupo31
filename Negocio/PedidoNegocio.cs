using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class PedidoNegocio
    {
        public List<Pedido> ListarPedidosPorCliente(int idCliente)
        {
            List<Pedido> lista = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT 
                p.Id,
                p.NumeroPedido,
                p.Fecha,
                p.Estado,
                p.Subtotal,
                e.Precio AS PrecioEnvio,
                e.DireccionEnvio,
                (SELECT TOP 1 Metodo 
                 FROM PAGOS 
                 WHERE IdPedido = p.Id 
                 ORDER BY Fecha DESC) AS MetodoPago
            FROM PEDIDOS p
            LEFT JOIN ENVIOS e ON e.IdPedido = p.Id
            WHERE p.IdCliente = @IdCliente
            ORDER BY p.Fecha DESC
        ");

                datos.setearParametro("@IdCliente", idCliente);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    decimal subtotal = Convert.ToDecimal(datos.Lector["Subtotal"]);
                    decimal precioEnvio = datos.Lector["PrecioEnvio"] != DBNull.Value
                        ? Convert.ToDecimal(datos.Lector["PrecioEnvio"])
                        : 0;

                    string direccionEnvio = datos.Lector["DireccionEnvio"] != DBNull.Value
                        ? datos.Lector["DireccionEnvio"].ToString()
                        : null;

                    Pedido pedido = new Pedido
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        NumeroPedido = datos.Lector["NumeroPedido"].ToString(),
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Estado = datos.Lector["Estado"].ToString(),
                        Subtotal = subtotal,
                        Total = subtotal + precioEnvio,
                        Cliente = new Cliente { Id = idCliente },
                        DireccionEnvio = direccionEnvio
                    };

                    lista.Add(pedido);
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


        public int ObtenerIdClientePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TOP 1 Id FROM CLIENTES WHERE Email = @Email");
                datos.setearParametro("@Email", email);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return (int)datos.Lector["Id"];
                else
                    return 0;
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

        public Pedido ObtenerPedidoPorNumero(string numeroPedido)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
           SELECT 
               p.Id,p.NumeroPedido,p.Fecha,p.Estado,p.Subtotal,e.Precio AS PrecioEnvio,
               e.DireccionEnvio
           FROM PEDIDOS p
           LEFT JOIN ENVIOS e ON e.IdPedido = p.Id
           WHERE p.NumeroPedido = @NumeroPedido
        ");
                datos.setearParametro("@NumeroPedido", numeroPedido);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    decimal subtotal = Convert.ToDecimal(datos.Lector["Subtotal"]);
                    decimal precioEnvio = datos.Lector["PrecioEnvio"] != DBNull.Value
                        ? Convert.ToDecimal(datos.Lector["PrecioEnvio"])
                        : 0;

                    Pedido pedido = new Pedido
                    {
                        Id = Convert.ToInt32(datos.Lector["Id"]),
                        NumeroPedido = datos.Lector["NumeroPedido"].ToString(),
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Estado = datos.Lector["Estado"].ToString(),
                        Subtotal = subtotal,
                        Total = subtotal + precioEnvio
                    };

                    // Dirección de envío (tabla ENVIOS)
                    if (datos.Lector["DireccionEnvio"] != DBNull.Value)
                        pedido.DireccionEnvio = datos.Lector["DireccionEnvio"].ToString();
                  
                    return pedido;
                }

                return null;
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

        public int CrearPedido(Pedido pedido)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            INSERT INTO PEDIDOS
                (NumeroPedido, Fecha, Estado, Subtotal, Total, IdCliente)
            OUTPUT INSERTED.Id
            VALUES
                (@NumeroPedido, @Fecha, @Estado, @Subtotal, @Total, @IdCliente)
        ");

                datos.setearParametro("@NumeroPedido", pedido.NumeroPedido);
                datos.setearParametro("@Fecha", pedido.Fecha);
                datos.setearParametro("@Estado", pedido.Estado);
                datos.setearParametro("@Subtotal", pedido.Subtotal);
                datos.setearParametro("@Total", pedido.Total);
                datos.setearParametro("@IdCliente", pedido.Cliente.Id);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return (int)datos.Lector[0];

                return 0;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public string GenerarNumeroPedido()
        {
            // Ejemplo simple: PED-638293847293
            return "PED-" + DateTime.Now.Ticks;
        }

        public void CrearDetallePedido(int idPedido, ItemCarrito item)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            INSERT INTO PEDIDOS_DETALLE
            (IdPedido, IdLibro, Cantidad, PrecioUnitario)
            VALUES (@IdPedido, @IdLibro, @Cantidad, @PrecioUnitario)
        ");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@IdLibro", item.IdLibro);
                datos.setearParametro("@Cantidad", item.Cantidad);
                datos.setearParametro("@PrecioUnitario", item.PrecioUnitario);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void RegistrarPago(int idPedido, decimal monto, string metodo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            INSERT INTO PAGOS
            (IdPedido, Monto, Metodo, Estado)
            VALUES (@IdPedido, @Monto, @Metodo, @Estado)
        ");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@Monto", monto);
                datos.setearParametro("@Metodo", metodo);
                datos.setearParametro("@Estado", "Pendiente");

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void RegistrarEnvio(int idPedido, string metodo, decimal precio,
                           string barrio, string ciudad, string departamento,
                           string nombre, string apellido, string cp,string direccion)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            INSERT INTO ENVIOS
            (IdPedido, MetodoDeEnvio, Precio, EstadoEnvio, Barrio, Ciudad, Departamento,
             NombreEnvio, ApellidoEnvio, CPEnvio,DireccionEnvio)
            VALUES
            (@IdPedido, @MetodoDeEnvio, @Precio, @EstadoEnvio, @Barrio, @Ciudad, @Departamento,
             @NombreEnvio, @ApellidoEnvio, @CPEnvio,@DireccionEnvio)
        ");

                datos.setearParametro("@IdPedido", idPedido);
                datos.setearParametro("@MetodoDeEnvio", metodo);
                datos.setearParametro("@Precio", precio);
                datos.setearParametro("@EstadoEnvio", "Pendiente");
                datos.setearParametro("@Barrio", barrio);
                datos.setearParametro("@Ciudad", ciudad);
                datos.setearParametro("@Departamento", departamento);
                datos.setearParametro("@NombreEnvio", nombre);
                datos.setearParametro("@ApellidoEnvio", apellido);
                datos.setearParametro("@CPEnvio", cp);
                datos.setearParametro("@DireccionEnvio", direccion);


                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public string ObtenerMetodoPago(int idPedido)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT TOP 1 Metodo FROM PAGOS WHERE IdPedido=@id ORDER BY Fecha DESC");
                datos.setearParametro("@id", idPedido);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                    return datos.Lector["Metodo"].ToString();

                return "";
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Pedido> Listar()
        {
            List<Pedido> lista = new List<Pedido>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
    SELECT 
        p.Id,
        p.NumeroPedido,
        p.Fecha,
        p.Estado,
        p.Subtotal,
        p.Total,
        ISNULL(e.DireccionEnvio, '') AS DireccionEnvio,
        c.Id AS IdCliente,
        c.Nombre AS ClienteNombre
    FROM PEDIDOS p
    LEFT JOIN ENVIOS e ON e.IdPedido = p.Id
    INNER JOIN CLIENTES c ON p.IdCliente = c.Id
");


                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Pedido p = new Pedido();
                    p.Id = (int)datos.Lector["Id"];

                    p.Cliente = new Cliente
                    {
                        Id = (int)datos.Lector["IdCliente"],
                        Nombre = datos.Lector["ClienteNombre"].ToString()
                    };

                    p.NumeroPedido = datos.Lector["NumeroPedido"].ToString();
                    p.Fecha = (DateTime)datos.Lector["Fecha"];
                    p.Estado = datos.Lector["Estado"].ToString();
                    p.Subtotal = (decimal)datos.Lector["Subtotal"];
                    p.Total = (decimal)datos.Lector["Total"];
                    p.DireccionEnvio = datos.Lector["DireccionEnvio"].ToString();

                    lista.Add(p);
                }

                return lista;
            }
            finally { datos.cerrarConexion(); }
        }

        public void Agregar(Pedido p)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                INSERT INTO PEDIDOS 
                (IdCliente, NumeroPedido, Fecha, Estado, Subtotal, Total, DireccionEnvio)
                VALUES (@c, @n, @f, @e, @s, @t, @d)");

                datos.setearParametro("@c", p.Cliente.Id);
                datos.setearParametro("@n", p.NumeroPedido);
                datos.setearParametro("@f", p.Fecha);
                datos.setearParametro("@e", p.Estado);
                datos.setearParametro("@s", p.Subtotal);
                datos.setearParametro("@t", p.Total);
                datos.setearParametro("@d", p.DireccionEnvio);

                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Modificar(Pedido p)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta(@"
                UPDATE PEDIDOS SET
                IdCliente=@c, NumeroPedido=@n, Fecha=@f, Estado=@e,
                Subtotal=@s, Total=@t, DireccionEnvio=@d
                WHERE Id=@id");

                datos.setearParametro("@c", p.Cliente.Id);
                datos.setearParametro("@n", p.NumeroPedido);
                datos.setearParametro("@f", p.Fecha);
                datos.setearParametro("@e", p.Estado);
                datos.setearParametro("@s", p.Subtotal);
                datos.setearParametro("@t", p.Total);
                datos.setearParametro("@d", p.DireccionEnvio);
                datos.setearParametro("@id", p.Id);

                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("DELETE FROM PEDIDOS WHERE Id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            finally { datos.cerrarConexion(); }
        }

        public Pedido ObtenerPedido(int id)
        {
            Pedido pedido = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT P.Id, P.NumeroPedido, P.Fecha, P.Estado, 
                   P.Subtotal, P.Total, P.DireccionEnvio,
                   C.Id IdCliente, C.Nombre ClienteNombre
            FROM PEDIDOS P
            INNER JOIN CLIENTES C ON C.Id = P.IdCliente
            WHERE P.Id = @id");

                datos.setearParametro("@id", id);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    pedido = new Pedido
                    {
                        Id = (int)datos.Lector["Id"],
                        NumeroPedido = datos.Lector["NumeroPedido"].ToString(),
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Estado = datos.Lector["Estado"].ToString(),
                        Subtotal = (decimal)datos.Lector["Subtotal"],
                        Total = (decimal)datos.Lector["Total"],
                        DireccionEnvio = datos.Lector["DireccionEnvio"].ToString(),
                        Cliente = new Cliente
                        {
                            Id = (int)datos.Lector["IdCliente"],
                            Nombre = datos.Lector["ClienteNombre"].ToString()
                        }
                    };
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return pedido;
        }

        public List<PedidoDetalle> ListarDetalle(int idPedido)
        {
            List<PedidoDetalle> lista = new List<PedidoDetalle>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
            SELECT D.Id, D.Cantidad, D.PrecioUnitario,
                   L.Id IdLibro, L.Titulo
            FROM PEDIDOS_DETALLE D
            INNER JOIN LIBROS L ON L.Id = D.IdLibro
            WHERE D.IdPedido = @id");

                datos.setearParametro("@id", idPedido);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    PedidoDetalle det = new PedidoDetalle
                    {
                        Id = (int)datos.Lector["Id"],
                        Cantidad = (int)datos.Lector["Cantidad"],
                        PrecioUnitario = (decimal)datos.Lector["PrecioUnitario"],
                        Libro = new Libro
                        {
                            Id = (int)datos.Lector["IdLibro"],
                            Titulo = datos.Lector["Titulo"].ToString()
                        }
                    };

                    lista.Add(det);
                }
            }
            finally
            {
                datos.cerrarConexion();
            }

            return lista;
        }
    }
}
