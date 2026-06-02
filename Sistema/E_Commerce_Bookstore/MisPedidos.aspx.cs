using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class MisPedidos : System.Web.UI.Page
    {
        private PedidoNegocio negocio = new PedidoNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string origen = Request.QueryString["origen"];

                if (!string.IsNullOrEmpty(origen) && origen == "perfil")
                {
                    Session["OrigenPedidos"] = "perfil";
                }
                else
                {
                    Session["OrigenPedidos"] = "catalogo";
                }

                CargarPedidos();
                ConfigurarBotonVolver();
            }
            var master = this.Master as Site;
            if (master != null)
            {
                master.OcultarNavbar();
            }
        }

        private void CargarPedidos()
        {
            try
            {
                int idCliente = ObtenerIdClienteSesion();

                if (idCliente == 0)
                {
                    Response.Redirect("MiCuenta.aspx?ReturnUrl=MisPedidos.aspx", false);
                    return;
                }

                List<Pedido> lista = negocio.ListarPedidosPorCliente(idCliente);

                if (lista != null && lista.Count > 0)
                {
                    repPedidos.DataSource = lista;
                    repPedidos.DataBind();
                    repPedidos.Visible = true;
                    lblSinPedidos.Visible = false;
                }
                else
                {
                    repPedidos.Visible = false;
                    lblSinPedidos.Visible = true;
                    lblSinPedidos.Text = "No tenés pedidos registrados.";
                }
            }
            catch
            {
                repPedidos.Visible = false;
                lblSinPedidos.Visible = true;
                lblSinPedidos.Text = "Ocurrió un error al cargar tus pedidos. Intentá nuevamente más tarde.";
            }
        }




        private int ObtenerIdClienteSesion()
        {
            if (Session["IdCliente"] != null)
            {
                int id;
                if (int.TryParse(Session["IdCliente"].ToString(), out id))
                    return id;
            }

            return 0; // 0 = no hay cliente
        }
        private void ConfigurarBotonVolver()
        {
            string origen = Session["OrigenPedidos"] as string;

            if (origen == "perfil")
            {
                btnVolver.HRef = "MiPerfil.aspx";
                btnVolver.InnerText = "Volver a Mi Perfil";
            }
            else
            {
                btnVolver.HRef = "Catalogo.aspx";
                btnVolver.InnerText = "Seguir comprando";
            }
        }
    }
}