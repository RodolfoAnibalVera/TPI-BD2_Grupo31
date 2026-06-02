using Dominio;
using Negocio;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class ConfirmacionCompra : System.Web.UI.Page
    {
        private PedidoNegocio negocio = new PedidoNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarDatosPedido();
                    MostrarMensajes();
                }
                catch
                {
                    Response.Redirect("Error.aspx", false);
                }
            }
        }


        private void CargarDatosPedido()
        {
            if (Session["UltimoNumeroPedido"] == null)
                return;

            string numeroPedido = Session["UltimoNumeroPedido"].ToString();

            PedidoNegocio negocio = new PedidoNegocio();
            Pedido pedido = negocio.ObtenerPedidoPorNumero(numeroPedido);

            if (pedido == null)
                return;

            lblNumeroPedido.Text = pedido.NumeroPedido;
            lblFecha.Text = pedido.Fecha.ToString("dd/MM/yyyy HH:mm");
            lblTotal.Text = pedido.Total.ToString("C2");
            lblEstado.Text = pedido.Estado;

            if (!string.IsNullOrEmpty(pedido.DireccionEnvio))
                lblDireccionEnvio.Text = pedido.DireccionEnvio;
            else
                lblDireccionEnvio.Text = "Retiro en local";

        }

        private void MostrarMensajes()
        {
            string metodo = "";

            if (Session["MetodoPago"] != null)
                metodo = Session["MetodoPago"].ToString();

            // Título principal
            lblTitulo.Text = "¡Compra realizada!";

            // Badge (arriba)
            if (metodo == "TRANSFERENCIA")
                lblMetodo.Text = "Transferencia bancaria";

            if (metodo == "EFECTIVO")
                lblMetodo.Text = "Pago en efectivo";

            if (metodo == "DEBITO")
                lblMetodo.Text = "Tarjeta de débito";

            if (metodo == "CREDITO")
                lblMetodo.Text = "Tarjeta de crédito";

            // MENSAJE PRINCIPAL
            if (metodo == "TRANSFERENCIA")
            {
                lblMensaje.Text =
                    "Una vez realizada la transferencia, enviá el comprobante a:<br/>" +
                    "Email: <strong>contacto@bookcomicstore.com</strong><br/>" +
                    "WhatsApp: <strong>+34 600 123 456</strong>";

                boxMensaje.Attributes["class"] = "alert alert-info mb-4";
            }

            else if (metodo == "EFECTIVO")
            {
                lblMensaje.Text = "Tu pedido está listo para retirar en el local.";
                boxMensaje.Attributes["class"] = "alert alert-success mb-4";
            }
            else if (metodo == "DEBITO" || metodo == "CREDITO")
            {
                lblMensaje.Text = "Tu pago con tarjeta está siendo procesado. En breve confirmaremos la operación.";
                boxMensaje.Attributes["class"] = "alert alert-primary mb-4";
            }
            else
            {
                lblMensaje.Text = "Gracias por su compra.";
                boxMensaje.Attributes["class"] = "alert alert-primary mb-4";
            }
        }
    }
}