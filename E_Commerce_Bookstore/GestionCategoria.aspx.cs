using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_Commerce_Bookstore
{
    public partial class GestionCategoria : System.Web.UI.Page
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

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Text);
                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.Eliminar(id);

                lbMensaje.Text = "✔ Categoría eliminada.";
                CargarGrilla();
                Limpiar();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error: " + ex.Message;
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Categoria cat = new Dominio.Categoria
                {
                    Id = int.Parse(txtId.Text),
                    Nombre = txtNombre.Text.Trim(),
                    Activo = chkActivo.Checked
                };

                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.Modificar(cat);

                lbMensaje.Text = "✔ Categoría modificada correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Dominio.Categoria cat = new Dominio.Categoria
                {
                    Nombre = txtNombre.Text.Trim(),
                    Activo = chkActivo.Checked
                };

                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.Agregar(cat);

                lbMensaje.Text = "✔ Categoría agregada correctamente.";
                lbMensaje.ForeColor = System.Drawing.Color.Green;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error: " + ex.Message;
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
            }
        }

        protected void dgvCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(dgvCategorias.SelectedDataKey.Value);

            CategoriaNegocio negocio = new CategoriaNegocio();
            Dominio.Categoria cat = negocio.Listar().Find(x => x.Id == id);

            if (cat != null)
            {
                txtId.Text = cat.Id.ToString();
                txtNombre.Text = cat.Nombre;
                chkActivo.Checked = cat.Activo;
            }
        }

        private void CargarGrilla()
        {
            CategoriaNegocio negocio = new CategoriaNegocio();
            dgvCategorias.DataSource = negocio.Listar();
            dgvCategorias.DataBind();
        }

        private void Limpiar()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            chkActivo.Checked = true;
            lbMensaje.Text = "";
            CargarGrilla();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CategoriaNegocio negocio = new CategoriaNegocio();

            string termino = txtBuscar.Text.Trim();
            string estado = ddlEstado.SelectedValue;

            List<Dominio.Categoria> lista = negocio.Listar();

            if (!string.IsNullOrEmpty(termino))
            {
                lista = lista.FindAll(x =>
                    x.Nombre.ToUpper().Contains(termino.ToUpper()));
            }

            if (estado == "Activas")
            {
                lista = lista.FindAll(x => x.Activo == true);
            }
            else if (estado == "Inactivas")
            {
                lista = lista.FindAll(x => x.Activo == false);
            }
            
            dgvCategorias.DataSource = lista;
            dgvCategorias.DataBind();
        }

        protected void btnEliminar_Click1(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtId.Text);
                CategoriaNegocio negocio = new CategoriaNegocio();
                negocio.Eliminar(id);

                lbMensaje.Text = "✔ Categoría eliminada.";
                lbMensaje.ForeColor = System.Drawing.Color.OrangeRed;
                CargarGrilla();
            }
            catch (Exception ex)
            {
                lbMensaje.Text = "❌ Error: " + ex.Message;
            }
        }
    }
}