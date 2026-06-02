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
    public partial class Populares : System.Web.UI.Page
    {
        private int? LibroAgregadoId
        {
            get => Session["LibroAgregadoId"] as int?;
            set => Session["LibroAgregadoId"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CargarPopulares();
                }
                catch (Exception ex)
                {
                    Session["error"] = ex;
                    Response.Redirect("Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                
            }
        }

        private void CargarPopulares()
        {
            LibroNegocio negocio = new LibroNegocio();
            repPopulares.DataSource = negocio.ListarPopulares();
            repPopulares.DataBind();
        }

        protected void btnAccionCommand(object sender, CommandEventArgs e)
        {
            int idLibro = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Detalle")
            {
                Response.Redirect($"Detalle.aspx?id={idLibro}");
            }
            else if (e.CommandName == "Comprar")
            {
                LibroAgregadoId = idLibro;

                DetalleNegocio negocio = new DetalleNegocio();
                var libro = negocio.ObtenerPorId(idLibro);

                if (libro == null || !libro.Activo || libro.Stock == 0)
                    return;

                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                int? idCliente = Session["IdCliente"] as int?;

                CarritoNegocio carritoNegocio = new CarritoNegocio();
                CarritoCompra carrito = carritoNegocio.ObtenerOCrearCarritoActivo(cookieId, idCliente);

                var existente = carrito.Items.FirstOrDefault(i => i.IdLibro == idLibro);
                int cantidadActual = existente?.Cantidad ?? 0;

                if (cantidadActual >= libro.Stock)
                    return;

                carritoNegocio.AgregarItem(carrito.Id, idLibro, 1, libro.PrecioVenta);

                carrito = carritoNegocio.ObtenerCarritoActivo(cookieId, idCliente);
                Session["Carrito"] = carrito;

                ((Site)Master).ActualizarCarritoVisual();

                CargarPopulares();
            }
        }

        protected void repPopulares_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var libro = (Libro)e.Item.DataItem;
                var lblLocalMensaje = (Label)e.Item.FindControl("lblLocalMensaje");
                var litPopular = (Literal)e.Item.FindControl("litPopular");
                var litBestSeller = (Literal)e.Item.FindControl("litBestSeller");

                lblLocalMensaje.Text = string.Empty;
                lblLocalMensaje.Visible = false;

                if (libro.Stock == 0)
                {
                    lblLocalMensaje.Text = $"❌ '{libro.Titulo}' sin stock disponible.";
                    lblLocalMensaje.Visible = true;
                    return;
                }

                var carrito = Session["Carrito"] as CarritoCompra;
                if (carrito != null && carrito.Items != null)
                {
                    var item = carrito.Items.FirstOrDefault(i => i.IdLibro == libro.Id);
                    if (item != null && item.Cantidad >= libro.Stock)
                    {
                        lblLocalMensaje.Text = $"⚠️ Ya tenés el máximo disponible de '{libro.Titulo}'.";
                        lblLocalMensaje.Visible = true;
                        return;
                    }
                }

                if (LibroAgregadoId.HasValue && libro.Id == LibroAgregadoId.Value)
                {
                    lblLocalMensaje.Text = $"✅ '{libro.Titulo}' agregado al carrito.";
                    lblLocalMensaje.Visible = true;
                }

                litPopular.Text = "<span class='badge bg-primary text-light position-absolute top-0 start-0 m-2'>Popular</span>";

                if (libro.BestSeller)
                {
                    litBestSeller.Text = "<span class='badge bg-warning text-dark position-absolute top-0 end-0 m-2'>Best Seller</span>";
                }
            }
        }
    }
}