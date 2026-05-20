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
    public partial class Detalle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.UrlReferrer != null && Request.UrlReferrer.Host == Request.Url.Host)
                    {
                        string nombrePagina = System.IO.Path.GetFileNameWithoutExtension(Request.UrlReferrer.AbsolutePath)?.ToLower();

                        var paginasValidas = new[] { "catalogo", "bestseller", "ofertas", "populares" };

                        if (paginasValidas.Contains(nombrePagina))
                        {
                            Session["UrlAnterior"] = Request.UrlReferrer.AbsolutePath;
                            ConfigurarBotonVolver(nombrePagina);
                        }
                        else
                        {
                            // fallback seguro
                            Session["UrlAnterior"] = "~/Catalogo.aspx";
                            btnVolver.Text = "Volver al Catálogo";
                        }
                    }
                    else
                    {
                        Session["UrlAnterior"] = "~/Catalogo.aspx";
                        btnVolver.Text = "Volver al Catálogo";
                    }

                    if (int.TryParse(Request.QueryString["id"], out int idLibro))
                        CargarDetalleLibro(idLibro);
                    else
                    {
                        lblTitulo.Text = "Libro no disponible";
                        lblDescripcion.Text = "No se recibió un identificador válido para mostrar el detalle.";
                        btnAgregarCarrito.Visible = false;
                        grupoCantidad.Visible = false;
                        pnlSugerencias.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    Session["error"] = ex;
                    Response.Redirect("Error.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }

            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            string urlAnterior = Session["UrlAnterior"] as string;

            if (string.IsNullOrEmpty(urlAnterior))
            {
                urlAnterior = "~/Catalogo.aspx";
            }
            Response.Redirect(urlAnterior, true);
        }

        private void ConfigurarBotonVolver(string nombrePagina)
        {
            if (string.IsNullOrWhiteSpace(nombrePagina))
            {
                btnVolver.Text = "Volver";
                return;
            }

            switch (nombrePagina)
            {
                case "catalogo":
                    btnVolver.Text = "Volver al Catálogo";
                    break;

                case "bestseller":
                    btnVolver.Text = "Volver a Best Sellers";
                    break;

                case "ofertas":
                    btnVolver.Text = "Volver a Ofertas";
                    break;
                case "populares":
                    btnVolver.Text = "Volver a Populares";
                    break;

                default:
                    btnVolver.Text = "Volver";
                    break;
            }
        }

        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                int idProducto = int.Parse(Request.QueryString["id"]);
                int cantidadSolicitada;

                if (!int.TryParse(txtCantidad.Text, out cantidadSolicitada) || cantidadSolicitada <= 0)
                {
                    lblMensajeAgregado.Text = "⚠️ Ingresá una cantidad válida.";
                    lblMensajeAgregado.CssClass = "text-warning mt-2 d-block";
                    return;
                }

                DetalleNegocio negocio = new DetalleNegocio();
                var producto = negocio.ObtenerPorId(idProducto);
                if (producto == null || !producto.Activo)
                {
                    lblMensajeAgregado.Text = "❌ El libro no está disponible.";
                    lblMensajeAgregado.CssClass = "text-danger mt-2 d-block";
                    return;
                }

                if (producto.Stock == 0)
                {
                    lblMensajeAgregado.Text = "❌ No hay stock disponible.";
                    lblMensajeAgregado.CssClass = "text-danger mt-2 d-block";
                    return;
                }

                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                int? idCliente = Session["IdCliente"] as int?;

                CarritoNegocio carritoNegocio = new CarritoNegocio();
                CarritoCompra carrito = carritoNegocio.ObtenerOCrearCarritoActivo(cookieId, idCliente);

                var itemExistente = carrito.Items.FirstOrDefault(i => i.IdLibro == idProducto);
                int cantidadEnCarrito = itemExistente?.Cantidad ?? 0;
                int cantidadTotal = cantidadEnCarrito + cantidadSolicitada;

                if (cantidadTotal > producto.Stock)
                {
                    int disponibleParaAgregar = producto.Stock - cantidadEnCarrito;

                    if (disponibleParaAgregar <= 0)
                    {
                        lblMensajeAgregado.Text = $"⚠️ Ya seleccionaste el máximo disponible ({producto.Stock}).";
                        lblMensajeAgregado.CssClass = "text-warning mt-2 d-block";
                        return;
                    }

                    lblMensajeAgregado.Text = $"⚠️ Solo podés agregar {disponibleParaAgregar} unidad(es) más.";
                    lblMensajeAgregado.CssClass = "text-warning mt-2 d-block";
                    cantidadSolicitada = disponibleParaAgregar;
                }
                else if (cantidadTotal == producto.Stock)
                {
                    lblMensajeAgregado.Text = $"⚠️ Ya seleccionaste el máximo disponible ({producto.Stock}).";
                    lblMensajeAgregado.CssClass = "text-warning mt-2 d-block";
                }

                carritoNegocio.AgregarItem(carrito.Id, idProducto, cantidadSolicitada, producto.PrecioVenta);

                carrito = carritoNegocio.ObtenerCarritoActivo(cookieId, idCliente);
                Session["Carrito"] = carrito;

                ((Site)Master).ActualizarCarritoVisual();

                lblMensajeAgregado.Text = $"✅ Se agregaron {cantidadSolicitada} unidad(es) de '{producto.Titulo}' al carrito.";
                lblMensajeAgregado.CssClass = "text-success mt-2 d-block";
            }
            catch (Exception ex)
            {
                lblMensajeAgregado.Text = "❌ Error al agregar al carrito: " + ex.Message;
                lblMensajeAgregado.CssClass = "text-danger mt-2 d-block";
            }
        }
        private void CargarDetalleLibro(int idLibro)
        {
            try
            {
                DetalleNegocio negocio = new DetalleNegocio();
                Libro libro = negocio.ObtenerPorId(idLibro);

                if (libro != null && libro.Activo)
                {
                    lblTitulo.Text = libro.Titulo;
                    lblAutor.Text = libro.Autor?.Nombre ?? "Sin autor";
                    lblEditorial.Text = libro.Editorial?.Nombre ?? "Sin editorial";
                    lblCategoria.Text = libro.Categoria?.Nombre ?? "Sin categoría";
                    lblDescripcion.Text = libro.Descripcion;
                    lblIdioma.Text = libro.Idioma;
                    lblAnioEdicion.Text = libro.AnioEdicion.ToString();
                    lblPagina.Text = libro.Paginas.ToString();
                    lblPrecio.Text = libro.PrecioVenta.ToString("N2");
                    lblStock.Text = libro.Stock.ToString();
                    if (libro.Stock > 0)
                    {
                        lblStock.Text = libro.Stock.ToString();
                        pStock.Attributes["class"] = "text-success";
                        btnAgregarCarrito.Enabled = true;
                        grupoCantidad.Visible = true;
                        badgeStock.Visible = libro.Stock == 1;
                    }
                    else
                    {
                        lblStock.Text = "Sin stock";
                        pStock.Attributes["class"] = "text-danger";
                        btnAgregarCarrito.Visible = false;
                        grupoCantidad.Visible = false;
                        badgeStock.Visible = false;
                    }
                    imgLibro.ImageUrl = libro.ImagenUrl;
                    if (libro.BestSeller)
                    {
                        litBestSeller.Text = "<span class='badge bg-warning text-dark position-absolute top-0 start-0 m-2'>Best Seller</span>";
                    }

                    var sugeridos = negocio.ListarSugeridosPorCategoria(libro.Categoria.Id, libro.Id);
                    repSugeridos.DataSource = sugeridos;
                    repSugeridos.DataBind();



                    pnlSugerencias.Visible = sugeridos.Count > 0;
                }
                else
                {
                    lblTitulo.Text = "Libro no disponible";
                    lblDescripcion.Text = "Este libro no se encuentra activo o no existe.";
                    btnAgregarCarrito.Enabled = false;
                    txtCantidad.Enabled = false;
                    pnlSugerencias.Visible = false;

                }
            }
            catch (Exception)
            {
                lblTitulo.Text = "Error al cargar el libro";
                lblDescripcion.Text = "Ocurrió un problema al intentar mostrar los datos.";
                btnAgregarCarrito.Enabled = false;
                txtCantidad.Enabled = false;
                pnlSugerencias.Visible = false;
            }
        }
        protected void repSugeridos_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var libro = (Libro)e.Item.DataItem;
                var litBestSeller = (Literal)e.Item.FindControl("litBestSeller");

                if (libro.BestSeller)
                {
                    litBestSeller.Text = "<span class='badge bg-warning text-dark position-absolute top-0 start-0 m-2'>Best Seller</span>";
                }
            }
        }
    }
}