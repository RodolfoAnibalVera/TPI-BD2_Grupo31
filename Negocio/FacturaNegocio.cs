using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class FacturaNegocio
    {
        public void CrearFactura(Factura factura)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                INSERT INTO FACTURAS
                (IdPedido, Nombre, Apellido, Direccion, Barrio, Ciudad, CP, Depto)
                VALUES
                (@IdPedido, @Nombre, @Apellido, @Direccion, @Barrio, @Ciudad, @CP, @Depto)
            ");

                datos.setearParametro("@IdPedido", factura.IdPedido);
                datos.setearParametro("@Nombre", factura.Nombre);
                datos.setearParametro("@Apellido", factura.Apellido);
                datos.setearParametro("@Direccion", factura.Direccion);
                datos.setearParametro("@Barrio", factura.Barrio);
                datos.setearParametro("@Ciudad", factura.Ciudad);
                datos.setearParametro("@CP", factura.CP);
                datos.setearParametro("@Depto", factura.Depto);

                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public Factura ObtenerFacturaPorPedido(int idPedido)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta(@"
                SELECT * FROM FACTURAS
                WHERE IdPedido = @IdPedido
            ");

                datos.setearParametro("@IdPedido", idPedido);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Factura
                    {
                        Id = (int)datos.Lector["Id"],
                        IdPedido = idPedido,
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Direccion = datos.Lector["Direccion"].ToString(),
                        Barrio = datos.Lector["Barrio"].ToString(),
                        Ciudad = datos.Lector["Ciudad"].ToString(),
                        CP = datos.Lector["CP"].ToString(),
                        Depto = datos.Lector["Depto"].ToString()
                    };
                }

                return null;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void EnviarFacturaPorMail(int idPedido, string numeroPedido, string emailDestino)
        {
            // Obtener la factura
            Factura factura = ObtenerFacturaPorPedido(idPedido);
            if (factura == null)
                throw new Exception("No se encontró la factura del pedido.");

            // Obtener el pedido por NÚMERO (ahora sí el correcto)
            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            Pedido pedido = pedidoNegocio.ObtenerPedidoPorNumero(numeroPedido);

            if (pedido == null)
                throw new Exception("No se encontró el pedido.");

            // Armar cuerpo HTML
            StringBuilder cuerpo = new StringBuilder();
            cuerpo.AppendLine("<h2>Factura de tu compra</h2>");
            cuerpo.AppendLine($"<p><strong>Pedido:</strong> {pedido.NumeroPedido}</p>");
            cuerpo.AppendLine($"<p><strong>Fecha:</strong> {factura.Fecha:dd/MM/yyyy HH:mm}</p>");
            cuerpo.AppendLine($"<p><strong>Nombre:</strong> {factura.Nombre} {factura.Apellido}</p>");
            cuerpo.AppendLine($"<p><strong>Dirección:</strong> {factura.Direccion}</p>");
            cuerpo.AppendLine($"<p><strong>Barrio:</strong> {factura.Barrio}</p>");
            cuerpo.AppendLine($"<p><strong>Ciudad:</strong> {factura.Ciudad}</p>");
            cuerpo.AppendLine($"<p><strong>CP:</strong> {factura.CP}</p>");
            cuerpo.AppendLine($"<p><strong>Depto:</strong> {factura.Depto}</p>");
            cuerpo.AppendLine("<hr/>");
            cuerpo.AppendLine($"<p><strong>Total:</strong> {pedido.Total:C2}</p>");

            EmailService emailService = new EmailService();
            emailService.armarCorreo(
                emailDestino,
                $"Factura - Pedido {pedido.NumeroPedido}",
                cuerpo.ToString()
            );
            emailService.enviarEmail();
        }


    }
}
