using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class GestionDetalleVenta : System.Web.UI.Page
    {
        private PedidoDetalleNegocio negocio = new PedidoDetalleNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdTipoUsuario"] == null || Session["IdTipoUsuario"].ToString() != "1")
            {
                Session["error"] = new Exception("Acceso denegado: solo administradores pueden ingresar a esta página.");
                Response.Redirect("Error.aspx");
            }
            if (!IsPostBack)
            {
                if (Session["PedidoSeleccionado"] == null)
                {
                    Response.Redirect("GestionVentas.aspx");
                    return;
                }

                int idPedido = (int)Session["PedidoSeleccionado"];

                txtIdPedido.Text = idPedido.ToString(); // opcional
                CargarDetalles(idPedido);
                CargarLibros();
            }
        }

        private void CargarLibros()
        {
            LibroNegocio libNeg = new LibroNegocio();
            ddlLibros.DataSource = libNeg.Listar();
            ddlLibros.DataTextField = "Titulo";
            ddlLibros.DataValueField = "Id";
            ddlLibros.DataBind();
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            Response.Redirect("GestionVentas.aspx");
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                negocio.Eliminar(int.Parse(txtIdDetalle.Text));

                int idPedido = (int)Session["PedidoSeleccionado"];
                CargarDetalles(idPedido);

                lblMensaje.Text = "<div class='alert alert-danger'>Artículo eliminado.</div>";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"<div class='alert alert-danger'>{ex.Message}</div>";
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                PedidoDetalle mod = new PedidoDetalle
                {
                    Id = int.Parse(txtIdDetalle.Text),
                    Pedido = new Pedido { Id = int.Parse(txtIdPedido.Text) },
                    Libro = new Libro { Id = int.Parse(ddlLibros.SelectedValue) },
                    Cantidad = int.Parse(txtCantidad.Text),
                    PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text)
                };

                negocio.Modificar(mod);

                int idPedido = (int)Session["PedidoSeleccionado"];
                CargarDetalles(idPedido);

                lblMensaje.Text = "<div class='alert alert-warning'>Artículo modificado.</div>";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"<div class='alert alert-danger'>{ex.Message}</div>";
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                PedidoDetalle nuevo = new PedidoDetalle
                {
                    Pedido = new Pedido { Id = int.Parse(txtIdPedido.Text) },
                    Libro = new Libro { Id = int.Parse(ddlLibros.SelectedValue) },
                    Cantidad = int.Parse(txtCantidad.Text),
                    PrecioUnitario = decimal.Parse(txtPrecioUnitario.Text)
                };

                negocio.Agregar(nuevo);

                int idPedido = (int)Session["PedidoSeleccionado"];
                CargarDetalles(idPedido);

                lblMensaje.Text = "<div class='alert alert-success'>Artículo agregado.</div>";
            }
            catch (Exception ex)
            {
                lblMensaje.Text = $"<div class='alert alert-danger'>{ex.Message}</div>";
            }
        }

        private void CargarDetalles(int idPedido)
        {
            dgvDetalles.DataSource = negocio.ListarPorPedido(idPedido); // <-- DTO CORRECTO
            dgvDetalles.DataBind();
        }

        protected void dgvDetalles_SelectedIndexChanged1(object sender, EventArgs e)
        {
            GridViewRow row = dgvDetalles.SelectedRow;

            txtIdDetalle.Text = row.Cells[0].Text;

            int idDetalle = int.Parse(txtIdDetalle.Text);
            int idLibro = negocio.ObtenerIdLibro(idDetalle);

            ddlLibros.SelectedValue = idLibro.ToString();

            txtCantidad.Text = row.Cells[2].Text;
            txtPrecioUnitario.Text = row.Cells[3].Text.Replace("$", "");
        }
    }
}
