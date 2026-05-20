using Dominio;
using E_Commerce_Bookstore.Helpers;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SincronizarClienteConCarrito();
                CargarCategoriasNavbar();
                ActualizarCarritoVisual();

                if (Session["IdTipoUsuario"] != null && Session["IdTipoUsuario"].ToString() == "1")
                {
                    hlAdministracion.Visible = true;
                }
                else
                {
                    hlAdministracion.Visible = false;
                }

                if (Session["Cliente"] is Cliente cliente)
                {
                    hlBadgePerfil.NavigateUrl = "~/MiPerfil.aspx";
                    lblNombreUsuario.Text = cliente.Nombre;
                }
                else
                {
                    hlBadgePerfil.NavigateUrl = "~/MiCuenta.aspx?ReturnUrl=MiPerfil.aspx&origen=badge";
                    lblNombreUsuario.Text = "Ingresar";
                }

            }
        }

        private void CargarCategoriasNavbar()
        {
            try
            {
                List<Dominio.Categoria> listaCategorias = categoriaNegocio.Listar();
                repCategoriasNavbar.DataSource = listaCategorias;
                repCategoriasNavbar.DataBind();
            }
            catch
            {
                
            }
        }
        public void ActualizarCarritoVisual()
        {
            string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
            int? idCliente = Session["IdCliente"] as int?;

            CarritoNegocio negocio = new CarritoNegocio();
            CarritoCompra carrito = negocio.ObtenerCarritoActivo(cookieId, idCliente);

            Session["Carrito"] = carrito;

            Label lblCantidad = (Label)FindControl("lblCantidadCarrito");
            UpdatePanel upd = (UpdatePanel)FindControl("updCarrito");

            if (lblCantidad != null)
            {
                int cantidadTotal = carrito?.Items?.Sum(i => i.Cantidad) ?? 0;
                lblCantidad.Text = cantidadTotal.ToString();
                lblCantidad.Visible = cantidadTotal > 0;
            }

            if (upd != null)
            {
                upd.Update();
            }
        }

        public void OcultarNavbar()
        {
            pnlNavbar.Visible = false;
        }

        private void SincronizarClienteConCarrito()
        {
            string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
            CarritoNegocio carritoNegocio = new CarritoNegocio();
            CarritoCompra carrito = carritoNegocio.ObtenerCarritoActivo(cookieId, null);

            if (carrito != null)
            {
                // Guardar carrito en sesión siempre
                Session["Carrito"] = carrito;

                // Si el carrito tiene cliente y la sesión no lo tiene, sincronizar
                if (carrito.IdCliente.HasValue && Session["IdCliente"] == null)
                {
                    int idCliente = carrito.IdCliente.Value;

                    ClienteNegocio clienteNegocio = new ClienteNegocio();
                    Cliente cliente = clienteNegocio.ObtenerClientePorId(idCliente);

                    Session["IdCliente"] = idCliente;
                    Session["Cliente"] = cliente;
                }
            }
        }


    }
}