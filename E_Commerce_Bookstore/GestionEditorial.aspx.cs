using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class GestionEditorial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["IdTipoUsuario"] == null || Session["IdTipoUsuario"].ToString() != "1")
            {
                Session["error"] = new Exception("Acceso denegado: solo administradores pueden ingresar a esta página.");
                Response.Redirect("Error.aspx");
            }
            if (!IsPostBack)
                CargarGrilla();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtId.Text);
            EditorialNegocio negocio = new EditorialNegocio();
            negocio.Eliminar(id);

            lblMensaje.Text = "Editorial eliminada correctamente.";
            lblMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            CargarGrilla();
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            Editorial editorial = new Editorial
            {
                Id = int.Parse(txtId.Text),
                Nombre = txtNombre.Text.Trim(),
                Pais = txtPais.Text.Trim()
            };

            EditorialNegocio negocio = new EditorialNegocio();
            negocio.Modificar(editorial);

            lblMensaje.Text = "Editorial modificada correctamente.";
            lblMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            CargarGrilla();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Editorial nuevo = new Editorial
            {
                Nombre = txtNombre.Text.Trim(),
                Pais = txtPais.Text.Trim()
            };

            EditorialNegocio negocio = new EditorialNegocio();
            negocio.Agregar(nuevo);

            lblMensaje.Text = "Editorial agregada correctamente.";
            lblMensaje.ForeColor = System.Drawing.Color.Green;
            CargarGrilla();
        }

        private void CargarGrilla()
        {
            EditorialNegocio negocio = new EditorialNegocio();
            dgvEditorial.DataSource = negocio.Listar();
            dgvEditorial.DataBind();
        }

        protected void dgvEditorial_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = dgvEditorial.SelectedDataKey.Value.ToString();
            EditorialNegocio negocio = new EditorialNegocio();
            var lista = negocio.Listar();
            var editorial = lista.Find(x => x.Id == int.Parse(id));

            if (editorial != null)
            {
                txtId.Text = editorial.Id.ToString();
                txtNombre.Text = editorial.Nombre;
                txtPais.Text = editorial.Pais;
            }
        }

        protected void btnBuscarEdi_Click(object sender, EventArgs e)
        {
            EditorialNegocio negocio = new EditorialNegocio();

            string termino = txtBuscar.Text.Trim();

            List<Editorial> lista = negocio.Listar();

            if (!string.IsNullOrEmpty(termino))
            {
                lista = lista.FindAll(x =>
                    x.Nombre.ToUpper().Contains(termino.ToUpper()));
            }

            dgvEditorial.DataSource = lista;
            dgvEditorial.DataBind();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtPais.Text = "";
            CargarGrilla();
        }

        protected void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtPais.Text = "";
            CargarGrilla();
        }
    }
}