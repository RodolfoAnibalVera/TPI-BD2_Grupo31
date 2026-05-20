using Dominio;
using E_Commerce_Bookstore.Helpers;
using Negocio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class ProcesoPago : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                pnlTransferencia.Visible = false;
                pnlEfectivo.Visible = false;
                pnlTarjeta.Visible = false;

                var master = this.Master as Site;
                if (master != null)
                {
                    master.OcultarNavbar();
                }
            }
        }
        protected void rblMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ocultar todos los paneles
            
            pnlTransferencia.Visible = false;
            pnlEfectivo.Visible = false;
            pnlTarjeta.Visible = false;

            // Mostrar el seleccionado
            string metodo = rblMetodo.SelectedValue;
            if (metodo == "TRANSFERENCIA")
            {
                pnlTransferencia.Visible = true;

                // Traer total real del carrito
                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
               
                if (Session["IdCliente"] == null)
                    return;

                int idCliente = (int)Session["IdCliente"];

                CarritoNegocio negocio = new CarritoNegocio();
                CarritoCompra carrito = negocio.ObtenerCarritoActivo(cookieId, idCliente);

                if (carrito != null)
                    lblMontoTransferencia.Text = " $" + carrito.Total.ToString("N2");
            }
            else if (metodo == "EFECTIVO")
            {
                pnlEfectivo.Visible = true;
            }
            else if (metodo == "DEBITO" || metodo == "CREDITO")
            {
                pnlTarjeta.Visible = true;
            }
        }
        protected void btnConfirmarPago_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            // Verificamos que se haya elegido metodo de pago
            if (string.IsNullOrEmpty(rblMetodo.SelectedValue))
            {                
                return;
            }

            try
            {
                string metodoPago = rblMetodo.SelectedValue;
                Session["MetodoPago"] = metodoPago;

                // Validar sesion
                if (Session["IdCliente"] == null)
                {
                    Response.Redirect("MiCuenta.aspx?ReturnUrl=ProcesoPago.aspx", false);
                    return;
                }
                int idCliente = (int)Session["IdCliente"];

                // Obtener carrito
                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                CarritoNegocio carritoNegocio = new CarritoNegocio();
                CarritoCompra carrito = carritoNegocio.ObtenerCarritoActivo(cookieId, idCliente);

                if (carrito == null)
                {
                    Response.Redirect("Carrito.aspx", false);
                    return;
                }

                // Crear pedido
                PedidoNegocio pedidoNegocio = new PedidoNegocio();
                Pedido pedido = new Pedido();
                pedido.Cliente = new Cliente { Id = idCliente };
                pedido.NumeroPedido = pedidoNegocio.GenerarNumeroPedido();
                pedido.Fecha = DateTime.Now;
                pedido.Estado = "Pendiente";
                pedido.Subtotal = carrito.Total;
                pedido.Total = carrito.Total;

                int idPedido = pedidoNegocio.CrearPedido(pedido);

                // Crear factura
                FacturaNegocio facNeg = new FacturaNegocio();
                Factura fac = new Factura
                {
                    IdPedido = idPedido,
                    Nombre = Session["NombreFacturacion"]?.ToString(),
                    Apellido = Session["ApellidoFacturacion"]?.ToString(),
                    Direccion = Session["DireccionFacturacion"]?.ToString(),
                    Barrio = Session["BarrioFacturacion"]?.ToString(),
                    Ciudad = Session["CiudadFacturacion"]?.ToString(),
                    CP = Session["CPFacturacion"]?.ToString(),
                    Depto = Session["DeptoFacturacion"]?.ToString()
                };

                facNeg.CrearFactura(fac);

                // Envio por email
                string correoDestino = Session["EmailContacto"] != null
                    ? Session["EmailContacto"].ToString()
                    : string.Empty;

                if (!string.IsNullOrEmpty(correoDestino))
                {
                    facNeg.EnviarFacturaPorMail(idPedido, pedido.NumeroPedido, correoDestino);
                }

                // Guardar detalle del pedido
                if (carrito.Items != null)
                {
                    foreach (var item in carrito.Items)
                    {
                        pedidoNegocio.CrearDetallePedido(idPedido, item);
                    }
                }

                // Registrar pago
                decimal montoPago = carrito.Total;
                pedidoNegocio.RegistrarPago(idPedido, montoPago, metodoPago);

                // Registrar envio
                bool envioADomicilio = false;
                if (Session["EnvioADomicilio"] != null)
                {
                    bool.TryParse(Session["EnvioADomicilio"].ToString(), out envioADomicilio);
                }

                if (envioADomicilio)
                {
                    string barrio = Session["BarrioEnvio"] != null ? Session["BarrioEnvio"].ToString() : "";
                    string ciudad = Session["CiudadEnvio"] != null ? Session["CiudadEnvio"].ToString() : "";
                    string depto = Session["DeptoEnvio"] != null ? Session["DeptoEnvio"].ToString() : "";
                    string nombre = Session["NombreEnvio"]?.ToString();
                    string apellido = Session["ApellidoEnvio"]?.ToString();
                    string direccion = Session["DireccionEnvio"]?.ToString();
                    string cp = Session["CPEnvio"]?.ToString();

                    pedidoNegocio.RegistrarEnvio(idPedido, "Envío a domicilio", 0m,
                                                 barrio, ciudad, depto,
                                                 nombre, apellido, cp, direccion);
                }

                // Descontar stock y cerrar carrito
                carritoNegocio.DescontarStockPorCarrito(carrito.Id);
                carritoNegocio.DesactivarCarrito(carrito.Id);
                Session["Carrito"] = null;

                // Actualizar visual del carrito
                var master = this.Master as Site;
                if (master != null)
                {
                    master.ActualizarCarritoVisual();
                }

                // Guardar ultimos valores
                Session["UltimoPedidoId"] = idPedido;
                Session["UltimoNumeroPedido"] = pedido.NumeroPedido;

                Response.Redirect("ConfirmacionCompra.aspx", false);
            }
            catch
            {
                Response.Redirect("Error.aspx", false);
            }
        }

        protected void ValidarVencimientoTarjeta(object source, ServerValidateEventArgs e)
        {
            e.IsValid = false;

            string texto = (e.Value ?? "").Trim();
            
            if (texto.Length != 5 || texto[2] != '/')
                return;

            int mes;
            int anio2;
            if (!int.TryParse(texto.Substring(0, 2), out mes))
                return;
            if (!int.TryParse(texto.Substring(3, 2), out anio2))
                return;

            if (mes < 1 || mes > 12)
                return;

            int anioCompleto = 2000 + anio2;

            DateTime hoy = DateTime.Today;
            int anioActual = hoy.Year;
            int mesActual = hoy.Month;

            // Si el año es menor, esta vencida
            if (anioCompleto < anioActual)
                return;

            // Si es el mismo año pero mes menor, esta vencida
            if (anioCompleto == anioActual && mes < mesActual)
                return;

            // Si llego hasta aca, la fecha es valida
            e.IsValid = true;
        }

    }
}
