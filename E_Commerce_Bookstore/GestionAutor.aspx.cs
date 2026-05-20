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
    public partial class GestionAutor : System.Web.UI.Page
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

        protected void dgvAutores_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = (int)dgvAutores.SelectedDataKey.Value;

            AutorNegocio negocio = new AutorNegocio();
            Autor a = negocio.ListarGrilla().Find(x => x.Id == id);

            txtId.Value = a.Id.ToString();
            txtNombre.Text = a.Nombre;
            txtNacionalidad.Text = a.Nacionalidad;
        }

        protected void btnBuscarAutor_Click(object sender, EventArgs e)
        {
            AutorNegocio negocio = new AutorNegocio();
            dgvAutores.DataSource = negocio.Buscar(txtBuscar.Text.Trim());
            dgvAutores.DataBind();
        }

        protected void btnEliminarAutor_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Value);

                AutorNegocio negocio = new AutorNegocio();
                negocio.Eliminar(id);

                lbMensaje.Text = "Autor eliminado.";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = ex.Message;
            }
        }

        protected void btnModificarAutor_Click(object sender, EventArgs e)
        {
            try
            {
                Autor a = new Autor
                {
                    Id = int.Parse(txtId.Value),
                    Nombre = txtNombre.Text.Trim(),
                    Nacionalidad = txtNacionalidad.Text.Trim()
                };

                AutorNegocio negocio = new AutorNegocio();
                negocio.Modificar(a);

                lbMensaje.Text = "Autor modificado correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = ex.Message;
            }
        }

        protected void btnGuardarAutor_Click(object sender, EventArgs e)
        {
            try
            {
                Autor a = new Autor
                {
                    Nombre = txtNombre.Text.Trim(),
                    Nacionalidad = txtNacionalidad.Text.Trim()
                };

                AutorNegocio negocio = new AutorNegocio();
                negocio.Agregar(a);

                lbMensaje.Text = "Autor agregado correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.Green;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = ex.Message;
            }
        }

        private void CargarGrilla()
        {
            AutorNegocio negocio = new AutorNegocio();
            var lista = negocio.ListarGrilla();
            dgvAutores.DataSource = lista;
            dgvAutores.DataBind();
        }
    }
}