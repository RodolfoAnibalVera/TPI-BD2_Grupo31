using Negocio;
using Dominio;
using System;
using E_Commerce_Bookstore.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class Catalogo : System.Web.UI.Page
    {
        private LibroNegocio libroNegocio = new LibroNegocio();
        private CategoriaNegocio categoriaNegocio = new CategoriaNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                Cargar();
            }
        }
        private void Cargar()
        {
            try
            {
                int idCategoria = ObtenerIDCategoria();

                List<Libro> listaLibros;
                if (idCategoria > 0)
                {
                    listaLibros = libroNegocio.ListarPorCategoria(idCategoria);

                    if (listaLibros.Count == 0)
                    {
                        lblMensaje.CssClass = "text-warning d-block mb-3";
                        lblMensaje.Text = "No hay libros para la categoría seleccionada.";
                    }
                    else
                    {
                        lblMensaje.CssClass = "text-success d-block mb-3";
                        lblMensaje.Text = "Filtrado por categoría.";
                    }
                }
                else
                {
                    listaLibros = libroNegocio.Listar();
                    lblMensaje.Text = "";
                }

                repLibros.DataSource = listaLibros;
                repLibros.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "text-danger d-block mb-3";
                lblMensaje.Text = "Error al cargar catálogo: " + ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string texto = txtBuscar.Text.Trim().ToLower();
                int idCategoria = ObtenerIDCategoria();

                List<Libro> resultados =new List<Libro>();

                if (texto=="")
                {
                    if (idCategoria > 0)
                    {
                        resultados=libroNegocio.ListarPorCategoria(idCategoria);
                    }
                    else
                    {
                        resultados = libroNegocio.Listar();
                    }
                }
                else
                {
                    List<Libro> encontrados = libroNegocio.Buscar(texto);
                    foreach (var libro in encontrados)
                        {
                            if (idCategoria == 0 || (libro.Categoria != null && libro.Categoria.Id == idCategoria))
                        {
                            string titulo=(libro.Titulo ?? "").ToLower();
                            string autor = (libro.Autor?.Nombre ?? "").ToLower();
                            string isbn = (libro.ISBN ?? "").ToLower();

                            if (titulo.Contains(texto) || autor.Contains(texto) || isbn.Contains(texto))
                            {

                            resultados.Add(libro);
                            }
                        }
                    }
                }

                if (resultados.Count == 0)
                {
                    lblMensaje.CssClass = "text-warning d-block mb-3";
                    lblMensaje.Text = "No se encontraron resultados.";
                }
                else
                {
                    lblMensaje.CssClass = "text-success d-block mb-3";
                    lblMensaje.Text = "Resultados encontrados: " + resultados.Count;
                }

                repLibros.DataSource = resultados;
                repLibros.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.CssClass = "text-danger d-block mb-3";
                lblMensaje.Text = "Error al buscar: " + ex.Message;
            }
        }

        private int ObtenerIDCategoria()
        {
            string valor = Request.QueryString["idCategoria"];
            int id = 0;
            if (!string.IsNullOrEmpty(valor))
                int.TryParse(valor, out id);
            return id;
        }
        protected void repCategorias_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
           
        }
        protected void lnkVerTodo_Click(object sender, EventArgs e)
        {
           
        }

        protected void repLibros_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
          
        }
        private int? LibroAgregadoId
        {
            get => Session["LibroAgregadoId"] as int?;
            set => Session["LibroAgregadoId"] = value;
        }
        protected void btnAccionCommand(object sender, CommandEventArgs e)
        {
            int idLibro = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName == "Detalle")
            {
                Response.Redirect("Detalle.aspx?id=" + idLibro);
                return;
            }

            if (e.CommandName == "Comprar")
            {
                LibroAgregadoId = idLibro;

                DetalleNegocio negocio = new DetalleNegocio();
                var libro = negocio.ObtenerPorId(idLibro);

                if (libro == null || !libro.Activo || libro.Stock == 0)
                    return;

                //  Obtener cookie y cliente
                string cookieId = CookieHelper.ObtenerCookieId(Request, Response);
                int? idCliente = Session["IdCliente"] as int?;

                //  Obtener o crear carrito
                CarritoNegocio carritoNegocio = new CarritoNegocio();
                CarritoCompra carrito = carritoNegocio.ObtenerOCrearCarritoActivo(cookieId, idCliente);

                //  Validar stock antes de agregar
                var existente = carrito.Items.FirstOrDefault(i => i.IdLibro == idLibro);
                int cantidadActual = existente?.Cantidad ?? 0;

                if (cantidadActual >= libro.Stock)
                    return; //  No agregar si ya está al máximo

                //  Agregar o incrementar
                carritoNegocio.AgregarItem(carrito.Id, idLibro, 1, libro.PrecioVenta);

                //  Actualizar sesión y badge
                carrito = carritoNegocio.ObtenerCarritoActivo(cookieId, idCliente);
                Session["Carrito"] = carrito;

                ((Site)Master).ActualizarCarritoVisual();

                //  Rebind con mensaje local
                repLibros.DataSource = libroNegocio.Listar();
                repLibros.DataBind();
            }
        }
        protected void repLibros_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var libro = (Libro)e.Item.DataItem;
                var lblLocalMensaje = (Label)e.Item.FindControl("lblLocalMensaje");

                // 🔹 Reiniciar mensaje
                lblLocalMensaje.Text = string.Empty;
                lblLocalMensaje.Visible = false;

                // 🔹 Mostrar si no tiene stock
                if (libro.Stock == 0)
                {
                    lblLocalMensaje.Text = $"❌ '{libro.Titulo}' sin stock disponible.";
                    lblLocalMensaje.Visible = true;
                    return; // ⛔ Detener aquí, no seguir evaluando
                }

                // 🔹 Mostrar si ya se alcanzó el máximo en el carrito
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

                // 🔹 Mostrar si fue agregado recientemente
                if (LibroAgregadoId.HasValue && libro.Id == LibroAgregadoId.Value)
                {
                    lblLocalMensaje.Text = $"✅ '{libro.Titulo}' agregado al carrito.";
                    lblLocalMensaje.Visible = true;
                }

                var litBestSeller = (Literal)e.Item.FindControl("litBestSeller");
                if (libro.BestSeller)
                {
                    litBestSeller.Text = "<span class='badge bg-warning text-dark position-absolute top-0 start-0 m-2'>Best Seller</span>";
                }
                else
                {
                    litBestSeller.Text = string.Empty;
                }
            }
        }

    }
}
