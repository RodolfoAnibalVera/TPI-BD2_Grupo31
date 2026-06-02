using Dominio;
using Negocio;
using System;
using E_Commerce_Bookstore.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class Carrito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                    int? idCliente = Session["IdCliente"] as int?;

                    CarritoNegocio negocio = new CarritoNegocio();
                    CarritoCompra carrito = negocio.ObtenerCarritoActivo(cookieId, idCliente);

                    if (carrito == null)
                    {
                        carrito = new CarritoCompra(); // carrito vacío para evitar null
                    }

                    Session["Carrito"] = carrito;

                    rptCarrito.DataSource = carrito.Items;
                    rptCarrito.DataBind();
                    lblTotal.Text = carrito.Total.ToString("N2");

                    if (Request.UrlReferrer != null && Request.UrlReferrer.Host == Request.Url.Host)
                    {
                        string nombrePagina = System.IO.Path.GetFileNameWithoutExtension(Request.UrlReferrer.AbsolutePath)?.ToLower();

                        if (nombrePagina == "miperfil")
                            Session["UrlAnteriorCarrito"] = "~/MiPerfil.aspx";
                        else
                            Session["UrlAnteriorCarrito"] = "~/Catalogo.aspx";
                    }
                    else
                    {
                        Session["UrlAnteriorCarrito"] = "~/Catalogo.aspx";
                    }

                }
                catch (Exception ex)
                {
                    lblTotal.Text = "0.00";
                    lblError.Text = "❌ Error al cargar el carrito: " + ex.Message;
                    lblError.CssClass = "text-danger d-block";
                }
            }

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            string urlAnterior = Session["UrlAnteriorCarrito"] as string;

            if (string.IsNullOrEmpty(urlAnterior))
                urlAnterior = "~/Catalogo.aspx";

            Response.Redirect(urlAnterior, true);
        }


        protected void btnComprar_Click(object sender, EventArgs e)
        {
            try
            {
                CarritoCompra carrito = Session["Carrito"] as CarritoCompra;

                if (carrito == null || carrito.Items == null || !carrito.Items.Any())
                {
                    lblError.Text = "⚠️ Tu carrito está vacío. Agregá al menos un libro antes de continuar.";
                    lblError.CssClass = "text-warning d-block";
                    return;
                }

                LibroNegocio libroNegocio = new LibroNegocio();
                List<string> errores = new List<string>();

                foreach (var item in carrito.Items)
                {
                    var libro = libroNegocio.ObtenerPorId(item.IdLibro);

                    if (libro == null || !libro.Activo)
                    {
                        errores.Add($"❌ El libro '{libro.Titulo}' no está disponible.");
                        continue;
                    }

                    if (libro.Stock < item.Cantidad)
                    {
                        errores.Add($"⚠️ Stock insuficiente para '{libro.Titulo}'. Disponible: {libro.Stock}, solicitado: {item.Cantidad}.");
                    }
                }

                if (errores.Any())
                {
                    lblError.Text = string.Join("<br/>", errores);
                    lblError.CssClass = "text-danger d-block";
                    return;
                }

                bool envioADomicilio = false;
                if (chkEnvioDomicilio != null)
                    envioADomicilio = chkEnvioDomicilio.Checked;

                Session["EnvioADomicilio"] = envioADomicilio;

                if (Session["IdCliente"] == null)
                {
                    // No esta logueado -> lo mando a MiCuenta con retorno a ProcesoEnvio
                    Response.Redirect("MiCuenta.aspx?ReturnUrl=ProcesoEnvio.aspx", false);
                }
                else
                {
                    // Ya esta logueado -> sigo el flujo normal
                    Response.Redirect("ProcesoEnvio.aspx", false);
                }
            }
            catch (Exception ex)
            {
                Session["error"] = ex;
                Response.Redirect("Error.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void rptCarrito_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                int? idCliente = Session["IdCliente"] as int?;
                int idLibro = int.Parse(e.CommandArgument.ToString());

                CarritoNegocio negocio = new CarritoNegocio();
                CarritoCompra carrito = negocio.ObtenerCarritoActivo(cookieId, idCliente);

                if (carrito == null || carrito.Items == null) return;

                switch (e.CommandName)
                {
                    case "Incrementar":
                        negocio.IncrementarItem(carrito.Id, idLibro);
                        break;

                    case "Decrementar":
                        negocio.DisminuirItem(carrito.Id, idLibro);
                        break;

                    case "Eliminar":
                        negocio.EliminarItem(carrito.Id, idLibro);
                        break;
                }

                // 🔄 Recargar carrito actualizado
                carrito = negocio.ObtenerCarritoActivo(cookieId, idCliente);
                Session["Carrito"] = carrito;

                // 🔄 Actualizar controles
                rptCarrito.DataSource = carrito.Items;
                rptCarrito.DataBind();
                lblTotal.Text = carrito.Total.ToString("N2");

                ((Site)Master).ActualizarCarritoVisual();
            }
            catch (Exception ex)
            {
                lblTotal.Text = "0.00";
                lblError.Text = "❌ Error al actualizar el carrito: " + ex.Message;
                lblError.CssClass = "text-danger d-block";
            }
        }
    }
}